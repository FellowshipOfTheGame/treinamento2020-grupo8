using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    [SerializeField] private float leftCap;
	[SerializeField] private float rightCap;
    [SerializeField] private float jumpLength;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float health; // Stores player's maximum health

    
    private Collider2D coll;
    private Rigidbody2D rb;

    private bool facingLeft = true;

    private void Start()  
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }


     public void TakeDamage(int damage)
    {
        health -= damage;
    }
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject, 0.2f);
        }

        if (facingLeft)
        {
                if(transform.position.x > leftCap)
                {
                    if(transform.localScale.x != 1)
                    {
                        transform.localScale = new Vector3(1, 1);
                    }
                    if(coll.IsTouchingLayers(ground))
                    {
                        rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    }
                }
                else
                {
                    facingLeft = false;
                }
        }
        else
        {
            if(transform.position.x < rightCap)
                {
                    if(transform.localScale.x != -1)
                    {
                        transform.localScale = new Vector3(-1, 1);
                    }
                    if(coll.IsTouchingLayers(ground))
                    {
                        rb.velocity = new Vector2(jumpLength, jumpHeight);
                    }
                }
                else
                {
                    facingLeft = true;
                }
        }
    }

}
