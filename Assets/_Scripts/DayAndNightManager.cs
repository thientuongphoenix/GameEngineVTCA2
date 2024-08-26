using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayAndNightManager : MonoBehaviour
{
    public Text textTimeInGame;
    public float dayMultiplier = 500f;

    public Light2D light2D;
    public Gradient gradient;

    private void Update()
    {
        DateTime realTime = DateTime.Now;

        float realSecondInDay = (realTime.Hour * 3600) + (realTime.Minute * 60) +realTime.Second;
        realSecondInDay = (realSecondInDay * dayMultiplier) % 86400;
        //float gameTimeSeconds = (realSecondInDay / (24 * 60)) * (dayDuration * 60);

        int gameHours = Mathf.FloorToInt(realSecondInDay / 3600);
        int gameMinutes = Mathf.FloorToInt((realSecondInDay % 3600) / 60);

        string timeFormatted = string.Format("{0:00}:{1:00}", gameHours, gameMinutes);
        textTimeInGame.text = timeFormatted;

        ChangeColorByTime(realSecondInDay);
    }

    public void ChangeColorByTime(float seconds)
    {
        light2D.color = gradient.Evaluate(seconds / 86400);
    }
}
