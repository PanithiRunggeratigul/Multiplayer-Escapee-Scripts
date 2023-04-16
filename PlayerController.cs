using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed, ground_drag, jumpForce, jumpCooldown, airMultiplier;
    bool readyToJump;

    [SerializeField] GameObject ui;
    [SerializeField] GameObject Nameui;

    [Header("Stamina System")]
    [SerializeField] StaminaBar StaminaBar;
    float stamina = 100f;
    float currentStamina;
    float staminaCooldown = 1.0f;
    float staminaTimer = 0.0f;
    bool running;
    bool moving;
    Vector3 prev_pos;
    Vector3 curr_pos;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprint = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask Ground;
    bool grounded;

    [Header("Inventory")]
    // private Inventory inventory;
    // [SerializeField] UI_Inventory uiinventory;

    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    PhotonView PV;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine)
        {
            
        }
        else if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);
        }

        rb.freezeRotation = true;
        readyToJump = true;
        running = false;
        moving = false;
        currentStamina = stamina;
        // inventory = new Inventory();
        // uiinventory.setInventory(inventory);
    }

    private void MyInput()
    {
        // handle movement
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // handle jump and prevent player to spam jump
        if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(resetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // movement direction need to match with orientation, handle z and x axis movement together
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sprint and move
        running = false;
        if (grounded)
        {
            if (Input.GetKey(sprint) && currentStamina >= 0 && moving)
            {
                running = true;
                rb.AddForce(moveDirection.normalized * moveSpeed * 100f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            curr_pos = transform.position;
        }
        // jump
        else if (!grounded)
        {
            if (Input.GetKey(sprint) && currentStamina >= 0 && moving)
            {
                running = true;
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            }
            curr_pos = transform.position;
        }
    }


    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (running)
        {
            rb.AddForce(transform.up * jumpForce + moveDirection * 10f, ForceMode.Impulse);
        }
        else if (!running)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void resetJump()
    {
        readyToJump = true;
    }

    private void handleStamina()
    {
        StaminaBar.setStamina(currentStamina);
        if (running && moving)
        {
            currentStamina -= 0.5f;
            resetStaminaCooldown();
        }
        else if (!running && staminaTimer <= 1f)
        {
            staminaTimer += Time.deltaTime;
        }

        if (!running && currentStamina < 100 && staminaTimer >= staminaCooldown)
        {
            currentStamina += 0.5f;
        }
    }

    private void resetStaminaCooldown()
    {
        staminaTimer = 0f;
        staminaCooldown = 1f;
    }

    private void checkMoving()
    {
        moving = false;
        if (prev_pos != curr_pos)
        {
            moving = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }

        prev_pos = transform.position;
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);
        MyInput();
        speedControl();
        handleStamina();
        checkMoving();

        if (grounded)
        {
            rb.drag = ground_drag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            return;
        }
        MovePlayer();
    }
}
