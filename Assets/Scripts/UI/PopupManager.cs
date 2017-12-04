using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public enum State { Closed, Open, Opening, Proceeding, Closing }
    public enum Icon { None, Arrow, Box, Dots, Check }

    public RectTransform popupParent;
    public RectTransform messageRect;

    public Text messageText;
    public LayoutElement messageSpaceLE;

    public Fader panelFader;
    public Fader messageFader;
    public Fader arrowFader;
    public Fader boxFader;
    public List<Fader> dotsFaders = new List<Fader>();
    public Fader checkFader;

    public Bouncer arrowBouncer;
    public Bouncer boxBouncer;
    public List<Bouncer> dotsBouncers = new List<Bouncer>();
    public Bouncer checkBouncer;

    public ZoomBouncer messageZoomBouncer;
    public ZoomBouncer arrowZoomBouncer;
    public ZoomBouncer boxZoomBouncer;
    public ZoomBouncer checkZoomBouncer;

    private State state;
    private State targetState;
    private Vector2 currentPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 positionVelocity = Vector2.zero;
    private Icon icon;
    private List<Message> messages;
    private int currentIndex;

    private bool updatePosition = false;
    private bool updatePanel = true;        //these 4 bools should be false, once flow is built
    private bool updateMessage = true;      //
    private bool updateMessageSpace = true; //
    private bool updateIcon = true;         //

    private bool iconSelected = false;

    private Vector2 messageSpaceDimensions = new Vector2(120f, 30f);
    private Vector2 msdVelocity = Vector2.zero;
    private float dampTime = 0.25f;

    public void Initialize()
    {
        messages = new List<Message>();
        InitializeFaders();
        InitializeBouncers();
        InitializeZoomBouncers();
        Clear();
    }

    private void InitializeFaders()
    {
        panelFader.InitializeFader();
        messageFader.InitializeFader();
        arrowFader.InitializeFader();
        boxFader.InitializeFader();
        foreach (Fader f in dotsFaders)
            f.InitializeFader();
        checkFader.InitializeFader();
    }

    private void InitializeBouncers()
    {
        arrowBouncer.InitializeBouncer();
        boxBouncer.InitializeBouncer();
        foreach (Bouncer b in dotsBouncers)
            b.InitializeBouncer();
        checkBouncer.InitializeBouncer();
    }

    private void InitializeZoomBouncers()
    {
        messageZoomBouncer.InitializeZoomBouncer();
        arrowZoomBouncer.InitializeZoomBouncer();
        boxZoomBouncer.InitializeZoomBouncer();
        checkZoomBouncer.InitializeZoomBouncer();
    }

    private void Clear()
    {
        state = State.Closed;
        currentPosition = GetPosition(Message.Position.Bottom);
        popupParent.anchoredPosition = currentPosition;
        icon = Icon.None;
        if (messages.Count > 0)
            messages.Clear();
        currentIndex = 0;
        messageText.text = "";
        panelFader.SetPresentAlpha(0f);
        messageFader.SetPresentAlpha(0f);
        arrowFader.SetPresentAlpha(0f);
        boxFader.SetPresentAlpha(0f);
        foreach (Fader f in dotsFaders)
            f.SetPresentAlpha(0f);
        checkFader.SetPresentAlpha(0f);
        popupParent.gameObject.SetActive(false);
        arrowFader.gameObject.SetActive(false);
        boxFader.gameObject.SetActive(false);
        foreach (Fader f in dotsFaders)
            f.gameObject.SetActive(false);
        checkFader.gameObject.SetActive(false);
        updatePosition = false;
        updatePanel = false;
        updateMessage = false;
        updateMessageSpace = false;
        updateIcon = false;
    }

    public void LoadPopup(List<Message> _messages)
    {
        if (messages.Count > 0)
            messages.AddRange(_messages);
        else
        {
            messages = _messages;
            currentIndex = 0;
            messageText.text = messages[currentIndex].message;
        }

        //temp:
        state = targetState = State.Open;
        icon = Icon.Arrow;
        popupParent.gameObject.SetActive(true);
        messageText.gameObject.SetActive(true);
        arrowFader.gameObject.SetActive(true);
        panelFader.SetPresentAlpha(1f);
        messageFader.SetPresentAlpha(1f);
        arrowFader.SetPresentAlpha(1f);
        //updatePosition = true;
        updatePanel = true;
        updateMessage = true;
        updateMessageSpace = true;
        updateIcon = true;

        foreach (Message m in _messages)
            Debug.Log("Popup: " + m.message);
    }

    public void Refresh(float dt)
    {
        if (updatePosition)
            UpdatePosition(dt);
        if (updatePanel)
            UpdatePanel(dt);
        if (updateMessage)
            UpdateMessage(dt);
        if (updateMessageSpace)
            UpdateMessageSpace(dt);
        if (updateIcon)
            UpdateIcon(dt);
    }

    private void UpdatePosition(float dt)
    {
        float nearZeroMargin = 0.1f;
        if (Mathf.Abs(targetPosition.x - currentPosition.x) > nearZeroMargin || Mathf.Abs(targetPosition.y - currentPosition.y) > nearZeroMargin)
        {
            currentPosition = Vector2.SmoothDamp(currentPosition, targetPosition, ref positionVelocity, dampTime, Mathf.Infinity, dt);
        }
        else
        {
            currentPosition = targetPosition;
            positionVelocity = Vector2.zero;
            updatePosition = false;
        }
        popupParent.anchoredPosition = currentPosition;
    }

    private void UpdatePanel(float dt)
    {
        panelFader.UpdateFader(dt);
        //popup position, messages[currentIndex].position
    }

    private void UpdateMessage(float dt)
    {
        messageFader.UpdateFader(dt);
        messageZoomBouncer.UpdateZoomBouncer(dt);
    }

    private void UpdateMessageSpace(float dt)
    {
        Vector2 messageTextDimensions = new Vector2((float)((int)(messageRect.rect.width / 2f) * 2), (float)((int)(messageRect.rect.height / 2f) * 2));
        float nearZeroMargin = 0.1f;
        if (Mathf.Abs(messageTextDimensions.x - messageSpaceDimensions.x) > nearZeroMargin || Mathf.Abs(messageTextDimensions.y - messageSpaceDimensions.y) > nearZeroMargin)
        {
            messageSpaceDimensions = Vector2.SmoothDamp(messageSpaceDimensions, messageTextDimensions, ref msdVelocity, dampTime, Mathf.Infinity, dt);
            messageSpaceLE.minWidth = messageSpaceDimensions.x;
            messageSpaceLE.minHeight = messageSpaceDimensions.y;
        }
        else
        {
            messageSpaceDimensions = messageTextDimensions;
            msdVelocity = Vector2.zero;
            messageSpaceLE.minWidth = messageSpaceDimensions.x;
            messageSpaceLE.minHeight = messageSpaceDimensions.y;
            //updateMessageSpace = false; //un-comment once flow is built
        }
    }

    private void UpdateIcon(float dt)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowFader.UpdateFader(dt);
                arrowBouncer.UpdateBouncer(dt);
                arrowZoomBouncer.UpdateZoomBouncer(dt);
                break;
            case Icon.Box:
                boxFader.UpdateFader(dt);
                boxBouncer.UpdateBouncer(dt);
                boxZoomBouncer.UpdateZoomBouncer(dt);
                break;
            case Icon.Dots:
                foreach (Fader f in dotsFaders)
                    f.UpdateFader(dt);
                foreach (Bouncer b in dotsBouncers)
                    b.UpdateBouncer(dt);
                break;
            case Icon.Check:
                checkFader.UpdateFader(dt);
                checkBouncer.UpdateBouncer(dt);
                checkZoomBouncer.UpdateZoomBouncer(dt);
                foreach (Fader f in dotsFaders)
                    f.UpdateFader(dt);
                foreach (Bouncer b in dotsBouncers)
                    b.UpdateBouncer(dt);
                break;
            default:
                break;
        }
    }

    private void ApplyMessage()
    {
        messageText.text = messages[currentIndex].message;
    }

    private void Flow()
    {
        if (state != targetState)
        {
            bool validTarget = false;
            switch (state)
            {
                case State.Closed:
                    if (targetState == State.Opening)
                    {
                        BeginOpening();
                        validTarget = true;
                    }
                    break;
                case State.Opening:
                    if (targetState == State.Open)
                    {
                        state = targetState;
                        validTarget = true;
                    }
                    break;
                case State.Open:
                    if (targetState == State.Proceeding)
                    {
                        BeginProceeding();
                        validTarget = true;
                    }
                    else if (targetState == State.Closing)
                    {
                        BeginClosing();
                        validTarget = true;
                    }
                    break;
                case State.Proceeding:
                    if (targetState == State.Open)
                    {
                        state = targetState;
                        validTarget = true;
                    }
                    break;
                case State.Closing:
                    if (targetState == State.Closed)
                    {
                        state = targetState;
                        validTarget = true;
                    }
                    break;
            }
            if (!validTarget)
            {
                targetState = state;
                Debug.Log("Error: PopupManager: Flow(): targetState is invalid");
            }
        }


    }

    private void BeginOpening()
    {
        

        updateMessageSpace = true;
        updatePanel = true;
        updateIcon = false;
    }

    private void BeginProceeding()
    {

    }

    private void BeginClosing()
    {

    }

    public void ClosePopup()
    {
        //temp
        messages.Clear();
        messageText.text = "";
        messageFader.SetPresentAlpha(0f);
        currentIndex = 0;
        popupParent.gameObject.SetActive(false);
        TAEventManager.Instance.RecieveActionTrigger("ClosePopup");
    }

    public void Next() //called by button click
    {
        if (state == State.Open)
        {
            if (currentIndex >= messages.Count - 1 || messages.Count == 0)
            {
                ClosePopup();
                // targetState = State.Closing;
                // Flow();
            }
            else
            {
                currentIndex++;
                messageText.text = messages[currentIndex].message;
                updateMessageSpace = true;
                // targetState = State.Proceeding;
                // Flow();
            }
        }
    }

    public void PromptSuccess() //called by TAEventManager
    {

    }

    private bool IsCurrentIndexLast()
    {
        return (currentIndex == messages.Count - 1);
    }

    private Vector2 GetPosition(Message.Position _position)
    {
        switch (_position)
        {
            case Message.Position.Bottom:
                return new Vector2(0f, 60f);
            default:
                return new Vector2(0f, 0f);
        }
    }
}
