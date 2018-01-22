﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimingSheep_Manager : MonoBehaviour {

    public static TimingSheep_Manager TM = null;

    public static SheepPrefab sh;

    public GameObject Btn_Catch;//잡았다! 버튼
    public GameObject Btn_Catch2;//잡았다! 버튼
    public GameObject Btn_CatchTxt;//잡았다! 버튼
    public GameObject[] SheepPreFab_Mini;//누적될 미니 양
    public GameObject Round; //
    public GameObject TS_Sheep = null;
    public Vector3 TScurrPosition;

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

        Btn_Catch.SetActive(true);
        Btn_Catch2.SetActive(false);
        Btn_CatchTxt.SetActive(false);
    }

    void Awake()
    {
        TimingSheep_Manager.TM = this;
        
    }
    // Update is called once per frame
    void Update () {
        //transform.Translate(new Vector3(0.0f, 0.0f, 1.0f) * Time.deltaTime);
        TScurrPosition = TS_Sheep.transform.position;
        print(TScurrPosition);
    }

    public void Return_ExpressionBasic()
    {
        Expression_Basic.SetActive(true);
        Expression_Frown.SetActive(false);
        Expression_Success.SetActive(false);

        Btn_Catch.SetActive(true);
        Btn_Catch2.SetActive(false);
        Btn_CatchTxt.SetActive(false);
    }

    public void Btn_MsgText_Click()
    {
        if(TScurrPosition.x > 790.0f)
        { Debug.Log("눌러야돼!"); }
        if(ispopedup == false || isCatch == false || 
            TS_Sheep.transform.position.x > 800.0f && 
            TS_Sheep.transform.position.y == 976.0f &&
            TS_Sheep.transform.position.y > 900.0f)
        {
            isCatch = true;

            Expression_Basic.SetActive(false);
            Expression_Frown.SetActive(false);
            Expression_Success.SetActive(true);

            Btn_Catch.SetActive(false);
            Btn_Catch2.SetActive(true);
            Btn_CatchTxt.SetActive(true);

            Txt_MsgText.text = "좋아!";

            Score += 1;

            Txt_Score.text = Score.ToString(); // 점수 표시
            
            Invoke("Return_ExpressionBasic", 1.0f);
            ispopedup = true;

            if(Score == 21) {
                GameOver();
            }
        }

       if(ispopedup == true || isCatch == true || 
            TS_Sheep.transform.position.y < 900.0f)
        {
            isCatch = false;

            Expression_Basic.SetActive(false);
            Expression_Frown.SetActive(true);
            Expression_Success.SetActive(false);

            Txt_MsgText.text = "아...안돼...";
            //Debug.Log("놓침");

            Invoke("Return_ExpressionBasic", 1.0f);
            ispopedup = false;
        }
    }

    public void GameOver()
    {
        TurningPoint.gameState = true;

        if (isGameOver == false)
        {
            isGameOver = true;
            Ending_Notsleep.SetActive(true);
            Time.timeScale = 0;
            GetComponent<AudioSource>().Pause();
        }

        SceneManager.LoadScene("turning_point");
    }
}