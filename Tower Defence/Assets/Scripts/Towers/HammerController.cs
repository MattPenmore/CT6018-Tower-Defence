using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    Upgrades upgrades;

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

        upgrades = FindObjectOfType<Upgrades>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToHit -= Time.deltaTime;

        //Check if enemies in range still exist. If they don't remove from list
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

        //If enemies in range, and can start hitting, start swinging hammer down
        if (enemiesInRange.Count != 0 && timeToHit <= 0 && !hammerSwingingUp)
        {
            hammerSwingingDown = true;
            timeToHit = hitSpeed / upgrades.hammerSpeedUpgrade;
            timeSwingingDown = 0;
        }

        //If hammer is currently swinging down
        if(hammerSwingingDown)
        {
            //Rotate hammer from pivot
            timeSwingingDown += Time.deltaTime;
            hammerRotator.transform.localRotation = Quaternion.Lerp(hammerRotator.transform.localRotation, Quaternion.Euler(angleBottom, 0, 90), swingSpeedMultiplier * Time.deltaTime / (swingTimeDown * upgrades.hammerSpeedUpgrade));

            //If finished swinging down, deal damage to all monsters in range and start swinging back up
            if (timeSwingingDown >= swingTimeDown)
            {
                foreach (GameObject monster in enemiesInRange)
                {
                    monster.GetComponent<MonsterController>().currentHealth -= damage * upgrades.hammerDamageUpgrade;
                }

                hammerSwingingDown = false;
                hammerSwingingUp = true;
                timeSwingingUp = 0;
            }
        }

        //Rotate hammer back up form pivot
        if(hammerSwingingUp)
        {
            timeSwingingUp += Time.deltaTime;

            hammerRotator.transform.localRotation = Quaternion.Lerp(hammerRotator.transform.localRotation, Quaternion.Euler(angleTop, 0, 90), 3 * Time.deltaTime / (swingTimeUp * upgrades.hammerSpeedUpgrade));
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
