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

    [Header("Aiming / Cursor Parameters")]
    [Range(0.01f, 50f)] public float playerRotateSpeed = 20f;
    public LayerMask cursorLayermask;

    private Vector3 movement;
    private Vector3 mousePosition;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        if(!cam) cam = Camera.main; 
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {

        //moves player rigidbody
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //gets cursor positon with raycast and rotate player object to look at it
        CursorRaycast();
        Quaternion rotation = Quaternion.LookRotation(mousePosition - rb.position);
        rotation.x = 0f;
        rotation.z = 0f;
        rb.rotation = Quaternion.Lerp(rb.rotation, rotation, Time.fixedDeltaTime * playerRotateSpeed);
    }



    private void PlayerInput()
    {
        //calculate movement vector
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void CursorRaycast()
    {
        //raycast from camera to game world to see where cursor is
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if raycast hits anything, set mousePosition to that
        if(Physics.Raycast(camRay, out hit)) mousePosition = hit.point;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(mousePosition, 1f);
    }
}
