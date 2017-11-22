using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    

    //The highest the player can jump
    public float maxJump = 1.0f;
    //The amount of time it will take them to reach that height
    public float maxTime = 1.0f;
    //The minimum height the player is committed to after hitting the jump button
    public float minJump = 0.2f;
    //
    public float maxSpeed = 3.0f;
    private float gravity;
    private float jumpSpeed;
    private Vector2 vel;
    private Vector2 curMove;

    //Buffers and things for collisions
    //I want to be able to set this from the editor for now
    public ContactFilter2D contactFilter;
    private RaycastHit2D[] cBuff;
    private Rigidbody2D rb2d;
    private bool grounded;

    void onEnable() {
        //rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
        cBuff = new RaycastHit2D[1];
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;

        //Set up movement vars
        //Why are these not constants? Because maybe I'd like to change them later, Jenkins. Sheesh.
        gravity = 2 * maxJump / (maxTime * maxTime);
        //Fuck man, dudes on the internet are dumb
        jumpSpeed = 2*maxJump/maxTime;
        rb2d = GetComponent<Rigidbody2D>();
        grounded = false;
        curMove = Vector2.zero;
        vel = Vector2.zero;
	}
	
    void Update() {
        ComputeVelocity();
    }

	//Use fixed update for physics because daddy unity told me so
	void FixedUpdate () {
        Move();
        rb2d.position += (curMove);
    }

    void ComputeVelocity()
    {
        vel.x += Input.GetAxis("Horizontal");
        vel.y += Input.GetAxis("Vertical");
        if (vel.magnitude > maxSpeed)
        {
            vel = vel.normalized * maxSpeed;
        }
    }

    void Move()
    {
        if (vel.magnitude > 0)
        {
            curMove = vel * Time.deltaTime;
            Vector2 velNorm =  PhysicsUtil.handleCollisions(ref curMove, rb2d, contactFilter, cBuff);
            if (curMove==Vector2.zero)
            {
                vel = Vector2.zero;
                return;
            }
            vel = Vector2.Dot(vel, velNorm) * velNorm;
            Debug.DrawLine(rb2d.position, rb2d.position + vel, Color.cyan);
        }
    }

}
