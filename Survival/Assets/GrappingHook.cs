using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappingHook : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] private float hookDistance;
    [SerializeField] private LayerMask mask;
    [SerializeField] private float baseDelay;
    [SerializeField] private float cameraFriction;
    [SerializeField] private float basePower;

    private float timer;
    private bool startTimeUpdate = default;
    private Ray ray;
    private RaycastHit hit;
    private StrafeMovement strafeMovement;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        strafeMovement = GetComponent<StrafeMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= baseDelay && Input.GetAxis("Hook") == 1)
        {
            startTimeUpdate = false;
            timer = 0.0f;

            ray.origin = transform.position;
            ray.direction = Camera.main.transform.forward.normalized;
            if (Physics.Raycast(ray, out hit, hookDistance, mask.value))
            {
                Rigidbody rigidbody = hit.collider.attachedRigidbody;

                if (rigidbody is null)
                {
                    Vector3 direction = (hit.point - transform.position).normalized;
                    float speed = direction.magnitude * basePower;
                    Debug.DrawLine(transform.position, hit.point, Color.blue);
                    Accelerate(direction, speed);
                }
                else if(!rigidbody.isKinematic) 
                {
                    Vector3 direction = (hit.point - transform.position).normalized;
                    float speed = direction.magnitude * basePower;

                   // float newSpeed;
                   // Vector3 newDir;

    
                    Vector3 impulse = ((-direction * speed * strafeMovement.mass) + (direction * rigidbody.velocity.magnitude * rigidbody.mass)) / (rigidbody.mass + strafeMovement.mass);
                    rigidbody.velocity = -rigidbody.velocity + impulse;

                    Accelerate(direction, speed - impulse.magnitude);
                }
            }
        }

        if (!startTimeUpdate)
        {
            timer += Time.deltaTime;

            if(timer >= baseDelay)
            {
                startTimeUpdate = true;
            }
        }
    }

    void Accelerate(Vector3 wishDir, float wishSpeed)
    {
        float accelVelocity = Vector3.Dot(wishDir, strafeMovement.playerVelocity);
        float additionalSpeed = wishSpeed - accelVelocity;

        if (additionalSpeed <= 0f)
            return;

        float accelSpeed = additionalSpeed * basePower * Time.deltaTime;

        if(accelSpeed > additionalSpeed)
        {
            accelSpeed = additionalSpeed;
        }

        strafeMovement.playerVelocity.x += wishDir.x * accelSpeed;
        strafeMovement.playerVelocity.y += wishDir.y * accelSpeed;
        strafeMovement.playerVelocity.z += wishDir.z * accelSpeed;
    }
}
