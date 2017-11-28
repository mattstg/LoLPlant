using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public List<Bouncer> bouncers = new List<Bouncer>();

    public void Initialize()
    {
        if (bouncers.Count != 0)
            for (int i = 0; i < bouncers.Count; i++)
                bouncers[i].Initialize();
    }

    public void Refresh(float dt)
    {
        if (bouncers.Count != 0)
            for (int i = 0; i < bouncers.Count; i++)
                bouncers[i].UpdateBouncer(dt);
    }

    public void InitializePopup(string msgText)
    {
        Debug.Log("Popup: " + msgText);
        //ClosePopup();
    }

    public void ClosePopup()
    {
        TAEventManager.Instance.RecieveActionTrigger("ClosePopup");
    }
}
