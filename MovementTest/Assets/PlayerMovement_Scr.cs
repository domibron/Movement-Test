using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using TMPro;

public class PlayerMovement_Scr : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] bool DebugMode = false;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float airMultiplier = 1f;
    [SerializeField] float movementMultiplier = 10f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space; //can set as null and use a keybind manager
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag and gravity")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 0f;

    [SerializeField] float gravity;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    bool isCrouchGrounded;
    float groundDistance = 0.4f;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    Vector3 jumpMoveDirection;

    Rigidbody rb;

    bool isCrouching = false;

    RaycastHit slopeHit;

    //when called it will send a raycast out and return is true if the vector does not return stright up
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void Start()
    {
        //rigid body settings are set as well as varibles and the script get the rigidbody
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.drag = 0f;
    }


    private void Update()
    {
        //is grounded check
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0f, playerHeight / 2f, 0f), groundDistance, groundMask);
        

        //runs myinput funtion and controll drag (more over this farther down)
        MyInput();
        ControllDrag();

        //allows the player to jump if they are on the ground and presses the jump key
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.C) && (isGrounded || isCrouchGrounded))
        {
            Crouch();
        }
        else
        {
            Stand();
        }

            //sets the direction following the slope
            slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        //gravity for in air
        gravity = Time.deltaTime * -1f;

        //moive direction so the player can control inair movement
        jumpMoveDirection = moveDirection * 0.1f;

        //shows all changeable values for debbuging
        if (DebugMode)
        {
            Debug.DrawRay(transform.position, Vector3.down, color: Color.red, playerHeight / 2 + 0.5f);

            Debug.Log("moveDirection " + moveDirection + " jumpMoveDirection " + jumpMoveDirection 
            + " horizontalMovement " + horizontalMovement + " verticalMovement " + verticalMovement 
            + " isGrounded " + isGrounded + " OnSlope() " + OnSlope() + " slopeMoveDirection " + slopeMoveDirection 
            + " gravity " + gravity + " Time.deltaTime " + Time.deltaTime + " playerHeight " + playerHeight 
            + " groundDistance " + groundDistance + " isCrouching " + isCrouching);
        }

        //gets the player's height by getting the scale of the Rigidbody and timesing by 2 as defult is 1
        playerHeight = rb.transform.localScale.y * 2;
        groundDistance = rb.transform.localScale.y * 2 / 5;
    }

    //this draws the sphere check for debbuging
    private void OnDrawGizmos()
    {
        if (DebugMode) { Gizmos.DrawSphere(transform.position - new Vector3(0f, playerHeight / 2f, 0f), groundDistance); }
    }

    //when called it will grab movement input
    void MyInput()
    {
        //grabs key inputs
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        //combines both inputs into one direction
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }

    //when called then the player will jump in the air
    void Jump()
    {

        //sets the jump direction
        jumpMoveDirection = moveDirection * 0.1f;
        //add a jump force to the rigid body component.
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void Crouch()
    {
        isCrouchGrounded = (Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.5f, groundMask));
        Debug.DrawRay(transform.position, Vector3.down, color: Color.blue, playerHeight / 2 + 0.5f);
        isCrouching = true;
        //sets to crouch size
        Vector3 crouchSize = new Vector3(0.7f, 0.5f, 0.7f); // had a problem here CS0165 so I added a vaule be for it is set
        rb.transform.localScale = crouchSize;
    }

    void Stand()
    {
        isCrouching = false;
        //sets size back to normal
        Vector3 standSize = Vector3.one;
        rb.transform.localScale = standSize;
    }

    //when called then drag will be controlled
    void ControllDrag()
    {
        //drag is controlled if in air or ground so the player is not slow falling
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        //calls the Move player funtion
        MovePlayer();
    }

    //when called then the rigid body will get force applyed
    void MovePlayer()
    {
        //if on the ground but not on a slope
        if (isGrounded && !OnSlope())
        {
            //if left shift is held down
            if (Input.GetKey(sprintKey) && !isCrouching)
            {
                //sprint speed on flat ground
                rb.AddForce(moveDirection.normalized * sprintSpeed * movementMultiplier, ForceMode.Acceleration);
            }
            else if (isCrouching)
            {
                rb.AddForce(moveDirection.normalized * crouchSpeed * movementMultiplier, ForceMode.Acceleration);
            }
            else
            {
                //walk speed on flat ground
                rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            }
        }

        //if the player is on the ground and on a slope
        else if (isGrounded && OnSlope())
        {
            //if left shift is held down
            if (Input.GetKey(sprintKey) && !isCrouching)
            {
                //sprint speed on slope
                rb.AddForce(slopeMoveDirection.normalized * sprintSpeed * movementMultiplier, ForceMode.Acceleration);
            }
            else if (isCrouching)
            {
                rb.AddForce(slopeMoveDirection.normalized * crouchSpeed * movementMultiplier, ForceMode.Acceleration);
            }
            else
            {
                //walk speed on slope
                rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
            }
        }

        //if the player is not on the ground
        else if (!isGrounded)
        {
            //jumping in mid air force with a downwards force
            rb.AddForce(jumpMoveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Acceleration);
            rb.AddForce(0, gravity * 17f, 0, ForceMode.Acceleration);
        }
    }
}
