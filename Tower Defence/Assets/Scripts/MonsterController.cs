using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public List<GameObject> Path;
    GameObject pathfind;
    public GameObject currentCell;
    GameObject mainBase;
    public GameObject endPoint;

    public float timeToPathfind;
    float timeUntilPathfind = 0;
    // Start is called before the first frame update
    void Start()
    {
        mainBase = GameObject.FindGameObjectWithTag("Base");
        pathfind = gameObject;
        endPoint = mainBase.GetComponent<MainBase>().closest;

        currentCell = FindCurentCell();
        Path = new List<GameObject>();
        Path = pathfind.GetComponent<Pathfind>().FindPath(currentCell, endPoint);
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilPathfind -= Time.deltaTime;
        if (timeUntilPathfind <= 0)
        {
            currentCell = FindCurentCell();
            Path = pathfind.GetComponent<Pathfind>().FindPath(currentCell, endPoint);
            timeUntilPathfind = timeToPathfind;
        }
    }

    GameObject FindCurentCell()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Cell");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }

        return closest;
    } 
}
