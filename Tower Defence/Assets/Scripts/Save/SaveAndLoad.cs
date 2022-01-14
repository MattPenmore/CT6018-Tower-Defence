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

    [SerializeField]
    Shop gems;

    Vector3 offset = new Vector3();

    private void Start()
    {

        //Create list of key value pairs for tower prefabs and their names
        foreach(GameObject prefabTower in prefabTowers)
        {
            KeyValuePair<string, GameObject> pair = new KeyValuePair<string, GameObject>(prefabTower.tag, prefabTower);
            towerNames.Add(pair);
        }

        //Create list of key value pairs for spawner prefabs and their names
        foreach (GameObject prefabSpawner in prefabSpawners)
        {
            KeyValuePair<string, GameObject> pair = new KeyValuePair<string, GameObject>(prefabSpawner.tag, prefabSpawner);
            towerNames.Add(pair);
        }

        //Create list of key value pairs for monster prefabs and their names
        foreach (GameObject prefabMonster in prefabMonsters)
        {
            KeyValuePair<string, GameObject> pair = new KeyValuePair<string, GameObject>(prefabMonster.GetComponent<MonsterController>().monsterName, prefabMonster);
            monsterNames.Add(pair);


        }
    }

    private void Update()
    {
        //Open and close menu by pressing esc
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

        //Get all cells
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Cell");

        foreach (GameObject go in gos)
        {
            //Identify cell by it's x and y coordinates
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

        //Save how many of each tower is on the map
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

        //Save how many of each spawner is on the map
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

        //Save whether each upgrade button is availabe and if it has already been pressed
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
        save.gems = gems.Gems;
        return save;
    }

    //Serialize all save information to save file
    public void SaveGame()
    {
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved " + Application.dataPath.ToString());
    }

    public void LoadGame()
    {
        // If save file exists
        if (File.Exists(Application.dataPath + "/gamesave.save"))
        {
            //Destroy all towers, spawners and monsters on the map
            ClearTowers();
            ClearSpawners();
            ClearMonsters();

            // Get information from save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // Get all Cells
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Cell");

            foreach (GameObject go in gos)
            {
                //Identify cell by it's x and y coordinates
                KeyValuePair<float, float> pair = new KeyValuePair<float, float>(go.GetComponent<GridCell>().coordinates.x, go.GetComponent<GridCell>().coordinates.y);

                //If saved river tiles contains cell, make cell a river tile, otherwise make it not a bridge. If bridge in save, make it a bridge
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

                //If tile is obstacle in the save, make it an obstacle, otherwise make it not an obstacle
                if(save.obstacles.Contains(pair))
                {
                    go.GetComponent<GridCell>().isObstacle = true;
                }
                else
                {
                    go.GetComponent<GridCell>().isObstacle = false;
                }

                //If tile is selectable in the save, make it selectable, otherwise make it not selectable
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
                    //If saved towers position matches cell position
                    if(tower.Key.Key == pair.Key && tower.Key.Value == pair.Value)
                    {
                        //Get type of tower and spawn it.
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

                //Load number of each type of tower from save
                int k = 0;
                foreach (int num in save.numTowers)
                {
                    numTowersTarget[k].numTowers = num;
                    k++;
                }

                //Load number of each type of spawner from save
                k = 0;
                foreach (int num in save.numSpawners)
                {
                    numSpawnersTarget[k].numSpawners = num;
                    k++;
                }

                //Load Spawners
                foreach (KeyValuePair<KeyValuePair<float, float>, string> spawner in save.spawnerPositions)
                {
                    //If saved spawners position matches cell position
                    if (spawner.Key.Key == pair.Key && spawner.Key.Value == pair.Value)
                    {
                        //Get type of spawner and spawn it.
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
            //For each type of monster
            foreach (KeyValuePair<string, GameObject> monsterName in monsterNames)
            {              
                int j = 0;
                foreach (string name in save.monsterNames)
                {
                    //Check if saved monster matches current monster type
                    if (name == monsterName.Key)
                    {
                        //Get monster position
                        Vector3 position = new Vector3(save.monsterPositionX[j], save.monsterPositionY[j], save.monsterPositionZ[j]);
                        //Get closest cell to that position
                        RaycastHit hit;
                        Ray ray = new Ray(position, transform.TransformDirection(Vector3.down));
                        if (Physics.Raycast(ray, out hit))
                        {
                            Transform objectHit = hit.transform;
                            
                            //If got closest cell
                            if (objectHit.gameObject.tag == "Cell")
                            {
                                closest = objectHit.gameObject;
                                //Set y position of monster dependant on type
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
                                //Spawn monster in center of cell
                                GameObject monster = Instantiate(monsterName.Value, closest.transform.position + offset, monsterName.Value.transform.rotation);
                                monster.transform.parent = null;
                            }
                        }
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
            gems.Gems = save.gems;

            Debug.Log("Game Loaded");

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

    public void Quit()
    {
        Application.Quit();
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

    void ClearSpawners()
    {
        foreach (KeyValuePair<string, GameObject> spawnerName in spawnerNames)
        {
            GameObject[] spawners = GameObject.FindGameObjectsWithTag(spawnerName.Key);
            foreach (GameObject spawner in spawners)
            {
                Destroy(spawner);
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
