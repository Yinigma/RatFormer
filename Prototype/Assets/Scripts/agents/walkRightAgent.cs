using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(walkAction))]
public class walkRightAgent : MonoBehaviour {

    walkAction walker;
    Rigidbody2D rb;

	// Use this for initialization
	void Awake() {
        walker = GetComponent<walkAction>();
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        float[] args = { 1.0f, 1.0f };
        walker.doAction(args);
	}
}
