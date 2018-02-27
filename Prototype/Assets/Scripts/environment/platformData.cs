using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public abstract class platformData : MonoBehaviour{
    [SerializeField]
    private bool upNorm = true;
    [SerializeField]
    private bool solidNormal = true;
    [SerializeField]
    private bool solidNeg = true;

    public abstract Vector2[] getPoints();
}
