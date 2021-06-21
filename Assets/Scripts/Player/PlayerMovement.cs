using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // Check if the player is grounded by casting a sphere.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Reset the user's velocity if they are grounded. 
        // This isn't that important because the player can't jump anyway.
        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        // Get directional input.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;

        move = Vector3.ClampMagnitude(move, 1f);

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
