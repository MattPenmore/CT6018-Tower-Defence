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
    // Start is called before the first frame update
    void Start()
    {

        gos = GameObject.FindGameObjectsWithTag("Cell");
        spawnerToggle.onValueChanged.AddListener(OnToggleValueChanged);
        toggleBackground = spawnerToggle.GetComponentInChildren<Image>();
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

                if (objectHit.gameObject.tag == "Spawner" && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
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

                        if (Input.GetMouseButtonDown(0) && !go.GetComponent<GridCell>().notSelectable)
                        {
                            GameObject towerInstant = Instantiate(Spawner);
                            towerInstant.transform.parent = go.transform;
                            towerInstant.transform.localPosition = Spawner.transform.position;
                            towerInstant.transform.localRotation = Spawner.transform.rotation;
                            go.GetComponent<GridCell>().notSelectable = true;
                            go.GetComponent<GridCell>().isObstacle = true;
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
