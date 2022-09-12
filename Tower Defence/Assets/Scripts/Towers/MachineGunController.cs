using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController : TowerController
{
    // Update is called once per frame
    void Update()
    {
        timeToShoot -= Time.deltaTime;
        //Set distance tower can do damage
        rangeTrigger.radius = (Range + upgrades.machineGunRangeUpgrade) * singleGridDistance + (singleGridDistance / 2);

        if (enemiesInRange.Count != 0)
        {
            EnemyCheck();
            FindEnemies();

            //If have a target and can see it, rotate to face target, otherwise find new target
            if (enemiesInSight.Contains(target))
            {
                AttackTarget();
            }
            else if (enemiesInSight.Count != 0)
            {
                FindTarget();
                AttackTarget();
            }
            else
            {
                target = null;
            }
        }
    }

    new void Shoot()
    {
        //If angle tower is facing, is close enough to facing target, Deal damage to target.
        float angle = 25;
        if (Vector3.Angle(turretRotator.transform.forward, target.transform.position - turretRotator.transform.position) < angle && timeToShoot <= 0)
        {

            Debug.DrawRay(turretRotator.transform.position, target.transform.position - turretRotator.transform.position,
            Color.red);
            target.GetComponent<MonsterController>().currentHealth -= damage * upgrades.machineGunDamageUpgrade;
            shootAnim.SetActive(true);
            timeToShoot = shootSpeed / upgrades.machineGunSpeedUpgrade;
        }
    }
}
