using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashboardManager : MonoBehaviour
{
    public enum DashboardElementSet { None, Sun, Water, Photosynthesis, Food, Height, All }
        // None = nothing except: upper + lower dashboards + menu button
        // n to n+1 = revealing another element
        // Height to All = revealing sundial + time + day
    public DashboardElementSet visibleElementSet = DashboardElementSet.All;

    public Image lowerDashboard;

    public RectTransform sunParent;
    public RectTransform sunMeter;
    public RectTransform sunDiscGreen;
    public RectTransform sunDiscOrange;
    public RectTransform sunDiscRed;

    public RectTransform waterParent;
    public RectTransform waterMeter;
    public RectTransform waterDiscGreen;
    public RectTransform waterDiscOrange;
    public RectTransform waterDiscRed;

    public RectTransform psParent;
    public RectTransform psMeter;
    public RectTransform spinner;
    public RectTransform spinnerBall;
    public Image spinnerTracer;
    public Fader ballFader;
    public bool fpsOverride = false;
    private float fpsDamp = 0f;
    private float fpsVelocity = 0f;

    public RectTransform foodParent;
    public Image foodMeter;
    public Image foodLoss;
    private FoodLossState foodLossState = FoodLossState.Normal;
    public RectTransform foodIcon;

    public RectTransform foodScoreParent;
    public Text foodScoreText;

    public Transform heightParent;
    public Text heightText;

    public RectTransform sundialParent;
    public RectTransform sundialSun;
    public RectTransform sundialLine;
    private Vector2 sunPosition;
    private float groundToSunAngle = 0f;
    private float groundToSunMagnitude = 0f;

    public Text hourText;
    public Text amPmText;
    public Text dayText;

    public Slider sunControl;
    public Slider waterControl;

    private Plant plant;

    public Bouncer heightBouncer;
    public Bouncer foodBouncer;

    public void Initialize()
    {
        plant = GV.ws.plant;
        ballFader.InitializeFader();
        if (sunControl)
            sunControl.onValueChanged.AddListener(delegate { SunControlValueChanged(); });
        if (waterControl)
            waterControl.onValueChanged.AddListener(delegate { WaterControlValueChanged(); });
        foodBouncer.InitializeBouncer();
        heightBouncer.InitializeBouncer();
        foodScoreText.text = "0";
        heightText.text = "0";
        //Application.targetFrameRate = 120;
    }

    public void Refresh(float dt)
    {
        UpdateControls();
        UpdateInputs();
        UpdatePhotosynthesis();
        UpdateSpinner(dt);
        UpdateFood(dt);
        UpdateSundial();
        UpdateDayText();
        UpdateHourText();
        UpdateHeight(dt);

        if (fpsOverride)    //dev feature: psMeter shows fps instead
        {
            //fpsDamp = (1f / dt) / 120f;
            fpsDamp = Mathf.SmoothDamp(fpsDamp, (1f / dt) / 120f, ref fpsVelocity, 0.1f);
            psMeter.eulerAngles = new Vector3(psMeter.eulerAngles.x, psMeter.eulerAngles.y, -180 * fpsDamp + 90);
        }
    }

    public void UpdateControls()
    {
        if (sunControl)
        {
            //sunControl.value = plant.sun;
            sunControl.value = Mathf.Clamp01(GV.ws.dnc.sunPosition.y * (2f / 3f));
        }
        if (waterControl)
            waterControl.value = plant.water;
    }

    public void SunControlValueChanged()
    {
        //plant.shadowFactor = Mathf.Min(sunControl.value / GV.ws.dnc.ambientSunLevel, 1f);
        //plant.sun = GV.ws.dnc.ambientSunLevel * plant.shadowFactor;
    }

    public void WaterControlValueChanged()
    {
        plant.water = waterControl.value;
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
            tracerFill = Mathf.Clamp(plant.psProgressVelocity * 0.5f, 0, 0.4f);
            spinnerTracer.gameObject.SetActive(true);
            spinnerTracer.fillAmount = tracerFill;
            spinnerTracer.rectTransform.localEulerAngles = new Vector3(spinnerTracer.rectTransform.localEulerAngles.x, spinnerTracer.rectTransform.localEulerAngles.y, (1f - tracerFill) * -360f);
        }

        float ballScale = Mathf.Clamp(tracerFill / 0.4f, 0.15f, 1f);
        spinnerBall.localScale = new Vector3(ballScale, ballScale, spinnerBall.localScale.z);

        if (plant.psDamp < 0.02f && ballFader.GetTargetAlpha() == 1f)
            ballFader.SetTargetAlpha(0f, 1.0f);
        else if (plant.psDamp >= 0.02f && ballFader.GetTargetAlpha() < 1f)
            ballFader.SetTargetAlpha(1f, 0.5f);
        ballFader.UpdateFader(dt);
        if (ballFader.IsStateChanged())
            spinnerTracer.color = ballFader.GetImage().color;
    }

    public void UpdateFood(float dt)
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
        float x = -5.5f + iconFillAmount * (-18.5f + 5.5f);
        float y = 24.5f + iconFillAmount * (416.5f - 24.5f);
        x = 0.5f + (int)x;
        y = 0.5f + (int)y;
        foodIcon.anchoredPosition = new Vector2(x, y);

        bool lockRequested = foodBouncer.lockRequested = (plant.psDamp < 0.08f);
        if (!lockRequested)
            foodBouncer.locked = false;
        foodBouncer.UpdateBouncer(dt);

        if (plant.foodScore != plant.oldFoodScore)
            foodScoreText.text = plant.foodScore.ToString();

            // bottom: A(x, y); top: B(x, y); iconFillAmount: K; position = (Ax + K(Bx - Ax), Ay + K(By- Ay))

        //iconFillAmount = Mathf.Max(iconFillAmount, 0.095f);
        //foodIcon.anchorMin = new Vector2(0.8f - iconFillAmount * 0.8f, 0f);
        //foodIcon.anchorMax = new Vector2(0.8f, iconFillAmount);
    }

    public void UpdateSundial()
    {
        float normalTime = GV.ws.dnc.normalTime;

        sunPosition = GV.GetRadialCoordinates(GV.GetSunRotation(normalTime), 45.5f, -0.5f);
        groundToSunAngle = GV.GetAngle(sunPosition);
        groundToSunMagnitude = GV.GetDistance(sunPosition);
        float lineScale = groundToSunMagnitude * (2f / 3f) / 45.5f;

        sundialSun.localPosition = sunPosition;
        sundialLine.eulerAngles = new Vector3(sundialLine.eulerAngles.x, sundialLine.eulerAngles.y, groundToSunAngle - 90f);
        sundialLine.localScale = new Vector3(sundialLine.localScale.x, lineScale, sundialLine.localScale.z);
    }

    public void UpdateDayText()
    {
        dayText.text = GV.GetWeekdayString(GV.ws.dnc.day);
    }

    public void UpdateHourText()
    {
        hourText.text = GV.ws.dnc.hour12.ToString();
        amPmText.text = (GV.ws.dnc.isMorning) ? "AM" : "PM";
    }

    public void UpdateHeight(float dt)
    {
        if (plant.height != plant.oldHeightInt)
            heightText.text = plant.heightInt.ToString();
        bool lockRequested = heightBouncer.lockRequested = !plant.isGrowing;
        if (!lockRequested)
            heightBouncer.locked = false;
        heightBouncer.UpdateBouncer(dt);
    }

    public void ShowNextElementSet()
    {
        ShowElementSet((visibleElementSet == DashboardElementSet.All) ? 0 : visibleElementSet + 1);
    }

    public void ShowElementSet(DashboardElementSet elementSet)
    {
        if (elementSet == visibleElementSet)
            return;
        switch (elementSet)
        {
            case DashboardElementSet.None:
                sunParent.gameObject.SetActive(false);
                waterParent.gameObject.SetActive(false);
                psParent.gameObject.SetActive(false);
                foodParent.gameObject.SetActive(false);
                foodScoreParent.gameObject.SetActive(false);
                heightParent.gameObject.SetActive(false);
                sundialParent.gameObject.SetActive(false);

                lowerDashboard.sprite = Resources.Load("UI/Graphics/lowerDashboard0", typeof(Sprite)) as Sprite;

                visibleElementSet = DashboardElementSet.None;
                break;
            case DashboardElementSet.Sun:
                sunParent.gameObject.SetActive(true);
                waterParent.gameObject.SetActive(false);
                psParent.gameObject.SetActive(false);
                foodParent.gameObject.SetActive(false);
                foodScoreParent.gameObject.SetActive(false);
                heightParent.gameObject.SetActive(false);
                sundialParent.gameObject.SetActive(false);

                lowerDashboard.sprite = Resources.Load("UI/Graphics/lowerDashboard1", typeof(Sprite)) as Sprite;

                visibleElementSet = DashboardElementSet.Sun;
                break;
            case DashboardElementSet.Water:
                sunParent.gameObject.SetActive(true);
                waterParent.gameObject.SetActive(true);
                psParent.gameObject.SetActive(false);
                foodParent.gameObject.SetActive(false);
                foodScoreParent.gameObject.SetActive(false);
                heightParent.gameObject.SetActive(false);
                sundialParent.gameObject.SetActive(false);

                lowerDashboard.sprite = Resources.Load("UI/Graphics/lowerDashboard2", typeof(Sprite)) as Sprite;

                visibleElementSet = DashboardElementSet.Water;
                break;
            case DashboardElementSet.Photosynthesis:
                sunParent.gameObject.SetActive(true);
                waterParent.gameObject.SetActive(true);
                psParent.gameObject.SetActive(true);
                foodParent.gameObject.SetActive(false);
                foodScoreParent.gameObject.SetActive(false);
                heightParent.gameObject.SetActive(false);
                sundialParent.gameObject.SetActive(false);

                if (visibleElementSet < DashboardElementSet.Photosynthesis)
                    lowerDashboard.sprite = Resources.Load("UI/Graphics/lowerDashboard3", typeof(Sprite)) as Sprite;

                visibleElementSet = DashboardElementSet.Photosynthesis;
                break;
            case DashboardElementSet.Food:
                sunParent.gameObject.SetActive(true);
                waterParent.gameObject.SetActive(true);
                psParent.gameObject.SetActive(true);
                foodParent.gameObject.SetActive(true);
                foodScoreParent.gameObject.SetActive(true);
                heightParent.gameObject.SetActive(false);
                sundialParent.gameObject.SetActive(false);

                if (visibleElementSet < DashboardElementSet.Photosynthesis)
                    lowerDashboard.sprite = Resources.Load("UI/Graphics/lowerDashboard3", typeof(Sprite)) as Sprite;

                visibleElementSet = DashboardElementSet.Food;
                break;
            case DashboardElementSet.Height:
                sunParent.gameObject.SetActive(true);
                waterParent.gameObject.SetActive(true);
                psParent.gameObject.SetActive(true);
                foodParent.gameObject.SetActive(true);
                foodScoreParent.gameObject.SetActive(true);
                heightParent.gameObject.SetActive(true);
                sundialParent.gameObject.SetActive(false);

                if (visibleElementSet < DashboardElementSet.Photosynthesis)
                    lowerDashboard.sprite = Resources.Load("UI/Graphics/lowerDashboard3", typeof(Sprite)) as Sprite;

                visibleElementSet = DashboardElementSet.Height;
                break;
            case DashboardElementSet.All:
                sunParent.gameObject.SetActive(true);
                waterParent.gameObject.SetActive(true);
                psParent.gameObject.SetActive(true);
                foodParent.gameObject.SetActive(true);
                foodScoreParent.gameObject.SetActive(true);
                heightParent.gameObject.SetActive(true);
                sundialParent.gameObject.SetActive(true);

                if (visibleElementSet < DashboardElementSet.Photosynthesis)
                    lowerDashboard.sprite = Resources.Load("UI/Graphics/lowerDashboard3", typeof(Sprite)) as Sprite;

                visibleElementSet = DashboardElementSet.All;
                break;
        }
    }

    public void ToggleFpsOverride()
    {
        fpsOverride = !fpsOverride;
    }
}
