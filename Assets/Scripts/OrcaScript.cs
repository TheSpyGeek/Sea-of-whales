using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public double lonelinessWhaleDistance = 20;


    private string currentState;

    // Start is called before the first frame update
    void Start() {
        // Le changment de direction d'un groupe est commun et dans le World !!!

        spead = wiggleSpead;
        currentState = "StateWiggle";
        worldObject = GameObject.Find("World");

        FindMyTeamates();
        
    }

    // Update is called once per frame
    void Update() {

        switch (currentState) {
            case "StateWiggle":
                Wiggle(worldObject.GetComponent<WorldScript>().orcaGroupVector[groupeID]);
                break;

            case "StateHunt":
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
            countdownToRotate = baseCountdowToRotate;
            transform.Rotate(rotation);

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

        transform.Translate(new Vector3(0, 0, wiggleSpead) * Time.deltaTime);

        

    }


    bool BeginTheHunt() {
        GameObject[] whaleTab = GameObject.FindGameObjectsWithTag("WhaleTAG");
        
        foreach (GameObject w in whaleTab) {
            double distance = Math.Sqrt(Math.Pow(w.transform.position.x - transform.position.x, 2) + Math.Pow(w.transform.position.y - transform.position.y, 2) + Math.Pow(w.transform.position.z - transform.position.z, 2));
            if (distance < maxDetectionWhale && !w.GetComponent<WhaleScript>().GetNeighbors(lonelinessWhaleDistance)) {
                target = w;
                return true;
            }
        }

        return false;
    }

    void Hunt() {
        if (target != null) { // Doute si le null correspond bien comme test 
            if (orcaID == 0) {
                transform.LookAt(target.transform);
                double distance = Math.Sqrt(Math.Pow(target.transform.position.x - transform.position.x, 2) + Math.Pow(target.transform.position.y - transform.position.y, 2) + Math.Pow(target.transform.position.z - transform.position.z, 2));
                if (distance < 10) {
                    Destroy(target.gameObject);
                    target = null;
                }
            }

            else {
                // CA C EST DE LA MERDE
                float angle = transform.rotation.y - myQueenOrca.transform.rotation.y;
                transform.Rotate(new Vector3(0, angle, 0));
            }
            transform.Translate(new Vector3(0, 0, huntSpead) * Time.deltaTime);
        }
        else {
            currentState = "StateWiggle";
        }
    }



}
