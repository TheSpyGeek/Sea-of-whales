using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSeason : MonoBehaviour
{

    public GameObject active;
    public GameObject notactive;

    public WorldScript world;

 

    // Update is called once per frame
    void Update()
    {
        if (world.season)
        {
            active.SetActive(true);
            notactive.SetActive(false);
        } else
        {
            active.SetActive(false);
            notactive.SetActive(true);
        }
    }
}
