using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour {


    public float speed;
    public float jumpForce;
    public float mouseSensitivity = 2f;
    public float groundCheckDistance = .1f;
    public float stickToGroundHelperDistance = .6f;

    private float speedMultiplyer = 30f;
    public float jumpMultiplyer = 30f;

    private Camera mainCam;
    private Rigidbody playerRigidBody;
    private Vector3 groundNormal;
    private bool isJump, isPreviouslyGrounded, isJumping, isGrounded;
    private CapsuleCollider playerCapsule;

    // Use this for initialization
    void Start () {
        mainCam = Camera.main;
        playerRigidBody = GetComponent<Rigidbody>();
        playerCapsule = GetComponent<CapsuleCollider>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move();




    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("escape"))
        {

            switch (Cursor.lockState)
            {
                case CursorLockMode.None:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case CursorLockMode.Locked:
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }

        }

        if (Input.GetKeyDown("space"))
            isJump = true;


        RotateView();
    }

    void LateUpdate()
    {
        
    }


    private void Move()
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f,
            y = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f
        };

        if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && isGrounded)
        {
            MoveWSAD(input, speedMultiplyer);
        }

        if (isGrounded)
        {
            playerRigidBody.drag = 10f;

            if (isJump)
            {
                playerRigidBody.drag = 0f;
                playerRigidBody.velocity = new Vector3(playerRigidBody.velocity.x, 0f, playerRigidBody.velocity.z);
                playerRigidBody.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
                isJumping = true;
            }

            if (!isJumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && playerRigidBody.velocity.magnitude < 1f)
            {
                playerRigidBody.Sleep();
            }
        }
        else
        {
            playerRigidBody.drag = 0f;
            MoveWSAD(jumpMultiplyer);

            if (isPreviouslyGrounded && !isJumping)
            {
                StickToGroundHelper();
            }
        }

        isJump = false;


    }

    private void MoveWSAD(float multiplyer)
    {
        Vector2 input = new Vector2
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f,
            y = Input.GetAxis("Vertical") * Time.deltaTime * 150.0f
        };

        if (Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon)
            MoveWSAD(input, multiplyer);
    }

    private void MoveWSAD(Vector2 input, float multiplyer)
    {

        Vector3 desiredMove = mainCam.transform.forward * input.y + mainCam.transform.right * input.x;
        desiredMove = Vector3.ProjectOnPlane(desiredMove, groundNormal).normalized;

        desiredMove.x *= speed * multiplyer;
        desiredMove.y *= speed * multiplyer;
        desiredMove.z *= speed * multiplyer;

        playerRigidBody.AddForce(desiredMove);
    }

    private void StickToGroundHelper()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, playerCapsule.radius, Vector3.down, out hitInfo,
                               ((playerCapsule.height / 2f) - playerCapsule.radius) +
                               stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
            {
                playerRigidBody.velocity = Vector3.ProjectOnPlane(playerRigidBody.velocity, hitInfo.normal);
            }
        }
    }

    private void RotateView()
    {
        if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

        float xRot = Input.GetAxis("Mouse X") * mouseSensitivity;
        float yRot = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //Quaternion characterTargetRot = transform.localRotation;
        //Quaternion camTargetRot = mainCam.transform.localRotation;

        transform.localRotation *= Quaternion.Euler(0f, xRot, 0f);
        mainCam.transform.localRotation *= Quaternion.Euler(-yRot, 0f, 0f);
    }

    // sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
    private void GroundCheck()
    {
        isPreviouslyGrounded = isGrounded;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, playerCapsule.radius, Vector3.down, out hitInfo,
                               ((playerCapsule.height / 2f) - playerCapsule.radius) + groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
            groundNormal = hitInfo.normal;
        }
        else
        {
            isGrounded = false;
            groundNormal = Vector3.up;
        }
        if (!isPreviouslyGrounded && isGrounded && isJumping)
        {
            isJumping = false;
        }
    }

}
