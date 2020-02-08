using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafeMovement : MonoBehaviour
{
#pragma warning disable 0649
    private CharacterController controller;
    [SerializeField] private Transform camera;


    /* Frame occuring factors */
    [SerializeField] float gravity = 20f;
    [SerializeField] float friction = 0.5f;                // Ground friction

    /* Movement stuff */
    [SerializeField] float moveSpeed = 40.0f;                // Ground move speed
    [SerializeField] float runAcceleration = 0.5f;           // Ground accel
    [SerializeField] float runDeacceleration = 10f;          // Deacceleration that occurs when running on the ground
    [SerializeField] float airAcceleration = 0.08f;           // Air accel
    [SerializeField] float airDeacceleration = 0.08f;         // Deacceleration experienced when opposite strafing
    [SerializeField] float airControl = 0.3f;                // How precise air control is
    [SerializeField] float sideStrafeAcceleration = 1f;     // How fast acceleration occurs to get up to sideStrafeSpeed when side strafing
    [SerializeField] float sideStrafeSpeed = 1f;             // What the max speed to generate when side strafing
    [SerializeField] float jumpSpeed = 8.0f;                 // The speed at which the character's up axis gains when hitting jump
    bool holdJumpToBhop = false;            // When enabled allows player to just hold jump button to keep on bhopping perfectly. Beware: smells like casual.


    /* Debug stuff */
    float updateRate = 4f;
    float fps = 0f;
    float timer = 0f;
    int frameCount = 0;

    public Vector3 playerVelocity = Vector3.zero;
    Vector3 playerDirectionNormal = Vector3.zero;
    Vector3 playerDirection = Vector3.zero;
    float playerTopVelocity = 0f;

    bool wishJump = false;

    float playerFriction = 0f;

    public float mass;
    [SerializeField] float massMult;

    public bool collisionStop = true;
#pragma warning restore 0649

    struct cmd
    {
        public float forwardMove;
        public float rightMove;
        public float upMove;
    }

#pragma warning disable 0649
    private cmd commands;

    /*stuff for spawn player*/
    private bool isDead = false;
    [SerializeField] private Transform spawnPoint;
#pragma warning restore 0649

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        commands = new cmd();

        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        mass = (transform.localScale.x + transform.localScale.y + transform.localScale.z) * 0.333f;
        mass *= massMult;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        timer += Time.deltaTime;
        if (timer > 1f / updateRate)
        {
            fps = Mathf.Round(frameCount / timer);
            timer -= 1f / updateRate;
            frameCount = 0;
        }

        QueueJump();

        if (controller.isGrounded)
        {
            GroundMove();
        }
        else
        {
            AirMove();
        }

        //playerVelocity.x = Mathf.Clamp(playerVelocity.x, -1f, 1f);
        //playerVelocity.z = Mathf.Clamp(playerVelocity.z, -1f, 1f);
        //playerVelocity.y = Mathf.Clamp(playerVelocity.y, -1f, 1f);

        controller.Move(playerVelocity * Time.deltaTime);


        Vector3 udp = playerVelocity;
        udp.y = 0.0f;
        playerTopVelocity = Mathf.Max(playerTopVelocity, udp.magnitude);


        if (Input.GetKeyDown(KeyCode.X))
        {
            Killplayer();
        }
        if (Input.GetMouseButtonDown(0) && isDead)
        {
            SpawnPlayer();
        }
    }


    //---------------------------------------------------MOVEMENT---------------------------------------------------------------//
    private void SetMovementDir()
    {
        commands.forwardMove = Input.GetAxis("Vertical");
        commands.rightMove = Input.GetAxis("Horizontal");
    }

    private void GroundMove()
    {
        Vector3 wishVelocity;
        float wishSpeed;

        if (!wishJump)
        {
            CalculateFriction();
        }

        SetMovementDir();

        wishVelocity = new Vector3(commands.rightMove, 0f, commands.forwardMove);
        wishVelocity = transform.TransformDirection(wishVelocity);

        wishSpeed = wishVelocity.magnitude * moveSpeed;
        wishVelocity.Normalize();

        Accelerate(wishVelocity, wishSpeed, runAcceleration);

        playerVelocity.y = 0f;

        if (wishJump)
        {
            playerVelocity.y = jumpSpeed;
            wishJump = false;
        }
    }

    private void Accelerate(Vector3 wishVelocity, float wishSpeed, float runAcceleration)
    {
        float currentSpeed;
        float additionalSpeed;
        float accelSpeed;

        currentSpeed = Vector3.Dot(playerVelocity, wishVelocity);
        additionalSpeed = wishSpeed - currentSpeed;

        if (additionalSpeed <= 0)
            return;

        accelSpeed = runAcceleration * Time.deltaTime * wishSpeed;

        if (accelSpeed > additionalSpeed)
            accelSpeed = additionalSpeed;

        playerVelocity.x += wishVelocity.x * accelSpeed;
        playerVelocity.z += wishVelocity.z * accelSpeed;
    }

    private void CalculateFriction()
    {
        float control = 0f;
        float speedDrop = 0f;
        Vector3 vec = playerVelocity;
        vec.y = 0f;
        float speed = vec.magnitude;
        float newSpeed = 0f;

        if (IsGrounded())
        {
            control = speed < runDeacceleration ? runDeacceleration : speed;
            speedDrop = control * friction * Time.deltaTime;
        }

        newSpeed = speed - speedDrop;
        newSpeed = Mathf.Max(0f, newSpeed);

        playerFriction = newSpeed;

        if (speed > 0)
            newSpeed /= speed;

        playerVelocity.x *= newSpeed;
        playerVelocity.z *= newSpeed;
    }

    private void QueueJump()
    {
        if (holdJumpToBhop)
        {
            wishJump = Input.GetKey(KeyCode.Space);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !wishJump)
            wishJump = true;
        if (Input.GetKeyUp(KeyCode.Space))
            wishJump = false;
    }

    private void AirMove()
    {
        Vector3 wishDirection;
        float wishSpeed;
        float accel = 0f;

        SetMovementDir();

        wishDirection = new Vector3(commands.rightMove, 0f, commands.forwardMove);
        wishDirection = transform.TransformDirection(wishDirection);
        wishSpeed = wishDirection.magnitude;
        wishDirection = wishDirection.normalized;
        wishSpeed *= moveSpeed;

        if (Vector3.Dot(wishDirection, playerVelocity) < 0)
        {
            accel = airAcceleration;
        }
        else
        {
            accel = airDeacceleration;
        }

        //if (commands.forwardMove == 0 && commands.rightMove != 0)
        //{
        //    if (wishSpeed > sideStrafeSpeed)
        //        wishSpeed = sideStrafeSpeed;
        //    accel = sideStrafeAcceleration;
        //}

        Accelerate(wishDirection, wishSpeed, accel);

        // AirControl(wishDirection, wishSpeed);

        playerVelocity.y -= gravity * Time.deltaTime;
    }

    //private void AirControl(Vector3 wishVelocity, float wishSpeed)
    //{
    //    float zspeed;
    //    float speed;
    //    float currentSpeed;
    //    float k;

    //    // Can't control movement if not moving forward or backward
    //    if (commands.forwardMove == 0 || wishSpeed == 0)
    //        return;

    //    zspeed = playerVelocity.y;
    //    playerVelocity.y = 0;
    //    /* Next two lines are equivalent to idTech's VectorNormalize() */
    //    speed = playerVelocity.magnitude;
    //    playerVelocity.Normalize();

    //    currentSpeed = Vector3.Dot(playerVelocity, wishVelocity);
    //    k = 32;
    //    k *= airControl * Mathf.Pow(currentSpeed, 2) * Time.deltaTime;

    //    // Change direction while slowing down
    //    if (currentSpeed > 0f)
    //    {
    //        playerVelocity.x = playerVelocity.x * speed + wishVelocity.x * k;
    //        playerVelocity.y = playerVelocity.y * speed + wishVelocity.y * k;
    //        playerVelocity.z = playerVelocity.z * speed + wishVelocity.z * k;

    //        playerVelocity.Normalize();
    //    }

    //    playerVelocity.x *= speed;
    //    playerVelocity.y = zspeed; // Note this line
    //    playerVelocity.z *= speed;
    //}

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.09f);
    }

    private void Killplayer()
    {
        isDead = true;
    }

    private void SpawnPlayer()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        playerVelocity = Vector3.zero;
        camera.rotation = Quaternion.Euler(0f, 0f, 0f);
        isDead = false;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 100), $"FPS: {fps}");
        GUI.Label(new Rect(0, 15, 200, 100), $"Player speed {controller.velocity}");
        GUI.Label(new Rect(0, 30, 200, 100), $"Top speed {playerTopVelocity}");
        GUI.Label(new Rect(0, 45, 200, 100), $"isGrounded {controller.isGrounded}");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        Vector3 playerDir = playerVelocity.normalized;
        float playerSpeed = playerVelocity.magnitude;
        float playerMass = this.mass;

        if (rigidbody == null)
        {
            if (collisionStop)
            {
                Vector3 hitDir = hit.normal;
                float hitMass = playerMass;

                Vector3 sumVelocity = ((playerMass * playerDir * playerSpeed)) / (playerMass + hitMass);
                playerVelocity = (-playerSpeed * playerDir) + sumVelocity;
                //playerVelocity = 0f + ;
            }
        }
        else if (!rigidbody.isKinematic)
        {
            //Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);

            //rigidbody.velocity = pushDir;

            //playerVelocity -= pushDir * 2f;
            Vector3 hitDir = -playerDir;
            float hitSpeed = rigidbody.velocity.magnitude;
            float hitMass = rigidbody.mass;

            Vector3 sumVelocity = ((playerMass * playerDir * playerSpeed) + (hitMass * hitDir * hitSpeed)) / (playerMass + hitMass);
            rigidbody.velocity = (-hitSpeed * hitDir) + sumVelocity;
            playerVelocity += (-playerSpeed * playerDir) + sumVelocity;
        }
    }
}
