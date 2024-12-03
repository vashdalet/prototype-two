using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask giantDeepMask;
    public LayerMask gravityCannonMask;
    private float x;
    private float z;
    private Vector3 move;
    private Vector3 velocity;

    public float speed = 12f;
    public float gravity;

    public float jumpForce = 3f;
    private bool isGrounded;
    private Vector3 desiredUp = Vector3.up;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // Get the player's current forward vector
        Vector3 forward = transform.forward;

        // Project forward onto the plane perpendicular to desiredUp
        Vector3.OrthoNormalize(ref desiredUp, ref forward);

        // Compute the new rotation
        Quaternion targetRotation = Quaternion.LookRotation(forward, desiredUp);

        // Smoothly rotate towards targetRotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);

        bool isInGiantsDeep = Physics.CheckSphere(groundCheck.position, groundDistance, giantDeepMask);
        bool isInGravityCannon = Physics.CheckSphere(groundCheck.position, groundDistance, gravityCannonMask);
        if (isInGiantsDeep) { 
            gravity = -20f;
        } 
        else if(isInGravityCannon)
        {
            gravity = 20f;
        }
        else {
            gravity = -20f;
        }
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpForce * 4f;
        }

        velocity.y += gravity * Time.deltaTime;
        Vector3 change = velocity * Time.deltaTime;
        change = targetRotation * change;
        controller.Move(change);
    }

    void OnTriggerEnter(Collider other)
    {
        desiredUp = other.gameObject.transform.up;
    }

    void OnTriggerExit(Collider other)
    {
        desiredUp = Vector3.up;
    }
}