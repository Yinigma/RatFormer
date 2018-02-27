using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class action : MonoBehaviour{
    
    public enum actionID { NONE, WALK, JUMP };

    protected abstract actionID getId();
    
    public virtual actionID ID { get{ return getId(); } }

    public abstract void doAction(float[] args);
}
