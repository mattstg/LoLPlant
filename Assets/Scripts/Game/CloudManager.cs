using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	public List<Cloud> clouds;

    public void Initialize()
    {
        foreach (Cloud c in clouds)
            c.Initialize();
    }

    public void Refresh(float dt)
    {
        foreach (Cloud c in clouds)
            c.Refresh(dt);
    }
}
