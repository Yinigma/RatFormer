using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//T-shirt idea - Skip the part where you call me an idiot. I already know.
public class PhysicsUtil
{


    private const float MINMOVE = 0.0001f;
    private const float SHELLSIZE = 0.01f;
    private const int HITLIMIT = 20;

    //Given a displacement vector and a collider, this method will change the dsiplacement such that it moves
    //corretly in the environment and return a normal vector in the direction it should be moving 
    //Fuck ups are only happening when I'm pressing the keys
    public static Vector2 handleCollisions(ref Vector2 inMov, Rigidbody2D objCollider, ContactFilter2D contactFilter, RaycastHit2D[] cBuff)
    {
       

        Vector2 norm = inMov.normalized;


        if (inMov.magnitude > MINMOVE)
        {
            Vector2 movement = inMov;
            Vector2 initPos = objCollider.position;
            Vector2 inShell;
            Vector2 moveNorm;
            Vector2 hitNorm;
            Vector2 cMove;
            Vector2 prevNorm = Vector2.zero;

            float dist;

            //int totalHits = 0;
            int numHits;
            int passes = 0;
            bool hit;

            do
            {
                /*
                * /////////////////////////////////////////////////////////////////
                * RAYCAST INTO THE SCENE ONLY TEST FURTHER IF THE RAY ACTUALLY HITS
                * /////////////////////////////////////////////////////////////////
                */
                numHits = objCollider.Cast(movement, contactFilter, cBuff);
                hit = false;
                cMove = movement;
                moveNorm = movement.normalized;
                if (numHits > 0)
                {
                    //get data
                    hitNorm = cBuff[0].normal;
                    dist = cBuff[0].distance;

                    //toHit is a vector from the collider to the thing it's casting onto
                    Vector2 toHit = moveNorm * dist;

                    inShell = toHit * SHELLSIZE / Vector2.Dot(toHit, -hitNorm);

                    /*
                     * /////////////////////////////////////////////////////////////////////
                     * TEST TO SEE IF COLLIDER IS CLOSE ENOUGH FOR IT TO BE CONSIDERED A HIT
                     * /////////////////////////////////////////////////////////////////////
                     */
                    if (dist <= movement.magnitude + inShell.magnitude)
                    {

                        //We need to remember this
                        hit = true;

                        //subtract
                        toHit -= inShell;

                        //Vector perpendicular to the surface we're hitting.
                        Vector2 deflect;

                        deflect.x = hitNorm.y;
                        deflect.y = -hitNorm.x;

                        deflect = deflect * Vector2.Dot(deflect, movement - toHit);

                        movement = deflect;

                        cMove = toHit;

                        if (passes > 0 && Vector2.Dot(hitNorm, prevNorm) <= 0)
                        {
                            Debug.Log("Acute corner");

                            //Kill movement
                            movement = Vector2.zero;
                            norm = (hitNorm+prevNorm).normalized;
                            objCollider.position += cMove;
                            passes++;
                            break;
                        }
                        prevNorm = hitNorm;
                    }
                }

                //get the displacement and the normal ready
                norm = movement.normalized;
                objCollider.position += cMove;
                passes++;
            }
            while (hit && passes < HITLIMIT && movement.magnitude>=MINMOVE);
            inMov = objCollider.position - initPos;
            objCollider.position = initPos;
        }
        else
        {
            inMov = Vector2.zero;
        }
        
        return norm;
    }

    public static Vector2 CorrectMovement(ref Vector2 vel, Rigidbody2D rb2d, ContactFilter2D contactFilter)
    {
        if (vel.magnitude > 0)
        {
            RaycastHit2D[] cBuff = new RaycastHit2D[1];
            Vector2 curMove = vel * Time.deltaTime;
            Vector2 velNorm = handleCollisions(ref curMove, rb2d, contactFilter, cBuff);
            if (curMove == Vector2.zero)
            {
                vel = Vector2.zero;
                return curMove;
            }
            vel = Vector2.Dot(vel, velNorm) * velNorm;
            Debug.DrawLine(rb2d.position, rb2d.position + vel, Color.cyan);
            return curMove;
        }
        return Vector2.zero;
    }
}