using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour 
{
    public Vector3 Position = Vector3.zero;
    public Vector3 OffsetPosition = Vector3.zero;
    public float LinearSpeed = 10;
    public float AngularSpeed = 50;
    public float RollSpeed = 0;

    void Awake()
    {
        OffsetPosition = this.transform.position;
        Position = this.transform.position;
        this.transform.position = Vector3.zero;
    }

    void Update()
    {
        Quaternion addRot = Quaternion.identity;

        float roll = 0;
        float pitch = 0;
        float yaw = 0;

        roll = Input.GetAxis("Roll") * RollSpeed * Time.deltaTime;
        pitch = Input.GetAxis("Mouse Y") * AngularSpeed * Time.deltaTime;
        yaw = Input.GetAxis("Mouse X") * AngularSpeed * Time.deltaTime;

        addRot.eulerAngles = new Vector3(-pitch, yaw, -roll);

        this.transform.rotation *= addRot;

        Vector3 forwardVelocity = this.transform.rotation * Vector3.forward * Input.GetAxis("Vertical") * LinearSpeed * Time.deltaTime;
        Vector3 rightVelocity = this.transform.rotation * Vector3.right * Input.GetAxis("Horizontal") * LinearSpeed * Time.deltaTime;
        Vector3 upVelocity = this.transform.rotation * Vector3.up * Input.GetAxis("Up") * LinearSpeed * Time.deltaTime;

        Vector3 finalVelocity = forwardVelocity + rightVelocity + upVelocity;
        OffsetPosition = finalVelocity;
        Position = Position + OffsetPosition;
    }
}
