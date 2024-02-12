using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSPlayerController : MonoBehaviour
{
    [SerializeField] Transform characterBody;
    [SerializeField] Transform cameraArm;

    [SerializeField] float power;
    [SerializeField] float jumpPower;
    [SerializeField] float maxSpeed;

    Rigidbody rigid;

    bool isWalking = false;
    bool isInAir = true;
    public bool isYZero;
    public bool IsWalking { get { return isWalking; } }
    public bool IsInAir { get {  return isInAir; } }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        LookAround();
        Move();
        Jump();

        Debug.Log(rigid.velocity.y);

        if (rigid.velocity.y == 0f)
        {
            isYZero = true;
        }
        else
        {
            isYZero = false;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !IsInAir)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        }
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        isWalking = moveInput.magnitude != 0;

        if (isWalking)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            characterBody.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * power;
            // rigid.AddForce(moveDir.normalized * power, ForceMode.VelocityChange);

            if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
            {
                Vector3 velocity = rigid.velocity;
                velocity.x = maxSpeed * moveDir.x;
                rigid.velocity = velocity;
            }

            if (Mathf.Abs(rigid.velocity.z) > maxSpeed)
            {
                Vector3 velocity = rigid.velocity;
                velocity.z = maxSpeed * moveDir.z;
                rigid.velocity = velocity;
            }
        }
    }

    void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            //Vector3 velocity = rigid.velocity;
            //rigid.velocity = new Vector3(velocity.x, 0f, velocity.z);

            isInAir = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            isInAir = true;
        }
    }
}