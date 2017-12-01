using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public enum State { Open, Closed, Opening, Closing, Proceeding }
    public enum Icon { None, Arrow, Square, Dots, Checkmark, Exclamation }

    public RectTransform popupParent;
    public RectTransform messageRect;
    public Text messageText;
    public LayoutElement messageSpaceLE;
    public List<Bouncer> bouncers = new List<Bouncer>();

    private State state;
    private Icon icon;
    private List<Message> messages;
    private int currentIndex;

    private Vector2 messageSpaceDimensions = new Vector2(120f, 30f);
    private Vector2 msdVelocity = Vector2.zero;
    private float dampTime = 0.25f;

    public void Initialize()
    {
        messages = new List<Message>();
        currentIndex = 0;
        //state = State.Closed;
        //popupParent.gameObject.SetActive(false);
        if (bouncers.Count != 0)
            for (int i = 0; i < bouncers.Count; i++)
                bouncers[i].Initialize();
    }

    public void Refresh(float dt)
    {
        Vector2 messageTextDimensions = new Vector2((float)((int)(messageRect.rect.width / 2f) * 2), (float)((int)(messageRect.rect.height / 2f) * 2));

        if (Mathf.Abs(messageTextDimensions.x - messageSpaceDimensions.x) > 0.1f || Mathf.Abs(messageTextDimensions.y - messageSpaceDimensions.y) > 0.1f)
        {
            messageSpaceDimensions = Vector2.SmoothDamp(messageSpaceDimensions, messageTextDimensions, ref msdVelocity, dampTime, Mathf.Infinity, dt);
            messageSpaceLE.minWidth = messageSpaceDimensions.x;
            messageSpaceLE.minHeight = messageSpaceDimensions.y;
            Debug.Log("Update message space dimensions");
        }
        else if (messageSpaceDimensions != messageTextDimensions)
        {
            messageSpaceDimensions = messageTextDimensions;
            msdVelocity = Vector2.zero;
            messageSpaceLE.minWidth = messageSpaceDimensions.x;
            messageSpaceLE.minHeight = messageSpaceDimensions.y;
            Debug.Log("Update message space dimensions");
        }

        if (bouncers.Count != 0)
            for (int i = 0; i < bouncers.Count; i++)
                bouncers[i].UpdateBouncer(dt);
    }

    public void LoadPopup(List<Message> _messages)
    {
        if (messages.Count > 0)
            messages.AddRange(_messages);
        else
            messages = _messages;
        foreach (Message m in messages)
            Debug.Log("Popup: " + m.message);
    }

    public void Next()
    {
        currentIndex++;

        if (currentIndex >= messages.Count || messages.Count == 0)
            ClosePopup();
        else
            messageText.text = messages[currentIndex].message;
    }

    public void ClosePopup()
    {
        messages.Clear();
        messageText.text = "";
        currentIndex = 0;
        popupParent.gameObject.SetActive(false);
        //if(prompt)
            //do some graphic stuff
            //then after a few Refresh cycles, send trigger "ClosePopup"
        //if(shutOnClose)
                //Then object turns off
        TAEventManager.Instance.RecieveActionTrigger("ClosePopup");
    }
}
