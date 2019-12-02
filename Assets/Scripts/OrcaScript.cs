using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class OrcaScript : MonoBehaviour {

    public int orcaID;
    public int groupeID;

    public GameObject worldObject;

    public GameObject myQueenOrca;
    public List<GameObject> orcaTeamates;


    public float wiggleSpead = 1.5f;
    public float huntSpead = 2.5f;
    private float spead;
    public int maxAngleRotation = 40;
    public int baseCountdowToRotate = 500;
    private int countdownToRotate;

    
    private GameObject target = null;
    public double maxDetectionWhale = 200;
    public float lonelinessWhaleDistance = 20;


    private NavMeshAgent agent;
    private string currentState;

    // Start is called before the first frame update
    void Start() {
        // Le changment de direction d'un groupe est commun et dans le World !!!

        spead = wiggleSpead;
        currentState = "StateWiggle";
        worldObject = GameObject.Find("World");

        FindMyTeamates();

        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update() {

        switch (currentState) {
            case "StateWiggle":
                agent.acceleration = 10;
                agent.speed = 6;
                Wiggle(worldObject.GetComponent<WorldScript>().orcaGroupVector[groupeID]);
                break;

            case "StateHunt":
                agent.speed = 15;
                agent.acceleration = 15;
                Hunt();
                break;

            default:
                break;

        }
    }

    void FindMyTeamates() {
        GameObject[] OrcaTab = GameObject.FindGameObjectsWithTag("OrcaTAG");
        foreach (GameObject o in OrcaTab) {
            if (o.GetComponent<OrcaScript>().groupeID == groupeID) {
                if (o.GetComponent<OrcaScript>().orcaID == 0) {
                    myQueenOrca = o;
                }
                orcaTeamates.Add(o);
            }
        }

    }


    void Wiggle(Vector3 rotation) {
        if (countdownToRotate < 0)
        {
            //countdownToRotate = baseCountdowToRotate;
            //transform.Rotate(rotation);

            //Vector3 destination = Vector3.RotateTowards(myQueenOrca.transform.forward, Vector3.up, rotation.y, 180);
            //agent.SetDestination(myQueenOrca.transform.position+ destination * 100);


            agent.SetDestination(rotation);


            if (orcaID == 0 && BeginTheHunt()) {
                currentState = "StateHunt";
                GameObject[] OrcaTab = GameObject.FindGameObjectsWithTag("OrcaTAG");
                foreach (GameObject o in OrcaTab) {
                    if (o.GetComponent<OrcaScript>().groupeID == groupeID)
                    {
                        o.GetComponent<OrcaScript>().currentState = "StateHunt";
                        print("HUNT");
                    }

                }
            }

        }
        else countdownToRotate -= 1;



        WrapAround();

    }

    void WrapAround()
    {
        if (transform.position.x < 0) transform.position = new Vector3(1000, transform.position.y, transform.position.z);
        if (transform.position.z < 0) transform.position = new Vector3(transform.position.x, transform.position.y, 1000);
        if (transform.position.x > 1000) transform.position = new Vector3(0, transform.position.y, transform.position.z);
        if (transform.position.z > 1000) transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }


    bool BeginTheHunt() {
        GameObject[] whaleTab = GameObject.FindGameObjectsWithTag("WhaleTAG");

        int maxcount = 0;
        const int nbWeakWhale = 3;

        foreach (GameObject w in whaleTab) {

            double distance = Math.Sqrt(Math.Pow(w.transform.position.x - transform.position.x, 2) + Math.Pow(w.transform.position.y - transform.position.y, 2) + Math.Pow(w.transform.position.z - transform.position.z, 2));
            if(distance < maxDetectionWhale &&  w.GetComponent<WhaleScript>().GetNeighbors(lonelinessWhaleDistance)) {
                target = w;
                Debug.Log("BEGIN HUNT");

                return true;
            }
        }

        return false;
    }

    void Hunt() {
        if (target != null) { // Doute si le null correspond bien comme test 
            if (orcaID == 0) {
                //transform.LookAt(target.transform);
                agent.SetDestination(target.transform.position);
                double distance = Math.Sqrt(Math.Pow(target.transform.position.x - transform.position.x, 2) + Math.Pow(target.transform.position.y - transform.position.y, 2) + Math.Pow(target.transform.position.z - transform.position.z, 2));
                if (distance < 10) {
                    Destroy(target.gameObject);
                    target = null;
                }
            }

            else {
                transform.rotation = myQueenOrca.transform.rotation;
                //Vector3 destination = Vector3.RotateTowards(myQueenOrca.transform.forward, Vector3.up, myQueenOrca.transform.rotation.y, 180);
                //agent.SetDestination(myQueenOrca.transform.position + destination * 100);
            }
            //transform.Translate(new Vector3(0, 0, huntSpead) * Time.deltaTime);
        }
        else {
            currentState = "StateWiggle";
        }
    }



}
