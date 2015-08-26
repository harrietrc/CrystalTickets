﻿using UnityEngine;
using System.Collections;

// Replace this; it was just for testing the platforms.
// See https://www.youtube.com/watch?v=Xnyb2f6Qqzg
public class TestPlayerController : MonoBehaviour {

    private Animator animator;
    private bool isFacingRight;

    //This will be our maximum speed as we will always be multiplying by 1
    public float maxSpeed;
    //to check ground and to have a jumpforce we can change in the editor
    bool grounded = false;
    public Transform groundCheck;
    public float groundRadius = 0.1f;

    // Should be set to Ground layer - put anything that you want to treat as ground on this layer
    public LayerMask whatIsGround;

    public float jumpForce = 700f;
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isFacingRight = true;
    }

    void FixedUpdate() {
        //set our grounded bool
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

        float move = Input.GetAxis("Horizontal");//Gives us of one if we are moving via the arrow keys
                                                 //move our Players rigidbody
        rigidBody.velocity = new Vector3(move * maxSpeed, rigidBody.velocity.y);
    }

    void Update() {

        //if we are on the ground and the space bar was pressed, change our ground state and add an upward force
        if (grounded) {

            bool isMovingDown = rigidBody.velocity.y < 0;

            if (Input.GetKeyDown(KeyCode.Space)) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
                // Updates the animations to show jumping
                animator.SetBool("Jump", true);
            } else if (isMovingDown) {
                // Stop the 'jump' state if the player's about to hit the ground
                animator.SetBool("Jump", false);
            }
        }

        /* Putting the animation transitions here seems to make it more responsive than when it's in FixedUpdate(),
           but this could use a bit more testing. */

        float move = Input.GetAxis("Horizontal");

        // Update the animations to show running if the player is moving
        bool isRunning = move == 0 ? false : true;
        animator.SetBool("Run", isRunning);

        // Update animations to reflect which way the player is moving
        bool changedDirection = move > 0 && !isFacingRight || move < 0 && isFacingRight;
        if (changedDirection)
            Flip();

        // Shooting - doesn't work if you just set 'Shoot' to the value of Input.GetKeyDown(KeyCode.Q) (hence second condition)
        if (Input.GetKeyDown(KeyCode.Q))
            animator.SetBool("Shoot", true);
        if (Input.GetKeyUp(KeyCode.Q))
            animator.SetBool("Shoot", false);
    }

    void Flip() {
        isFacingRight = !isFacingRight;
        Vector3 flippedScale = transform.localScale;
        flippedScale.x *= -1;
        transform.localScale = flippedScale;
    }
}