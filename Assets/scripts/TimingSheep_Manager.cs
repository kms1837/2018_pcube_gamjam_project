using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimingSheep_Manager : MonoBehaviour {

    public GameObject Btn_Catch; //잡았다! 버튼
    public GameObject Btn_CatchTxt; //잡았다! 버튼

    public Text Txt_MsgText;//잡았는지 표시될 텍스트
    public Text Txt_Score;//잡으면 증가될 점수

    private int score = 0;//실제 계산될 점수 변수

    /*---------------------
     * 표정변화 오브젝트
     ----------------------*/
    public GameObject Expression_Basic;
    public GameObject Expression_Frown;
    public GameObject Expression_Success;

    public GameObject Ending_Notsleep;//엔딩

    /*
     * 다른팝업작업 진행검사
     */
    public bool ispopedup = false;

    public Transform MiniSheep; // 복제할 작은양
    public Transform SheepGroup; // 복제 양이 속한 부모 오브젝트
    public Transform MiniSheepGroup; // 작은 복제 양이 속한 부모 오브젝트
    public Transform CatchCircle; // 잡는 기준 오브젝트
    private int duplicateSheep; // 복제한 양의 수

    private const int SuccessValue = 10; // 성공조건
    private const int MaxDuplication = 20; // 최대복제수

    private void Start () {
        Txt_Score.text = score.ToString(); // 점수 표시

        Btn_Catch.SetActive(true);
        Btn_CatchTxt.SetActive(false);

        duplicateSheep = 1;

        Return_ExpressionBasic();
    }

    public void sheepCountUp() {
        duplicateSheep++;

        if (duplicateSheep > MaxDuplication) {
            gameOver();
        }
    }

    public void catchSheep() {
        Return_ExpressionBasic();

        Expression_Basic.SetActive(false);

        Sheep sheepObj = SheepGroup.GetChild(0).GetComponent<Sheep>();
        if (sheepObj.Triger) {
            score += 1;
            Txt_Score.text = score.ToString(); // 점수 표시
            Txt_MsgText.text = "좋아!";

            Transform newMiniSheep = Instantiate(MiniSheep, MiniSheepGroup).transform;

            Rect groupRect = MiniSheepGroup.GetComponent<RectTransform>().rect;
            Rect sheepRect = MiniSheep.GetComponent<RectTransform>().rect;
            float groupWidth = (groupRect.width / 2) -  (sheepRect.width/2);
            float groupHeight = (groupRect.height / 2) - (sheepRect.height / 2);

            newMiniSheep.localPosition = new Vector2(Random.Range(groupWidth * -1, groupWidth), Random.Range(groupHeight*-1, groupHeight));
            newMiniSheep.Rotate(0, 0, Random.Range(0, 360));

            Expression_Success.SetActive(true);
            Btn_CatchTxt.SetActive(true);

        } else {
            Expression_Frown.SetActive(true);

            Txt_MsgText.text = "아...안돼...";
        }

        this.CancelInvoke();
        Invoke("Return_ExpressionBasic", 1.0f);

        sheepObj.newLife();
    }

    public void Return_ExpressionBasic()
    {
        Expression_Basic.SetActive(true);
        Expression_Frown.SetActive(false);
        Expression_Success.SetActive(false);

        Btn_Catch.SetActive(true);
        Btn_CatchTxt.SetActive(false);

        Txt_MsgText.text = "Zzz...";
    }
    
    public void gameOver()
    {
        this.CancelInvoke();

        TurningPoint.gameState = true;

        Destroy(SheepGroup.gameObject);
        Destroy(Btn_Catch.gameObject);

        if (score < SuccessValue) {
            Ending_Notsleep.SetActive(true);
            GetComponent<AudioSource>().Pause();
            TurningPoint.gameState = false;
        }

        Invoke("endedMoveScene", 3.0f);
    }

    private void endedMoveScene() {
        SceneManager.LoadScene("turning_point");
    }
}