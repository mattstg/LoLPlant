using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public enum Type { Image, Text, None }

    private Type type;
    private float fadeDuration = 1;
    private float fadeProgress = 1;
    private float sourceAlpha = 1;
    private float targetAlpha = 1;
    private Image image;
    private Text text;
    private bool stateChange = false;

    public void InitializeFader()
    {
        type = Type.None;
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
        if (fadeDuration <= 0)
            fadeDuration = 1;
        fadeProgress = fadeDuration;
        image = _image;
        type = Type.Image;
    }

    public void InitializeFader(Text _text)
    {
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
        if (fadeProgress != fadeDuration)
        {
            if (fadeDuration <= 0)
                fadeDuration = 1;

            float integral = GetIntegral(fadeProgress / fadeDuration);
            float newAlpha = targetAlpha * integral + sourceAlpha * (1 - integral);
            fadeProgress += dt;
            if (fadeProgress > fadeDuration)
                fadeProgress = fadeDuration;
            ApplyAlpha(newAlpha);
            stateChange = true;
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

    public void FadeIn(float duration)
    {
        SetTargetAlpha(1f, duration);
    }

    public void FadeOut(float duration)
    {
        SetTargetAlpha(0f, duration);
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
