using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractorScript : MonoBehaviour
{
    public float Gravity = -10;

    

    public void Attract(Transform body, Rigidbody rigidbody)
    {
        Vector3 targetDirection = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;

        body.rotation = Quaternion.FromToRotation(bodyUp, targetDirection) * body.rotation;
        rigidbody.AddForce(targetDirection * Gravity);
    }

}
