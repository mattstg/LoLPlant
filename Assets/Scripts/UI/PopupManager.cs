using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

public class PopupManager : MonoBehaviour
{
    public enum State { Closed, Open, Opening, Proceeding, Closing }
    public enum Icon { None, Arrow, Dots, Check }

    public RectTransform popupParent;
    public RectTransform messageRect;

    public Text messageText;
    public LayoutElement messageSpaceLE;

    public Fader panelFader;
    public Fader messageFader;
    public Fader arrowFader;
    public List<Fader> dotsFaders = new List<Fader>();
    public Fader checkFader;

    public Bouncer arrowBouncer;
    public List<Bouncer> dotsBouncers = new List<Bouncer>();
    public Bouncer checkBouncer;

    public ZoomBouncer messageZoomBouncer;
    public ZoomBouncer arrowZoomBouncer;
    public ZoomBouncer checkZoomBouncer;

    public List<Button> nextButtons = new List<Button>();

    public Image arrowImage;
    private Sprite arrowDefaultSprite;
    public Sprite arrowSelectedSprite;

    private State state;
    private State targetState;
    private Vector2 currentPosition = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 positionVelocity = Vector2.zero;
    private Icon icon;
    private List<Message> messages;
    private int currentIndex;
    private float transitionProgress = 2f;
    private readonly float transitionDuration = 3f;
    private readonly float promptProceedDelay = 3f;

    private bool updatePosition = false;
    private bool updatePanel = false;
    private bool updateMessage = false;
    private bool updateMessageSpace = false;
    private bool updateIcon = false;     

    private bool inputAccepted = false;
    private bool arrowSelected = false;

    private Vector2 messageSpaceDimensions = new Vector2(120f, 30f);
    private Vector2 msdVelocity = Vector2.zero;
    private float dampTime = 0.25f;

    public void Initialize()
    {
        messages = new List<Message>();
        InitializeArrow();
        InitializeFaders();
        InitializeBouncers();
        InitializeZoomBouncers();
        Clear();
    }

    private void InitializeArrow()
    {
        arrowDefaultSprite = arrowImage.sprite;
        SetArrowSelected(false, true);
    }

    private void InitializeFaders()
    {
        panelFader.InitializeFader();
        messageFader.InitializeFader();
        arrowFader.InitializeFader();
        foreach (Fader f in dotsFaders)
            f.InitializeFader();
        checkFader.InitializeFader();
    }

    private void InitializeBouncers()
    {
        arrowBouncer.InitializeBouncer();
        foreach (Bouncer b in dotsBouncers)
            b.InitializeBouncer();
        checkBouncer.InitializeBouncer();
    }

    private void InitializeZoomBouncers()
    {
        messageZoomBouncer.InitializeZoomBouncer();
        arrowZoomBouncer.InitializeZoomBouncer();
        checkZoomBouncer.InitializeZoomBouncer();
    }

    private void Clear()
    {
        state = State.Closed;

        if (messages.Count > 0)
            messages.Clear();

        currentIndex = 0;
        messageText.text = "";
        currentPosition = GetPositionVector(Message.Position.Bottom);
        popupParent.anchorMin = popupParent.anchorMax = currentPosition;

        icon = Icon.None;
        ApplyIcon(0f);
        ApplyInputState(false, true);
        SetArrowSelected(false);

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

        if ((state == State.Opening || state == State.Proceeding) && transitionProgress >= 0f)
        {
            transitionProgress += dt;
            if (transitionProgress >= transitionDuration + ((state == State.Proceeding && messages[currentIndex - 1].type == Message.Type.Prompt) ? promptProceedDelay : 0f))
            {
                transitionProgress = -1f;
                targetState = State.Open;
                Flow();
            }
        }
    }

    private void UpdatePosition(float dt)
    {
        float nearZeroMargin = 0.0001f;
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
        popupParent.anchorMin = popupParent.anchorMax = currentPosition;
    }

    private void UpdatePanel(float dt)
    {
        panelFader.UpdateFader(dt);
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
            //updateMessageSpace = false;
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

    private void ApplyIcon(float alpha = 0f)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowFader.gameObject.SetActive(true);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(false);
                checkFader.gameObject.SetActive(false);

                arrowFader.SetPresentAlpha(alpha);
                break;
            case Icon.Dots:
                arrowFader.gameObject.SetActive(false);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(true);
                checkFader.gameObject.SetActive(false);

                foreach (Fader f in dotsFaders)
                    f.SetPresentAlpha(alpha);
                break;
            case Icon.Check:
                arrowFader.gameObject.SetActive(false);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(true);
                checkFader.gameObject.SetActive(true);

                checkFader.SetPresentAlpha(alpha);
                break;
            default:
                arrowFader.gameObject.SetActive(false);
                foreach (Fader f in dotsFaders)
                    f.gameObject.SetActive(false);
                checkFader.gameObject.SetActive(false);
                break;
        }
    }

    private void FadeInIcon(float duration, float delay = 0f, Fader.FadeComplete onComplete = null)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowFader.FadeIn(duration, delay, onComplete);
                break;
            case Icon.Dots:
                foreach (Fader f in dotsFaders)
                    f.FadeIn(duration, delay, onComplete);
                break;
            case Icon.Check:
                checkFader.FadeIn(duration, delay, onComplete);
                break;
        }
    }

    private void FadeOutIcon(float duration, float delay = 0f, Fader.FadeComplete onComplete = null)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowFader.FadeOut(duration, delay, onComplete);
                break;
            case Icon.Dots:
                foreach (Fader f in dotsFaders)
                    f.FadeOut(duration, delay, onComplete);
                break;
            case Icon.Check:
                checkFader.FadeOut(duration, delay, onComplete);
                break;
        }
    }

    private void ZoomBounceInIcon(float duration, float delay = 0f)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowZoomBouncer.ZoomBounceIn(duration, delay);
                break;
            case Icon.Check:
                checkZoomBouncer.ZoomBounceIn(duration, delay);
                break;
        }
    }

    private void ZoomBounceOutIcon(float duration, float delay = 0f)
    {
        switch (icon)
        {
            case Icon.Arrow:
                arrowZoomBouncer.ZoomBounceOut(duration, delay);
                break;
            case Icon.Check:
                checkZoomBouncer.ZoomBounceOut(duration, delay);
                break;
        }
    }

    private void SetArrowSelected(bool selected, bool forceSetArrow = false)
    {
        if (selected != arrowSelected || forceSetArrow)
        {
            arrowSelected = selected;
            arrowImage.sprite = (arrowSelected) ? arrowSelectedSprite : arrowDefaultSprite;
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
    }

    private void BeginOpening()
    {
        updatePanel = true;
        updateMessage = true;
        updateMessageSpace = true;

        transitionProgress = 0f;

        currentIndex = 0;
        messageText.text = messages[currentIndex].message;
        popupParent.anchorMin = popupParent.anchorMax = targetPosition = currentPosition = GetPositionVector(messages[currentIndex].position);
        ApplyInputState(false);

        popupParent.gameObject.SetActive(true);
        panelFader.FadeIn(1f);
        messageFader.FadeIn(1f, 0.5f);

        GV.ws.pc.SetInputActive(messages[currentIndex].type == Message.Type.Prompt);

        LOLSDK.Instance.SpeakText(messages[currentIndex].message); //Read the message outloud here
    }

    private void BeginProceeding()
    {
        updatePosition = true;
        updateMessage = true;
        updateMessageSpace = true;
        updateIcon = true;

        currentIndex += 1;

        transitionProgress = 0f;
        ApplyInputState(false);

        if (messages[currentIndex - 1].type == Message.Type.Info)
        {
            messageFader.FadeOut(1f, 0.5f, MessageFadedForProceed);
            ZoomBounceOutIcon(0.7f);
            FadeOutIcon(0.7f);
            SetArrowSelected(true);
        }
        else
        {
            messageFader.FadeOut(1f, 2.5f, MessageFadedForProceed);
            FadeOutIcon(0.1f); //icon = dots
            icon = Icon.Check;
            ApplyIcon();
            FadeInIcon(0.3f, 0f, CheckFadedIn);
            ZoomBounceInIcon(0.7f);
        }

        GV.ws.pc.SetInputActive(messages[currentIndex].type == Message.Type.Prompt);

        LOLSDK.Instance.SpeakText(messages[currentIndex].message); //Read the message outloud here
    }

    private void BeginClosing()
    {
        //Force end text reading

        updatePanel = true;
        updateMessage = true;
        updateIcon = true;

        transitionProgress = 0f;
        ApplyInputState(false);

        if (messages[currentIndex].type == Message.Type.Info)
        {
            panelFader.FadeOut(1f, 1f, PanelFadedForClose);
            messageFader.FadeOut(1f, 0.5f);
            ZoomBounceOutIcon(0.7f);
            FadeOutIcon(0.7f);
            SetArrowSelected(true);
        }
        else
        {
            panelFader.FadeOut(1f, 3f, PanelFadedForClose);
            messageFader.FadeOut(1f, 2.5f);
            FadeOutIcon(0.1f);
            icon = Icon.Check;
            FadeInIcon(0.3f, 0f, CheckFadedIn);
            ZoomBounceInIcon(0.7f);
        }

        //GV.ws.pc.SetInputActive(true);
        LOLSDK.Instance.SpeakText("");
    }

    private void HasOpened()
    {
        updatePosition = false;
        updatePanel = false;
        updateMessage = false;
        updateMessageSpace = true; // false; //also, UpdateMessageSpace(): uncomment "updateMessageSpace = false;"
        updateIcon = true;

        bool callFlow = false;
        if (messages[currentIndex].type == Message.Type.Info)
            icon = Icon.Arrow;
        else
        {
            if (messages[currentIndex].promptSuccess)
            {
                icon = Icon.Check;
                targetState = (IsCurrentIndexLast()) ? State.Closing : State.Proceeding;
                callFlow = true;
            }
            else
                icon = Icon.Dots;
        }

        SetArrowSelected(false);
        ApplyIcon(0f);
        FadeInIcon(0.5f);
        if (icon == Icon.Check)
        {
            foreach (Fader f in dotsFaders)
            {
                f.SetPresentAlpha(0f);
                f.gameObject.SetActive(false);
            }
        }
        ApplyInputState(messages[currentIndex].type == Message.Type.Info);

        //if (messages[currentIndex].type == Message.Type.Info)
        //    GV.ws.pc.SetInputActive(false);
        //else
        //    GV.ws.pc.SetInputActive(true);

        if (callFlow)
            Flow();
    }

    private void HasClosed()
    {
        Clear();
        GV.ws.pc.SetInputActive(true);
        TAEventManager.Instance.ReceiveActionTrigger("ClosePopup");
    }

    public void MessageFadedForProceed()
    {
        if (state == State.Proceeding)
        {
            updateIcon = false;
            icon = Icon.None;

            if (messages[currentIndex].position != messages[currentIndex - 1].position)
            {
                targetPosition = GetPositionVector(messages[currentIndex].position);
                updatePosition = true;
            }
            messageText.text = messages[currentIndex].message;
            messageFader.FadeIn(1f, 0.5f);
        }
    }

    public void PanelFadedForClose()
    {
        if (state == State.Closing)
        {
            targetState = State.Closed;
            Flow();
        }
    }

    public void CheckFadedIn()
    {
        if (icon == Icon.Check)
            FadeOutIcon(0.5f, 2.5f);
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
        if (messages[currentIndex].name == messageName && messages[currentIndex].type == Message.Type.Prompt && state == State.Open)
        {
            if (!messages[currentIndex].promptSuccess)
            {
                messages[currentIndex].promptSuccess = true;
                FadeOutIcon(1f);
                icon = Icon.Check;
                ApplyIcon();
                FadeInIcon(1f);
                ZoomBounceInIcon(0.5f);
                targetState = (IsCurrentIndexLast()) ? State.Closing : State.Proceeding;
                Flow();
            }
        }
        else
        {
            bool messageFound = false;
            for (int i = 0; i < messages.Count; i++)
            {
                if ( messages[i].name == messageName && messages[i].type == Message.Type.Prompt )
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

    private Vector2 GetPositionVector(Message.Position _position)
    {
        switch (_position)
        {
            case Message.Position.Center:
                return new Vector2(0.5f, 0.5f);
            case Message.Position.Top:
                return new Vector2(0.5f, 1f);
            case Message.Position.Bottom:
                return new Vector2(0.5f, 0f);
            case Message.Position.Left:
                return new Vector2(0f, 0.5f);
            case Message.Position.Right:
                return new Vector2(1f, 0.5f);
            default:
                return new Vector2(0.5f, 0.5f);
        }
    }
}
