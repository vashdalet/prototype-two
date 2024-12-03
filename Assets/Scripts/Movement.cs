using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float horizontal;
    private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 40f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector3(horizontal * movementSpeed * Time.deltaTime,rb.velocity.y, rb.velocity.z);
        if (Input.GetButton("Jump"))
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForce * Time.deltaTime, rb.velocity.z);
        }
    }
}
