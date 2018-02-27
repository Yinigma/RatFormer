using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class edgePlatformData : platformData {

    private EdgeCollider2D col;

    void Start()
    {
        levelGraph.addEntry(this);
        col = GetComponent<EdgeCollider2D>();
    }

    public override Vector2[] getPoints()
    {
        return col.points;
    }
}
