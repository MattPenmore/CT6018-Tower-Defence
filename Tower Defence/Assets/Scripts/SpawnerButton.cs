using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpawnerButton : MonoBehaviour
{
    public Toggle spawnerToggle;
    public Toggle towerToggle;
    private Image toggleBackground;
    GameObject[] gos;
    public Camera cam;
    public GameObject Spawner;
    bool isPlacable = true;
    public GameObject mainBase;

    List<GameObject> isAdjacent;
    // Start is called before the first frame update
    void Start()
    {

        gos = GameObject.FindGameObjectsWithTag("Cell");
        spawnerToggle.onValueChanged.AddListener(OnToggleValueChanged);
        toggleBackground = spawnerToggle.GetComponentInChildren<Image>();
        isAdjacent = new List<GameObject>();
    }

    private void OnToggleValueChanged(bool isOn)
    {
        ColorBlock cb = spawnerToggle.colors;
        if (isOn == true)
        {
            toggleBackground.color = Color.blue;
            towerToggle.isOn = false;
        }
        else
        {
            toggleBackground.color = Color.white;
        }
    }

    private void Update()
    {
        if (spawnerToggle.isOn)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                //Sell spawner
                if (objectHit.gameObject.tag == "Spawner" && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    
                    objectHit.parent.GetComponent<GridCell>().adjacentCells = objectHit.parent.GetComponent<GridCell>().FindAdjacentCells();
                    foreach(GameObject cell in objectHit.parent.GetComponent<GridCell>().adjacentCells)
                    {
                        if (!cell.GetComponent<GridCell>().isObstacle)
                        {
                            cell.GetComponent<GridCell>().notSelectable = false;
                        }
                    }


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
                        //Check if space is large enough
                        go.GetComponent<GridCell>().isSelector = true;
                        go.GetComponent<GridCell>().adjacentCells = go.GetComponent<GridCell>().FindAdjacentCells();

                        bool adjacentsAvailable = true;
                        
                        foreach (GameObject cell in go.GetComponent<GridCell>().adjacentCells)
                        {
                            cell.GetComponent<GridCell>().isSelector = true;
                            isAdjacent.Add(cell);
                            if (cell.GetComponent<GridCell>().notSelectable)
                            {
                                adjacentsAvailable = false;
                            }
                        }

                        //Check if can path to target
                        GameObject endPoint = mainBase.GetComponent<MainBase>().closest;

                        List<GameObject> Path = new List<GameObject>();
                        Path = gameObject.GetComponent<Pathfind>().FindPath(objectHit.gameObject, endPoint);
                        if (Path.Count == 0)
                        {
                            isPlacable = false;
                        }

                        if (Input.GetMouseButtonDown(0) && !go.GetComponent<GridCell>().notSelectable && adjacentsAvailable && isPlacable)
                        {
                            GameObject spawnerInstant = Instantiate(Spawner);
                            spawnerInstant.transform.parent = go.transform;
                            spawnerInstant.transform.localPosition = Spawner.transform.position;
                            spawnerInstant.transform.localRotation = Spawner.transform.rotation;
                            go.GetComponent<GridCell>().notSelectable = true;
                            go.GetComponent<GridCell>().isObstacle = true;

                            foreach (GameObject cell in go.GetComponent<GridCell>().adjacentCells)
                            {
                                cell.GetComponent<GridCell>().notSelectable = true;                                
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
