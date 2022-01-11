using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OgreSpawnerInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    SpawnerButton costObj;

    [SerializeField]
    GameObject monster;

    [SerializeField]
    GameObject spawner;

    [SerializeField]
    GameObject nameText;

    [SerializeField]
    GameObject costText;

    [SerializeField]
    GameObject Image;

    [SerializeField]
    GameObject backGround;

    [SerializeField]
    Upgrades upgrades;

    [SerializeField]
    Text infoText;


    [SerializeField]
    GameObject score;

    GameObject currentHover;

    double cost;

    bool leftUI;

    float spawnRate;
    float speed;
    float health;
    float value;


    // Update is called once per frame
    void Update()
    {
        if (currentHover == gameObject || currentHover == nameText || currentHover == costText || currentHover == Image || currentHover == backGround)
        {
            cost = costObj.cost;
            health = monster.GetComponent<MonsterController>().maxHealth * upgrades.ogreHealthUpgrade;
            speed = monster.GetComponent<MonsterController>().moveSpeed;
            value = monster.GetComponent<MonsterController>().scoreValue * upgrades.ogreRewardUpgrade;
            spawnRate = spawner.GetComponent<MonsterSpawning>().spawnTime / upgrades.ogreSpawnUpgrade;
            infoText.transform.parent.gameObject.SetActive(true);
            infoText.text = "ogre Spawner \n \n" + "Cost: " + cost.ToString() + "\n" +
                "Health: " + health.ToString() + "\n" +
                "Speed: " + speed.ToString() + "\n" +
                "Value: " + value.ToString() + "\n" +
                "Spawn Rate: " + spawnRate.ToString();
            leftUI = false;

            infoText.transform.parent.transform.position = transform.position - new Vector3(transform.GetComponent<RectTransform>().sizeDelta.x, 0, 0) + new Vector3(-30, 0, 0);

            //Scale textbox to text
            infoText.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(infoText.GetComponent<RectTransform>().sizeDelta.x + 15, infoText.GetComponent<RectTransform>().sizeDelta.y + 5);
        }
        else if (!leftUI)
        {
            infoText.transform.parent.gameObject.SetActive(false);
            leftUI = true;
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
            currentHover = eventData.pointerCurrentRaycast.gameObject;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentHover = null;
    }
}
