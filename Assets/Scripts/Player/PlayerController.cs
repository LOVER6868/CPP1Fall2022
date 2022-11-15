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

    //Movement
    public float speed;
    public float jumpForce;

    //GroundCheck
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public float groundCheckRadius;
    public Transform groundCheck;

    //variables
    public int maxLives = 99;
    private int _lives = 5;

    public int lives
    {
        get { return _lives; }
        set
        {
            //if (_lives > value)
                //live lost

            _lives = value;

            if (_lives > maxLives)
                _lives = maxLives;

            //if (_lives < 0)
                //gameover
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (speed <=0)
        {
            speed = 6.0f;
        }

        if (jumpForce <= 0)
        {
            jumpForce = 300;
        }

        if (!groundCheck)
        {
            groundCheck = GameObject.FindGameObjectWithTag("GroundCheck").transform;
        }

        if (groundCheckRadius <=0)
        {
            groundCheckRadius = 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float fireInput = Input.GetAxisRaw("Fire1");
        float vInput = Input.GetAxisRaw("Vertical");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce);
        }

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
