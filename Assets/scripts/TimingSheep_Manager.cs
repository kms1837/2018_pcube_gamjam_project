using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingSheep_Manager : MonoBehaviour {

    public GameObject[] SheepPreFab;//불러올 양
    public GameObject[] SheepPreFab_Mini;//누적될 미니 양
    public GameObject Btn_Catch;//잡았다! 버튼
    public GameObject Round; //잡아야

    public Transform Sheep_TransForm; //이동시킬 양

    public Text Txt_MsgText;//잡았는지 표시될 텍스트
    public Text Txt_Score;//잡으면 증가될 점수

    public int Score = 0;//실제 계산될 점수 변수
    /*---------------------
     * 표정변화 오브젝트
     ----------------------*/
    public GameObject Expression_Basic;
    public GameObject Expression_Frown;
    public GameObject Expression_Success;

    public GameObject Ending_Notsleep;//엔딩

    public bool isCatch = false;//양 잡았는지 체크
    public bool isGameOver = false;//게임오버 되었는지?

    /*
     * 다른팝업작업 진행검사
     */
    public bool ispopedup = false;

    // Use this for initialization
    void Start () {
        Screen.SetResolution(Screen.width, Screen.width * 9 / 16, true); // 해상도 고정
        Txt_Score.text = Score.ToString(); // 점수 표시
    }
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * Time.deltaTime);
}

    void Return_ExpressionBasic()
    {
        Expression_Basic.SetActive(true);
        Expression_Frown.SetActive(false);
        Expression_Success.SetActive(false);
    }

    public void Btn_MsgText_Click()
    {
        if(ispopedup == false && isCatch == false)
        {
            isCatch = true;

            Expression_Basic.SetActive(false);
            Expression_Frown.SetActive(false);
            Expression_Success.SetActive(true);
            Txt_MsgText.text = "좋아!";
            Debug.Log("누름");
            Score += 1;
            Txt_Score.text = Score.ToString(); // 점수 표시

            //InvokeRepeating("Return_ExpressionBasic", 2.0f, 2.0f);
            Invoke("Return_ExpressionBasic", 1.0f);
            ispopedup = true;

            if(Score == 21) {
                GameOver();
            }
        }

        else if(ispopedup == true && isCatch == true)
        {
            isCatch = false;

            Expression_Basic.SetActive(false);
            Expression_Frown.SetActive(true);
            Expression_Success.SetActive(false);
            Txt_MsgText.text = "아...안돼...";
            Debug.Log("놓침");

            Invoke("Return_ExpressionBasic", 1.0f);
            ispopedup = false;
        }
    }

    void GameOver()
    {
        if (isGameOver == false)
        {
            isGameOver = true;
            Ending_Notsleep.SetActive(true);
        }
    }
}