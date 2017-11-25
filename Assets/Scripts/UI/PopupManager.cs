using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public List<Bouncer> bouncers = new List<Bouncer>();

    public void Initialize()
    {
        foreach (Bouncer b in bouncers)
            b.Initialize();
    }

    public void Refresh(float dt)
    {
        foreach (Bouncer b in bouncers)
            b.UpdateBouncer(dt);
    }

    public void InitializePopup(string msgText)
    {

    }

    public void ClosePopup()
    {

    }
}
