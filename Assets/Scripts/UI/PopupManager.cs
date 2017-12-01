using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public enum Type { Info, Prompt }
    public enum State { Open, Closed, Opening, Closing, Proceeding }
    public enum Icon { None, Arrow, Square, Dots, Checkmark, Exclamation }

    private Type type;
    private State state;
    private Icon icon;

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

    public void InitializePopup(List<string> msgText)
    {
        //Clear Text
        //Turn popup on
        Debug.Log("Popup: " + msgText);
        //ClosePopup();
    }

    public void ClosePopup()
    {
        //if(prompt)
            //do some graphic stuff
            //then after a few Refresh cycles, send trigger "ClosePopup"
        //if(shutOnClose)
                //Then object turns off
        TAEventManager.Instance.RecieveActionTrigger("ClosePopup");
    }
}
