﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countOrca : MonoBehaviour
{
    // Start is called before the first frame update

    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Nombre orca : " + GameObject.FindGameObjectsWithTag("OrcaTAG").Length;
    }
}
