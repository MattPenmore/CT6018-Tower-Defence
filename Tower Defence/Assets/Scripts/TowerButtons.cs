using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class TowerButtons : MonoBehaviour
{
    public Toggle towerToggle;
    public Toggle spawnerToggle;
    private Image toggleBackground;
    GameObject[] gos;
    public Camera cam;
    public GameObject Tower;
    public GameObject mainBase;
    public int maxPathLength = 500;
    List<GameObject> Spawners;

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
            spawnerToggle.isOn = false;
        }
        else
        {
            toggleBackground.color = Color.white;
        }
    }

    private void Update()
    {
        if (towerToggle.isOn)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                

                if(objectHit.gameObject.tag == "Tower" && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    objectHit.parent.GetComponent<GridCell>().notSelectable = false;
                    objectHit.parent.GetComponent<GridCell>().isObstacle = false;
                    objectHit.parent = null;
                    Destroy(objectHit.gameObject);
                    return;
                }
                
                foreach (GameObject go in gos)
                {
                    if (GameObject.ReferenceEquals(objectHit.gameObject, go) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        go.GetComponent<GridCell>().isSelector = true;

                        if(!go.GetComponent<GridCell>().isObstacle)
                        {
                            go.GetComponent<GridCell>().isObstacle = true;
                            Spawners = GameObject.FindGameObjectsWithTag("Spawner").ToList();
                            GameObject endPoint = mainBase.GetComponent<MainBase>().closest;
                            bool isPlacable = true;
                            foreach (GameObject spawner in Spawners)
                            {
                                List<GameObject> Path = new List<GameObject>();
                                Path = gameObject.GetComponent<Pathfind>().FindPath(spawner.transform.parent.gameObject, endPoint);
                                if (Path.Count == 0)
                                {
                                    isPlacable = false;
                                }
                            }
                            if (!go.GetComponent<GridCell>().notSelectable && isPlacable)
                            {
                                if (Input.GetMouseButtonDown(0))
                                {
                                    GameObject towerInstant = Instantiate(Tower);
                                    towerInstant.transform.parent = go.transform;
                                    towerInstant.transform.localPosition = Tower.transform.position;
                                    towerInstant.transform.localRotation = Tower.transform.rotation;
                                    go.GetComponent<GridCell>().notSelectable = true;
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
            toggleBackground.color = Color.white;
        }
    }
}
