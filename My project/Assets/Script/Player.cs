using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public int health = 3;

    private bool isJumping;
    private bool porta;
    private GameObject Portall;
 

    private Rigidbody2D rig;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        Portall = GameObject.Find("Portall");
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        GameController.instance.UpdadeLives(health);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        novaPosicao();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);
        
        if (movement > 0)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        } 
        
        if (movement < 0)
        {
            if (!isJumping )
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if(!isJumping)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0,jumpForce), ForceMode2D.Impulse);
                isJumping = true;
            }
        }
     }
    public void Damage(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdadeLives(health);
        anim.SetTrigger("hit");
        
        if (transform.rotation.y == 0)
        {
            rig.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
        }
        if (transform.rotation.y == 180)
        {
            rig.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
        }
        if (health <= 0)
        {
            
        }
    }
    public void IncreaseLife(int value)
    {
        health += value;
        GameController.instance.UpdadeLives(health);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            rig.velocity = Vector2.zero;
            rig.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
            col.gameObject.GetComponent<Enemy>().enabled = false;
            col.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            col.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            col.gameObject.GetComponent<Animator>().SetBool("dead", true);
            col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(col.gameObject, 1f);
        }
        if (col.gameObject.layer == 8)
        {
            isJumping = false;
        }
            
        if(col.gameObject.CompareTag("next"))
        {
            porta = true;
        }
    }
    
    private void novaPosicao()
    {
        if(porta == true)
        {
            anim.transform.position = new Vector2(Portall.transform.position.x, Portall.transform.position.y);
            porta = false;
        }
    }
}
