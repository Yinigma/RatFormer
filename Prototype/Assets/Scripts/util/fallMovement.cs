using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallMovement : MonoBehaviour, movement {
    [SerializeField]
    private float fallAcceleration = 0.01f;
    [SerializeField]
    private float groundDot = 0.59f;
    [SerializeField]
    private float terminalDisplactment = 0.8f;

    private float fallDisplacement;

    void Start()
    {
        fallDisplacement = 0.0f;
    }

    public Vector2 nextMovement(movementManager.managerState state)
    {
        if (grounded(state))
        {
            fallDisplacement = 0.0f;
        }
        fallDisplacement += fallAcceleration;
        fallDisplacement = (fallDisplacement >= terminalDisplactment) ? terminalDisplactment: fallDisplacement;
        return fallDisplacement * Vector2.down;
    }

    public bool grounded(movementManager.managerState state)
    {
        return state.HasHit && Vector2.Dot(state.HitNorm, Vector2.up) >= groundDot;
    }

    public string type()
    {
        return "Fall";
    }
}
