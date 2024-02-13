using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSPlayerController : MonoBehaviour
{
    [SerializeField] Transform characterBody;
    [SerializeField] Transform cameraArm;
    [SerializeField] Transform shooterPos;

    [SerializeField] float walkSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float maxSpeed;

    Rigidbody rigid;
    Transform spine;
    Vector2 moveInput;

    bool isWalking = false;
    bool isInAir = true;
    bool isShooting = false;
    public Vector2 CurrentInput {  get { return moveInput; } }
    public bool IsWalking { get { return isWalking; } }
    public bool IsInAir { get {  return isInAir; } }
    public bool IsShooting { get { return isShooting; } }

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        spine = characterBody.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Spine);
    }

    void Update()
    {
        LookAround();
        Move();
        Jump();
        Shoot();
    }

    void LateUpdate()
    {
        Vector3 camForward = cameraArm.forward;
        camForward.y = 0;
        characterBody.rotation = Quaternion.LookRotation(camForward);

        // spine.LookAt(camForward);
        spine.rotation = Quaternion.LookRotation(new Vector3(cameraArm.forward.x, cameraArm.forward.y + 0.5f, cameraArm.forward.z));
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;

            RaycastHit hit;
            if (Physics.Raycast(shooterPos.position, Vector3.forward, out hit, 1f))
            {
                //float distanceToGround = hit.distance;
                //transform.position = new Vector3(transform.position.x, distanceToGround, transform.position.z);
            }
            Debug.DrawRay(shooterPos.position, Vector3.forward, Color.red);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
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
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveInput.Normalize();
        isWalking = moveInput.magnitude != 0;

        if (isWalking)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            transform.position += moveDir * Time.deltaTime * walkSpeed;

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

        //Vector3 camForward = cameraArm.forward;
        ////camForward.y = 0;
        ////characterBody.rotation = Quaternion.LookRotation(camForward);

        //// spine.LookAt(camForward);
        //spine.localRotation = Quaternion.LookRotation(camForward);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
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