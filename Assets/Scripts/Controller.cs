using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Controller : MonoBehaviour
{

    public float speed = 0.5f;
    Rigidbody _rigid;

    private Vector2 goTo;

    // Start is called before the first frame update
    void Start()
    {
        goTo = new Vector2(10,10);
        _rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){

        moveWhale();


    }



    void moveWhale() {
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);

        Vector2 direction = goTo - pos;

        if(direction.sqrMagnitude > 0.6f) {

            float angle = Vector2.Dot(direction.normalized, new Vector2(transform.forward.x, transform.forward.z).normalized);

            Debug.Log(angle);

            Quaternion qua = new Quaternion();
            qua.eulerAngles = new Vector3(0, angle, 0);

            transform.rotation = qua;

            Vector3 newPos = transform.position + transform.forward * speed * Time.deltaTime;
            


            //_rigid.MovePosition(newPos);
        }
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(goTo.x, 0, goTo.y), 0.5f);
    }
}
