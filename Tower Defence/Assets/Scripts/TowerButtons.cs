using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerButtons : MonoBehaviour
{
    public Toggle towerToggle;
    public Toggle spawnerToggle;
    private Image toggleBackground;
    GameObject[] gos;
    public Camera cam;
    public GameObject Tower;
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
                            GameObject towerInstant =  Instantiate(Tower);
                            towerInstant.transform.parent = go.transform;
                            towerInstant.transform.localPosition = Tower.transform.position;
                            towerInstant.transform.localRotation = Tower.transform.rotation;
                            go.GetComponent<GridCell>().notSelectable = true;
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
