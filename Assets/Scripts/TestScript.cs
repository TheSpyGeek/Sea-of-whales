using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {

            //transform.LookAt(new Vector3(0,0,0));
            transform.LookAt(target.transform);
            transform.Translate(new Vector3(0, 0, 2.0f) * Time.deltaTime);
        }
    }
}
