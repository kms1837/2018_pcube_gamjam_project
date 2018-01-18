using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clock : MonoBehaviour {
    public Text ClockLabel;
    public Text AccelLabel;
    // Use this for initialization

    private int hour;
    private int minute;
    private int second;

    private float clockSpeed;

    private const float limitSpeed = 0.02f;

	void Start () {
        hour = 0;
        minute = 0;
        second = 0;
        clockSpeed = 1.0f;

        InvokeRepeating("clockSpeedAccel", 1.0f, 1.0f);
        StartCoroutine(clockCounter());
    }
	
	// Update is called once per frame
	void Update () {
        string minuteStr = minute / 10 <= 0 ? "0" + minute.ToString() : minute.ToString();
        string hourStr = hour / 10 <= 0 ? "0" + hour.ToString() : hour.ToString();
        ClockLabel.text = hourStr + ":" + minuteStr;

        AccelLabel.text = clockSpeed.ToString();
    }

    void clockSpeedAccel() {
        clockSpeed = clockSpeed > limitSpeed ? clockSpeed - 0.05f : limitSpeed;
    }

    IEnumerator clockCounter() {
        while ( true ) {
            minute++;
            minute = minute % 60;
            hour = minute == 0 ? hour + 1 : hour;

            yield return new WaitForSeconds(clockSpeed);
        }
    }
}
