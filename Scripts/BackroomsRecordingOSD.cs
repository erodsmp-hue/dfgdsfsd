using UnityEngine;
using TMPro;
using System;

public class BackroomsRecordingOSD : MonoBehaviour
{
    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI recText;
    [SerializeField] private TextMeshProUGUI timecodeText;
    [SerializeField] private TextMeshProUGUI dateText;

    [Header("Ghost Layers (Chromatic Aberration)")]
    [SerializeField] private TextMeshProUGUI recRed;
    [SerializeField] private TextMeshProUGUI recBlue;
    [SerializeField] private TextMeshProUGUI timeRed;
    [SerializeField] private TextMeshProUGUI timeBlue;
    [SerializeField] private TextMeshProUGUI dateRed;
    [SerializeField] private TextMeshProUGUI dateBlue;

    [Header("Settings")]
    [SerializeField] private int fakeYear = 1996;
    [SerializeField] private float blinkRate = 0.8f;
    [SerializeField] private float bootDuration = 2.5f;

    private float timer;
    private float bootTimer;
    private bool isBooting = true;

    private void Start()
    {
        bootTimer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        bootTimer += Time.deltaTime;

        // Calculate analog tape hum for the ghost layers
        float hum = 0.3f + Mathf.Sin(Time.time * 15f) * 0.1f;
        Color rGhost = new Color(1f, 0f, 0f, hum);
        Color bGhost = new Color(0f, 0.5f, 1f, hum);

        // 1. Boot-Up Sequence (VCR PLAY mode)
        if (isBooting)
        {
            if (bootTimer < bootDuration)
            {
                // Flashing PLAY text
                string bootStr = (bootTimer % 0.5f < 0.25f) ? "" : "<cspace=0.1em>PLAY \u25B6</cspace>"; 
                UpdateTextGroup(recText, recRed, recBlue, bootStr, rGhost, bGhost);
                UpdateTextGroup(timecodeText, timeRed, timeBlue, "", rGhost, bGhost);
                UpdateTextGroup(dateText, dateRed, dateBlue, "", rGhost, bGhost);
                return;
            }
            else
            {
                isBooting = false;
                timer = 0f; // Reset timecode to 00:00:00 when recording actually starts
            }
        }

        // 2. REC Mode & SP Indicator
        string recStr = (timer % (blinkRate * 2) < blinkRate) 
            ? "<cspace=0.1em>REC <color=#FF0000>•</color>  SP</cspace>" 
            : "<cspace=0.1em>REC    SP</cspace>";
        UpdateTextGroup(recText, recRed, recBlue, recStr, rGhost, bGhost);

        // 3. Timecode (00:00:00)
        TimeSpan t = TimeSpan.FromSeconds(timer);
        string timeStr = string.Format("<cspace=0.1em>{0:D2}:{1:D2}:{2:D2}</cspace>", t.Hours, t.Minutes, t.Seconds);
        UpdateTextGroup(timecodeText, timeRed, timeBlue, timeStr, rGhost, bGhost);

        // 4. 90s Camcorder Date
        DateTime now = DateTime.Now;
        DateTime fakeDate = new DateTime(fakeYear, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        string dateStr = "<cspace=0.1em>" + fakeDate.ToString("MMM. dd yyyy\nHH:mm:ss").ToUpper() + "</cspace>";
        UpdateTextGroup(dateText, dateRed, dateBlue, dateStr, rGhost, bGhost);
    }

    private void UpdateTextGroup(TextMeshProUGUI main, TextMeshProUGUI red, TextMeshProUGUI blue, string text, Color rCol, Color bCol)
    {
        if (main != null) main.text = text;
        if (red != null) { red.text = text; red.color = rCol; }
        if (blue != null) { blue.text = text; blue.color = bCol; }
    }
}