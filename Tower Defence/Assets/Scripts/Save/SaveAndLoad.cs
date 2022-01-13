using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
public class SaveAndLoad : MonoBehaviour
{
    public GameObject[] Upgrades;

    GameObject closest;

    [SerializeField]
    TowerButtons[] numTowersTarget;

    [SerializeField]
    SpawnerButton[] numSpawnersTarget;

    [SerializeField]
    Upgrades upgradeVariables;

    [SerializeField]
    Score score;

    [SerializeField]
    GameObject savePanel;

    [SerializeField]
    GameObject shopPanel;

    [SerializeField]
    GameObject gemStorePanel;

    [SerializeField]
    GameObject optionsPanel;

    [SerializeField]
    List<GameObject> prefabTowers;

    [SerializeField]
    List<GameObject> prefabSpawners;

    [SerializeField]
    List<GameObject> prefabMonsters;

    List<KeyValuePair<string, GameObject>> towerNames = new List<KeyValuePair<string, GameObject>>();
    List<KeyValuePair<string, GameObject>> spawnerNames = new List<KeyValuePair<string, GameObject>>();
    List<KeyValuePair<string, GameObject>> monsterNames = new List<KeyValuePair<string, GameObject>>();


    Vector3 offset = new Vector3();

    private void Start()
    {
        foreach(GameObject prefabTower in prefabTowers)
        {
            KeyValuePair<string, GameObject> pair = new KeyValuePair<string, GameObject>(prefabTower.tag, prefabTower);
            towerNames.Add(pair);
        }

        foreach (GameObject prefabSpawner in prefabSpawners)
        {
            KeyValuePair<string, GameObject> pair = new KeyValuePair<string, GameObject>(prefabSpawner.tag, prefabSpawner);
            towerNames.Add(pair);
        }

        foreach (GameObject prefabMonster in prefabMonsters)
        {
            KeyValuePair<string, GameObject> pair = new KeyValuePair<string, GameObject>(prefabMonster.GetComponent<MonsterController>().monsterName, prefabMonster);
            monsterNames.Add(pair);


        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            savePanel.SetActive(true);
            shopPanel.SetActive(false);
            gemStorePanel.SetActive(false);
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
    }


    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Cell");

        foreach (GameObject go in gos)
        {
            KeyValuePair<float, float> pair = new KeyValuePair<float, float>(go.GetComponent<GridCell>().coordinates.x, go.GetComponent<GridCell>().coordinates.y);


            //Save Rivers
            if (go.GetComponent<GridCell>().isRiver)
            {
                if(go.GetComponent<GridCell>().isBridge)
                {
                    save.riverTiles.Add(pair, true);
                }
                else
                {
                    save.riverTiles.Add(pair, false);
                }
            }

            //Save obstacle and selectable information
            if(go.GetComponent<GridCell>().isObstacle)
            {
                save.obstacles.Add(pair);
            }

            if(go.GetComponent<GridCell>().notSelectable)
            {
                save.notSelectable.Add(pair);
            }


        }

        //Save tower positions
        foreach(KeyValuePair<string, GameObject> towerName in towerNames)
        {
            GameObject[] towers = GameObject.FindGameObjectsWithTag(towerName.Key);
            foreach(GameObject tower in towers)
            {
                KeyValuePair<float, float> towerPos = new KeyValuePair<float, float>(tower.transform.parent.GetComponent<GridCell>().coordinates.x, tower.transform.parent.GetComponent<GridCell>().coordinates.y);
                save.towerPositions.Add(towerPos, towerName.Key);
            }
        }

        foreach(TowerButtons button in numTowersTarget)
        {
            save.numTowers.Add(button.numTowers);
        }

        //Save spawner positions
        foreach (KeyValuePair<string, GameObject> spawnerName in spawnerNames)
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag(spawnerName.Key);
            foreach (GameObject spawner in spawners)
            {
                KeyValuePair<float, float> spawnerPos = new KeyValuePair<float, float>(spawner.transform.parent.GetComponent<GridCell>().coordinates.x, spawner.transform.parent.GetComponent<GridCell>().coordinates.y);
                save.spawnerPositions.Add(spawnerPos, spawnerName.Key);
            }
        }

        foreach (SpawnerButton button in numSpawnersTarget)
        {
            save.numSpawners.Add(button.numSpawners);
        }

        //Save Monsters
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (KeyValuePair<string, GameObject> monsterName in monsterNames)
        {
            foreach (GameObject monster in monsters)
            {
                if(monster.GetComponent<MonsterController>().monsterName == monsterName.Key)
                {
                    save.monsterNames.Add(monsterName.Key);
                    save.monsterPositionX.Add(monster.transform.position.x);
                    save.monsterPositionY.Add(monster.transform.position.y);
                    save.monsterPositionZ.Add(monster.transform.position.z);

                }
            }
        }

        //Save Upgrades
        foreach (GameObject upgrade in Upgrades)
        {
            if(upgrade.GetComponent<UpgradeButton>().isAvailable)
            {
                save.upgradesAvailable.Add(true);
                
            }
            else
            {
                save.upgradesAvailable.Add(false);
            }

            if (upgrade.GetComponent<UpgradeButton>().hasBeenPressed)
            {
                save.upgradesPressed.Add(true);

            }
            else
            {
                save.upgradesPressed.Add(false);
            }

        }

        save.upgradeValues = upgradeVariables.GetVariables();

        save.score = score.score;

        return save;
    }


    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        //FileStream file = File.Create(/*Application.persistentDataPath +*/ "C:/Users/Matt/Pictures/Idle Tower Farm/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        // 3

        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            ClearTowers();
            ClearMonsters();

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Cell");

            foreach (GameObject go in gos)
            {
                KeyValuePair<float, float> pair = new KeyValuePair<float, float>(go.GetComponent<GridCell>().coordinates.x, go.GetComponent<GridCell>().coordinates.y);

                if(save.riverTiles.ContainsKey(pair))
                {
                    go.GetComponent<GridCell>().isRiver = true;
                    go.GetComponent<GridCell>().isBridge = save.riverTiles[pair];
                }
                else
                {
                    go.GetComponent<GridCell>().isRiver = false;
                    go.GetComponent<GridCell>().isBridge = false;
                }

                if(save.obstacles.Contains(pair))
                {
                    go.GetComponent<GridCell>().isObstacle = true;
                }
                else
                {
                    go.GetComponent<GridCell>().isObstacle = false;
                }

                if (save.notSelectable.Contains(pair))
                {
                    go.GetComponent<GridCell>().notSelectable = true;
                }
                else
                {
                    go.GetComponent<GridCell>().notSelectable = false;
                }

                //Load Towers
                foreach(KeyValuePair<KeyValuePair<float, float>, string> tower in save.towerPositions)
                {
                    if(tower.Key.Key == pair.Key && tower.Key.Value == pair.Value)
                    {
                        foreach (KeyValuePair<string, GameObject> towerName in towerNames)
                        {
                            if(tower.Value == towerName.Key)
                            {
                                GameObject towerInstant = Instantiate(towerName.Value);
                                towerInstant.transform.localScale = new Vector3(1, 1, 1);
                                towerInstant.transform.parent = go.transform;
                                towerInstant.transform.localPosition = towerName.Value.transform.position;
                                towerInstant.transform.localRotation = towerName.Value.transform.rotation;
                            }
                        }
                    }
                }

                int k = 0;
                foreach (int num in save.numTowers)
                {
                    numTowersTarget[k].numTowers = num;
                    k++;
                }

                k = 0;
                foreach (int num in save.numSpawners)
                {
                    numSpawnersTarget[k].numSpawners = num;
                    k++;
                }



                //Load Spawners
                foreach (KeyValuePair<KeyValuePair<float, float>, string> spawner in save.spawnerPositions)
                {
                    if (spawner.Key.Key == pair.Key && spawner.Key.Value == pair.Value)
                    {
                        foreach (KeyValuePair<string, GameObject> spawnerName in spawnerNames)
                        {
                            if (spawner.Value == spawnerName.Key)
                            {
                                GameObject spawnerInstant = Instantiate(spawnerName.Value);
                                spawnerInstant.transform.localScale = new Vector3(1, 1, 1);
                                spawnerInstant.transform.parent = go.transform;
                                spawnerInstant.transform.localPosition = spawnerName.Value.transform.position;
                                spawnerInstant.transform.localRotation = spawnerName.Value.transform.rotation;

                            }
                        }
                    }
                }
            }

            //Load Monsters
            foreach (KeyValuePair<string, GameObject> monsterName in monsterNames)
            {
                int j = 0;
                foreach (string name in save.monsterNames)
                {
                    if (name == monsterName.Key)
                    {
                        Vector3 position = new Vector3(save.monsterPositionX[j], save.monsterPositionY[j], save.monsterPositionZ[j]);
                        RaycastHit hit;
                        Ray ray = new Ray(position, transform.TransformDirection(Vector3.down));
                        if (Physics.Raycast(ray, out hit))
                        {
                            Transform objectHit = hit.transform;

                            if (objectHit.gameObject.tag == "Cell")
                            {
                                closest = objectHit.gameObject;

                                if (monsterName.Key == "goblin")
                                {
                                    offset = new Vector3(0, 5, 0);
                                }
                                if (monsterName.Key == "skeleton")
                                {
                                    offset = new Vector3(0, 3, 0);
                                }
                                if (monsterName.Key == "slime")
                                {
                                    offset = new Vector3(0, 5, 0);
                                }
                                if (monsterName.Key == "ogre")
                                {
                                    offset = new Vector3(0, 8, 0);
                                }

                                GameObject monster = Instantiate(monsterName.Value, closest.transform.position + offset, monsterName.Value.transform.rotation);
                                monster.transform.parent = null;
                            }
                        }

                        //GameObject monster = Instantiate(monsterName.Value,position, monsterName.Value.transform.rotation);
                        
                        //monster.transform.parent = null;

                        //monster.GetComponent<MonsterController>().needToPathfind = true;
                    }
                    j++;
                }
            }

            int i = 0;
            foreach(GameObject upgrade in Upgrades)
            {
                upgrade.GetComponent<UpgradeButton>().isAvailable = save.upgradesAvailable[i];
                upgrade.GetComponent<UpgradeButton>().hasBeenPressed = save.upgradesPressed[i];
                i++;
            }

            upgradeVariables.SetVariables(save.upgradeValues);
            score.score = save.score;
            // 4

            Debug.Log("Game Loaded");

            //Unpause();
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    void ClearTowers()
    {
        foreach (KeyValuePair<string, GameObject> towerName in towerNames)
        {
            GameObject[] towers = GameObject.FindGameObjectsWithTag(towerName.Key);
            foreach (GameObject tower in towers)
            {
                Destroy(tower);
            }
        }
    }

    void ClearMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach(GameObject monster in monsters)
        {
            Destroy(monster);
        }
    }
}
