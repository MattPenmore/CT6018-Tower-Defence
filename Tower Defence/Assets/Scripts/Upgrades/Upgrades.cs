using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public int turretRangeUpgrade;
    public int turretDamageUpgrade;
    public int turretSpeedUpgrade;

    public int hammerDamageUpgrade;
    public int hammerSpeedUpgrade;

    public int electricRangeUpgrade;
    public int electricDamageUpgrade;
    public int electricSpeedUpgrade;

    public int rocketRangeUpgrade;
    public int rocketDamageUpgrade;
    public int rocketSpeedUpgrade;

    public int machineGunRangeUpgrade;
    public int machineGunDamageUpgrade;
    public int machineGunSpeedUpgrade;

    public void TurretIncreaseRange()
    {
        turretRangeUpgrade += 1;
    }

    public void TurretDoubleDamage()
    {
        turretDamageUpgrade *= 2;
    }

    public void TurretDoubleSpeed()
    {
        turretSpeedUpgrade *= 2;
    }

    ////////

    public void HammerDoubleDamage()
    {
        hammerDamageUpgrade *= 2;
    }

    public void HammerDoubleSpeed()
    {
        hammerSpeedUpgrade *= 2;
    }

    ////////

    public void ElectricIncreaseRange()
    {
        electricRangeUpgrade += 1;
    }

    public void ElectricDoubleDamage()
    {
        electricDamageUpgrade *= 2;
    }

    public void ElectricDoubleSpeed()
    {
        electricSpeedUpgrade *= 2;
    }

    ////////

    public void RocketIncreaseRange()
    {
        rocketRangeUpgrade += 1;
    }

    public void RocketDoubleDamage()
    {
        rocketDamageUpgrade *= 2;
    }

    public void RocketDoubleSpeed()
    {
        rocketSpeedUpgrade *= 2;
    }

    ////////

    public void MachineGunIncreaseRange()
    {
        machineGunRangeUpgrade += 1;
    }

    public void MachineGunDoubleDamage()
    {
        machineGunDamageUpgrade *= 2;
    }

    public void MachineGunDoubleSpeed()
    {
        machineGunSpeedUpgrade *= 2;
    }
}
