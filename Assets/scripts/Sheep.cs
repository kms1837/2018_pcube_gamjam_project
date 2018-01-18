using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {
    public static TimingSheep_Manager TS_Manager;

    public GameObject[] SheepPreFab;//불러올 양
    public GameObject[] SheepPreFab_Mini;//누적될 미니 양

    //public Transform Sheep_TransForm; //이동시킬 양

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (SheepPreFab[0]) {
        transform.Translate(new Vector3(10.0f, 10.0f, 0.0f) * Time.deltaTime * 10);
        }
        print(transform.position);
    }

    void Move()
    {
        if (transform.position.x > 425 || transform.position.y > 164)
        {
            transform.Translate(new Vector3(-10.0f, -10.0f, 0.0f) * Time.deltaTime * 10);
            //transform.position += new Vector3(10.0f, 10.0f, 0.0f) * Time.deltaTime * 10);
            if (transform.position.x > 919 || transform.position.y < -185)
            {
                Destroy(gameObject);
            }
        }
    }
}
