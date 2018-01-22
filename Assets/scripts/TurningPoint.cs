using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public string load;
    public List<string> failList; // 실패시 대사
    public string achievement;
}

public class TurningPoint : MonoBehaviour {
    //장면 상태 저장
    public static string turningFileName = string.Empty;
    public static string answerType = string.Empty; // 전에 선택한 선택지 타입
    public static int answerPoint = -1; // 전에 선택한 선택지
    public static int questionPoint = -1; // 전에 진행중인 질문
    public static bool gameState = false; // 미니게임 성공여부
    public static string failMsg = string.Empty; // 실패 메시지
    
    public static int turningPoint = 0; // 엔딩지점

    //업적
    public static int successPoint = 0;

    public Text QuestionLabel;
    public GameObject AnswerBtn;
    public AudioSource source;
    public AudioClip AnswerEffectSound;
    public GameObject startPanel;

    turningObject turningData;

    private int typingPoint;

    private questionObject nowQuetion;
    private Transform answerGroup;

    private float answerDeg;

    const float typingSpeed = 0.1f;
    const float answerRotationSpeed = 5.0f;
    const int endingPoint = 5;

    private Rect originResolution;
    private Rect resizeResolution;

    void Awake()
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        Rect originSize = canvas.GetComponent<RectTransform>().rect;
        originResolution = new Rect(0, 0, originSize.width, originSize.height);
        resizeResolution = new Rect(0, 0, originResolution.width * canvas.localScale.x, originResolution.width * canvas.localScale.x);
    }

    private void Start () {
        QuestionLabel.text = "";

        if (turningFileName == string.Empty) {
            startPanel.SetActive(true);
            InvokeRepeating("startFade", 0, 1.0f / 60.0f);
            Invoke("endStart", 5.0f);
        } else {
            startPanel.SetActive(false);
            reLoadGame();
        }
    }

    private void Update () {
    }

    private void pushFail(answernObject answern) {
        failMsg = failMsg == string.Empty ? "후회된다..." : failMsg;
        failMsg = string.Format("[{0}]\n{1}", answern.text, failMsg);
        Ending.regret.Add(failMsg);
    }

    private void reLoadGame() {
        loadJSon(turningFileName);
        nowQuetion = turningData.turning[questionPoint];

        if (answerType == "game") {
            answernObject selectAnswern = nowQuetion.answer[answerPoint];
            if (gameState) {
                successPoint++;
                if (selectAnswern.achievement != null) {
                    Ending.achievements.Add(selectAnswern.achievement);
                }
            } else {
                pushFail(selectAnswern);
            }

            Debug.Log(nowQuetion.question);
            Debug.Log("==========================");
            Debug.Log(answerType);
            Debug.Log("qp :" + questionPoint);
            Debug.Log("ap :" + answerPoint.ToString());
            Debug.Log("sp :" + selectAnswern.next);
            Debug.Log("==========================");

            int nextIndex = int.Parse(selectAnswern.next);
            nextQuestion(nextIndex);
        }
    }

    private void loadJSon(string fileName) {
        TextAsset jsonText = Resources.Load(fileName) as TextAsset;
        turningData = JsonUtility.FromJson<turningObject>(jsonText.text);

        turningFileName = fileName;

        nowQuetion = turningData.turning[0];
    }

    private void selectAnswer(int selectNumber) {
        answernObject answerResult = nowQuetion.answer[selectNumber];
        source.PlayOneShot(AnswerEffectSound);

        answerType = answerResult.type;
        answerPoint = selectNumber;

        if (answerGroup != null) {
            Destroy(answerGroup.gameObject);
            answerGroup = null;
        }

        if (answerResult.failList.Count > 0) {
            failMsg = answerResult.failList[Random.Range(0, answerResult.failList.Count)];
        }

        int nextIndex = int.Parse(answerResult.next);
        switch (answerResult.type) {
            case "achievement":
                Ending.achievements.Add(answerResult.achievement);
                nextQuestion(nextIndex);
                break;
            case "next":
                nextQuestion(nextIndex);
                break;
            case "fail":
                pushFail(answerResult);
                nextQuestion(nextIndex);
                break;
            case "game":
                moveScene(answerResult.load);
                break;
            case "load":
                loadJSon(answerResult.load);
                turningPoint = turningPoint + 1;
                if (turningPoint >= endingPoint) {
                    moveScene("ending");
                }
                break;
            case "ending":
                moveScene("ending");
                break;
        }
    }

    private void nextQuestion(int nextIndex) {
        questionPoint = nextIndex;
        nowQuetion = turningData.turning[nextIndex];
        runTyping();
    }

    private void moveScene(string sceneName) {
        CancelInvoke();
        Debug.Log("==========================");
        Debug.Log("qp :" + questionPoint);
        Debug.Log("ap :" + answerPoint.ToString());
        Debug.Log("sp :" + nowQuetion.answer[answerPoint].next);
        Debug.Log("==========================");
        SceneManager.LoadScene(sceneName);
    }

    private void duplicateAnswer() {
        Transform view = GameObject.Find("Canvas").transform;
        int answerCounter = nowQuetion.answer.Count;

        answerGroup = new GameObject("AnswerGroup").transform;
        answerGroup.SetParent(GameObject.Find("Canvas").transform);
        answerGroup.localPosition = Vector3.zero;

        int index = 0;
        foreach (answernObject answer in nowQuetion.answer) {
            GameObject newBtn = Instantiate(AnswerBtn, new Vector2(Screen.width, Screen.height), Quaternion.identity);
            Button btnObj = newBtn.GetComponent<Button>();
            int localIndex = index;
            btnObj.onClick.AddListener(() => { selectAnswer(localIndex); });

            Text btnText = newBtn.transform.Find("Text").GetComponent<Text>();
            btnText.text = answer.text;

            newBtn.transform.SetParent(answerGroup);
            newBtn.transform.localPosition = new Vector2(300, 300);

            newBtn.transform.localScale = new Vector3(view.localScale.x, view.localScale.y, 1);

            if (index >= (answerCounter/2)) {
                newBtn.transform.localScale = new Vector3(-view.localScale.x, view.localScale.y, 1);
                newBtn.transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
            }

            index++;
        }

        answerDeg = 0;
        InvokeRepeating("answerRotation", 0, 1.0f / 60.0f);
    }

    private void answerRotation() {
        Transform view = GameObject.Find("Canvas").transform;
        Rect btnSize = AnswerBtn.GetComponent<RectTransform>().rect;
        float width = (resizeResolution.width / 2) - ((btnSize.width * view.localScale.x)/4);
        float height = (resizeResolution.height / 2) - ((btnSize.height * view.localScale.y)/2);
        // 버튼 최대 위치 (btnSize.width * view.localScale.x은 해상도 변경후의 버튼 사이즈 수치입니다)
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
            ry = height * Mathf.Sin(rad) + ((btnSize.height * view.localScale.y));
            // 버튼 위치
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

    private void startFade() {
        Image panel = startPanel.transform.GetChild(0).GetComponent<Image>();
        Transform textObj = startPanel.transform.GetChild(1);
        Text text = textObj.transform.GetComponent<Text>();
        
        if (panel.color.a > 0.8f) {
            float mx = textObj.position.x + 0.8f;
            text.color = new Color(255, 255, 255, text.color.a + 0.01f);
            textObj.position = new Vector2(mx, textObj.position.y);
        } else {
            panel.color = new Color(0, 0, 0, panel.color.a + 0.01f);
        }
    }

    private void startFadeOut()
    {
        Image panel = startPanel.transform.GetChild(0).GetComponent<Image>();
        Transform textObj = startPanel.transform.GetChild(1);
        Text text = textObj.transform.GetComponent<Text>();

        panel.color = new Color(0, 0, 0, panel.color.a - 0.01f);
        text.color = new Color(255, 255, 255, text.color.a - 0.01f);

        if (panel.color.a <= 0.0f) {
            startPanel.SetActive(false);

            loadJSon("json/turning2");
            questionPoint = 0;
            runTyping();
            CancelInvoke("startFadeOut");
        }
    }

    private void endStart() {
        InvokeRepeating("startFadeOut", 0, 1.0f / 60.0f);
        CancelInvoke("startFade");
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
