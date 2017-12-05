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

    public List<Button> nextButtons = new List<Button>();

    private State state;
    private State targetState;
    private Vector2 currentPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 positionVelocity = Vector2.zero;
    private Icon icon;
    private List<Message> messages;
    private int currentIndex;
    private float transitionProgress = 2f;
    private readonly float transitionDuration = 2f;

    private bool updatePosition = false;
    private bool updatePanel = true;        //these 4 bools should be false, once flow is built
    private bool updateMessage = true;      //
    private bool updateMessageSpace = true; //
    private bool updateIcon = true;         //

    private bool inputAccepted = true;       //should be false, once flow is built
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

        if (messages.Count > 0)
            messages.Clear();

        currentIndex = 0;
        messageText.text = "";
        currentPosition = GetPosition(Message.Position.Bottom);
        popupParent.anchoredPosition = currentPosition;

        icon = Icon.None;
        ApplyIcon(0f);
        ApplyInputState(false, true);
        iconSelected = false;

        panelFader.SetPresentAlpha(0f);
        messageFader.SetPresentAlpha(0f);
        popupParent.gameObject.SetActive(false);

        updatePosition = false;
        updatePanel = false;
        updateMessage = false;
        updateMessageSpace = false;
        updateIcon = false;
    }

    public void LoadPopup(List<Message> _messages)
    {
        if (state == State.Closed)
        {
            messages = _messages;
            targetState = State.Opening;
            currentIndex = 0;
        }
        else if (state == State.Closing) // should not happen
        {
            Clear();
            messages = _messages;
            targetState = State.Opening;
            currentIndex = 0;
        }
        else
        {
            messages.AddRange(_messages);
        }
        Flow();

        /*
        if (messages.Count > 0 &&)
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
        */
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

        if (state == State.Opening || state == State.Proceeding || state == State.Closing)
        {
            transitionProgress += dt;
            if (transitionProgress >= transitionDuration)
            {
                transitionProgress = transitionDuration;
                if (state == State.Opening || state == State.Proceeding)
                    targetState = State.Open;
                else if (state == State.Closing)
                    targetState = State.Closed;
                Flow();
            }
        }
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

    private void ApplyIcon(float alpha = 0f)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowFader.gameObject.SetActive(true);
                boxFader.gameObject.SetActive(false);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(false);
                checkFader.gameObject.SetActive(false);

                arrowFader.SetPresentAlpha(alpha);
                break;
            case Icon.Box:
                arrowFader.gameObject.SetActive(false);
                boxFader.gameObject.SetActive(true);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(false);
                checkFader.gameObject.SetActive(false);

                boxFader.SetPresentAlpha(alpha);
                break;
            case Icon.Dots:
                arrowFader.gameObject.SetActive(false);
                boxFader.gameObject.SetActive(false);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(true);
                checkFader.gameObject.SetActive(false);

                foreach (Fader f in dotsFaders)
                    f.SetPresentAlpha(alpha);
                break;
            case Icon.Check:
                arrowFader.gameObject.SetActive(false);
                boxFader.gameObject.SetActive(false);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(true);
                checkFader.gameObject.SetActive(true);

                checkFader.SetPresentAlpha(alpha);
                break;
            default:
                arrowFader.gameObject.SetActive(false);
                boxFader.gameObject.SetActive(false);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(false);
                checkFader.gameObject.SetActive(false);
                break;
        }
    }

    private void FadeInIcon(float duration)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowFader.FadeIn(duration);
                break;
            case Icon.Box:
                boxFader.FadeIn(duration);
                break;
            case Icon.Dots:
                foreach (Fader f in dotsFaders)
                    f.FadeIn(duration);
                break;
            case Icon.Check:
                checkFader.FadeIn(duration);
                break;
        }
    }

    private void FadeOutIcon(float duration)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowFader.FadeOut(duration);
                break;
            case Icon.Box:
                boxFader.FadeOut(duration);
                break;
            case Icon.Dots:
                foreach (Fader f in dotsFaders)
                    f.FadeOut(duration);
                break;
            case Icon.Check:
                checkFader.FadeOut(duration);
                break;
        }
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
                        state = targetState;
                        BeginOpening();
                        validTarget = true;
                    }
                    break;
                case State.Opening:
                    if (targetState == State.Open)
                    {
                        state = targetState;
                        HasOpened();
                        validTarget = true;
                    }
                    break;
                case State.Open:
                    if (targetState == State.Proceeding && currentIndex < messages.Count - 1)
                    {
                        state = targetState;
                        BeginProceeding();
                        validTarget = true;
                    }
                    else if (targetState == State.Closing)
                    {
                        state = targetState;
                        BeginClosing();
                        validTarget = true;
                    }
                    break;
                case State.Proceeding:
                    if (targetState == State.Open)
                    {
                        state = targetState;
                        HasOpened();
                        validTarget = true;
                    }
                    break;
                case State.Closing:
                    if (targetState == State.Closed)
                    {
                        state = targetState;
                        HasClosed();
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

        if (icon == Icon.Box && currentIndex < messages.Count - 1 )
        {
            icon = Icon.Arrow;
            ApplyIcon(boxFader.GetPresentAlpha());
            FadeInIcon(1f);
            updateIcon = true;
        }
    }

    private void BeginOpening()
    {
        transitionProgress = 0f;

        currentIndex = 0;
        messageText.text = messages[currentIndex].message;
        targetPosition = GetPosition(messages[currentIndex].position);
        ApplyInputState(false);

        updatePosition = true;
        updatePanel = true;
        updateMessage = true;
        updateMessageSpace = true;
        updateIcon = false;

        popupParent.gameObject.SetActive(true);
        panelFader.FadeIn(1f);
        messageFader.FadeIn(2f);

    }

    private void BeginProceeding()
    {
        transitionProgress = 0f;
        ApplyInputState(false);

    }

    private void BeginClosing()
    {
        transitionProgress = 0f;
        ApplyInputState(false);

    }

    private void HasOpened()
    {
        //activate icon
        //icon fader: fade in
        ApplyInputState(messages[currentIndex].type == Message.Type.Info);
        bool callFlow = false;
        if (messages[currentIndex].type == Message.Type.Info)
            icon = (IsCurrentIndexLast()) ? Icon.Box : Icon.Arrow;
        else
        {
            if (messages[currentIndex].promptSuccess)
            {
                icon = Icon.Check;
                targetState = (IsCurrentIndexLast()) ? State.Closing : State.Proceeding;
                callFlow = true; 
            }
            
        }

        ApplyIcon(0f);
        FadeInIcon(1f);
        updateIcon = true;

        if (callFlow)
            Flow();
    }

    private void HasClosed()
    {
        ApplyInputState(false);

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
            if (IsCurrentIndexLast() || messages.Count == 0)
            {
                targetState = State.Closing;
                Flow();
            }
            else
            {
                targetState = State.Proceeding;
                Flow();
            }
        }
    }

    public void PromptSuccess(string messageName) //called by TAEventManager
    {
        if (messages[currentIndex].name == messageName && messages[currentIndex].type == Message.Type.Prompt)
        {
            messages[currentIndex].promptSuccess = true;
            if (state == State.Open && icon == Icon.Dots)
            {
                FadeOutIcon(1f);
                icon = Icon.Check;
                FadeInIcon(1f);
                //ZoomBounceInIcon();
                targetState = (IsCurrentIndexLast()) ? State.Closing : State.Proceeding;
                Flow();
            }
        }
        else
        {
            bool messageFound = false;
            for (int i = 0; i < messages.Count; i++)
            {
                if ( messages[i].name == messageName && messages[i].type == Message.Type.Prompt && i != currentIndex )
                {
                    messages[i].promptSuccess = true;
                    messageFound = true;
                    break;
                }
            }
            if (!messageFound)
                Debug.Log("Error: PopupManager: PromptSuccess() called by TAEventManager without corresponding prompt-message loaded");
        }
    }

    private void ApplyInputState(bool _inputAccepted, bool forceApply = false)
    {
        if (inputAccepted != _inputAccepted || forceApply)
        {
            inputAccepted = _inputAccepted;
            foreach (Button b in nextButtons)
                b.interactable = inputAccepted;
        }
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
