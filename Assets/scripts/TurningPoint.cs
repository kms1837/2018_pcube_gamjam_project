using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
class turningObject
{
    public List<questionObject> turning;
}

[System.Serializable]
class questionObject
{
    public string question;
    public List<answernObject> answer;
}

[System.Serializable]
class answernObject
{
    public string type;
    public string text;
    public string next;
}

public class TurningPoint : MonoBehaviour {
    public Text QuestionLabel;
    public GameObject AnswerBtn;

    turningObject turningData;

    private int typingPoint;

    private questionObject nowQuetion;
    private Transform answerGroup;

    private float answerDeg;

    const float typingSpeed = 0.1f;
    const float answerRotationSpeed = 5.0f;

    void Start () {
        TextAsset jsonText = Resources.Load("json/turning") as TextAsset;
        turningData = JsonUtility.FromJson<turningObject>(jsonText.text);

        nowQuetion = turningData.turning[1];

        runTyping();
    }
	
	void Update () {
		
	}

    private void selectAnswer(int selectNumber) {
        answernObject answerResult = nowQuetion.answer[selectNumber];

        if (answerGroup != null) {
            Destroy(answerGroup.gameObject);
            answerGroup = null;
        }

        switch (answerResult.type) {
            case "next":
                int nextIndex = int.Parse(nowQuetion.answer[selectNumber].next);
                nowQuetion = turningData.turning[nextIndex];
                runTyping();
                break;
            case "game":
                break;
            case "ending":
                break;
        }
    }

    private void duplicateAnswer() {
        Rect view = Camera.main.rect;

        answerGroup = new GameObject("answerGroup").transform;
        answerGroup.SetParent(GameObject.Find("Canvas").transform);
        answerGroup.localPosition = Vector3.zero;

        int index = 0;
        foreach (answernObject answer in nowQuetion.answer) {
            GameObject newBtn = Instantiate(AnswerBtn, new Vector2(view.width, view.height), Quaternion.identity);
            Button btnObj = newBtn.GetComponent<Button>();
            int localIndex = index;
            btnObj.onClick.AddListener(() => { selectAnswer(localIndex); });

            Text btnText = newBtn.transform.Find("Text").GetComponent<Text>();
            btnText.text = answer.text;

            newBtn.transform.SetParent(answerGroup);
            newBtn.transform.localPosition = new Vector2(300, 300);

            index++;
        }

        answerDeg = 0;
        InvokeRepeating("answerRotation", 0.02f, 0.02f);
    }

    private void answerRotation() {
        Rect view = Camera.main.rect;
        float width = (view.width / 2) - 130;
        float height = (view.height / 2) - 380;
        float rx = 0.0f, ry = 0.0f;
        float rad = 0.0f;

        int childCount = answerGroup.transform.childCount;

        answerDeg += answerRotationSpeed;

        float deg = answerDeg / (childCount + 1);

        for (int i = 0; i < childCount; i++) {
            Transform child = answerGroup.GetChild(i);
            Vector2 childPosition = child.localPosition;

            rad = (deg * (i+1)) * (Mathf.PI / 180);
            rx = width * Mathf.Cos(rad);
            ry = height * Mathf.Sin(rad) + 250;
            child.localPosition = new Vector2(rx, ry);
        }

        if (answerDeg >= 180.0f) {
            CancelInvoke("answerRotation");
        }
        
    }

    private void runTyping() {
        typingPoint = 0;
        QuestionLabel.text = "";
        InvokeRepeating("typing", typingSpeed, typingSpeed);
    }

    private void typing() {
        if (typingPoint >= nowQuetion.question.Length) {
            CancelInvoke("typing");
            QuestionLabel.text = nowQuetion.question;

            duplicateAnswer();
            return;
        }

        QuestionLabel.text += nowQuetion.question[typingPoint];
        typingPoint++;
    }
}
