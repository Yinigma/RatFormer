using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class movementManager : MonoBehaviour {

    private const float MINMOVE = 0.0001f;
    private const float SHELLSIZE = 0.01f;
    private const int HITLIMIT = 20;
    public const float CASTDISTANCE = 10;

    [SerializeField]
    private ContactFilter2D contactFilter;
    [SerializeField]
    private MonoBehaviour[] rawMoves;

    private movement[] moves;
    private RaycastHit2D[] cBuff;
    private Vector2 nextMovement;
    private Rigidbody2D objCollider;

    public struct managerState
    {
        public bool hasHit;
        public Vector2 hitNorm;

        public bool HasHit { get { return hasHit; } }
        public Vector2 HitNorm { get { return hitNorm; } }
    }

    private managerState state;

    void Start()
    {
        objCollider = GetComponent<Rigidbody2D>();
        cBuff = new RaycastHit2D[1];
        state.hasHit = false;
        state.hitNorm = Vector2.zero;
        moves = new movement[rawMoves.Length];
        for (int i = 0; i < rawMoves.Length; i++)
        {
            moves[i] = (movement)rawMoves[i];
        }
    }

    void Update()
    {
        nextMovement = Vector2.zero;
        for(int i = 0; i<moves.Length; i++)
        {
            nextMovement += moves[i].nextMovement(state);
        }
        Debug.DrawLine(objCollider.position, objCollider.position + 10*nextMovement, Color.magenta);
        //handleCollisions(ref nextMovement);
        redirect();
        Debug.DrawLine(objCollider.position, objCollider.position + 10*nextMovement, Color.cyan);
        objCollider.position += nextMovement;
        if (state.HasHit)
        {
            Debug.DrawLine(objCollider.position, state.HitNorm + objCollider.position);
        }
    }

    //Given a displacement vector and a collider, this method will change the displacement such that it moves
    //corretly in the environment and return a normal vector in the direction it should be moving
    //Let this be a lesson as to why I should name my variables well and not document like a putz
    public Vector2 handleCollisions(ref Vector2 inMov)
    {
        Vector2 norm = inMov.normalized;
        bool hit = false;
        Vector2 hitNorm = Vector2.zero;

        if (inMov.magnitude > MINMOVE)
        {
            Vector2 movement = inMov;
            Vector2 initPos = objCollider.position;
            Vector2 inShell;
            Vector2 moveNorm;
            Vector2 cMove;
            Vector2 prevNorm = Vector2.zero;

            float dist;

            int numHits;
            int passes = 0;
            

            do
            {
                /*
                * /////////////////////////////////////////////////////////////////
                * RAYCAST INTO THE SCENE ONLY TEST FURTHER IF THE RAY ACTUALLY HITS
                * /////////////////////////////////////////////////////////////////
                */
                numHits = objCollider.Cast(movement, contactFilter, cBuff, CASTDISTANCE);
                cMove = movement;
                moveNorm = movement.normalized;

                //clear until proven to be trying to ram into something
                hit = false;
                state.hasHit = false;

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
                        state.hasHit = true;

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

                            //Kill movement
                            movement = Vector2.zero;
                            norm = (hitNorm + prevNorm).normalized;
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
            while (hit && passes < HITLIMIT && movement.magnitude >= MINMOVE);
            inMov = objCollider.position - initPos;
            objCollider.position = initPos;
        }
        else
        {
            inMov = Vector2.zero;
        }

        state.hitNorm = hitNorm;

        return norm;
    }

    public void redirect()
    {
        state.hasHit = false;
        state.hitNorm = Vector2.zero;

        //Set up the context of the problem
        int numHits = 0;
        Vector2 loopMovement = nextMovement;
        Vector2 initPos = objCollider.position;

        Vector2 prevNorm = Vector2.zero;
        int hitCounter = 0;
        bool collision;

        do //a collision check
        {
            collision = false;
            float moveMag = loopMovement.magnitude;

            if (loopMovement.magnitude >= MINMOVE)
            {
                numHits = objCollider.Cast(loopMovement, contactFilter, cBuff, CASTDISTANCE);

                if (numHits > 0)
                {
                    Vector2 hitNorm = cBuff[0].normal;
                    float hitDistance = cBuff[0].distance;

                    Vector2 moveNorm = loopMovement.normalized;
                    float shelldist = -SHELLSIZE / Vector2.Dot(moveNorm, hitNorm);

                    //This is the actual hit condition. Only register a hit when it's the correct distance
                    if (hitDistance <= moveMag + shelldist)
                    {
                        collision = true;
                        state.hasHit = true;
                        state.hitNorm = hitNorm;
                        float remainingDistance = hitDistance - shelldist;

                        //reset loop movement to what we'll need to test for the next iteration
                        Vector2 tangent = new Vector2(hitNorm.y, -hitNorm.x);
                        loopMovement = (moveMag-remainingDistance)* Vector2.Dot(tangent, moveNorm) * tangent;

                        objCollider.position += moveNorm * remainingDistance;

                        //This is to kill movement in accute corners so it doesn't loop infinitely
                        if (hitCounter > 0 && Vector2.Dot(hitNorm, prevNorm) <= 0)
                        {
                            loopMovement = Vector2.zero;
                            state.hitNorm = (hitNorm + prevNorm).normalized;
                        }

                        prevNorm = hitNorm;
                        hitCounter++;
                    }

                }
            }
            else
            {
                loopMovement = Vector2.zero; 
            }
        }
        while (collision);//the collider is hitting something

        nextMovement = objCollider.position - initPos + loopMovement;
        objCollider.position = initPos;
    }
}
