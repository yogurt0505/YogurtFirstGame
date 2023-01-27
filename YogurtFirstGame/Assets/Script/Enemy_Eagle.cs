using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{

    private Rigidbody2D rb;
    private Collider2D coll;
    public Transform upPoint, downPoint;
    private float upy, downy;
    private bool isUp;

    public float Speed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        upy = upPoint.position.y;
        downy = downPoint.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, Speed);
            if (transform.position.y > upy)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -Speed);
            if (transform.position.y < downy)
            {
                isUp = true;
            }
        }
    }
}
