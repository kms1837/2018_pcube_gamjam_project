using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingCircle : MonoBehaviour {

    public GameObject target;

    public bool isTargeted;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("닿기 시작");
        //if (collision.gameObject.tag == "SheepPF")
        //{
        //    target = collision.gameObject;
        //    if (!isTargeted)   // 순찰 상태
        //    {
        //        GetComponentInParent<>().targeting(collision.gameObject);
        //        collider2d.size = new Vector2(attackRange, collider2d.size.y);
        //        isTargeted = true;
        //    }
        //    else //쫒는 상태
        //    {
        //        StartCoroutine(waitAndAttack());
        //        GetComponentInParent<following>().isAttacking = true;
        //    }
        //}
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "CirCle")
            Debug.Log("닿는 중");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("닿기 끝");
    }
}
