using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
    public GameObject prefab;
    public int nbWhales = 1000;
    public int minX = 400;
    public int maxX = 500;
    public int minZ = 600;
    public int maxZ = 700;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(whaleCreation());
    }

    IEnumerator whaleCreation()
    {
        for (int i = 0; i < nbWhales; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 5, Random.Range(minZ, maxZ));
            Instantiate(prefab, randomPos, Quaternion.identity);
        }
    }
}