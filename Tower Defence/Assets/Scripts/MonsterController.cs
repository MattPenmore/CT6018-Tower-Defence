using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterController : MonoBehaviour
{
    public int maxPathLength = 500;
    public GameObject[] Path;
    GameObject pathfind;
    GameObject previousPathingCell;
    public GameObject currentCell;
    GameObject previousCell;
    GameObject mainBase;
    public GameObject endPoint;
    public float moveSpeed;
    Rigidbody rb;
    //
    public Vector3 offset;

    public int maxHealth = 100;
    public int currentHealth;
    public int scoreValue;
    public bool needToPathfind = false;

    // Start is called before the first frame update
    void Start()
    {
        mainBase = GameObject.FindGameObjectWithTag("Base");
        pathfind = gameObject;
        endPoint = mainBase.GetComponent<MainBase>().closest;
        Path = new GameObject[maxPathLength];
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        PathFind();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            mainBase.GetComponent<Score>().score += scoreValue; 
            Destroy(gameObject);
        }

        if (needToPathfind)
        {
            PathFind();
            needToPathfind = false;
        }

        FindCurentCell();
        if (previousPathingCell != null && Path.Contains(currentCell) && !needToPathfind)
        {
            int index = System.Array.IndexOf(Path, currentCell);
            if (Path.Length == index + 1)
            {
                Destroy(gameObject);
            }

            if(Path.Length > index + 1)
            {
                offset = Path[index].transform.position - transform.position + new Vector3(0, transform.position.y - Path[index].transform.position.y, 0);
                if (offset.magnitude > 1f && previousCell != currentCell)
                {
                    Vector3 Target = Path[index].transform.position;
                    MoveTowards(Target);
                }
                else
                {
                    Vector3 Target = Path[index + 1].transform.position;
                    MoveTowards(Target);
                    previousCell = currentCell;
                }
            }
        }
        else
        {
            needToPathfind = true;
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

    public void PathFind()
    {
        FindCurentCell();

        if (previousPathingCell != null && Path.Contains(currentCell))
        {
            bool usePreviousPath = true;
            foreach (GameObject cell in Path)
            {
                if (cell.GetComponent<GridCell>().isObstacle)
                {
                    usePreviousPath = false;
                }
            }
            if (usePreviousPath)
            {
                int index = System.Array.IndexOf(Path, currentCell);
                if (Path.Length == index + 1)
                {
                    Destroy(gameObject);
                }

                offset = Path[index].transform.position - transform.position + new Vector3(0, transform.position.y - Path[index].transform.position.y, 0);
                if (offset.magnitude > 1f && previousCell != currentCell)
                {
                    Vector3 Target = Path[index].transform.position;
                    MoveTowards(Target);
                }
                else
                {
                    Vector3 Target = Path[index + 1].transform.position;
                    MoveTowards(Target);
                    previousCell = currentCell;
                }
            }
            else
            {
                Path = pathfind.GetComponent<Pathfind>().FindPath(currentCell, endPoint).ToArray();
                if (Path.Length == 0)
                {
                    MoveTowards(endPoint.transform.position);
                }
                else
                {
                    if (Path.Length == 1)
                    {
                        Destroy(gameObject);
                    }

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
                        previousPathingCell = currentCell;
                    }
                }
            }
        }
        else if (currentCell != null)
        {
            Path = pathfind.GetComponent<Pathfind>().FindPath(currentCell, endPoint).ToArray();
            if (Path.Length == 0)
            {
                MoveTowards(endPoint.transform.position);
            }
            else
            {
                if (Path.Length == 1)
                {
                    Destroy(gameObject);
                }

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
                    previousPathingCell = currentCell;
                }
            }
        }
    }
}
