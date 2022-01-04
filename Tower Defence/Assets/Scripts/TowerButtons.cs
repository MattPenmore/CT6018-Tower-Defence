using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class TowerButtons : MonoBehaviour
{
    [SerializeField]
    string towerName;

    public Toggle towerToggle;
    public Toggle[] otherTowersToggle;
    private Image toggleBackground;
    GameObject[] gos;
    public Camera cam;
    public GameObject Tower;
    public GameObject mainBase;
    public int maxPathLength = 500;
    List<GameObject> Spawners;

    [SerializeField]
    double cost;
    [SerializeField]
    double numTowers = 0;
    [SerializeField]
    double baseValue;
    [SerializeField]
    float exponenntialRatio = 1.25f;

    [SerializeField]
    GameObject score;

    [SerializeField]
    Text costText;


    GameObject currentHit;
    GameObject previousHit;
    bool isPlacable = true;

    // Start is called before the first frame update
    void Start()
    {
        
        gos = GameObject.FindGameObjectsWithTag("Cell");
        towerToggle.onValueChanged.AddListener(OnToggleValueChanged);
        toggleBackground = towerToggle.GetComponentInChildren<Image>();
    }

    private void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = towerToggle.colors;
        if (isOn == true)
        {
            toggleBackground.color = Color.blue;
            foreach(Toggle tog in otherTowersToggle)
            {
                tog.isOn = false;
            }
        }
        else
        {
            toggleBackground.color = new Color32(44, 44, 44, 255)/*Color.white*/;
        }
    }

    private void Update()
    {
        cost = System.Math.Floor(baseValue * System.Math.Pow(exponenntialRatio, numTowers));
        if (numTowers == 0 && towerName == "Turret")
        {
            cost = 0;
        }
        double exponent = (System.Math.Floor(System.Math.Log10(System.Math.Abs(cost))));
        double mantissa = (cost / System.Math.Pow(10, exponent));

        if (cost >= 1000000)
        {
            costText.text = mantissa.ToString("F3") + "e" + exponent.ToString();
        }
        else
        {
            costText.text = cost.ToString();
        }

        if (towerToggle.isOn)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                

                if(objectHit.gameObject.tag == towerName && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    objectHit.parent.GetComponent<GridCell>().notSelectable = false;
                    objectHit.parent.GetComponent<GridCell>().isObstacle = false;
                    objectHit.parent = null;
                    Destroy(objectHit.gameObject);
                    numTowers -= 1;
                    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                    foreach (GameObject monster in monsters)
                    {
                        monster.GetComponent<MonsterController>().needToPathfind = true;
                    }
                    return;
                }
                
                foreach (GameObject go in gos)
                {
                    if (GameObject.ReferenceEquals(objectHit.gameObject, go) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        currentHit = go;
                        go.GetComponent<GridCell>().isSelector = true;

                        if(!go.GetComponent<GridCell>().isObstacle)
                        {
                            go.GetComponent<GridCell>().isObstacle = true;
                            Spawners = GameObject.FindGameObjectsWithTag("Spawner").ToList();
                            GameObject endPoint = mainBase.GetComponent<MainBase>().closest;
                            if(currentHit != previousHit)
                            {
                                isPlacable = true;
                                foreach (GameObject spawner in Spawners)
                                {
                                    List<GameObject> Path = new List<GameObject>();
                                    Path = gameObject.GetComponent<Pathfind>().FindPath(spawner.transform.parent.gameObject, endPoint);
                                    if (Path.Count == 0)
                                    {
                                        isPlacable = false;
                                    }
                                }
                            }
                            if (!go.GetComponent<GridCell>().notSelectable && isPlacable)
                            {
                                if (Input.GetMouseButtonDown(0) && score.GetComponent<Score>().score >= cost)
                                {
                                    GameObject towerInstant = Instantiate(Tower);
                                    towerInstant.transform.localScale = new Vector3(1,1,1);
                                    towerInstant.transform.parent = go.transform;
                                    towerInstant.transform.localPosition = Tower.transform.position;
                                    towerInstant.transform.localRotation = Tower.transform.rotation;
                                    go.GetComponent<GridCell>().notSelectable = true;
                                    score.GetComponent<Score>().score -= cost;
                                    numTowers += 1;
                                    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                                    foreach (GameObject monster in monsters)
                                    {
                                        monster.GetComponent<MonsterController>().needToPathfind = true;
                                    }
                                }
                                else
                                {
                                    go.GetComponent<GridCell>().isObstacle = false;
                                }
                            }
                            else
                            {
                                go.GetComponent<GridCell>().isObstacle = false;
                                go.GetComponent<GridCell>().selector.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
                            }
                        }
                        previousHit = currentHit;
                    }
                    else
                    {
                        go.GetComponent<GridCell>().isSelector = false;                      
                    }
                }
            }            
        }
        else
        {
            toggleBackground.color = new Color32(44,44,44,255)/*Color.white*/;
            currentHit = null;
            previousHit = null;
        }
    }
}
