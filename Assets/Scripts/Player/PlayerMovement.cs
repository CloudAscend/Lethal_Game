using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float stamina;
    float maxStamina;
    public Image staminaBar;
    public float descountValue;
    public float addValue;

    private float moveSpeed; // 이동 속도
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    public KeyCode jumpkey = KeyCode.Space;
    public KeyCode SprintKey = KeyCode.LeftShift;

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
        air
    }

    private void Start()
    {
        maxStamina = stamina;
        staminaBar.fillAmount = maxStamina;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
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

        if(Input.GetKey(jumpkey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ReseJump), jumpCooldown);
        }
    }

    private void MovePlayer() // 움직임
    {
        // 바라보는 방향 움직이기
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded) // 바닥일때
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded) // 공중일때
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }

    private void StateHandler()
    {
        if (grounded && Input.GetKey(SprintKey) && stamina > 0)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            DecreaseStamina();
        }
        else if(grounded)
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
}
