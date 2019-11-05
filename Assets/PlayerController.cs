using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        transform.position =  new Vector3(transform.position.x + speed*inputX * Time.deltaTime, transform.position.y, transform.position.z + speed*inputY * Time.deltaTime);

        Debug.Log(inputX);

        //_rb.MovePosition(newPos);
    }
}
