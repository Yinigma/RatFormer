using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class fallBridgePolicy : bridgePolicy
{
    public static readonly action.actionID[] reqs = { action.actionID.WALK };

    //I'm an abstract class. I don't have any provisions for making sure each one of my child classes has some constant value.
    protected override action.actionID[] requiredIDs()
    {
        return reqs;
    }
    public override actionSequence getActionSequence(Vector2 a, Vector2 b, action[] available)
    {
        validateAndLoadIncoming(available);

        throw new NotImplementedException();
    }

}
