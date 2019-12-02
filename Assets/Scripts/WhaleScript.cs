using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class WhaleScript : MonoBehaviour
{

    // A mettre dans la classe World :
    public int timeDilataion = 1000;
    public GameObject worldObject;
    public GameObject WhaleObject;

    public int baseDisbandTime = 50;
    private int disbandTime;


    public double probaMigration = 0.80;
    private bool migre = false;
    private bool decided = false;
    public bool pregnent = false;
    private bool recentMother = false;


    private int id;
    public bool sex;
    public int maxLifeExperence = 50;
    public int minLifeExperence = 20;
    private int lifeExperence;
    private int age;
    private int ageInTics;

    [SerializeField]
    private string currentSuperState = "SuperStateIDLE";
    [SerializeField]
    private string currentState = "StateIDLE";

    public float spead = 0.7f;
    public int maxAngleRotation = 30;
    public int baseCountdowToRotate = 100;
    private int countdownToRotate;

    public int zoneGoto = 60;
    public double distanceRapprochement = 10.0;
    public double neighborsRadar = 600.0;
    public int baseCountdowToRefreshRadar = 100;
    private int countdowToRefreshRadar;
    private List<GameObject> neighborsList = new List<GameObject>();


    private NavMeshAgent agent;


    public GameObject MyMom;
    public GameObject baby;



    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        worldObject = GameObject.Find("World");

        disbandTime = Random.Range(0, baseDisbandTime);


        agent.nextPosition = transform.position;

        if (Random.Range(0, 100) > 60) { sex = true; }
        else { sex = false; }


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
        if(ageInTics < 50 && MyMom == null)
        {
            Destroy(this.gameObject);
        }

        if (ageInTics < 0)
        {
            Destroy(this.gameObject);
        }
        ageInTics -= 1;
        age = ageInTics / timeDilataion;


        FindMyNeighborsOnce();

        Wiggle();

        switch (currentSuperState)
        {

            case "SuperStateBaby":
                Baby();
                break;

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

    void Baby() {

        if (InReposZone())
        {
            currentSuperState = "SuperStateIDLE";
            currentState = "StateIDLE";
        }
        else if (worldObject.GetComponent<WorldScript>().season)
        {

        }
        else
        {
            //FollowMom(MyMom);
            Wiggle();
        }


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

        if (sex == false)
        {
            if (Random.Range(0, 100) > 50) { pregnent = false; }
            else { pregnent = true; }

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


    // A un moment RESTEINZONE 


    void SuperStateMigrationBack()
    { // Chemin retour
        switch (currentState)
        {

            case "StateRegroupementBack":
                StateRegroupementBack();
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
                if (pregnent) {
                    pregnent = false;
                    Vector3 offset = new Vector3(10, 0, 10);
                    var obj = (GameObject)Instantiate(WhaleObject, transform.position + offset, Quaternion.identity);
                    obj.GetComponent<WhaleScript>().MyMom = this.gameObject;
                    obj.GetComponent<WhaleScript>().currentSuperState = currentSuperState;
                    obj.GetComponent<WhaleScript>().currentState = currentState;
                    obj.GetComponent<WhaleScript>().age = Random.Range(maxLifeExperence, maxLifeExperence + 20);
                    obj.GetComponent<NavMeshAgent>().nextPosition = transform.position + offset;
                    Debug.LogWarning("Naissance");
                }

            }
            else
            {
                agent.SetDestination(FindLove().transform.position);
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
        if (!InReposZone())
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
            if (distance < minDistance && neighborsList[i].GetComponent<WhaleScript>().sex == false && neighborsList[i].GetComponent<WhaleScript>().pregnent == false)
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

    public bool GetNeighbors(float d) {

        GameObject NN = FindMyNearestNeighbor();
        double distance = (transform.position - NN.transform.position).magnitude;
        if (distance < d) return true;

        return false;
    }





    /* --------------------------- // MOUVEMENT \\ ---------------------------  */


    void GoTo(Vector3 meetingPoint) {
        double d = distanceEucToMe(meetingPoint);
        if (d > zoneGoto)
        {
            NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
            agent.SetDestination(meetingPoint);
            /*transform.LookAt(meetingPoint);
            transform.Translate(new Vector3(0, 0, spead) * Time.deltaTime);*/

        }
        else { Wiggle();  }
    }

    void FollowMom(GameObject mom) {
        if(mom == null)
        {
            Destroy(this.gameObject);
        }
        transform.rotation = mom.transform.rotation;
        transform.Translate(new Vector3(0, 0, spead) * Time.deltaTime);
    }


    void Wiggle()
    { // 
        if (countdownToRotate < 0)
        {
            countdownToRotate = baseCountdowToRotate;
            Vector3 randomPos = new Vector3(Random.Range(0, 1000), 0, Random.Range(0, 1000));
            agent.SetDestination(randomPos);
        }
        else countdownToRotate -= 1;

        //RestInTheFuckingWorld();                                                                                    // Sortir (Wrap) ce code du wiggle et le metre dans le super etat 
    }

    void RestInTheFuckingWorld() {
        if (InWorld())
        {

            Vector3 randomPos = new Vector3(Random.Range(0, 1000), 0, Random.Range(0, 1000));
            agent.SetDestination(randomPos);

        }

        //Vector3 randomPos = new Vector3(Random.Range(0, 1000), 0, Random.Range()
    }

    void RestInTheFuckingReposZone()
    {
        if (InReposZone())
        {
            Vector3 randomPos = new Vector3(Random.Range(700, 1000), 0, Random.Range(400, 1000));
            agent.SetDestination(randomPos);
        }
    }

    void RestInTheFuckingReproductionZone() {
        if (InReproductionZone())
        {
            Vector3 randomPos = new Vector3(Random.Range(0, 200), 0, Random.Range(0, 300));
            agent.SetDestination(randomPos);
        }
    }

    bool InReposZone()
    {
        if (transform.position.x > 700 && transform.position.x < 1000 && transform.position.z > 400 && transform.position.z < 1000) return true;
        return false;
    }

    bool InWorld()
    {
        return transform.position.x < 0 || transform.position.x > 1000 || transform.position.z < 0 || transform.position.z > 1000;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position,30f);
    }
}






