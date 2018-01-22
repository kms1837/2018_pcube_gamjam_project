using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {
    void Awake()
    {
        Screen.SetResolution(500, 900, false);
    }
    // Use this for initialization
    void Start () {
        SceneManager.LoadScene("turning_point");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
