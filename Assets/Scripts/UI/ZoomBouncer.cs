using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomBouncer : MonoBehaviour
{
    public RectTransform rectTransform;
    private Vector2 origin = new Vector2(0f, 0f);

    public Vector2 offset = new Vector2(0f, 0f);
    public float bounceHeight = 10f;
    public float bounceTime = 1f;
    public float pauseTime = 0f;

    public float timer = 0f;
    public float timerOffset = 0f;
    public bool isPausing = false;
    public float progress = 0f;

    public bool useWaveMotion = false;

    public void InitializeZoomBouncer()
    {
        //if (!rectTransform)
        //    rectTransform = GetComponent<RectTransform>();
        //origin = rectTransform.anchoredPosition;
        //timer = 0f;
    }

    public void UpdateZoomBouncer(float dt)
    {
        //timer += dt;
        //
        //if (isPausing && pauseTime <= 0f)
        //{
        //    pauseTime = 0f;
        //    isPausing = false;
        //    timer = 0f;
        //}
        //if (bounceTime <= 0f)
        //    bounceTime = 1f;
        //
        //progress = (isPausing) ? (timer + timerOffset) / pauseTime : (timer + timerOffset) / bounceTime;
        //
        //if (progress >= 1f)
        //{
        //    timer -= (isPausing) ? pauseTime : bounceTime;
        //    isPausing = (!isPausing && pauseTime > 0f);
        //    progress = (isPausing) ? (timer + timerOffset) / pauseTime : (timer + timerOffset) / bounceTime;
        //}
        //
        //rectTransform.anchoredPosition = new Vector2(origin.x + offset.x, origin.y + offset.y + ((isPausing) ? 0f : ((useWaveMotion) ? GV.WaveFactor(progress) : GV.BounceFactor(progress)) * bounceHeight));
    }

    public void ZoomBounceIn(float delay = 0f, float peakScale = 1.2f, float peakTime = 0.25f)
    {

    }

    public void ZoomBounceOut(float delay = 0f, float peakScale = 1.2f, float peakTime = 0.25f)
    {

    }
}
