using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldScript : MonoBehaviour
{
    public GameObject prefabWhale, prefabOrca;
    public int nbWhales = 1000;
    public int nbOrcaGroup = 5;
    
    public int minX = 700;
    public int maxX = 1000;
    public int minZ = 400;
    public int maxZ = 1000;

    public Slider sliderWhale;
    public Slider sliderOrca;

    //public List<Vector3> orcaGroupVector;
    public Vector3[] orcaGroupVector;
    public List<Vector3> orcaGroupDispertion;
    public int orcaDispertionOffset = 3;
    public int maxOrcaAngleRotation = 45;


    public bool season; // 1 = hiver & 0 = ete
    public int baseSeasonDuration = 10000;
    private int seasonDuration;
    public Vector3 meetingPointRepos;
    public Vector3 meetingPointReproduction;

    public int BaseRegroupementTime = 300;
    private int regroupementTime;
    public bool inRegroupement = true;


    public float updateDirectionOrca = 1;
    private float lastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    public void Setup() {
        season = false;
        seasonDuration = baseSeasonDuration;
        meetingPointRepos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
        meetingPointReproduction = new Vector3(Random.Range(0, 200), 5, Random.Range(0, 300));



        regroupementTime = BaseRegroupementTime;


        orcaGroupVector = new Vector3[nbOrcaGroup];


        WhaleCreation();
        OrcaCreation();

        computeVectorDirectionOrca();
        lastUpdate = Time.time;

    }


    public void CleanScene() {
        GameObject [] objs = GameObject.FindGameObjectsWithTag("WhaleTAG");
        foreach(GameObject o in objs) {
            Destroy(o);
        }

        GameObject[] obj = GameObject.FindGameObjectsWithTag("OrcaTAG");
        foreach(GameObject o in obj) {
            Destroy(o);
        }
    }

    void WhaleCreation()
    {
        for (int i = 0; i < sliderWhale.value; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
            Instantiate(prefabWhale, randomPos, Quaternion.identity);
        }
    }

    void OrcaGroupDispertionInit() {

        orcaGroupDispertion.Add(new Vector3(0, 0, 0));
        orcaGroupDispertion.Add(new Vector3(-3 * orcaDispertionOffset, 0, 0));
        orcaGroupDispertion.Add(new Vector3(3 * orcaDispertionOffset, 0, 0));
        orcaGroupDispertion.Add(new Vector3(0, 0, -3 * orcaDispertionOffset));
        orcaGroupDispertion.Add(new Vector3(0, 0, 3 * orcaDispertionOffset));

        for (int i = -3; i <= 3; i++) {
            for (int j = -3; j <= 3; j++) {
                if (i != j && i != -3 && i != 3 && j != -3 && j != 3) {
                    orcaGroupDispertion.Add(new Vector3(i * orcaDispertionOffset, 0, j * orcaDispertionOffset));
                }
            }
        }
    }

    void OrcaCreation()
    {
        OrcaGroupDispertionInit();
        for (int i = 0; i < sliderOrca.value; i++)
        {
            Vector3 randomPos = new Vector3(Random.Range(0, 1000), 5, Random.Range(0, 1000));

            int orcaID = 0;
            foreach(Vector3 d in orcaGroupDispertion) {
                var obj = (GameObject)Instantiate(prefabOrca, randomPos + d, Quaternion.identity);
                obj.GetComponent<OrcaScript>().groupeID = i;
                obj.GetComponent<OrcaScript>().orcaID = orcaID;
                orcaID += 1;
            } 
        }
    }



    void Update()
    {
        //print("Season : " + season + " Season duration : " + seasonDuration);
        if (seasonDuration < 0)
        {
            season = !season;
            seasonDuration = baseSeasonDuration;
            meetingPointRepos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
            inRegroupement = true;
            //print("Season : " + season);
        }
        seasonDuration -= 1;

        if (inRegroupement)
        {
            //print("Regroupement : " + regroupementTime);
            regroupementTime -= 1;
            if (regroupementTime < 0)
            {
                inRegroupement = false;
                regroupementTime = BaseRegroupementTime;
            }
        }



        if(lastUpdate + updateDirectionOrca < Time.time)
        {
            computeVectorDirectionOrca();
            lastUpdate = Time.time;
        }



    }


    void computeVectorDirectionOrca()
    {
        for (int i = 0; i < nbOrcaGroup; i++)
        {
            orcaGroupVector[i] = new Vector3(Random.Range(0, 1000), 0, Random.Range(0, 1000));
        }
    }
}