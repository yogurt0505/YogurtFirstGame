using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Final_Movement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Collider2D coll;
    private Animator anim;

    public float speed, jumpForce;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump;

    bool jumpPressed;
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, ground);

        GroundMovement();
        Jump();
        SwitchAnim();
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

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }

        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }

        else if(jumpPressed && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }

void SwitchAnim()
{
    anim.SetFloat("Running", Mathf.Abs(rb.velocity.x));

    if (isGround)
    {
        anim.SetBool("Falling", false);
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
   
}
