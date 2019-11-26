using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhaleScript : MonoBehaviour {

    // A mettre dans la classe World :
    public int timeDilataion = 1000;
    public GameObject worldObject;


    private int id;
    private bool sex;
    public int maxLifeExperence = 50;
    public int minLifeExperence = 20;
    private int lifeExperence;
    private int life;

    private string currentState = "StateIDLE";

    public float spead = 0.7f;
    public int maxAngleRotation = 30;
    public int baseCountdowToRotate = 100;
    private int countdownToRotate;

    public double distanceRapprochement = 10.0;
    public double neighborsRadar = 200.0;
    public int baseCountdowToRefreshRadar = 50;
    private int countdowToRefreshRadar;
    private List<GameObject> neighborsList = new List<GameObject>();




    // Start is called before the first frame update
    void Start() {
        worldObject = GameObject.Find("World");
        transform.Rotate(new Vector3(90, 0, 0));


        if (Random.Range(0, 1) == 0) { sex = false; }
        else { sex = true; }

        lifeExperence = Random.Range(minLifeExperence, maxLifeExperence);
        life = Random.Range(5, lifeExperence);

        countdownToRotate = baseCountdowToRotate;
        countdowToRefreshRadar = baseCountdowToRefreshRadar;

    }

    // Update is called once per frame
    void Update() {

        switch (currentState) {

            case "StateIDLE":
                print("StateIDLE \n");
                StateIDLE();
                break;

            case "StateRegroupementCall":
                print("StateRegroupementCall \n");
                StateRegroupementCall();
                break;

            case "StateRegroupement":
                print("StateRegroupement \n");
                StateRegroupement();
                break;

            case "StateVoyage":
                StateVoyage();
                break;

            default :
                StateIDLE();
                break;
        }

    }

    /* --------------------------- // FSM \\ ---------------------------  */


    void StateIDLE() {
        Wiggle();
        if (Input.GetKey(KeyCode.A)) {
            currentState = "StateRegroupementCall";
        }
    }

    void StateRegroupementCall() {
        FindMyNeighborsOnce();
        if (neighborsList.Count != 0) {
            currentState = "StateRegroupement";
        }
        else {
            currentState = "StateIDLE";
        }
    }

    void StateRegroupement() {
        if (GoTo(FindMyNearestNeighbor())) { } // il maque du code here ^ê
        else currentState = "StateIDLE";
    }


    void StateVoyage() {

    }


    /* --------------------------- // SOCIAL \\ ---------------------------  */



    void FindMyNeighbors() {

        if (countdowToRefreshRadar < 0) {
            countdowToRefreshRadar = baseCountdowToRefreshRadar;
            FindMyNeighborsOnce();
        }

        else countdowToRefreshRadar -= 1;

    }

    void FindMyNeighborsOnce() {

        neighborsList.Clear();
        GameObject[] neighborsTab = GameObject.FindGameObjectsWithTag("WhaleTAG");

        foreach (GameObject ng in neighborsTab) {
            double distance = Math.Sqrt(Math.Pow(ng.transform.position.x - transform.position.x, 2) + Math.Pow(ng.transform.position.y - transform.position.y, 2) + Math.Pow(ng.transform.position.z - transform.position.z, 2));
            if (distance < neighborsRadar) neighborsList.Add(ng);
        }

    }

    GameObject FindMyNearestNeighbor() {
        double minDistance = 100000.00;
        double distance;
        int indexOfMinDistanceNeighbor = 0;

        for (int i=0; i<neighborsList.Count; i++) {
            distance = distanceEucToMe(neighborsList[i]);
            if (distance < minDistance) {
                minDistance = distance;
                indexOfMinDistanceNeighbor = i;
            }
        }

        return neighborsList[indexOfMinDistanceNeighbor];
    }




    /* --------------------------- // MOUVEMENT \\ ---------------------------  */


    bool GoTo(GameObject target) {
        double distance = Math.Sqrt(Math.Pow(target.transform.position.x - transform.position.x, 2) + Math.Pow(target.transform.position.y - transform.position.y, 2) + Math.Pow(target.transform.position.z - transform.position.z, 2));
        if (distance > distanceRapprochement) {
            transform.LookAt(target.transform);
            transform.Rotate(new Vector3(90, 0, 0));
            transform.Translate(new Vector3(0, spead, 0) * Time.deltaTime);
            return true;
        }
        else return false;
    }


    void Wiggle() {
        if (countdownToRotate < 0)
        {
            countdownToRotate = baseCountdowToRotate;
            transform.Rotate(new Vector3(0, 0, Random.Range(-maxAngleRotation, maxAngleRotation)));
        }
        else countdownToRotate -= 1;

        transform.Translate(new Vector3(0, spead, 0) * Time.deltaTime);
    }





    /* --------------------------- // OUTILS \\ ---------------------------  */


    double distanceEuc(GameObject go1, GameObject go2) {
        return Math.Sqrt(Math.Pow(go1.transform.position.x - go2.transform.position.x, 2) + Math.Pow(go1.transform.position.y - go2.transform.position.y, 2) + Math.Pow(go1.transform.position.z - go2.transform.position.z, 2));
    }

    double distanceEucToMe(GameObject go)
    {
        return Math.Sqrt(Math.Pow(go.transform.position.x - transform.position.x, 2) + Math.Pow(go.transform.position.y - transform.position.y, 2) + Math.Pow(go.transform.position.z - transform.position.z, 2));
    }

}




    