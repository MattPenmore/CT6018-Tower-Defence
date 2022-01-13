using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public int Gems;

    [SerializeField]
    GameObject shop;

    [SerializeField]
    GameObject saveMenu;

    [SerializeField]
    GameObject GemStore;

    [SerializeField]
    Upgrades upgrades;

    [SerializeField]
    Text gemStoreGems;

    [SerializeField]
    Text shopGems;

    private void Update()
    {
        gemStoreGems.text = Gems.ToString();
        shopGems.text = Gems.ToString();
    }

    public void OpenGemStore()
    {
        GemStore.SetActive(true);
        saveMenu.SetActive(false);
        shop.SetActive(false);
    }

    public void OpenShop()
    {
        GemStore.SetActive(false);
        saveMenu.SetActive(false);
        shop.SetActive(true);
    }

    public void OpenSaveMenu()
    {
        GemStore.SetActive(false);
        saveMenu.SetActive(true);
        shop.SetActive(false);
    }

    public void IncreaseGems100()
    {
        Gems += 100;
    }

    public void IncreaseGems250()
    {
        Gems += 250;
    }

    public void IncreaseGems500()
    {
        Gems += 500;
    }

    public void IncreaseGems1000()
    {
        Gems += 1000;
    }

    public void DoubleGoblinValue(int cost)
    {
        if(Gems >= cost)
        {
            upgrades.goblinRewardUpgrade *= 2;
            Gems -= cost;
        }
    }

    public void DoubleSkeletonValue(int cost)
    {
        if (Gems >= cost)
        {
            upgrades.skeletonRewardUpgrade *= 2;
            Gems -= cost;
        }
    }

    public void DoubleSlimeValue(int cost)
    {
        if (Gems >= cost)
        {
            upgrades.slimeRewardUpgrade *= 2;
            Gems -= cost;
        }
    }

    public void DoubleOgreValue(int cost)
    {
        if (Gems >= cost)
        {
            upgrades.ogreRewardUpgrade *= 2;
            Gems -= cost;
        }
    }
}
