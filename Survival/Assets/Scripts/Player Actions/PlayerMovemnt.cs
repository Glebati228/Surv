using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemnt : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float speed;
    [SerializeField, Range(0f, 1000f)] private float jumpForce;

    private Rigidbody rigidbody;
    private Ray ray;
    private RaycastHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        ray.direction = Vector3.down;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float vMove = Input.GetAxis("Vertical");
        float hMove = Input.GetAxis("Horizontal");

        Vector3 move = vMove * transform.forward * speed * Time.deltaTime;
        transform.position += move;

        move = hMove * transform.right * speed * Time.deltaTime;
        transform.position += move;

        ray.origin = transform.position;
        //if (!Physics.Raycast(ray, out hit, 1f))
        //{
            float jump = Input.GetAxis("Jump");
            move = jump * Vector3.up * jumpForce;

            rigidbody.AddForce(move, ForceMode.Force);
        //}
    }
}
