using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    //Comps
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    AudioSourceManager asm;

    //Movement
    public float speed;
    public float jumpForce;

    //GroundCheck
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public float groundCheckRadius;
    public Transform groundCheck;

    //variables

    //Audio Source
    public AudioClip jumpSound;
    public AudioClip jumpAttack;
    public AudioClip squish;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        asm = GetComponent<AudioSourceManager>();

        if (speed <= 0)
        {
            speed = 6.0f;
        }

        if (jumpForce <= 0)
        {
            jumpForce = 400;
        }

        if (!groundCheck)
        {
            groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused == false)
        {
            float hInput = Input.GetAxisRaw("Horizontal");
            float fireInput = Input.GetAxisRaw("Fire1");
            float vInput = Input.GetAxisRaw("Vertical");

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * jumpForce);
                asm.PlayOneShot(jumpSound, false);
            }

            if (!isGrounded && Input.GetButtonDown("Jump"))
                asm.PlayOneShot(jumpAttack, false);

            Vector2 moveDirection = new Vector2(hInput * speed, rb.velocity.y);
            rb.velocity = moveDirection;

            if (hInput != 0)
                sr.flipX = (hInput < 0);


            anim.SetFloat("hInput", Mathf.Abs(hInput));
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("fireInput", fireInput);
            anim.SetFloat("vInput", vInput);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish"))
        {
            collision.gameObject.GetComponentInParent<Goomba>().Squish();
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
            asm.PlayOneShot(squish, false);
        }

        if (collision.CompareTag("Checkpoint"))
        {
            GameManager.instance.currentLevel.UpdateCheckpoint(collision.gameObject.transform);
        }

        if (collision.CompareTag("DeathZone"))
        {
            GameManager.instance.lives--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            GameManager.instance.lives--;
        }
    }
}
