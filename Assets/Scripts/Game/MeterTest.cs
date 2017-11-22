using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterTest : MonoBehaviour
{

    public float sun = 0;
    public float water = 0;
    public float sunFactor = 0;
    public float waterFactor = 0;
    public float photosynthesis = 0;
    public float food = 0;
    public float height = 0;
    public float time = 0;
    private float sunVelocity = 0;
    private float waterVelocity = 0;
    private float photosynthesisVelocity = 0;
    private float foodVelocity = 0;
    public float sunDamp = 0;
    public float waterDamp = 0;
    public float photosynthesisDamp = 0;
    public float foodDamp = 0;
    public float dampTime = 0.25f;
    public RectTransform sunMeter;
    public RectTransform sunDiscGreen;
    public RectTransform sunDiscOrange;
    public RectTransform sunDiscRed;
    public RectTransform waterMeter;
    public RectTransform waterDiscGreen;
    public RectTransform waterDiscOrange;
    public RectTransform waterDiscRed;
    public RectTransform photosynthesisMeter;
    public Image foodMeter;
    public RectTransform foodSpinner;
    public RectTransform spinnerBall;
    public Image foodTracerGreen;
    public Image foodTracerRed;
    public RectTransform heightMeter;
    public Text heightText;
    public Transform clockMeter;
    public TextMesh clockText;

    private float fpsVelocity = 0;
    public float fpsDamp = 0;


    void Update()
    {
        time += Time.deltaTime;
        UpdateClock();
        UpdateInputs();
        UpdatePhotosynthesis();
        UpdateFood();
        DepleteWater();

        //HACK FEATURE: Photosynthesis meter shows average framerate. [0 - 1] on the meter is mapped to [0fps - 200fps].
        //fpsDamp = Mathf.SmoothDamp(fpsDamp, (1f / Time.deltaTime) / 200f, ref fpsVelocity, dampTime);
        //photosynthesisMeter.eulerAngles = new Vector3(photosynthesisMeter.eulerAngles.x, photosynthesisMeter.eulerAngles.y, -180 * fpsDamp + 90);
    }

    public void UpdateClock()
    {
        clockMeter.eulerAngles = new Vector3(clockMeter.eulerAngles.x, clockMeter.eulerAngles.y, -4 * time + 30);
        float timeNormal = (time * (24f / 90f)) + 4;
        int day = (int)(timeNormal / 24f);
        int hour = (int)(timeNormal % 12f);
        if (hour == 0)
            hour = 12;
        string amOrPm = (timeNormal % 24f < 12f) ? "am" : "pm";
        string dayString;
        switch (day)
        {
            case 0:
                dayString = "Monday";
                break;
            case 1:
                dayString = "Tuesday";
                break;
            case 2:
                dayString = "Wednesday";
                break;
            case 3:
                dayString = "Thursday";
                break;
            case 4:
                dayString = "Friday";
                break;
            default:
                dayString = "ERROR";
                break;
        }
        clockText.text = hour + "<size=36> " + amOrPm + "  " + dayString + "</size>";     
    }

    public void UpdateInputs()
    {
        sun = Mathf.Clamp(sun, 0, 1);
        water = Mathf.Clamp(water, 0, 1);

        sunDamp = Mathf.SmoothDamp(sunDamp, sun, ref sunVelocity, dampTime);
        sunMeter.eulerAngles = new Vector3(sunMeter.eulerAngles.x, sunMeter.eulerAngles.y, -180 * sunDamp + 90);

        waterDamp = Mathf.SmoothDamp(waterDamp, water, ref waterVelocity, dampTime);
        waterMeter.eulerAngles = new Vector3(waterMeter.eulerAngles.x, waterMeter.eulerAngles.y, -180 * waterDamp + 90);

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
        sunFactor = GV.SunFactor(sun);
        waterFactor = GV.WaterFactor(water);
        photosynthesis = sunFactor * waterFactor;
        photosynthesisDamp = Mathf.SmoothDamp(photosynthesisDamp, photosynthesis, ref photosynthesisVelocity, dampTime);
        photosynthesisMeter.eulerAngles = new Vector3(photosynthesisMeter.eulerAngles.x, photosynthesisMeter.eulerAngles.y, -180 * photosynthesisDamp + 90);
    }

    public void UpdateFood()
    {
        food += photosynthesis * Time.deltaTime / 80f;
        Mathf.Clamp(food, 0, 1);
        foodDamp = Mathf.SmoothDamp(foodDamp, food, ref foodVelocity, dampTime * 4);
        foodMeter.fillAmount = foodDamp;
        foodSpinner.eulerAngles = new Vector3(foodSpinner.eulerAngles.x, foodSpinner.eulerAngles.y, foodDamp * -360 * 96);
        float tracerFill = 0;
        if (foodVelocity == 0)
        {
            foodTracerGreen.gameObject.SetActive(false);
            foodTracerRed.gameObject.SetActive(false);
        }
        else if (foodVelocity > 0)
        {
            tracerFill = Mathf.Clamp(foodVelocity * 30f, 0, 0.4f);
            foodTracerGreen.gameObject.SetActive(true);
            foodTracerRed.gameObject.SetActive(false);
            foodTracerGreen.fillAmount = tracerFill;
            foodTracerGreen.rectTransform.localEulerAngles = new Vector3(foodTracerGreen.rectTransform.localEulerAngles.x, foodTracerGreen.rectTransform.localEulerAngles.y, (1f - tracerFill) * -360f);
        }
        else
        {
            tracerFill = Mathf.Clamp(foodVelocity * -30f, 0, 0.4f);
            foodTracerGreen.gameObject.SetActive(false);
            foodTracerRed.gameObject.SetActive(true);
            foodTracerRed.fillAmount = tracerFill;
            foodTracerRed.rectTransform.localEulerAngles = new Vector3(foodTracerRed.rectTransform.localEulerAngles.x, foodTracerRed.rectTransform.localEulerAngles.y, (1f - tracerFill) * 360f);
        }
        float ballScale = Mathf.Max(tracerFill / 0.4f, 0.15f);
        spinnerBall.localScale = new Vector3(ballScale, ballScale, spinnerBall.localScale.z);
    }

    public void UpdateHeight()
    {
        //height += photosynthesis * Time.deltaTime / 10;
        //heightMeter.eulerAngles = new Vector3(heightMeter.eulerAngles.x, heightMeter.eulerAngles.y, height * -360 - 90);
        //int heightInteger = (int)height;
        //int heightDecimal = (int)((height - (float)heightInteger) * 10f);
        //heightText.text = heightInteger + "<size=36>." + heightDecimal + " cm</size>";
    }

    public void DepleteWater()
    {
        water -= Time.deltaTime * (photosynthesis + sun) / 150f;
        if (water < 0)
            water = 0;
        waterFactor = GV.WaterFactor(water);
    }
}
