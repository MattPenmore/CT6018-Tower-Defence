using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnerButton : MonoBehaviour
{
    [SerializeField]
    string spawnerName;

    public Toggle spawnerToggle;
    public Toggle[] otherSpawnersToggle;
    private Image toggleBackground;
    GameObject[] gos;
    public Camera cam;
    public GameObject Spawner;
    bool isPlacable = true;
    public GameObject mainBase;

    [SerializeField]
    GameObject score;

    public double cost;

    public int numSpawners = 0;
    [SerializeField]
    double baseValue;
    [SerializeField]
    float exponenntialRatio = 1.25f;

    [SerializeField]
    List<UpgradeButton> upgradeButtons;

    [SerializeField]
    List<int> upgradeButtonValues;

    //Upgrade Buttons for the towers, and number of towers of that type needed to become available
    Dictionary<UpgradeButton, int> upgradeButtonsPair;

    [SerializeField]
    Text costText;

    List<GameObject> isAdjacent;
    // Start is called before the first frame update
    void Start()
    {
        upgradeButtonsPair = new Dictionary<UpgradeButton, int>();
        int i = 0;
        foreach (UpgradeButton button in upgradeButtons)
        {
            upgradeButtonsPair.Add(button, upgradeButtonValues[i]);
            i++;
        }

        //Make upgrade buttons available if requirements for number of spawners is met.
        foreach (KeyValuePair<UpgradeButton, int> button in upgradeButtonsPair)
        {
            if (button.Value <= numSpawners)
            {
                button.Key.isAvailable = true;
            }
        }

        gos = GameObject.FindGameObjectsWithTag("Cell");
        spawnerToggle.onValueChanged.AddListener(OnToggleValueChanged);
        toggleBackground = spawnerToggle.GetComponentInChildren<Image>();
        isAdjacent = new List<GameObject>();
    }

    private void OnToggleValueChanged(bool isOn)
    {
        //If Button is turned on, turn all other spawner buttons off, change colour to blue
        //If button turned off, change colour to base.
        ColorBlock cb = spawnerToggle.colors;
        if (isOn == true)
        {
            toggleBackground.color = Color.blue;
            foreach(Toggle tog in otherSpawnersToggle)
            {
                tog.isOn = false;
            }
        }
        else
        {
            toggleBackground.color = new Color32(44, 44, 44, 255);
        }
    }

    private void Update()
    {
        //Get cost of buying a spawner
        cost = System.Math.Floor(baseValue * System.Math.Pow(exponenntialRatio, numSpawners));

        double exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(cost))));
        double mantissa = (cost / System.Math.Pow(10, exponent));

        //Display cost as text
        if (cost >= 1000000)
        {
            costText.text = mantissa.ToString("F3") + "e" + exponent.ToString();
        }
        else
        {
            costText.text = cost.ToString();
        }

        //If button is on
        if (spawnerToggle.isOn)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                //If mouse is hovering over this type of spawner, and right mouse button is clicked, sell spawner
                if (objectHit.gameObject.tag == spawnerName && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    //Set adjacent cells to be selectable for placing towers and spawners
                    objectHit.parent.GetComponent<GridCell>().adjacentCells = objectHit.parent.GetComponent<GridCell>().FindAdjacentCells();
                    foreach(GameObject cell in objectHit.parent.GetComponent<GridCell>().adjacentCells)
                    {
                        if (!cell.GetComponent<GridCell>().isObstacle)
                        {
                            cell.GetComponent<GridCell>().notSelectable = false;
                        }
                    }

                    //Destroy selected spawner, and gain money equal to 1/5th of buy value
                    objectHit.parent.GetComponent<GridCell>().notSelectable = false;
                    objectHit.parent.GetComponent<GridCell>().isObstacle = false;
                    objectHit.parent = null;
                    Destroy(objectHit.gameObject);
                    numSpawners -= 1;
                    score.GetComponent<Score>().score += System.Math.Floor(baseValue * System.Math.Pow(exponenntialRatio, numSpawners) / 5f);

                    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                    foreach (GameObject monster in monsters)
                    {
                        monster.GetComponent<MonsterController>().needToPathfind = true;
                    }
                    return;
                }

                foreach (GameObject go in gos)
                {
                    //If cell is one being hovered over
                    if (GameObject.ReferenceEquals(objectHit.gameObject, go) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        //turn cell selector on
                        go.GetComponent<GridCell>().isSelector = true;
                        //Check if space is large enough to place spawner
                        go.GetComponent<GridCell>().adjacentCells = go.GetComponent<GridCell>().FindAdjacentCells();

                        bool adjacentsAvailable = true;
                        //Check adjacent cells to make sure they are free
                        foreach (GameObject cell in go.GetComponent<GridCell>().adjacentCells)
                        {
                            cell.GetComponent<GridCell>().isSelector = true;
                            isAdjacent.Add(cell);
                            if (cell.GetComponent<GridCell>().notSelectable)
                            {
                                adjacentsAvailable = false;
                            }
                        }

                        //Check if can path to players base
                        GameObject endPoint = mainBase.GetComponent<MainBase>().closest;

                        List<GameObject> Path = new List<GameObject>();
                        Path = gameObject.GetComponent<Pathfind>().FindPath(objectHit.gameObject, endPoint);
                        if (Path.Count == 0)
                        {
                            isPlacable = false;
                        }

                        //If left mouse button pressed, there is space for the spawner, can pathfind to players base, and have enough money, create spawner
                        if (Input.GetMouseButtonDown(0) && !go.GetComponent<GridCell>().notSelectable && adjacentsAvailable && isPlacable && score.GetComponent<Score>().score >= cost)
                        {
                            //Spawn in correct position
                            GameObject spawnerInstant = Instantiate(Spawner);
                            spawnerInstant.transform.parent = go.transform;
                            spawnerInstant.transform.localPosition = Spawner.transform.position;
                            spawnerInstant.transform.localRotation = Spawner.transform.rotation;
                            go.GetComponent<GridCell>().notSelectable = true;
                            go.GetComponent<GridCell>().isObstacle = true;
                            //remove gold from player
                            score.GetComponent<Score>().score -= cost;
                            numSpawners += 1;

                            //Set adjacent cells, to not be able to place turrets or towers there
                            foreach (GameObject cell in go.GetComponent<GridCell>().adjacentCells)
                            {
                                cell.GetComponent<GridCell>().notSelectable = true;                                
                            }

                            //Tell all monsters to pathfind again
                            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                            foreach (GameObject monster in monsters)
                            {
                                monster.GetComponent<MonsterController>().needToPathfind = true;
                            }

                            //Make upgrade buttons available if requirements for number of towers is met.
                            foreach (KeyValuePair<UpgradeButton, int> button in upgradeButtonsPair)
                            {
                                if (button.Value <= numSpawners)
                                {
                                    button.Key.isAvailable = true;
                                }
                            }
                        }
                        isPlacable = true;

                    }
                    else if (isAdjacent.Contains(go))
                    {
                        isAdjacent.Remove(go);
                    }
                    else
                    {
                        //Turn cell selector off
                        go.GetComponent<GridCell>().isSelector = false;
                    }
                }
            }
        }
        else
        {
            //Set colour of button to base
            toggleBackground.color = new Color32(44, 44, 44, 255);
        }
    }
}
