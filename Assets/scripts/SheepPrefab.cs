using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepPrefab : MonoBehaviour
{
    public static SheepPrefab SP;

    //public GameObject[] SheepPreFab;//불러올 양
    public GameObject SheepPF;//불러올 양
    public Transform CanvasS;

    public Transform SheepTransForm;

    public GameObject SheepObject;
    public int Sheepcount = 0; //양 몇 마리 지나갔니?

    /*------------------------- 
     * 이동해야될 위치 배열
     * 현재 위치
     * 이동위치 인덱스
     -------------------------*/
    private Vector3[] PositionPoints;
    private Vector3 currPosition;
    private int PointsIndex = 0;

    private float distanceLength_SC;//시작~중간 거리
    private float distanceLength_CE;//중간~마지막 거리

    //몬스터를 발생시킬 주기
    public float createTime;
    float times;

    public bool isMoveClick = false; //해당 위치에 눌렀는지?

    //public Transform Sheep_TransForm; //이동시킬 양

    // Update is called once per frame
    void Update()
    {
        //Move();
        //if (SheepPreFab[0]) {
        //}
        //print(transform.position);

        //print(Sheepcount);
        times += Time.deltaTime;

        if (times > 5)
        {
            times = 0;
            GameObject Sheep = Instantiate(SheepPF, Vector2.zero, Quaternion.identity) as GameObject;
            Sheep.transform.SetParent(CanvasS);
            Sheep.transform.localPosition = new Vector3(-94f, -183f, 0);
            Sheepcount++;
            //StartCoroutine(this.createSheep());
        }
        //print(Sheepcount);
        
        if(Sheepcount > 20)
        {
            GetComponent<AudioSource>().Pause();
            TimingSheep_Manager.TM.GameOver();
        }
    }
    
    //IEnumerator createSheep()
    //{
    //    while (!TS_Manager.isGameOver && PointsIndex < PositionPoints.Length)
    //    {
    //        int SheepCount = (int)GameObject.FindGameObjectsWithTag("SheepPreFab").Length;

    //        if (SheepCount < 2)
    //        {
    //            yield return new WaitForSeconds(createTime);

    //            Instantiate(SheepPreFab);
    //            gameObject.SetActive(true);
    //        }
    //        else yield return null;
    //    }
    //}
}