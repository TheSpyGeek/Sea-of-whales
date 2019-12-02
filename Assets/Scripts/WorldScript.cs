using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
    public GameObject prefabWhale, prefabOrca;
    public int nbWhales = 1000;
    public int nbOrcaGroup = 5;
    
    public int minX = 700;
    public int maxX = 1000;
    public int minZ = 400;
    public int maxZ = 1000;

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

    // Start is called before the first frame update
    void Start()
    {
        season = false;
        seasonDuration = baseSeasonDuration;
        meetingPointRepos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
        meetingPointReproduction = new Vector3(Random.Range(0, 200), 5, Random.Range(0, 300));

        

        regroupementTime = BaseRegroupementTime;


        orcaGroupVector = new Vector3[nbOrcaGroup];
        

    WhaleCreation();
    OrcaCreation();

        for (int i = 0; i < nbOrcaGroup; i++)
        {
            orcaGroupVector[i] = new Vector3(0, Random.Range(-maxOrcaAngleRotation, maxOrcaAngleRotation), 0);
        }

    }

    void WhaleCreation()
    {
        for (int i = 0; i < nbWhales; i++)
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
        for (int i = 0; i < nbOrcaGroup; i++)
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
        if (seasonDuration < 0)
        {
            season = !season;
            seasonDuration = baseSeasonDuration;
            meetingPointRepos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
            inRegroupement = true;

        }
        seasonDuration -= 1;

        if (inRegroupement)
        {
            regroupementTime -= 1;
            if (regroupementTime < 0)
            {
                inRegroupement = false;
                regroupementTime = BaseRegroupementTime;
            }
        }

        for (int i = 0; i < nbOrcaGroup; i++)
        {
            orcaGroupVector[i] = new Vector3(0, Random.Range(-maxOrcaAngleRotation, maxOrcaAngleRotation), 0);
        }


    }
}