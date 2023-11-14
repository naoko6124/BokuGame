using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpKingBokuMovement : MonoBehaviour
{
    [Header("Jump")]
    public float jumpSpeed;
    public float maxJumpTime;
    private Vector2 jumpDirection;
    private float jumpTimer = 0f;
    private bool startJumping = false;
    [SerializeField] private bool isGrounded = false;

    [Header("Physics")]
    public PhysicsMaterial2D bouncy;
    public PhysicsMaterial2D noBouncy;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private string currentAnim = "";
    private JumpKingActions actions;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        actions = new JumpKingActions();
        actions.Enable();

        actions.Boku.Jump.started += StartJump;
        actions.Boku.Jump.canceled += FinishJump;
    }

    private void OnDestroy()
    {
        actions.Disable();
    }

    private void Update()
    {
        float horizontal = actions.Boku.Direction.ReadValue<float>();
        jumpDirection = new Vector2(horizontal, 1.7f);

        isGrounded = rb.velocity.y == 0f;

        if (startJumping)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer > maxJumpTime) jumpTimer = maxJumpTime;
        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y > 0f)
        {
            rb.gravityScale = 3.0f;
            rb.sharedMaterial = bouncy;
            SwitchAnimation("JumpUp");
        }
        else if (rb.velocity.y < 0f)
        {
            rb.gravityScale = 5.0f;
            rb.sharedMaterial = noBouncy;
            SwitchAnimation("JumpDown");
        }
        else if (!startJumping)
            SwitchAnimation("Idle");
        if (rb.velocity.x > 0f)
            sprite.flipX = false;
        if (rb.velocity.x < 0f)
            sprite.flipX = true;
    }

    private void StartJump(InputAction.CallbackContext context)
    {
        if (isGrounded && !startJumping)
        {
            SwitchAnimation("JumpHold");
            jumpTimer = 0f;
            startJumping = true;
        }
    }
    private void FinishJump(InputAction.CallbackContext context)
    {
        if (isGrounded && startJumping)
        {
            startJumping = false;
            rb.gravityScale = 3.0f;
            rb.AddForce(jumpDirection * jumpSpeed * jumpTimer, ForceMode2D.Impulse);
        }
    }

    private void SwitchAnimation(string name)
    {
        if (currentAnim != name)
        {
            currentAnim = name;
            anim.Play(name);
        }
    }
}
