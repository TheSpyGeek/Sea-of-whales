using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2Script : MonoBehaviour
{
    public GameObject target;
    public GameObject target2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //Vector3 ba = new Vector3(transform.rotation.x - target.transform.rotation.x, transform.rotation.y - target.transform.rotation.y, transform.rotation.z - target.transform.rotation.z);

            transform.rotation = target.transform.rotation;
        }
    }
}
