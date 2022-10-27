using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElectricInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    TowerButtons costObj;

    [SerializeField]
    GameObject tower;

    [SerializeField]
    GameObject nameText;

    [SerializeField]
    GameObject costText;

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

    float damage;
    float speed;
    float range;

    // Update is called once per frame
    void Update()
    {
        //If mouse over button Set text of text box, to display basic information. If mouse leaves button, turn text box off.  Bug where box flickers when over image in build, but not editor
        if (currentHover == backGround)
        {
            cost = costObj.cost;
            damage = tower.GetComponent<ElectricController>().damage * upgrades.electricDamageUpgrade;
            speed = tower.GetComponent<ElectricController>().shockSpeed / upgrades.electricSpeedUpgrade;
            range = tower.GetComponent<ElectricController>().Range + upgrades.electricRangeUpgrade;
            infoText.transform.parent.gameObject.SetActive(true);
            infoText.text = "Electric \n \n" + "Cost: " + cost.ToString() + "\n" +
                "Damage: " + damage.ToString() + "\n" +
                "Range: " + range.ToString() + "\n" +
                "Speed: " + speed.ToString();
            leftUI = false;

            //Set position of textbox to be next to button
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
            //Debug.Log("Mouse Over: " + eventData.pointerCurrentRaycast.gameObject.name);
            currentHover = eventData.pointerCurrentRaycast.gameObject;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentHover = eventData.pointerCurrentRaycast.gameObject;
    }
}
