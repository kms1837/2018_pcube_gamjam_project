﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {
    public static TimingSheep_Manager TS_Manager;

    //public GameObject[] SheepPreFab;//불러올 양
    public GameObject SheepPreFab;//불러올 양
    public GameObject[] SheepPreFab_Mini;//누적될 미니 양

    public Transform SheepTransForm;
    public int Sheepcount = 0; //양 몇 마리 지나갔니?

    /*------------------------- 
     * 시작 위치, 
     * 잡아야할 중간 위치, 
     * 없어지는 마지막 위치
     * 이동해야될 위치 배열
     * 현재 위치
     * 이동위치 인덱스
     -------------------------*/
    //private Vector3 StartPosition;
    //private Vector3 CenterPosition;
    //private Vector3 EndPosition;
    private Vector3[] PositionPoints;
    private Vector3 currPosition;
    private int PointsIndex = 0;

    private float distanceLength_SC;//시작~중간 거리
    private float distanceLength_CE;//중간~마지막 거리

    //몬스터를 발생시킬 주기
    public float createTime;

    public bool isMoveClick = false; //해당 위치에 눌렀는지?

    //public Transform Sheep_TransForm; //이동시킬 양

    // Use this for initialization
    void Start () {
        //StartPosition = new Vector3(356.0f, 617.0f);
        //CenterPosition = new Vector3(765.0f, 1026.0f);
        //EndPosition = new Vector3(1365.4f, 617.0f);

        PositionPoints = new Vector3[3];

        PositionPoints.SetValue(new Vector3(356.0f, 617.0f, 0), 0);//처음
        PositionPoints.SetValue(new Vector3(765.0f, 1026.0f, 0), 1);//가운데
        PositionPoints.SetValue(new Vector3(1365.4f, 617.0f, 0), 2);//사라질 마지막 위치
    }
	
	// Update is called once per frame
	void Update () {
        //Move();
        //if (SheepPreFab[0]) {
            Move();
        //}
        //print(transform.position);
        
        //print(Sheepcount);
        
        //SheepPreFab = Instantiate(SheepPreFab) as GameObject;
        ////if (SheepPreFab.Length > 0)
        ////{
        ////    StartCoroutine(this.createSheep());
        ////}
        //StartCoroutine(this.createSheep());
    }

    void Move()
    {
        currPosition = transform.position; //현재 위치 받아옴

        if (PointsIndex < PositionPoints.Length)
        {
            float speed = 100 * Time.deltaTime;

            transform.position = Vector3.MoveTowards(currPosition, PositionPoints[PointsIndex], speed);
        }

        if (Vector3.Distance(PositionPoints[PointsIndex], currPosition) == 0f)
        {
            PointsIndex++;
            if(Vector3.Distance(PositionPoints[2], currPosition) == 0f)
            {
                Destroy(gameObject);
                Sheepcount++;
                PointsIndex = 0;
            }
        }

        if(Vector3.Distance(PositionPoints[1], currPosition) == 0f)
        {
            GetComponent<TimingSheep_Manager>().isCatch = true;
            isMoveClick = true;
        }

        isMoveClick = false;


    }

    //IEnumerator createSheep()
    //{
    //    while(!TS_Manager.isGameOver && PointsIndex < PositionPoints.Length)
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