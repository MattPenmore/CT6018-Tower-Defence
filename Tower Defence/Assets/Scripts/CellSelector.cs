using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSelector : MonoBehaviour
{

    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SelectCell();
        }

        
    }

    public void SelectCell()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.GetComponent<GridCell>() != null)
            {
                objectHit.GetComponent<GridCell>().isSelector = true;
            }
        }
    }
}
