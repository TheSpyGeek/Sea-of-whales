using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WhaleScript : MonoBehaviour
{

    // A mettre dans la classe World :
    public int timeDilataion = 1000;
    public GameObject worldObject;

    public int baseDisbandTime = 50;
    private int disbandTime;


    public double probaMigration = 0.80;
    private bool migre = false;
    private bool decided = false;
    private bool pregnent = false;
    private bool recentMother = false;


    private int id;
    public bool sex;
    public int maxLifeExperence = 50;
    public int minLifeExperence = 20;
    private int lifeExperence;
    private int age;
    private int ageInTics;

    private string currentSuperState = "SuperStateIDLE";
    private string currentState = "StateIDLE";

    public float spead = 0.7f;
    public int maxAngleRotation = 30;
    public int baseCountdowToRotate = 100;
    private int countdownToRotate;

    public double distanceRapprochement = 10.0;
    public double neighborsRadar = 600.0;
    public int baseCountdowToRefreshRadar = 100;
    private int countdowToRefreshRadar;
    private List<GameObject> neighborsList = new List<GameObject>();




    // Start is called before the first frame update
    void Start()
    {
        worldObject = GameObject.Find("World");

        disbandTime = Random.Range(0, baseDisbandTime);


        if (Random.Range(0, 1) == 0) { sex = false; }
        else { sex = true; }


        lifeExperence = Random.Range(minLifeExperence, maxLifeExperence);
        age = Random.Range(5, lifeExperence);
        ageInTics = age * timeDilataion;

        countdownToRotate = baseCountdowToRotate;
        countdowToRefreshRadar = 0;

        FindMyNeighbors();

    }

    // Update is called once per frame
    void Update()
    {
        if (ageInTics < 0)
        {
            Destroy(this.gameObject);
        }
        ageInTics -= 1;
        age = ageInTics / timeDilataion;

        FindMyNeighbors();

        Wiggle();

        switch (currentSuperState)
        {

            case "SuperStateIDLE":
                SuperStateIDLE();
                break;

            case "SuperStateMigration":
                SuperStateMigration();
                break;

            case "SuperStateReproduction":
                SuperStateReproduction();
                break;

            case "SuperStateMigrationBack":
                SuperStateMigrationBack();
                break;

            default:
                SuperStateIDLE();
                currentSuperState = "SuperStateIDLE";
                currentState = "StateIDLE";
                break;
        }

    }

    /* --------------------------- // FSM \\ ---------------------------  */


    

    

    void Reflex()
    {

    }


    void SuperStateIDLE()
    { // Quand on est en zone de repos 

        Wiggle(); // + Nage vers / dans la zone repos
        RestInTheFuckingReposZone();

        if (!decided && worldObject.GetComponent<WorldScript>().season && InReposZone())
        {

            decided = true;

            if (migrationDecision())
            {
                currentSuperState = "SuperStateMigration";
                currentState = "StateRegroupement";
            }
        }

        if (decided && !worldObject.GetComponent<WorldScript>().season)
        {
            decided = false;
        }

    }

    void SuperStateMigration()
    {
        switch (currentState)
        {

            case "StateRegroupement":
                StateRegroupement();
                break;

            case "StateTrip":
                StateTrip();
                break;

            default:
                print("erreur SpuperStateMigration Default Statement");
                break;

        }

    }

    void SuperStateReproduction()
    { // Quand on est en zone de reproduction 
        switch (currentState)
        {

            case "StateRegroupementSex":
                StateRegroupementSex();
                break;

            
        }

    }


    void SuperStateMigrationBack()
    { // Chemin retour
        switch (currentState)
        {

            case "StateRegroupementBack":
                StateRegroupement();
                break;

            case "StateTripBack":
                StateTripBack();
                break;

            default:
                print("erreur SpuperStateMigrationBack Default Statement");
                break;

        }

    }


    void StateIDLE()
    {
        Wiggle();
    }

    // SUPERSTATE : MIGRATION

    void StateRegroupement()
    {
        if (worldObject.GetComponent<WorldScript>().inRegroupement)
        {
            GoTo(worldObject.GetComponent<WorldScript>().meetingPointRepos);
        }
        else
        {
            currentState = "StateTrip";
        }
    }

    void StateTrip()
    {
        if (!InReproductionZone())
        {
            GoTo(worldObject.GetComponent<WorldScript>().meetingPointReproduction);
        }
        else
        {
            currentSuperState = "SuperStateReproduction";
            currentState = "StateRegroupementSex";
        }
    }

    // SUPERSTATE : REPRODUCTION

    void StateRegroupementSex()
    {
        if (worldObject.GetComponent<WorldScript>().season)
        {
            if (sex == false)
            {
                Wiggle();
                RestInTheFuckingReproductionZone();

            }
            else
            {
                transform.LookAt(FindLove().transform);
                transform.Translate(new Vector3(0, 0, spead) * Time.deltaTime);
            }
        }

        else {
            currentSuperState = "SuperStateMigrationBack";
            currentState = "StateRegroupementBack";
        }
    }

    // SUPERSATE : MIGRATIONBACK

    void StateRegroupementBack()
    {
        if (worldObject.GetComponent<WorldScript>().inRegroupement)
        {
            GoTo(worldObject.GetComponent<WorldScript>().meetingPointReproduction);
        }
        else
        {
            currentState = "StateTripBack";
        }
    }

    void StateTripBack()
    {
        if (!InReproductionZone())
        {
            GoTo(worldObject.GetComponent<WorldScript>().meetingPointRepos);
        }
        else
        {
            currentSuperState = "SuperStateIDLE";
            currentState = "StateIDLE";
        }
    }


    /* --------------------------- // SOCIAL \\ ---------------------------  */

    GameObject FindLove() {
        FindMyNeighbors();

        double minDistance = 100000.00;
        double distance;
        int indexOfMinDistanceNeighbor = 0;

        for (int i = 0; i < neighborsList.Count; i++)
        {
            distance = distanceEucToMe(neighborsList[i]);
            if (distance < minDistance && neighborsList[i].GetComponent<WhaleScript>().sex == false)
            {
                minDistance = distance;
                indexOfMinDistanceNeighbor = i;
            }
        }

        return neighborsList[indexOfMinDistanceNeighbor];
    }


    void FindMyNeighbors()
    {

        if (countdowToRefreshRadar < 0)
        {
            countdowToRefreshRadar = baseCountdowToRefreshRadar;
            FindMyNeighborsOnce();
        }

        else countdowToRefreshRadar -= 1;

    }

    void FindMyNeighborsOnce()
    {

        neighborsList.Clear();
        GameObject[] neighborsTab = GameObject.FindGameObjectsWithTag("WhaleTAG");
        foreach (GameObject ng in neighborsTab)
        {
            neighborsList.Add(ng);
        }

    }

    GameObject FindMyNearestNeighbor()
    {
        double minDistance = 100000.00;
        double distance;
        int indexOfMinDistanceNeighbor = 0;

        for (int i = 0; i < neighborsList.Count; i++)
        {
            distance = distanceEucToMe(neighborsList[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                indexOfMinDistanceNeighbor = i;
            }
        }

        return neighborsList[indexOfMinDistanceNeighbor];
    }

    public bool GetNeighbors(double d) {

        GameObject NN = FindMyNearestNeighbor();
        double distance = Math.Sqrt(Math.Pow(NN.transform.position.x - transform.position.x, 2) + Math.Pow(NN.transform.position.y - transform.position.y, 2) + Math.Pow(NN.transform.position.z - transform.position.z, 2));

        if (distance < d) return true;

        return false;
    }





    /* --------------------------- // MOUVEMENT \\ ---------------------------  */


    void GoTo(Vector3 meetingPoint) {
        double d = distanceEucToMe(meetingPoint);
        if (d > 30)
        {
            transform.LookAt(meetingPoint);
            transform.Translate(new Vector3(0, 0, spead) * Time.deltaTime);
        }
        else { Wiggle();  }
    }


    void Wiggle()
    { // 
        if (countdownToRotate < 0)
        {
            countdownToRotate = baseCountdowToRotate;
            transform.Rotate(new Vector3(0, Random.Range(-maxAngleRotation, maxAngleRotation), 0));
        }
        else countdownToRotate -= 1;

        transform.Translate(new Vector3(0, 0, spead) * Time.deltaTime);
        //RestInTheFuckingWorld();                                                                                    // Sortir (Wrap) ce code du wiggle et le metre dans le super etat 
    }

    void RestInTheFuckingWorld() {
        if (transform.position.x < 0 || transform.position.x > 1000 || transform.position.z < 0 || transform.position.z > 1000)
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    void RestInTheFuckingReposZone()
    {
        if (transform.position.x < 700 || transform.position.x > 1000 || transform.position.z < 400 || transform.position.z > 1000)
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    void RestInTheFuckingReproductionZone() {
        if (transform.position.x < 0 || transform.position.x > 200 || transform.position.z < 0 || transform.position.z > 300)
        {
            transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    bool InReposZone()
    {
        if (transform.position.x > 700 && transform.position.x < 1000 && transform.position.z > 400 && transform.position.z < 1000) return true;
        return false;
    }

    bool InReproductionZone()
    {
        if (transform.position.x > 0 && transform.position.x < 200 && transform.position.z > 0 && transform.position.z < 300) return true;
        return false;
    }



    /* --------------------------- // OUTILS \\ ---------------------------  */


    static double distanceEuc(GameObject go1, GameObject go2)
    {
        return Math.Sqrt(Math.Pow(go1.transform.position.x - go2.transform.position.x, 2) + Math.Pow(go1.transform.position.y - go2.transform.position.y, 2) + Math.Pow(go1.transform.position.z - go2.transform.position.z, 2));
    }

    double distanceEucToMe(GameObject go)
    {
        return Math.Sqrt(Math.Pow(go.transform.position.x - transform.position.x, 2) + Math.Pow(go.transform.position.y - transform.position.y, 2) + Math.Pow(go.transform.position.z - transform.position.z, 2));
    }

    double distanceEucToMe(Vector3 go)
    {
        return Math.Sqrt(Math.Pow(go.x - transform.position.x, 2) + Math.Pow(go.y - transform.position.y, 2) + Math.Pow(go.z - transform.position.z, 2));
    }


    bool migrationDecision()
    {
        int dice = (int)Random.Range(0, (float)(100 * probaMigration));
        if (dice <= 100 * probaMigration) return true;
        return false;
    }
}






