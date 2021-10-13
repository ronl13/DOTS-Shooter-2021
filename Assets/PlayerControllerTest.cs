using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Camera")]
    public Camera cam;

    [Header("Movement Parameters")]
    public float moveSpeed = 10;

    private Vector3 movement;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {

        //moves player rigidbody
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }



    private void PlayerInput()
    {
        //calculate movement vector
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }
}
