using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterController : MonoBehaviour
{
    public int maxPathLength = 500;
    public GameObject[] Path;
    GameObject pathfind;
    public GameObject currentCell;
    GameObject previousCell;
    GameObject mainBase;
    public GameObject endPoint;
    public float moveSpeed;
    Rigidbody rb;

    //
    public Vector3 offset;
    //
    // Start is called before the first frame update
    void Start()
    {
        mainBase = GameObject.FindGameObjectWithTag("Base");
        pathfind = gameObject;
        endPoint = mainBase.GetComponent<MainBase>().closest;
        Path = new GameObject[maxPathLength];
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        FindCurentCell();

        if(!ReferenceEquals(currentCell, previousCell))
        {
            Path = pathfind.GetComponent<Pathfind>().FindPath(currentCell, endPoint).ToArray();
            offset = Path[0].transform.position - transform.position + new Vector3(0, transform.position.y - Path[0].transform.position.y, 0);
            if (offset.magnitude > 1f)
            {
                Vector3 Target = Path[0].transform.position;
                MoveTowards(Target);
            }
            else
            {               
                Vector3 Target = Path[1].transform.position;
                MoveTowards(Target);
                previousCell = currentCell;
            }
        }
        
    }

    void FindCurentCell()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            if (objectHit.gameObject.tag == "Cell")
            {
                currentCell = objectHit.gameObject;
            }
        }
    }

    void MoveTowards(Vector3 Target)
    {
        offset = Target - transform.position + new Vector3(0,transform.position.y - Target.y ,0);
        if (offset.magnitude > 0.1f)
        {
            //If we're further away than .1 unit, move towards the target.
            //The minimum allowable tolerance varies with the speed of the object and the framerate. 
            // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
            offset = offset.normalized * moveSpeed;
            //normalize it and account for movement speed.
            rb.velocity = offset;
            //actually move the character.
        }
    }
}
