using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour

{

    public float heightOfVariation;
    public float timeOfCycle;

    // Start is called before the first frame update
    void Start()
    {
        iTween.MoveBy(this.gameObject, iTween.Hash("y", heightOfVariation, "time", timeOfCycle, "looptype", "pingpong", "easetype", iTween.EaseType.easeInOutSine));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
