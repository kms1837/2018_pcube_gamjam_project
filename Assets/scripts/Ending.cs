using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public GameObject CreditLabel;
    public GameObject AchievementIcon;
    public Text FailLabel;

    public static List<string> regret = new List<string>();

    private const int maxWidth = 400;

    private Transform AchievementGroup;

    public static List<string> achievements = new List<string>();

    void Start() {
        if (regret.Count > 0) {
            foreach (string text in regret) {
                FailLabel.text += string.Format("\n{0}\n", text);
            }
        } else {
            FailLabel.text += "\n 후회없는 완벽한 인생이였습니다.";
        }

        InvokeRepeating("creditLabelUp", 0, 1.0f/60.0f);
    }

    void Update() {
    }

    private void creditLabelUp() {
        Rect view = GameObject.Find("Canvas").transform.GetComponent<RectTransform>().rect;
        Vector2 labelPosition = CreditLabel.transform.position;

        labelPosition.y += 4.0f;

        CreditLabel.transform.position = labelPosition;

        if ((view.height * 2) < labelPosition.y) {
            CancelInvoke("creditLabelUp");
            achievementIconShow();
        }
    }

    private void achievementIconShow() {
        Rect iconSIze = AchievementIcon.GetComponent<RectTransform>().rect;
        Rect view = Camera.main.rect;
        
        int achievement = achievements.Count;
        int gap = 10;

        if (achievements.Count <= 0) {
            achievements.Add("4");
        } // 업적 후회가득 인생 획득

        float startPoint = -(maxWidth / 2) + (iconSIze.width / 2);

        float width = startPoint;
        float height = 0;

        AchievementGroup = new GameObject("AchievementIconGroup").transform;
        AchievementGroup.SetParent(GameObject.Find("Canvas").transform);

        AchievementGroup.localPosition = new Vector2(0, 0);

        for (int i=0; i< achievement; i++) {
            GameObject newIcon = Instantiate(AchievementIcon, Vector2.zero, Quaternion.identity);
            newIcon.transform.SetParent(AchievementGroup);

            Image iconImage = newIcon.transform.GetComponent<Image>();

            iconImage.color = new Color(255, 255, 255, 0);

            iconImage.overrideSprite = Resources.Load<Sprite>("img/icon" + achievements[i]);

            newIcon.transform.localPosition = new Vector2(width, height);

            width += iconSIze.width + gap;
            
            if (width > (maxWidth/2)) {
                height -= iconSIze.height + gap;
                width = startPoint;
            }
        }
        
        AchievementGroup.localPosition = new Vector2(-(gap*2), iconSIze.height);
        // issue - 중앙에 맞추기 위한 계산필요

        InvokeRepeating("achievementIconFade", 0, 1.0f / 60.0f);
    }

    private void achievementIconFade() {
        int childCount = AchievementGroup.transform.childCount;
        float fadeAceel = 0.02f;
        float startAlpha = 0.05f;
        bool nextFade = true;

        float fade = 0;

        int i;
        Image child;
        for (i = 0; i < childCount; i++) {
            child = AchievementGroup.GetChild(i).GetComponent<Image>();
            fade = nextFade ? fadeAceel : 0;
            nextFade = (child.color.a >= startAlpha);

            if (child.color.a < 255) {
                child.color = new Color(255, 255, 255, child.color.a + fade);
            }
        }

        if (childCount > 0) {
            child = AchievementGroup.GetChild(childCount - 1).GetComponent<Image>();
            if (child.color.a >= 1.0f) {
                CancelInvoke("achievementIconFade");
            }
        }
    }
}
