using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {

	List<Cloud> clouds;
    bool cloudsVisible = true;
    
    

    public void Initialize()
    {
        clouds = new List<Cloud>();
        foreach (Transform child in transform)
        {
            Cloud c = child.GetComponent<Cloud>();
            clouds.Add(c);
            c.Initialize();
        }
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
