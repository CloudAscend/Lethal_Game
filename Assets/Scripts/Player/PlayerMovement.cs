using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed; // 이동 속도
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    public float crouchSpeed;
    public float crouchYscale;
    private float startYscale;
    private bool crouch;

    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public float stamina;
    float maxStamina;
    public Image staminaBar;
    public float descountValue;
    public float addValue;

    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        maxStamina = stamina;
        staminaBar.fillAmount = maxStamina;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        crouch = false;

        startYscale = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        staminaBar.fillAmount = stamina;

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpkey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ReseJump), jumpCooldown);
        }


        //if (Input.GetKeyDown(crouchKey))
        //{
        //    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        //}
        //if (Input.GetKey(crouchKey))
        //{
        //    transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
        //}
        //if (Input.GetKeyUp(crouchKey))
        //{
        //    transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        //}

        if (Input.GetKeyDown(crouchKey) && !crouch)
        {
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
            crouch = true;
        }
        else if (Input.GetKeyDown(crouchKey) && crouch)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
            crouch = false;
        }
    }

    private void MovePlayer() // 움직임
    {
        // 바라보는 방향 움직이기
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded) // 바닥일때
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded) // 공중일때
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }

    private void StateHandler()
    {
        //if (Input.GetKey(crouchKey))
        //{
        //    state = MovementState.crouching;
        //    moveSpeed = crouchSpeed;
        //}

        if (crouch && Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        else if (crouch && grounded && Input.GetKey(sprintKey) && stamina > 0)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            DecreaseStamina();
            crouch = false;
            transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
        }
        else if (grounded && Input.GetKey(sprintKey) && stamina > 0)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            DecreaseStamina();
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            IncreaseStamina();
        }
        else
        {
            state = MovementState.air;
        }
    }

    private void DecreaseStamina()
    {
        if (stamina != 0)
        {
            stamina -= descountValue * Time.deltaTime;
        }
    }

    private void IncreaseStamina()
    {
        if(stamina < 1)
        {
            stamina += addValue * Time.deltaTime;
        }
        else
        {
            return;
        }
    }

    private void SpeedControl() // 속도 제한 주기
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ReseJump()
    {
        readyToJump = true;
    }

    public void SpendStamina(float value)
    {
        stamina -= value;
    }
}
