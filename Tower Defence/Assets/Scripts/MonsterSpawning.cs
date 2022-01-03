using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterSpawning : MonoBehaviour
{
    public GameObject Monster;
    public float spawnTime;
    float timeToSpawn;
    public GameObject[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        timeToSpawn = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeToSpawn -= Time.deltaTime;

        if(timeToSpawn <= 0)
        {
            int spawnNumber = Random.Range(0, spawnPoints.Length);
            GameObject monster = Instantiate(Monster, spawnPoints[spawnNumber].transform);
            monster.transform.parent = null;
            monster.transform.localScale = Monster.transform.localScale;
            timeToSpawn = spawnTime;
        }
    }
}
