using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class walkAction : action {

    [SerializeField]
    private float terminalSpeed;
    private Rigidbody2D rb;

    protected override actionID getId()
    {
        return actionID.WALK;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void doAction(float[] args)
    {
        float startPos = rb.position.x;
        float intensity = Mathf.Clamp01(args[0]);
        intensity = (args[1] < 0) ? -intensity : intensity;
        rb.position += new Vector2(moveFunc(Time.deltaTime, intensity), 0.0f);
    }

    public float moveFunc(float timeDiff, float intensity)
    {
        return timeDiff * intensity * terminalSpeed;
    }
}
