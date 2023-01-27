using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    //Player
    public Collider2D coll;
    public Collider2D disColl;
    public Transform ceilingCheck, groundCheck;
    public Joystick joystick;
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public int Cherry;
    private bool isHurt; //default = false
    public bool isGround, isJump;
    bool jumpPressed;
    int jumpCount;
    //UI
    public Text CherryNum;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Jump();
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        CherryNum.text = Cherry.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt)
        {
            GroundMovement();
        }
        Jump();

        Crouch();
        SwitchAnim();
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);

// #region Run
//         float targetSpeed = moveInput * moveSpeed;
//         float speedDif = targetSpeed - rb.velocity.x;
//         float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
//         float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        
//         rb.AddForce(movement * Vector2.right);
//         #endregion
        
        }


    void GroundMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    // void Movement()
    // {
    //     // 1 = move left, -1 = move right

    //     // float horizontalMove = joystick.Horizontal;
    //     // float faceDirection = joystick.Horizontal;
    //     float horizontalMove = Input.GetAxis("Horizontal");
    //     float faceDirection = Input.GetAxisRaw("Horizontal");

    //     float verticalMove = Input.GetAxis("Vertical");

    //     //Movement Action
    //     if (horizontalMove != 0)
    //     {
    //         rb.velocity = new Vector2(horizontalMove * speed * Time.fixedDeltaTime, rb.velocity.y);
    //         anim.SetFloat("Running", Mathf.Abs(faceDirection));
    //     }
    //     if (faceDirection != 0)
    //     {
    //         transform.localScale = new Vector3(faceDirection, 1, 1);
    //     }

    //     // if (faceDirection > 0f)
    //     // {
    //     //     transform.localScale = new Vector3(1, 1, 1);
    //     // }

    //     // if (faceDirection <0f)
    //     // {
    //     //     transform.localScale = new Vector3(-1, 1, 1);
    //     // }


    // }

    void SwitchAnim()
    {
        anim.SetFloat("Running", Mathf.Abs(rb.velocity.x));
        anim.SetBool("Idle", false);

        if (!isHurt)
        {
            if (isGround)
            {
                anim.SetBool("Falling", false);
                anim.SetBool("Idle", true);
            }
            else if (!isGround && rb.velocity.y > 0)
            {
                anim.SetBool("Jumping", true);
            }
            else if (rb.velocity.y < 0)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }
        else
        {

            anim.SetBool("Hurt", true);

            anim.SetBool("Jumping", false);
            anim.SetBool("Idle", false);

            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                isHurt = false;
                anim.SetBool("Hurt", false);
                anim.SetBool("Idle", true);
            }
        }

    }
    //Collect Item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {
            //Destroy(collision.gameObject);
            SoundManager.instance.CherryAudio();
            collision.GetComponent<Animator>().Play("Item_Collected");
            //Cherry++;
            //CherryNum.text = Cherry.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            //GetComponent<AudioSource>().enabled = false;
            Invoke("Restart", 2f);
        }
    }

    public void CherryCount()
    {
        Cherry++;
    }
    //Destroy Enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (anim.GetBool("Falling"))
            {
                enemy.JumpOn();
                rb.velocity = Vector2.up * jumpForce;
                anim.SetBool("Jumping", true);
                isGround = true;
            }

            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-3, rb.velocity.y);
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }

            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(3, rb.velocity.y);
                SoundManager.instance.HurtAudio();
                isHurt = true;
            }
        }
    }

    void Crouch()
    {
        if (!Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, ground))
        {
            if (Input.GetButton("Crouch")) //|| joystick.Vertical < -0.5f
            {
                anim.SetBool("Crouching", true);
                disColl.enabled = false;
            }

            else
            {
                anim.SetBool("Crouching", false);
                disColl.enabled = true;
            }
        }
    }

    /*void Jump()
    {
        if (Input.GetButton("Jump") || joystick.Vertical > 0.5f)
        {
            if (anim.GetBool("ReadyJump") == false)
            { }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
                jumpAudio.Play();
                anim.SetBool("Jumping", true);
            }
        }
    }*/

    //Double Jump Function
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 1;
            isJump = false;
        }

        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            SoundManager.instance.JumpAudio();
        }

        else if (jumpPressed && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            SoundManager.instance.JumpAudio();
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
