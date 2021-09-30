Shader "Custom/AtmosphereIEShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define saturate(a) clamp( a, 0.0, 1.0 )
            #define PI 3.141592
            #define PRIMARY_STEP_COUNT 16
            #define LIGHT_STEP_COUNT 8            

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
                float3 viewVector : TEXCOORD2;                   
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.viewVector = UnityWorldSpaceViewDir(o.worldPos.xyz);                 
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            float3 CameraPos;
            float4x4 CameraToWorld;
            float4x4 CameraInverseProjection;

            float3 PlanetCenter;
            float PlanetRadius;
            float AtmosphereRadius;

            float3 DirToSun;   

            float SoftLight(float a, float b) 
            {
                return (b < 0.5 ?
                    (2.0 * a * b + a * a * (1.0 - 2.0 * b)) :
                    (2.0 * a * (1.0 - b) + sqrt(a) * (2.0 * b - 1.0))
                );
            }

            float3 SoftLight(float3 a, float3 b) 
            {
                return float3(
                    SoftLight(a.x, b.x),
                    SoftLight(a.y, b.y),
                    SoftLight(a.z, b.z)
                );
            }            

            bool RayIntersectsSphere(float3 rayStart, float3 rayDir, float3 sphereCenter, 
                                      float sphereRadius, out float t0, out float t1) 
            {
                float3 oc = rayStart - sphereCenter;
                float a = dot(rayDir, rayDir);
                float b = 2.0 * dot(oc, rayDir);
                float c = dot(oc, oc) - sphereRadius * sphereRadius;
                float d =  b * b - 4.0 * a * c;
                // Also skip single point of contact
                if (d <= 0.0) 
                {
                    return false;
                }

                float r0 = (-b - sqrt(d)) / (2.0 * a);
                float r1 = (-b + sqrt(d)) / (2.0 * a);
                t0 = min(r0, r1);
                t1 = max(r0, r1);
                
                return (t1 >= 0.0);
            }

            float3 SampleLightRay(float3 origin, float3 sunDir, float planetScale, float planetRadius, float totalRadius,
                                  float rayleighScale, float mieScale, float absorptionHeightMax, float absorptionFalloff) 
            {
                float t0, t1;
                RayIntersectsSphere(origin, sunDir, PlanetCenter, totalRadius, t0, t1);
                float actualLightStepSize = (t1 - t0) / float(LIGHT_STEP_COUNT);
                float virtualLightStepSize = actualLightStepSize * planetScale;
                float lightStepPosition = 0.0;
                float3 opticalDepthLight = float3(0.0, 0.0, 0.0);
                for (int j = 0; j < LIGHT_STEP_COUNT; j++) {
                float3 currentLightSamplePosition = origin + sunDir * (lightStepPosition + actualLightStepSize * 0.5);
                // Calculate the optical depths and accumulate
                float currentHeight = length(currentLightSamplePosition) - planetRadius;
                float currentOpticalDepthRayleigh = exp(-currentHeight / rayleighScale) * virtualLightStepSize;
                float currentOpticalDepthMie = exp(-currentHeight / mieScale) * virtualLightStepSize;
                float currentOpticalDepthOzone = (1.0 / cosh((absorptionHeightMax - currentHeight) / absorptionFalloff));
                currentOpticalDepthOzone *= currentOpticalDepthRayleigh * virtualLightStepSize;
                opticalDepthLight += float3(
                    currentOpticalDepthRayleigh,
                    currentOpticalDepthMie,
                    currentOpticalDepthOzone);
                lightStepPosition += actualLightStepSize;
                }
                return opticalDepthLight;
            }


            void _ComputeScattering(float3 worldSpacePos, float3 rayDirection, float3 rayOrigin, float3 sunDir,
                                    out float3 scatteringColour, out float3 scatteringOpacity) 
            {
                scatteringColour = float3(0,0,0);
                float3 betaRayleigh = float3(5.5e-6, 13.0e-6, 22.4e-6);
                float betaMie = 21e-6;
                float3 betaAbsorption = float3(2.04e-5, 4.97e-5, 1.95e-6);
                float g = 0.76;
                float sunIntensity = 40.0;
                float planetRadius = PlanetRadius;
                float atmosphereRadius = AtmosphereRadius - planetRadius;
                float totalRadius = planetRadius + atmosphereRadius;
                float referencePlanetRadius = 6371000.0;
                float referenceAtmosphereRadius = 100000.0;
                float referenceTotalRadius = referencePlanetRadius + referenceAtmosphereRadius;
                float referenceRatio = referencePlanetRadius / referenceAtmosphereRadius;
                float scaleRatio = planetRadius / atmosphereRadius;
                float planetScale = referencePlanetRadius / planetRadius;
                float atmosphereScale = scaleRatio / referenceRatio;
                float maxDist = distance(worldSpacePos, rayOrigin);
                float rayleighScale = 8500.0 / (planetScale * atmosphereScale);
                float mieScale = 1200.0 / (planetScale * atmosphereScale);
                float absorptionHeightMax = 32000.0 * (planetScale * atmosphereScale);
                float absorptionFalloff = 3000.0 / (planetScale * atmosphereScale);;
                float mu = dot(rayDirection, sunDir);
                float mumu = mu * mu;
                float gg = g * g;
                float phaseRayleigh = 3.0 / (16.0 * PI) * (1.0 + mumu);
                float phaseMie = 3.0 / (8.0 * PI) * ((1.0 - gg) * (mumu + 1.0)) / (pow(1.0 + gg - 2.0 * mu * g, 1.5) * (2.0 + gg));
                // Early out if ray doesn't intersect atmosphere.
                float t0, t1;
                if (!RayIntersectsSphere(rayOrigin, rayDirection, PlanetCenter, totalRadius, t0, t1)) {
                scatteringOpacity = float3(1.0, 1.0, 1.0);
                return;
                }
                // Clip the ray between the camera and potentially the planet surface.
                t0 = max(0.0, t0);
                t1 = min(maxDist, t1);
                float actualPrimaryStepSize = (t1 - t0) / float(PRIMARY_STEP_COUNT);
                float virtualPrimaryStepSize = actualPrimaryStepSize * planetScale;
                float primaryStepPosition = 0.0;
                float3 accumulatedRayleigh = float3(0.0, 0.0, 0.0);
                float3 accumulatedMie = float3(0.0, 0.0, 0.0);
                float3 opticalDepth = float3(0.0, 0.0, 0.0);
                // Take N steps along primary ray
                for (int i = 0; i < PRIMARY_STEP_COUNT; i++) {
                float3 currentPrimarySamplePosition = rayOrigin + rayDirection * (
                    primaryStepPosition + actualPrimaryStepSize * 0.5);
                float currentHeight = max(0.0, length(currentPrimarySamplePosition) - planetRadius);
                float currentOpticalDepthRayleigh = exp(-currentHeight / rayleighScale) * virtualPrimaryStepSize;
                float currentOpticalDepthMie = exp(-currentHeight / mieScale) * virtualPrimaryStepSize;
                // Taken from https://www.shadertoy.com/view/wlBXWK
                float currentOpticalDepthOzone = (1.0 / cosh((absorptionHeightMax - currentHeight) / absorptionFalloff));
                currentOpticalDepthOzone *= currentOpticalDepthRayleigh * virtualPrimaryStepSize;
                opticalDepth += float3(currentOpticalDepthRayleigh, currentOpticalDepthMie, currentOpticalDepthOzone);
                // Sample light ray and accumulate optical depth.
                float3 opticalDepthLight = SampleLightRay(
                    currentPrimarySamplePosition, sunDir,
                    planetScale, planetRadius, totalRadius,
                    rayleighScale, mieScale, absorptionHeightMax, absorptionFalloff);
                float3 r = (
                    betaRayleigh * (opticalDepth.x + opticalDepthLight.x) +
                    betaMie * (opticalDepth.y + opticalDepthLight.y) + 
                    betaAbsorption * (opticalDepth.z + opticalDepthLight.z));
                float3 attn = exp(-r);
                accumulatedRayleigh += currentOpticalDepthRayleigh * attn;
                accumulatedMie += currentOpticalDepthMie * attn;
                primaryStepPosition += actualPrimaryStepSize;
                }
                scatteringColour = sunIntensity * (phaseRayleigh * betaRayleigh * accumulatedRayleigh + phaseMie * betaMie * accumulatedMie);
                scatteringOpacity = exp(
                    -(betaMie * opticalDepth.y + betaRayleigh * opticalDepth.x + betaAbsorption * opticalDepth.z));
            }



            float3 ApplyGroundFog(float3 color, float distToPoint, float height, float3 worldSpacePos, float3 rayOrigin,
                                  float3 rayDir, float3 sunDir)
            {
                float3 up = normalize(rayOrigin);
                float skyAmt = dot(up, rayDir) * 0.25 + 0.75;
                skyAmt = saturate(skyAmt);
                skyAmt *= skyAmt;
                float3 DARK_BLUE = float3(0.1, 0.2, 0.3);
                float3 LIGHT_BLUE = float3(0.5, 0.6, 0.7);
                float3 DARK_ORANGE = float3(0.7, 0.4, 0.05);
                float3 BLUE = float3(0.5, 0.6, 0.7);
                float3 YELLOW = float3(1.0, 0.9, 0.7);
                float3 fogCol = lerp(DARK_BLUE, LIGHT_BLUE, skyAmt);
                float sunAmt = max(dot(rayDir, sunDir), 0.0);
                fogCol = lerp(fogCol, YELLOW, pow(sunAmt, 16.0));
                float be = 0.0025;
                float fogAmt = (1.0 - exp(-distToPoint * be));
                // Sun
                sunAmt = 0.5 * saturate(pow(sunAmt, 256.0));

                return lerp(color, fogCol, fogAmt) + sunAmt * YELLOW;                                
            }


            float3 ApplySpaceFog(float3 color, float distToPoint, float height, float3 worldSpacePos, float3 rayOrigin,
                                 float3 rayDir, float3 sunDir)
            {
                float atmosphereThickness = (AtmosphereRadius - PlanetRadius);

                float t0 = -1.0;
                float t1 = -1.0;

                if(RayIntersectsSphere(rayOrigin, rayDir, PlanetCenter, PlanetRadius, t0, t1))
                {
                    if(distToPoint > t0)
                    {
                        distToPoint = t0;
                        worldSpacePos = rayOrigin + t0 * rayDir;
                    }
                }

                if(!RayIntersectsSphere(rayOrigin, rayDir, PlanetCenter, PlanetRadius + atmosphereThickness * 1.0, t0, t1))
                {
                    return color * 0.5;
                }

                float silhouette = saturate(distToPoint - 10000.0) / 10000.0;

                // Glow around planet
                float scaledDistanceToSurface = 0.0;

                // Calculate the closest point btw ray direction and planet. Use a point in front of the 
                // camera to force differences as you get closer to planet.
                float3 fakeOrigin = rayOrigin + rayDir * atmosphereThickness;
                float t = max(0.0, dot(rayDir, PlanetCenter - fakeOrigin) / dot(rayDir, rayDir));
                float3 pb = fakeOrigin + t * rayDir;

                scaledDistanceToSurface = saturate(distance(pb, PlanetCenter) - PlanetRadius) / atmosphereThickness;
                scaledDistanceToSurface = smoothstep(0.0, 1.0, 1.0 - scaledDistanceToSurface);

                float scatteringFactor = scaledDistanceToSurface * silhouette;

                // Fog on surface
                t0 = max(0.0, t0);
                t1 = min(distToPoint, t1);

                float3 intersectionPoint = rayOrigin + t1 * rayDir;
                float3 normalAtIntersection = normalize(intersectionPoint);
                float distFactor = exp(-distToPoint * 0.0005 / (atmosphereThickness));
                float fresnel = 1.0 - saturate(dot(-rayDir, normalAtIntersection));
                fresnel = smoothstep(0.0, 1.0, fresnel);
                float extinctionFactor = saturate(fresnel * distFactor) * (1.0 - silhouette);

                // Front/Back Lighting
                float3 BLUE = float3(0.5, 0.6, 0.75);
                float3 YELLOW = float3(1.0, 0.9, 0.7);
                float3 RED = float3(0.035, 0.0, 0.0);
                float NdotL = dot(normalAtIntersection, sunDir);
                float wrap = 0.5;
                float NdotL_wrap = max(0.0, (NdotL + wrap) / (1.0 + wrap));
                float RdotS = max(0.0, dot(rayDir, sunDir));
                float sunAmount = RdotS;
                float3 backLightingColour = YELLOW * 0.1;
                float3 frontLightingColour = lerp(BLUE, YELLOW, pow(sunAmount, 32.0));
                float3 fogColour = lerp(backLightingColour, frontLightingColour, NdotL_wrap);
                extinctionFactor *= NdotL_wrap;
                
                // Sun
                float specular = pow((RdotS + 0.5) / (1.0 + 0.5), 64.0);
                fresnel = 1.0 - saturate(dot(-rayDir, normalAtIntersection));
                fresnel *= fresnel;
                float sunFactor = (length(pb) - PlanetRadius) / (atmosphereThickness * 5.0);
                sunFactor = (1.0 - saturate(sunFactor));
                sunFactor *= sunFactor;
                sunFactor *= sunFactor;
                sunFactor *= specular * fresnel;
                float3 baseColour = lerp(color, fogColour, extinctionFactor);
                float3 litColour = baseColour + SoftLight(fogColour * scatteringFactor + YELLOW * sunFactor, baseColour);
                float3 blendedColour = lerp(baseColour, fogColour, scatteringFactor);
                blendedColour += blendedColour + SoftLight(YELLOW * sunFactor, blendedColour);
                
                return lerp(litColour, blendedColour, scaledDistanceToSurface * 0.25);
            } 

            float3 ApplyFog(float3 color, float distToPoint, float height, float3 worldSpacePos, float3 rayOrigin, 
                            float3 rayDir, float3 sunDir)
            {
                float distToPlanet = max(0.0, length(rayOrigin) - PlanetRadius);
                float atmosphereThickness = (AtmosphereRadius - PlanetRadius);

                float3 groundColor = ApplyGroundFog(color, distToPoint, height, worldSpacePos, rayOrigin, rayDir, sunDir);
                float3 spaceColor = ApplySpaceFog(color, distToPoint, height, worldSpacePos, rayOrigin, rayDir, sunDir); //float3(0, 1, 0);

                float blendFactor = saturate(distToPlanet / (atmosphereThickness * 0.5f));

                blendFactor = smoothstep(0.0, 1.0, blendFactor);

                return lerp(groundColor, spaceColor, blendFactor);
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float dist = length(i.worldPos - CameraPos);
                float height = max(0.0, length(CameraPos) - PlanetRadius);

                float3 origin = mul(CameraToWorld, float4(0,0,0,1)).xyz;
                float3 direction = mul(CameraInverseProjection, float4((i.uv * 2) - 1,0,1)).xyz;
                direction = mul(CameraToWorld, float4(direction,0)).xyz;
                direction = normalize(direction);  

             //   float3 diff = ApplyFog(col, dist, height, i.worldPos, origin, direction, DirToSun);
                float3 scatteringColor = float3(0,0,0);
                float3 scatteringOpacity = float3(1,1,1);

                _ComputeScattering(i.worldPos, direction, origin, DirToSun, scatteringColor, scatteringOpacity);

                float3 diff = col.rgb * scatteringOpacity + scatteringColor;

                return float4(diff.rgb, 1);
            }
            ENDCG
        }
    }
}
