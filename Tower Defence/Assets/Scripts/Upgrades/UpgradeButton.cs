using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField]
    string UpgradeName;
    [SerializeField]
    GameObject score;

    public bool isAvailable = false;
    public bool hasBeenPressed = false;

    public double cost;

    public void ButtonPressed()
    {
        score.GetComponent<Score>().score -= cost;
        hasBeenPressed = true;
    }
}
