using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    MeshFilter meshFilter;
    public Vector3 cooridinates;
    public MeshRenderer selector;
    public bool isSelector;
    public bool notSelectable;
    public bool isTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (notSelectable)
        {
            selector.material.SetColor("_Color", Color.red);
        }
        else
        {
            selector.material.SetColor("_Color", Color.blue);
        }

       if(isSelector)
        {
            selector.enabled = true;
        }
        else
        {
            selector.enabled = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelector = false;
        }
    }
}
