using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMoving : MonoBehaviour
{
    [SerializeField] private bool start;
    [SerializeField] private float speed;
    [SerializeField] private float offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            Vector3 newPos = new Vector3()
            {
                x = Mathf.Sin(Time.time * speed),
                y = 0f,
                z = Mathf.Cos(Time.time * speed),
            };

            transform.position += newPos * Time.deltaTime * offset;
        }
    }
}
