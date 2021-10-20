using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    public SphereCollider range;
    public float shootSpeed;
    float timetoShoot;
    public float rotationSpeed;
    public GameObject turretRotator;
    public List<GameObject> enemiesInRange;
    public List<GameObject> enemiesInSight;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        enemiesInSight = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timetoShoot -= Time.deltaTime;

        if(enemiesInRange.Count != 0)
        {
            for(int i = enemiesInRange.Count -1; i >= 0; i--)
            {
                if(enemiesInRange[i] == null)
                {
                    enemiesInRange.RemoveAt(i);
                }
            }
            enemiesInSight.Clear();

            //Find enemies in both line of sight and range of the tower;
            foreach (GameObject enemy in enemiesInRange)
            {
                bool isObstacle = false;
                RaycastHit[] hits = Physics.RaycastAll(turretRotator.transform.position, enemy.transform.position - turretRotator.transform.position);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.tag == "Tower" || hit.transform.tag == "Spawner")
                    {
                        isObstacle = true;
                        break;
                    }
                }

                if (!isObstacle)
                {
                    enemiesInSight.Add(enemy);
                }
            }

                if(enemiesInSight.Contains(target))
                {
                    Vector3 finalRotation = Quaternion.LookRotation(target.transform.position - turretRotator.transform.position).eulerAngles;

                    Vector3 currentRotation = turretRotator.transform.eulerAngles;

                    if(currentRotation.y - finalRotation.y > 180)
                    {
                        currentRotation.y -= 360;
                    }
                if (finalRotation.y - currentRotation.y > 180)
                {
                    currentRotation.y += 360;
                }

                currentRotation.y = Mathf.Lerp(currentRotation.y, finalRotation.y, Time.deltaTime * rotationSpeed);
                    turretRotator.transform.eulerAngles = currentRotation;

                    float angle = 10;
                    if (Vector3.Angle(turretRotator.transform.forward, target.transform.position - turretRotator.transform.position) < angle)
                    {

                        Debug.DrawRay(turretRotator.transform.position, target.transform.position - turretRotator.transform.position,
                        Color.red);
                    }
                }
                else if (enemiesInSight.Count != 0)
                {
                    GameObject closest = null;
                    float distance = Mathf.Infinity;

                    foreach (GameObject enem in enemiesInSight)
                    {
                        Vector3 diff = enem.transform.position - transform.position;
                        float curDistance = diff.sqrMagnitude;
                        if (curDistance < distance)
                        {
                            closest = enem;
                            distance = curDistance;
                        }
                    }

                    target = closest;

                    Vector3 finalRotation = Quaternion.LookRotation(closest.transform.position - turretRotator.transform.position).eulerAngles;

                    Vector3 currentRotation = turretRotator.transform.eulerAngles;
                    currentRotation.y = Mathf.Lerp(currentRotation.y, finalRotation.y, Time.deltaTime * rotationSpeed);
                    turretRotator.transform.eulerAngles = currentRotation;


                    float angle = 10;
                    if (Vector3.Angle(turretRotator.transform.forward, target.transform.position - turretRotator.transform.position) < angle)
                    {

                        Debug.DrawRay(turretRotator.transform.position, target.transform.position - turretRotator.transform.position,
                        Color.red);
                    }
                }
            else
            {
                target = null;
            }

                //turretRotator.transform.rotation = Quaternion.Lerp(turretRotator.transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
                //turretRotator.transform.localRotation = new Quaternion(0, turretRotator.transform.localRotation.y, 0, 1);
            }

        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
}
