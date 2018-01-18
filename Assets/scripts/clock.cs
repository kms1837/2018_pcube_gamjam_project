using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clock : MonoBehaviour {
    public Text ClockLabel;
    public Text SecondLabel;
    public Text AccelLabel;

    private int hour;
    private int minute;
    private int second;

    private float clockSpeed;
    private bool secondVisualFlag;

    private float limit;

    private Coroutine clockRoutine = null;

    private const int startHour = 6;
    private const int endHour = 9;
    private const float startSpeed = 0.1f;
    private const float accel = 0.02f;
    private const float limitSecondSpeed = 0.001f;
    private const float limitMinuteSpeed = 0.05f;

	void Start () {
        hour = startHour;
        minute = 0;
        second = 0;
        clockSpeed = startSpeed;
        secondVisualFlag = false;

        limit = limitSecondSpeed;

        InvokeRepeating("clockSpeedAccel", 1.0f, 1.0f);
        clockRoutine = StartCoroutine(clockCounter());
    }
	
	void Update () {
        string minuteStr = minute / 10 <= 0 ? "0" + minute.ToString() : minute.ToString();
        string hourStr = hour / 10 <= 0 ? "0" + hour.ToString() : hour.ToString();
        string secondStr = second / 10 <= 0 ? "0" + second.ToString() : second.ToString();
        
        ClockLabel.text = hourStr + ":" + minuteStr;
        SecondLabel.text = "." + secondStr;
        AccelLabel.text = clockSpeed.ToString();
    }

    void clockSpeedAccel() {
        clockSpeed = clockSpeed > limit ? clockSpeed - accel : limit;
    }

    void secondVisual() {
        second++;
        second = second % 60;
    }

    void gameOver(){
        Debug.Log("game over");
        stopClock();

        second = 0;
    }

    public void stopClock() {
        Debug.Log("game Stop");
        CancelInvoke("secondVisual");
        StopCoroutine(clockRoutine);
    }

    IEnumerator clockCounter() {
        while ( true ) {
            if (!secondVisualFlag && clockSpeed >= limitSecondSpeed) {
                second++;
                minute = second >= 60 ? minute + 1 : minute;
                second = second % 60;
            } else {
                if (!secondVisualFlag) {
                    secondVisualFlag = true;
                    clockSpeed = 0.2f;
                    limit = limitMinuteSpeed;
                    InvokeRepeating("secondVisual", limitSecondSpeed, limitSecondSpeed);
                }
                minute++;
            }
            
            hour = minute >= 60 ? hour + 1 : hour;

            minute = minute % 60;

            if (hour >= endHour) {
                gameOver();
                yield break;
            }

            yield return new WaitForSeconds(clockSpeed);
        }
    }
}
