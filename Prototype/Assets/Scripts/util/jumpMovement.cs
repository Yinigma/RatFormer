using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(fallMovement))]
public class jumpMovement : MonoBehaviour, movement {

    [SerializeField]
    private int numJumps = 1;
    [SerializeField]
    private int jumpFrames = 30;
    [SerializeField]
    private float jumpHeight = 0.8f;
    private int remainingFrames;
    private int remainingJumps;
    private float jumpPerFrame;
    private bool isJumping;
    private bool prevJump;
    private Vector2 nextMove;
    private fallMovement faller;


    void Start()
    {
        isJumping = false;
        prevJump = false;
        jumpPerFrame = jumpHeight / jumpFrames;
        remainingJumps = numJumps;
        remainingFrames = jumpFrames;
        nextMove = Vector2.zero;
        faller = GetComponent<fallMovement>();
    }

    public Vector2 nextMovement(movementManager.managerState state)
    {
        nextMove = Vector2.zero;
        if (isJumping)
        {
            if (remainingJumps > 0)
            {
                if (remainingFrames > 0)
                {
                    remainingFrames--;
                    nextMove += Vector2.up * jumpPerFrame;
                }
                //A new jump
                if (!prevJump)
                {
                    remainingFrames = jumpFrames;
                    remainingJumps--;
                }
            }
            isJumping = false;
        }
        prevJump = isJumping;
        if (faller.grounded(state))
        {
            //Debug.Log("Jumper is grounded");
            remainingJumps = numJumps;
        }
        return nextMove;
    }

    public void doJump()
    {
        isJumping = true;
    }

    public string type()
    {
        return "Jump";
    }

    public int NumJumps { get { return numJumps; } }

}
