using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpf1Behavior : MonoBehaviour {

    [SerializeField] private Rigidbody2D rb2d;
    private static Vector2 move = new Vector2(0.1f, 0.0f);

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(move);
    }

    void FixedUpdate()
    {
        rb2d.position += move;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag=="Enemy")
        {
            tpf0Behavior tpf0 = col.gameObject.GetComponent<tpf0Behavior>();
            tpf0.Yell();
        }
    }
}
