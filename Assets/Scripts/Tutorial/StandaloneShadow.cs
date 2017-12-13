using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandaloneShadow : Platform{

    public Transform[] shadowEdges;

    public override void Initialize()
    {
        GV.ws.shadowManager.RegisterShadow(this, transform);
    }

    public override Vector2[] RetrieveShadowEdges()
    {
        return new Vector2[] { shadowEdges[0].position, shadowEdges[1].position };
    }
}
