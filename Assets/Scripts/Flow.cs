using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow
{
    public virtual void Initialize(int progressNumber)
    {

    }
    // Update is called once per frame
    public virtual void Update (float dt)
    {
		

	}
    public virtual void EndFlow(CurrentState nextState)
    { //Ends the current flow, loads next scene, then runs initialize flow

    }
}
