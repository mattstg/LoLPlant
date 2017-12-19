using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow
{
    protected bool initialized;

    public virtual void Initialize(int progressNumber)
    {

    }
    // Update is called once per frame
    public virtual void Update (float dt)
    {
		

	}
    public virtual void EndFlow()
    { 
        initialized = false;
    }
    
    
}
