using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upDown : MonoBehaviour {

    public float framesToMax;
    public float maxAmp;
    public float frequency;

    private bool maxReached;
    private float amplitude;
    private Vector3 oPos;


    // Use this for initialization
    void Start () {
        //I don't know if this is necessary, but I'm not in the mood to risk it
        oPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (!maxReached)
        {
            amplitude += maxAmp / framesToMax;
            if (amplitude >= maxAmp)
            {
                amplitude = maxAmp;
                maxReached = true;
            }
        }
        transform.Translate(0, amplitude*Mathf.Sin(frequency*2*Mathf.PI*Time.time), 0);
        
	}

    public void Reset(){
        transform.SetPositionAndRotation(oPos, transform.rotation);
        amplitude = 0;
        maxReached = false;
    }
}
