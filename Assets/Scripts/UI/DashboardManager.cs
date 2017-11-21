using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardManager : MonoBehaviour {

    public RectTransform sunMeter;
    public RectTransform sunDiscGreen;
    public RectTransform sunDiscOrange;
    public RectTransform sunDiscRed;

    public RectTransform waterMeter;
    public RectTransform waterDiscGreen;
    public RectTransform waterDiscOrange;
    public RectTransform waterDiscRed;

    public RectTransform psMeter;
    public RectTransform spinner;
    public RectTransform spinnerBall;
    public Image spinnerTracer;
    public ImageFade ballFade;

    public Image foodMeter;
    public Image foodLoss;
    private FoodLossState foodLossState = FoodLossState.Normal;
    public RectTransform foodIcon;

    private Plant plant;

    //private float fpsDamp = 0f;
    //private float fpsVelocity = 0f;

    public void Initialize()
    {
        plant = GV.ws.plant;
        ballFade.Initialize();
    }

    public void Refresh(float dt)
    {
        UpdateInputs();
        UpdatePhotosynthesis();
        UpdateSpinner(dt);
        UpdateFood();

        //fpsDamp = Mathf.SmoothDamp(fpsDamp, (1f / Time.deltaTime) / 100f, ref fpsVelocity, 0.1f);
        //psMeter.eulerAngles = new Vector3(psMeter.eulerAngles.x, psMeter.eulerAngles.y, -180 * fpsDamp + 90);
    }

    public void UpdateInputs()
    {
        sunMeter.eulerAngles = new Vector3(sunMeter.eulerAngles.x, sunMeter.eulerAngles.y, -180 * plant.sunDamp + 90);
        waterMeter.eulerAngles = new Vector3(waterMeter.eulerAngles.x, waterMeter.eulerAngles.y, -180 * plant.waterDamp + 90);

        float normalAngle = GV.NormalizeAngle180(sunMeter.eulerAngles.z);
        if (normalAngle > 32)
        {
            sunDiscGreen.gameObject.SetActive(false);
            sunDiscOrange.gameObject.SetActive(false);
            sunDiscRed.gameObject.SetActive(true);
        }
        else if (normalAngle < -32)
        {
            sunDiscGreen.gameObject.SetActive(true);
            sunDiscOrange.gameObject.SetActive(false);
            sunDiscRed.gameObject.SetActive(false);
        }
        else
        {
            sunDiscGreen.gameObject.SetActive(false);
            sunDiscOrange.gameObject.SetActive(true);
            sunDiscRed.gameObject.SetActive(false);
        }

        normalAngle = GV.NormalizeAngle180(waterMeter.eulerAngles.z);
        if (normalAngle > 69 || normalAngle < -69)
        {
            waterDiscGreen.gameObject.SetActive(false);
            waterDiscOrange.gameObject.SetActive(false);
            waterDiscRed.gameObject.SetActive(true);
        }
        else if ((normalAngle > 32 && normalAngle <= 69) || (normalAngle < -32 && normalAngle >= -69))
        {
            waterDiscGreen.gameObject.SetActive(false);
            waterDiscOrange.gameObject.SetActive(true);
            waterDiscRed.gameObject.SetActive(false);
        }
        else
        {
            waterDiscGreen.gameObject.SetActive(true);
            waterDiscOrange.gameObject.SetActive(false);
            waterDiscRed.gameObject.SetActive(false);
        }
    }

    public void UpdatePhotosynthesis()
    {
        psMeter.eulerAngles = new Vector3(psMeter.eulerAngles.x, psMeter.eulerAngles.y, -180 * plant.psDamp + 90);
    }

    public void UpdateSpinner(float dt)
    {
        spinner.eulerAngles = new Vector3(spinner.eulerAngles.x, spinner.eulerAngles.y, plant.psProgressDamp * -360 * GV.SpinnerSpeed);

        float tracerFill = 0;
        if (plant.psProgressVelocity == 0)
            spinnerTracer.gameObject.SetActive(false);
        else if (plant.psProgressVelocity > 0)
        {
            tracerFill = Mathf.Clamp(plant.psProgressVelocity * 0.6f, 0, 0.4f);
            spinnerTracer.gameObject.SetActive(true);
            spinnerTracer.fillAmount = tracerFill;
            spinnerTracer.rectTransform.localEulerAngles = new Vector3(spinnerTracer.rectTransform.localEulerAngles.x, spinnerTracer.rectTransform.localEulerAngles.y, (1f - tracerFill) * -360f);
        }

        float ballScale = Mathf.Clamp(tracerFill / 0.4f, 0.15f, 1f);
        spinnerBall.localScale = new Vector3(ballScale, ballScale, spinnerBall.localScale.z);

        if (plant.psDamp < 0.08f && ballFade.GetTargetOpacity() == 1f)
            ballFade.SetTargetOpacity(0f, 1.0f);
        else if (plant.psDamp >= 0.08f && ballFade.GetTargetOpacity() < 1f)
            ballFade.SetTargetOpacity(1f, 0.5f);
        ballFade.Refresh(dt);
        if (ballFade.IsStateChanged())
            spinnerTracer.color = ballFade.GetImage().color;
    }

    public void UpdateFood()
    {
        foodMeter.fillAmount = Mathf.Clamp(plant.foodDamp / GV.FoodMaximum, 0, 1);
        if (plant.foodLossState != foodLossState)
        {
            foodLossState = plant.foodLossState;
            if (foodLossState == FoodLossState.Normal)
                foodLoss.gameObject.SetActive(false);
            else
                foodLoss.gameObject.SetActive(true);
        }
        float iconFillAmount;
        if (foodLossState != FoodLossState.Normal)
        {
            foodLoss.fillAmount = Mathf.Clamp(plant.foodLossDamp / GV.FoodMaximum, 0, 1);
            iconFillAmount = foodLoss.fillAmount;
        }
        else
            iconFillAmount = foodMeter.fillAmount;
        iconFillAmount = Mathf.Max(iconFillAmount, 0.095f);
        foodIcon.anchorMin = new Vector2(0.8f - iconFillAmount * 0.8f, 0f);
        foodIcon.anchorMax = new Vector2(0.8f, iconFillAmount);
    }
}
