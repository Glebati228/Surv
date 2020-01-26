using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]                       private float MaxDistance;
    [Header("Vertical"), SerializeField]   private float verticalSpeed;
    [Header("Horizontal"), SerializeField] private float horizontalSpeed;
    [SerializeField]                       private float jumpForce;
    [SerializeField]                       private float acceleration;
    [SerializeField]                       private float maxAcceleration;
    private Rigidbody rigidbody;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float shift;
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (IsGrounded())
        {
            float vMove = Input.GetAxis("Vertical");
            float hMove = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                shift += acceleration;
            }
            else if (shift > 0f)
            {
                shift -= Mathf.Pow(acceleration, 2f);
            }
            shift = Mathf.Clamp(shift, 0f, maxAcceleration);

            Vector3 verticalMove = transform.forward * vMove * verticalSpeed * Time.fixedDeltaTime * Mathf.Max(1f, shift);
            Vector3 horizontalMove = transform.right * hMove * horizontalSpeed * Time.fixedDeltaTime;
            rigidbody.MovePosition(transform.position + ((verticalMove + horizontalMove) * 0.25f));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.AddForce(0f, jumpForce, 0f, ForceMode.Acceleration);
            }
        }
    }

    //private void MovePlayer()
    //{
    //    float vMove = Input.GetAxis("Vertical");
    //    float hMove = Input.GetAxis("Horizontal");

    //    Vector3 moveVertical = transform.forward * verticalSpeed * vMove * Time.deltaTime;
    //    Vector3 moveHorzontal = hMove * transform.right * horizontalSpeed * Time.deltaTime;

    //    if (!IsGrounded())
    //    {
    //        velocity.x = (moveVertical.x * verticalFallSlowing) + (moveHorzontal.x * horizontalFallSlowing);
    //        velocity.z = (moveVertical.z * verticalFallSlowing) + (moveHorzontal.z * horizontalFallSlowing);
    //        velocity.y += gravity * 0.5f * Mathf.Pow(Time.deltaTime, 2f);
    //    }
    //    else
    //    {
    //        Vector3 direction = Vector3.Normalize(moveHorzontal + moveVertical);

    //        velocity = direction * 0.125f;

    //        if (Input.GetKeyDown(KeyCode.Space))
    //        {
    //            velocity.y = Mathf.Sqrt(jumpForce * -1f * gravity);
    //        }
    //    }

    //    controller.Move(velocity);
    //}

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, MaxDistance);
    }
}
