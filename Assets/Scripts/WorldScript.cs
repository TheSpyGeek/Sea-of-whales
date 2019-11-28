using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
    public GameObject prefab;
    public int nbWhales = 1000;
    public int minX = 700;
    public int maxX = 1000;
    public int minZ = 400;
    public int maxZ = 1000;


    public bool season; // 1 = hiver & 0 = ete
    public int baseSeasonDuration = 10000;
    private int seasonDuration;
    public Vector3 meetingPointRepos;

    public int BaseRegroupementTime = 300;
    private int regroupementTime;
    public bool inRegroupement = true;

    // Start is called before the first frame update
    void Start() {
        season = false;
        seasonDuration = baseSeasonDuration;
        meetingPointRepos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));


        regroupementTime = BaseRegroupementTime;


        StartCoroutine(whaleCreation());

    }

    IEnumerator whaleCreation() {
        for (int i = 0; i < nbWhales; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
            Instantiate(prefab, randomPos, Quaternion.identity);
        }
    }

    void Update() {
        if (seasonDuration < 0) {
            season = !season;
            seasonDuration = baseSeasonDuration;
            meetingPointRepos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
            inRegroupement = true;

        }
        seasonDuration -= 1;

        if (inRegroupement) {
            regroupementTime -= 1;
            if (regroupementTime < 0) {
                inRegroupement = false;
                regroupementTime = BaseRegroupementTime;
            }
        }
        

        
    }
}