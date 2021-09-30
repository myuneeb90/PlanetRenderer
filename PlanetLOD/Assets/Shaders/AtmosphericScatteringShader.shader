Shader "Custom/AtmosphericScatteringShader"
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
            float DensityFalloff;
            int NumOpticalDepthPoints;
            int NumInScatteringPoints;
            float3 ScatteringCoefficients;            

            float2 RaySphereIntersection(float3 sphereCenter, float sphereRadius, float3 rayOrigin, float3 rayDir)
            {
                float3 offset = rayOrigin - sphereCenter;
                float a = 1;
                float b = 2 * dot(offset, rayDir);
                float c = dot(offset, offset) - sphereRadius * sphereRadius;
                float d = b * b - 4 * a * c;

                if(d > 0)
                {
                    float s = sqrt(d);
                    float dstToSphereNear = max(0, (-b - s) / (2 * a));
                    float dstToSphereFar = (-b + s) / (2 * a);

                    if(dstToSphereFar >= 0)
                    {
                        return float2(dstToSphereNear, dstToSphereFar - dstToSphereNear);
                    }
                }

                return float2(100000, 0);
            }

            float DensityAtPoint(float3 densitySamplePoint)
            {
                float heightAboveSurface = length(densitySamplePoint - PlanetCenter) - PlanetRadius;
                float height01 = heightAboveSurface / (AtmosphereRadius - PlanetRadius);
                float localDensity = exp(-height01 * DensityFalloff) * (1 - height01);
                return localDensity;
            } 

            float OpticalDepth(float3 rayOrigin, float3 rayDir, float rayLength)
            {
                float3 densitySamplePoint = rayOrigin;
                float stepSize = rayLength / (NumOpticalDepthPoints - 1);
                float opticalDepth = 0;

                for(int i = 0; i < NumOpticalDepthPoints; i++)
                {
                    float localDensity = DensityAtPoint(densitySamplePoint);
                    opticalDepth += localDensity * stepSize;
                    densitySamplePoint += rayDir * stepSize;
                }

                return opticalDepth;
            }                       

            float3 CalculateLight(float3 rayOrigin, float3 rayDir, float rayLength, float3 originalCol)
            {
                float3 inScatterPoint = rayOrigin;
                float stepSize = rayLength / (NumInScatteringPoints - 1);
                float3 inScatteredLight = 0;
                float viewRayOpticalDepth = 0;

                for(int i = 0; i < NumInScatteringPoints; i++)
                {
                    float sunRayLength = RaySphereIntersection(PlanetCenter, AtmosphereRadius, inScatterPoint, DirToSun).y;
                    float sunRayOpticalDepth = OpticalDepth(inScatterPoint, DirToSun, sunRayLength);//OpticalDepthBaked(inScatterPoint, dirToSun); //OpticalDepth(inScatterPoint, dirToSun, sunRayLength);
                    viewRayOpticalDepth = OpticalDepth(inScatterPoint, -rayDir, stepSize * i);//OpticalDepthBaked(inScatterPoint, -rayDir); //OpticalDepth(inScatterPoint, -rayDir, stepSize * i);
                    float3 transmittance = exp(-(sunRayOpticalDepth + viewRayOpticalDepth) * ScatteringCoefficients);
                    float localDensity = DensityAtPoint(inScatterPoint);
                    inScatteredLight += localDensity * transmittance * ScatteringCoefficients * stepSize;
                    inScatterPoint += rayDir * stepSize; 
                }
                float originalColTransmittance = exp(-viewRayOpticalDepth);

                return originalCol * originalColTransmittance + inScatteredLight;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float3 origin = mul(CameraToWorld, float4(0,0,0,1)).xyz;
                float3 direction = mul(CameraInverseProjection, float4((i.uv * 2) - 1,0,1)).xyz;
                direction = mul(CameraToWorld, float4(direction,0)).xyz;
                direction = normalize(direction);        
        
        
                float sceneDepthNonLinear = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv);
                float sceneDepth = LinearEyeDepth(sceneDepthNonLinear) * length(direction);


                float3 rayOrigin = origin;
                float3 rayDir = direction;

                float2 hitInfo = RaySphereIntersection(PlanetCenter, AtmosphereRadius, rayOrigin, rayDir);

                float dstToAtmosphere = hitInfo.x;
                float dstThroughAtmosphere = min(hitInfo.y, sceneDepth - dstToAtmosphere);

//                float atmoCol = dstThroughAtmosphere / (AtmosphereRadius * 2);

                if(dstThroughAtmosphere > 0)
                {
                    const float epsilon = 0.0001;
                    float3 pointInAtmosphere = rayOrigin + rayDir * (dstToAtmosphere + epsilon);
                    float3 light = CalculateLight(pointInAtmosphere, rayDir, dstThroughAtmosphere - epsilon * 2, col);
                    return float4(light, 0);
                }


                return col;
                //return float4(atmoCol, atmoCol, atmoCol, 1);
            }
            ENDCG
        }
    }
}
