using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour, ICapturable
{
    [SerializeField] float ground_drag, jumpForce, jumpCooldown, airMultiplier;
    bool readyToJump;
    float buffed_movespeed;
    float normal_movespeed;
    public float moveSpeed = 7;

    [SerializeField] GameObject ui;
    [SerializeField] GameObject Nameui;

    [Header("Stamina System")]
    [SerializeField] StaminaBar StaminaBar;
    float stamina = 100f;
    float currentStamina;
    float staminaCooldown = 1.0f;
    float staminaTimer = 0.0f;
    float stamina_rate = 0.5f;
    float normal_rate = 0.5f;
    float buffed_rate = 0;
    float effectduration = 5f;
    float effecttimer = 0f;
    bool onBuff = false;
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

    [Header("Animation")]
    [SerializeField] Animator animator;

    // [Header("Inventory")]
    // private Inventory inventory;
    // [SerializeField] UI_Inventory uiinventory;

    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    PlayerManager playerManager;

    Rigidbody rb;
    PhotonView PV;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
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
            if (GetComponent<Camera>() != null)
            {
                Destroy(GetComponent<Camera>().gameObject);
            }
            Destroy(rb);
            Destroy(ui);
        }

        rb.freezeRotation = true;
        readyToJump = true;
        running = false;
        moving = false;
        currentStamina = stamina;
        buffed_movespeed = moveSpeed * 2;
        normal_movespeed = moveSpeed;
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
        animator.SetFloat("Running", Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

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
            currentStamina -= stamina_rate;
            resetStaminaCooldown();
        }
        else if (!running && staminaTimer <= 1f)
        {
            staminaTimer += Time.deltaTime;
        }

        if (!running && currentStamina < 100 && staminaTimer >= staminaCooldown)
        {
            currentStamina += stamina_rate;
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

        if (onBuff)
        {
            StartEffectTimer();
        }

        if (grounded)
        {
            animator.SetBool("IsJumping", false);
            rb.drag = ground_drag;
        }
        else
        {
            animator.SetBool("IsJumping", true);
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

    public void Captured()
    {
        playerManager.Die();
    }

    public void AddBuff()
    {
        moveSpeed = buffed_movespeed;
        currentStamina = stamina;
        onBuff = true;
        effecttimer += effectduration;
        stamina_rate = buffed_rate;
    }

    void StartEffectTimer()
    {
        if (effecttimer >= 0)
        {
            effecttimer -= Time.deltaTime;
        }
        else
        {
            onBuff = false;
            stamina_rate = normal_rate;
            moveSpeed = normal_movespeed;
        }
    }
}
