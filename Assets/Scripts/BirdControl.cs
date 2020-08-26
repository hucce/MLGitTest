using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdControl : MonoBehaviour
{
    public float upForce;                   //Upward force of the "flap".
    public float birdSpeed = 1.5f;

    public bool isDead = false;            //Has the player collided with a wall?
    public bool isClear = false;
    public bool checkColumn = false;

    private Animator anim;                  //Reference to the Animator component.
    private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "goal")
        {
            // Zero out the bird's velocity
            rb2d.velocity = Vector2.zero;
            // If the bird collides with something set it to dead...
            isClear = true;
            //...tell the Animator about it...
            anim.SetTrigger("Die");
            //...and tell the game control about it.
        }
        else
        {
            // Zero out the bird's velocity
            rb2d.velocity = Vector2.zero;
            // If the bird collides with something set it to dead...
            isDead = true;
            //...tell the Animator about it...
            anim.SetTrigger("Die");
            //...and tell the game control about it.
        }
    }

    public void JumpBrid()
    {
        //...tell the animator about it and then...
        anim.SetTrigger("Flap");
        //...zero out the birds current y velocity before...
        rb2d.velocity = Vector2.zero;
        //	new Vector2(rb2d.velocity.x, 0);
        //..giving the bird some upward force.
        rb2d.AddForce(new Vector2(0, upForce));
    }

    public void SetTrigger(string _Idle)
    {
        anim.SetTrigger(_Idle);
    }

    public void ZeroVelocity()
    {
        rb2d.velocity = Vector2.zero;
    }
}