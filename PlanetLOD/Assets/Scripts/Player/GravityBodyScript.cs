using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBodyScript : MonoBehaviour
{
    public GravityAttractorScript planet;
    public Rigidbody RB;
    
    void Awake()
    {
        RB.useGravity = false;
        RB.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        planet.Attract(transform, RB);
    }
}
