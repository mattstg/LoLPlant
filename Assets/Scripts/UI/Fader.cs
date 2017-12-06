using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public enum Type { Image, Text, None }

    private Type type;
    private float delayDuration = 0;
    private float delayProgress = 0;
    private bool delaying = false;
    private float fadeDuration = 1;
    private float fadeProgress = 1;
    private float sourceAlpha = 1;
    private float targetAlpha = 1;
    private Image image;
    private Text text;
    private bool stateChange = false;

    public delegate void FadeComplete();
    private FadeComplete onFadeComplete;


    public void InitializeFader()
    {
        type = Type.None;
        onFadeComplete = null;
        if (fadeDuration <= 0)
            fadeDuration = 1;
        fadeProgress = fadeDuration;
        if (!image && gameObject.GetComponent<Image>())
        {
            image = gameObject.GetComponent<Image>();
            type = Type.Image;
        }
        else if (!text && gameObject.GetComponent<Text>())
        {
            text = gameObject.GetComponent<Text>();
            type = Type.Text;
        }
    }

    public void InitializeFader(Image _image)
    {
        onFadeComplete = null;
        if (fadeDuration <= 0)
            fadeDuration = 1;
        fadeProgress = fadeDuration;
        image = _image;
        type = Type.Image;
    }

    public void InitializeFader(Text _text)
    {
        onFadeComplete = null;
        if (fadeDuration <= 0)
            fadeDuration = 1;
        fadeProgress = fadeDuration;
        text = _text;
        type = Type.Text;
    }

    public void UpdateFader(float dt)
    {
        stateChange = false;
        if (type == Type.None)
            return;
        if (delaying)
        {
            if (delayProgress < delayDuration)
            {
                delayProgress += dt;
            }
            else
            {
                delayProgress = delayDuration = 0f;
                delaying = false;
            }
        }
        else if (fadeProgress != fadeDuration)
        {
            if (fadeDuration <= 0)
                fadeDuration = 1;

            float integral = GetIntegral(fadeProgress / fadeDuration);
            float newAlpha = targetAlpha * integral + sourceAlpha * (1 - integral);
            ApplyAlpha(newAlpha);
            stateChange = true;

            fadeProgress += dt;
            if (fadeProgress >= fadeDuration)
            {
                fadeProgress = fadeDuration;
                if (onFadeComplete != null)
                    onFadeComplete();
                onFadeComplete = null;
            }

        }
    }

    public float GetVelocity(float progress)
    {
        if (progress >= 0 && progress <= 1)
            return -4 * Mathf.Abs(progress - 0.5f) + 2;
        else
            return 0;
    }

    public float GetIntegral(float progress)
    {
        if (progress < 0)
            return 0;
        else if (progress > 1)
            return 1;
        else if (progress <= 0.5)
            return progress * GetVelocity(progress) / 2;
        else
            return 0.5f + ((progress - 0.5f) * (GetVelocity(progress) + 2) / 2);
    }

    public void FadeIn(float duration, float delay = 0, FadeComplete _onFadeComplete = null)
    {
        delayProgress = 0f;
        delayDuration = (delay > 0f) ? delay : 0f;
        delaying = (delayDuration > 0f);

        SetTargetAlpha(1f, duration);

        onFadeComplete = _onFadeComplete;
    }

    public void FadeOut(float duration, float delay = 0, FadeComplete _onFadeComplete = null)
    {
        delayProgress = 0f;
        delayDuration = (delay > 0f) ? delay : 0f;
        delaying = (delayDuration > 0f);

        SetTargetAlpha(0f, duration);

        onFadeComplete = _onFadeComplete;
    }

    public void SetTargetAlpha(float target, float duration)
    {
        sourceAlpha = GetPresentAlpha();
        targetAlpha = target;
        fadeProgress = 0;
        fadeDuration = duration;
    }

    public void SetPresentAlpha(float alpha)
    {
        sourceAlpha = alpha;
        targetAlpha = alpha;
        fadeProgress = fadeDuration;
        ApplyAlpha(targetAlpha);
    }

    public void ApplyAlpha(float newAlpha)
    {
        if (type == Type.Image)
        {
            Color c = image.color;
            c.a = newAlpha;
            image.color = c;
        }
        else if (type == Type.Text)
        {
            Color c = text.color;
            c.a = newAlpha;
            text.color = c;
        }
    }

    public float GetPresentAlpha()
    {
        float presentAlpha;
        if (type == Type.Image)
            presentAlpha = image.color.a;
        else if (type == Type.Text)
            presentAlpha = text.color.a;
        else
            presentAlpha = -1f;
        return presentAlpha;
    }

    public float GetTargetAlpha()
    {
        return targetAlpha;
    }

    public bool IsStateChanged()
    {
        return stateChange;
    }

    public Image GetImage()
    {
        return image;
    }
}
