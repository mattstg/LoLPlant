﻿using System.Collections;
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

    public Image foodMeter;
    public Image foodLoss;

    private Plant plant;

    public void Initialize()
    {
        plant = GV.ws.plant;
    }

    public void Refresh(float dt)
    {
        UpdateInputs();
        UpdatePhotosynthesis();
        UpdateSpinner();
        UpdateFood();
    }

    public void UpdateInputs()
    {
        sunMeter.eulerAngles = new Vector3(sunMeter.eulerAngles.x, sunMeter.eulerAngles.y, -180 * plant.sunDamp + 90);
        waterMeter.eulerAngles = new Vector3(waterMeter.eulerAngles.x, waterMeter.eulerAngles.y, -180 * plant.waterDamp + 90);

        float normalAngle = GV.NormalizeAngle(sunMeter.eulerAngles.z);
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

        normalAngle = GV.NormalizeAngle(waterMeter.eulerAngles.z);
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

    public void UpdateSpinner()
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

        float ballScale = Mathf.Max(tracerFill / 0.4f, 0.15f);
        spinnerBall.localScale = new Vector3(ballScale, ballScale, spinnerBall.localScale.z);
    }

    public void UpdateFood()
    {
        foodMeter.fillAmount = Mathf.Clamp(plant.foodDamp / GV.FoodMaximum, 0, 1);
    }
}
