using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    public GameObject gamedata;

    void Awake()
    {
        if (TimingSheep_Manager.TM == null)
            Instantiate(gamedata);
    }

}
