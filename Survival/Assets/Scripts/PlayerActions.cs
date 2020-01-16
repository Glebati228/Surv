using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    //[SerializeField] private Queue<IAction<Component>> actions;

    private PlayerMovement movement = new PlayerMovement();
    private PlayerCameraRotation rotation = new PlayerCameraRotation();
    private Transform camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
        movement.camera = camera;
    }

    // Update is called once per frame
    void Update()
    {
        movement.DoAction(transform);
        rotation.DoAction(camera);
    }
}
