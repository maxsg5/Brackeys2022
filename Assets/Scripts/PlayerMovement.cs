using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(BetterInputManager))]
public class PlayerMovement : MonoBehaviour
{
    public Transform cam;
    public Transform groundCheck; // a position marking where to check if the player is grounded
    public float groundDistance = 0.4f; // distance for the groundCheck ray
    public LayerMask groundMask; // mask for the ground layer
    public bool isGrounded; // is the player grounded?
    public float jumpHeight = 3f; // the height of the jump
    public float speed = 12f; // movement speed
    public Vector3 velocity; // the velocity of the player
    public float gravity = -9.81f; // the gravity of the player

    public float turnSmoothTime = 0.1f; // the time it takes to turn the player
    float turnSmoothVelocity;

    //public float horizontalSensitivity = 10f; // WARNING!!! THIS MUST MATCH VALUE IN CinamachinePOVExtension.cs!!!!!!!!!!!!!!!!!!!!!!

    float xRotation = 0f;
    private BetterInputManager inputManager; // NEW INPUT SYSTEM

    private CharacterController controller; //reference to the CharacterController component


    private void Start()
    {
         controller = GetComponent<CharacterController>();
         inputManager = BetterInputManager.Instance;
    }

    

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // check if the player is grounded

        if (isGrounded && velocity.y < 0) // if the player is grounded and is falling
            velocity.y = -2f; // set the velocity to -2f
        
        Vector2 moveInput = inputManager.GetPlayerMovement(); // get the move input from the input manager
        Vector3 move = new Vector3(moveInput.x,0,moveInput.y); // get the movement vector
        //Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y; // create a vector3 for the movement


        if(moveInput.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cam.eulerAngles.y; // get the angle of the movement vector
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // smooth the angle of the player
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // set the rotation of the player to the angle of the movement vector

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // create a vector3 for the movement direction
            controller.Move(moveDir.normalized * speed * Time.deltaTime); // move the player
        }
        
        
        

        // if the jump button is pressed and the player is grounded
        if( inputManager.PlayerJumpedThisFrame() && isGrounded) 
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // calculate the velocity of the jump
        
        velocity.y += gravity * Time.deltaTime; // add the gravity to the velocity

        controller.Move(velocity * Time.deltaTime); // Jump the player

        
       // transform.Rotate(Vector3.up, inputManager.GetMouseDelta().x * Time.deltaTime * horizontalSensitivity); // rotate the player



        
    }
}
