using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidManager : MonoBehaviour {

    List<Aphid> aphids = new List<Aphid>();
    bool aphidsActive = true;

    public void Initialize()
    {
        List<Platform> platWithAphids = GV.ws.platformManager.GetAllPlatformsWithAphids();
        foreach (Platform p in platWithAphids)
        {
            //Debug.Log("Creating aphid on platform: " + p.name + " where is true: " + p.hasAphid);
            CreateAphidOnPlatform(p);
        }
    }

    public void CreateAphidOnPlatform(Platform p)
    {
        GameObject aphid = GameObject.Instantiate(Resources.Load("Prefabs/Aphid")) as GameObject;
        aphid.transform.SetParent(transform);
        Aphid a = aphid.GetComponent<Aphid>();
        aphids.Add(a);
        a.Initialize(p);
    }

	public void SetAphidsActive(bool _setActive)
    {
        transform.gameObject.SetActive(_setActive);
        aphidsActive = _setActive;
    }

    public void Refresh(float dt)
    {
        if(aphidsActive)
            foreach (Aphid a in aphids)
                a.Refresh(dt);
    }

   
}
