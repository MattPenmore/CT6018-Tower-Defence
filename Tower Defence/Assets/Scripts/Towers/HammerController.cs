using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    public int damage = 100;

    public float hitSpeed;
    public float timeToHit;
    public float swingTimeDown;
    public float swingTimeUp;
    float timeSwingingDown;
    float timeSwingingUp;

    public float swingSpeedMultiplier;



    float angleBottom = 95f;
    float angleTop = 10;

    public GameObject hammerRotator;
    public List<GameObject> enemiesInRange;


    bool hammerSwingingDown;
    bool hammerSwingingUp;
    // Start is called before the first frame update
    void Start()
    {
        enemiesInRange = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToHit -= Time.deltaTime;

        if (enemiesInRange.Count != 0)
        {
            for (int i = enemiesInRange.Count - 1; i >= 0; i--)
            {
                if (enemiesInRange[i] == null)
                {
                    enemiesInRange.RemoveAt(i);
                }
            }
        }

        if (enemiesInRange.Count != 0 && timeToHit <= 0 && !hammerSwingingUp)
        {
            hammerSwingingDown = true;
            timeToHit = hitSpeed;
            timeSwingingDown = 0;
        }

        if(hammerSwingingDown)
        {
            timeSwingingDown += Time.deltaTime;
            hammerRotator.transform.localRotation = Quaternion.Lerp(hammerRotator.transform.localRotation, Quaternion.Euler(angleBottom, 0, 90), swingSpeedMultiplier * Time.deltaTime / swingTimeDown);
            if (timeSwingingDown >= swingTimeDown)
            {
                foreach (GameObject monster in enemiesInRange)
                {
                    monster.GetComponent<MonsterController>().currentHealth -= damage;
                }

                hammerSwingingDown = false;
                hammerSwingingUp = true;
                timeSwingingUp = 0;
            }
        }

        if(hammerSwingingUp)
        {
            timeSwingingUp += Time.deltaTime;

            hammerRotator.transform.localRotation = Quaternion.Lerp(hammerRotator.transform.localRotation, Quaternion.Euler(angleTop, 0, 90), 3 * Time.deltaTime / swingTimeUp);
            if (timeSwingingUp >= swingTimeUp)
            {
                hammerSwingingUp = false;
            }
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
