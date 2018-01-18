using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class resultObject {
    public int hour;
    public int minute;
    public string success;
    public string fail;
    public List<string> message;
}

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

    private Coroutine clockRoutine;

    resultObject resultData;

    private const int startHour = 5;
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

        TextAsset jsonText = Resources.Load("json/clock_counter") as TextAsset;
        resultData = JsonUtility.FromJson<resultObject>(jsonText.text);
    }
	
	void Update () {
        string minuteStr = minute / 10 <= 0 ? "0" + minute.ToString() : minute.ToString();
        string hourStr = hour / 10 <= 0 ? "0" + hour.ToString() : hour.ToString();
        string secondStr = second / 10 <= 0 ? "0" + second.ToString() : second.ToString();
        
        ClockLabel.text = hourStr + ":" + minuteStr;
        SecondLabel.text = "." + secondStr;
        AccelLabel.text = clockSpeed.ToString();
    }

    private void clockSpeedAccel() {
        clockSpeed = clockSpeed > limit ? clockSpeed - accel : limit;
    }

    private void secondVisual() {
        second++;
        second = second % 60;
    }

    public void stopClock() {
        Debug.Log("game Stop");
        CancelInvoke("secondVisual");
        StopCoroutine(clockRoutine);

        string resultStr = string.Empty;

        int result = hour % startHour;
        if (hour == resultData.hour && minute < resultData.minute) {
            resultStr = resultData.success;
        } else {
            resultStr = result >= resultData.message.Count ? resultData.fail : resultData.message[result];
        }

        Debug.Log(resultStr);
    }

    void gameOver() {
        stopClock();

        second = 0;
    }

    private void gameResult() {

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
