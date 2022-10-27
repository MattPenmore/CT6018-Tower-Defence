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
    Vector3 monsterOffset;

    [SerializeField]
    float timeToSpawn;

    List<GameObject> spawnPoints;
    
    Upgrades upgrades;

    public GameObject[] Path;
    public GameObject currentCell;
    public GameObject endPoint;
    public GameObject mainBase;

    // Start is called before the first frame update
    void Start()
    {

        //Set spawnpoints to cells adjacent to spawner
        spawnPoints = transform.parent.GetComponent<GridCell>().adjacentCells;

        //Set time between spawning dependant on type of monster
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

        //If can spawn, spawn a monster. Set time between spawning dependant on type of monster
        if (timeToSpawn <= 0)
        {
            int spawnNumber = Random.Range(0, spawnPoints.Count);
            GameObject monster = Instantiate(Monster, spawnPoints[spawnNumber].transform.position + monsterOffset, Monster.transform.rotation);
            monster.transform.parent = null;
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

    public void Pathfind()
    {
        //FindCurrentCell();
        Path = gameObject.GetComponent<Pathfind>().FindPath(currentCell, endPoint).ToArray();
    }
}
