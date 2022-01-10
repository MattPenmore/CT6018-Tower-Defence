using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterSpawning : MonoBehaviour
{
    [SerializeField]
    string monsterName;
    public GameObject Monster;
    public float spawnTime;

    [SerializeField]
    float timeToSpawn;

    public GameObject[] spawnPoints;

    
    Upgrades upgrades;

    // Start is called before the first frame update
    void Start()
    {
        upgrades = FindObjectOfType<Upgrades>();
        if (monsterName == "goblin")
        {
            timeToSpawn = spawnTime / upgrades.goblinSpawnUpgrade;
        }
        if (monsterName == "skeleton")
        {
            timeToSpawn = spawnTime / upgrades.skeletonSpawnUpgrade;
        }
        if (monsterName == "slime")
        {
            timeToSpawn = spawnTime / upgrades.slimeSpawnUpgrade;
        }
        if (monsterName == "ogre")
        {
            timeToSpawn = spawnTime / upgrades.ogreSpawnUpgrade;
        }
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
            if (monsterName == "goblin")
            {
                timeToSpawn = spawnTime / upgrades.goblinSpawnUpgrade;
            }
            if (monsterName == "skeleton")
            {
                timeToSpawn = spawnTime / upgrades.skeletonSpawnUpgrade;
            }
            if (monsterName == "slime")
            {
                timeToSpawn = spawnTime / upgrades.slimeSpawnUpgrade;
            }
            if (monsterName == "ogre")
            {
                timeToSpawn = spawnTime / upgrades.ogreSpawnUpgrade;
            }

        }
    }
}
