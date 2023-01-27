using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{

    private Rigidbody2D rb;
    //private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftPoint, rightPoint;
    private float leftx, rightx;

    public float Speed, JumpForce;
    private bool FaceLeft = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        transform.DetachChildren();
        leftx = leftPoint.position.x;
        rightx = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if (FaceLeft)
        {
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                FaceLeft = false;
            }
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("Jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
            }

        }
        else
        {

            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                FaceLeft = true;
            }
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("Jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
            }

        }
    }

    void SwitchAnim()
    {
        if (anim.GetBool("Jumping"))
        {
            if (rb.velocity.y < 0.1)
            {
                anim.SetBool("Jumping", false);
                anim.SetBool("Falling", true);
            }
        }

        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
        }
    }

}

