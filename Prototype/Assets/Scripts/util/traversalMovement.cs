using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class traversalMovement : MonoBehaviour, movement {

    [SerializeField]
    private float maximumDisplacement = 0.002f;
    [SerializeField]
    private bool fullRange = false;
    private float intensityX;
    private float intensityY;
    
    void Start()
    {
        intensityX = 0.0f;
        intensityY = 0.0f;
    }

    public Vector2 nextMovement(movementManager.managerState state)
    {
        if (fullRange)
        {
            return new Vector2(intensityX, intensityY) * maximumDisplacement;
        }
        return intensityX * maximumDisplacement * Vector2.right ;
    }

    public string type()
    {
        return "Traversal";
    }

    public void moveX(float intensity)
    {
        intensityX = Mathf.Clamp(intensity, -1.0f, 1.0f);
    }

    public void moveY(float intensity)
    {
        if (fullRange)
        {
            intensityY = Mathf.Clamp(intensity, -1.0f, 1.0f);
        }
    }
}
