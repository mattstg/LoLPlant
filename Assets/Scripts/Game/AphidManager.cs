using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidManager : MonoBehaviour {

    bool aphidsActive = true;

	public void SetAphidsActive(bool _setActive)
    {
        transform.gameObject.SetActive(_setActive);
    }

    public void Refresh(float dt)
    {
        //if(aphidsActive)

    }
}
