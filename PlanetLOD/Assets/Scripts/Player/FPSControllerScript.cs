using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSControllerScript : MonoBehaviour
{   
    public Camera SceneCamera;
    public Rigidbody RB;

    public Vector3 OffsetPosition;

    public float MouseSensitivityX = 250f;
    public float MouseSensitivityY = 250f;

    float VerticalLookRotation;

    Vector3 MoveAmount;
    Vector3 SmoothMoveVelocity;
    public float MoveSpeed = 10;

    public float JumpForce = 220;
    bool Grounded = false;
    public LayerMask GroundedMask;

    void Awake()
    {
        RB.transform.position = this.transform.position;
        OffsetPosition = this.transform.position;
        this.transform.position = Vector3.zero;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * MouseSensitivityX * Time.deltaTime);
        VerticalLookRotation += Input.GetAxis("Mouse Y") * MouseSensitivityY * Time.deltaTime;
        VerticalLookRotation = Mathf.Clamp(VerticalLookRotation, -80.0f, 80.0f);
        SceneCamera.transform.localEulerAngles = Vector3.left * VerticalLookRotation;
    
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 targetMoveAmount = moveDir * MoveSpeed;
        MoveAmount = Vector3.SmoothDamp(MoveAmount, targetMoveAmount, ref SmoothMoveVelocity, 0.15f);
    
        if(Input.GetButtonDown("Jump"))
        {
            if(Grounded == true)
            {
                RB.AddForce(transform.up * JumpForce);
            }
        }

        Grounded = false;
        Ray ray = new Ray(OffsetPosition, -transform.up);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1 + 0.1f, GroundedMask))
        {
            Grounded = true;
        }

        OffsetPosition = RB.transform.position;
//        this.transform.position = Vector3.zero;
    }

    void FixedUpdate()
    {
        RB.MovePosition(RB.position + transform.TransformDirection(MoveAmount) * Time.fixedDeltaTime);
    }
}
