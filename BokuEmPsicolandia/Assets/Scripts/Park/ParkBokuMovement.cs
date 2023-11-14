using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkBokuMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float moveInput;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private string currentAnim = "";
    private ParkActions actions;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        actions = new ParkActions();
        actions.Enable();
    }

    private void OnDestroy()
    {
        actions.Disable();
    }

    private void Update()
    {
        moveInput = actions.Boku.Move.ReadValue<float>();
        if (Mathf.Abs(moveInput) > 0.1)
            SwitchAnimation("Walk");
        else
            SwitchAnimation("Idle");

        if (moveInput > 0.1)
            sprite.flipX = false;
        else if (moveInput < -0.1)
            sprite.flipX = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
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
