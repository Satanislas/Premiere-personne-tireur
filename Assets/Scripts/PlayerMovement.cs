using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;

    private bool isGrounded;
    private bool isMoving;

    private Vector3 lastPosition = new Vector3(0, 0, 0);

    private CharacterController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        //reset the default velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        
        //getting the inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        
        //moving
        controller.Move(move * (speed * Time.deltaTime));
        
        //jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //falling
        velocity.y += gravity * Time.deltaTime;
        
        //executing the velocity
        controller.Move(velocity * Time.deltaTime);

        if ((lastPosition != gameObject.transform.position) && isGrounded)
        {
            isMoving = true;
                //for later
        }
        else
        {
            isMoving = false;
                //for later
        }

        isMoving = !!isMoving; // no warning fuck off unity
        lastPosition = gameObject.transform.position;
    }
}
