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
    
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "SheepPF")
           Debug.Log("닿는 중");
    }
    //private void OnTriggerStay2D(Collider other)
    //{
    //    if (other.gameObject.tag == "SheepPF")
    //        Debug.Log("닿는 중");
    //}

    //private void OnTriggerExit2D(Collider other)
    //{
    //    Debug.Log("닿기 끝");
    //}
}