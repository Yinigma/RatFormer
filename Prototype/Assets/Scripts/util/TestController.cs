using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(traversalMovement))]
[RequireComponent(typeof(jumpMovement))]
public class TestController : MonoBehaviour {

    private traversalMovement mov;
    private jumpMovement jump;

    void Start () {
        mov = GetComponent<traversalMovement>();
        jump = GetComponent<jumpMovement>();
    }
	
	void Update () {
        mov.moveX(Input.GetAxis("Horizontal"));
        mov.moveY(Input.GetAxis("Vertical"));
        if (Input.GetButton("Jump")) {
            jump.doJump();
        }
    }
}
