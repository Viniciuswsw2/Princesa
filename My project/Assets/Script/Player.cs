using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private bool isGrounded; // Rename 'pulan' to 'isGrounded'
    
    private Rigidbody2D rig;
    private Animator anim;
    private bool isFire;
    private float movement;

    private bool canFire = true;
   
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput(); // Combine movement and jump handling into a single function
    }

    private void FixedUpdate()
    {
        Move();
    }

    void HandleInput()
    {
        movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        if (movement > 0 && isGrounded)
        {
            anim.SetInteger("Transition", 1);
            transform.eulerAngles = Vector3.zero;
        }
        else if (movement < 0 && isGrounded)
        {
            anim.SetInteger("Transition", 1);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movement == 0 && isGrounded && !isFire)
        {
            anim.SetInteger("Transition", 0);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.F) && isGrounded)
        {
            Attack();
        }
    }

    void Jump()
    {
        anim.SetInteger("Transition", 2);
        rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;
    }

    void Attack()
    {
        anim.SetInteger("Transition", 3);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.layer == 8) // Assuming the ground's layer is 8
        {
            isGrounded = true;
        }
    }
}
