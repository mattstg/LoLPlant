using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	public List<Cloud> clouds;
    bool cloudsVisible;

    public void Initialize()
    {
        foreach (Cloud c in clouds)
            c.Initialize();
    }

    public void SetCloudsVisible(bool _setVisible)
    {
        foreach (Cloud c in clouds)
            c.gameObject.SetActive(_setVisible);
    }

    public void Refresh(float dt)
    {
        if(cloudsVisible)
            foreach (Cloud c in clouds)
                c.Refresh(dt);
    }
}
