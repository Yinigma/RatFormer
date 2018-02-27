using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class bridgePolicy {

    protected abstract action.actionID[] requiredIDs();
    protected action[] bridgeActions;
    public abstract actionSequence getActionSequence(Vector2 a, Vector2 b, action[] available);

    //Helper method for loading in the actions when this is started up 
    protected virtual bool validateAndLoadIncoming(action[] available)
    {
        action.actionID[] reqIDs = requiredIDs();
        if (reqIDs==null)
        {
            Debug.Log("The list of required actions was not defined in the script.");
            return false;
        }
        bridgeActions = new action[reqIDs.Length];
        int dex = 0;
        bool valid = false;
        foreach(action.actionID id in reqIDs)
        {
            foreach(action a in available)
            {
                if (a.ID==id)
                {
                    bridgeActions[dex] = a;
                    valid = true;
                }
            }
            if (!valid)
            {
                return false;
            }
            valid = false;
            dex++;
        }
        return true;
    }
}
