using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MK2Movement : MonoBehaviour
{
    [SerializeField] Transform groundCheckLocation;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    CharacterController cc;
    [SerializeField] float speed = 10f;

    [SerializeField] float gravity = -9.81f;

    Vector3 moveDirection;
    Vector3 velocity;

    bool isGrounded;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheckLocation.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        moveDirection = transform.right * x + transform.forward * z;

        cc.Move(moveDirection.normalized * speed * Time.deltaTime);

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(3f * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetAxis("Vertical") > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            speed = Mathf.Lerp(speed, 20f, 5f * Time.deltaTime);
        }
        else
        {
            speed = Mathf.Lerp(speed, 10f, 5f * Time.deltaTime);
        }

        if (isGrounded && Input.GetKey(KeyCode.C))
        {
            groundDistance = 1f;
            cc.transform.localScale = new Vector3(1f, 0.6f, 1f);
        }
        else
        {
            groundDistance = 0.4f;
            cc.transform.localScale = Vector3.one;
        }
        

    }
}
