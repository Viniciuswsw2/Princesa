using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float ataqueRanger = 0.5f;
    public LayerMask enemyLayers;
    public int health = 3;
    public Transform ataquePoint;
    public Text chaveText;
    private int Chave;
    private AudioSource sound;

    private bool isFire;
    private bool isJumping;
    private bool porta;
    private GameObject Portall;
 

    private Rigidbody2D rig;
    public Animator anim;
    
    void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    void Start()
    {
        Portall = GameObject.Find("Portall");
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        GameController.instance.UpdadeLives(health);
        Chave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        novaPosicao();
        BowFire();
        
        chaveText.text = Chave.ToString();
    }

    void Move()
    {
        float movement = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);
        
        if (movement > 0 && !isFire)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
        } 
        
        if (movement < 0 && !isFire)
        {
            if (!isJumping )
            {
                anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (movement == 0 && !isJumping && !isFire)
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
                sound.Play();
                
            }
        }
    }
    void BowFire()
    {
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(!isFire)
            {
                isFire = true;
                anim.SetInteger("transition", 3);
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ataquePoint.position, ataqueRanger, enemyLayers);
                
                foreach (Collider2D enemy in hitEnemies)
                {
                    enemy.GetComponent<Enemy>().Damage(1);
                }
                yield return new WaitForSeconds(0.5f);
                anim.SetInteger("transition", 0);
                isFire = false;
            }
            
        }
    }
    public void Damage(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdadeLives(health);
        anim.SetTrigger("hit");

        if (health <= 0)
        {
            GameController.instance.GameOver();
        }
    }
    public void IncreaseLife(int value)
    {
        health += value;
        GameController.instance.UpdadeLives(health);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 8)
        {
            isJumping = false;
        }
    
        if (col.gameObject.layer == 10)
        {
            GameController.instance.GameOver();
        }
        if (col.gameObject.layer == 11)
        {
            GameController.instance.SairGame();
        }
            
        if(col.gameObject.CompareTag("next"))
        {
            porta = true;
        }

        if(col.gameObject.CompareTag("Chave"))
        {
            Chave = Chave + 1;
            Destroy(col.gameObject);
        }
    }
    
    private void novaPosicao()
    {
        if(Chave == 1)
        {
            if(porta == true)
            {
                anim.transform.position = new Vector2(Portall.transform.position.x, Portall.transform.position.y);
                porta = false;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(ataquePoint.position, ataqueRanger);
    }
}