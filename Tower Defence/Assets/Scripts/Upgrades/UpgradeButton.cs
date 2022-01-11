using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    string upgradeName;

    [SerializeField]
    Upgrades upgrades;

    [SerializeField]
    GameObject score;

    GameObject image;

    [SerializeField]
    Text infoText;

    [SerializeField]
    string text;

    public bool isAvailable = false;
    public bool hasBeenPressed = false;

    public double cost;

    bool leftUI;

    GameObject currentHover;

    private void Start()
    {
        image = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if(currentHover == gameObject || currentHover == image)
        {
            infoText.transform.parent.gameObject.SetActive(true);
            infoText.text = "Cost: " + cost.ToString() + "\n \n" +
                text;
            leftUI = false;

            infoText.transform.parent.transform.position = transform.position - new Vector3(transform.GetComponent<RectTransform>().sizeDelta.x / 2, 0, 0) + new Vector3(-10, 0, 0);

            //Scale textbox to text
            infoText.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(infoText.GetComponent<RectTransform>().sizeDelta.x + 15, infoText.GetComponent<RectTransform>().sizeDelta.y + 5);
        }
        else if(!leftUI)
        {
            infoText.transform.parent.gameObject.SetActive(false);
            leftUI = true;
        }
    }

    public void ButtonPressed()
    {
        if(score.GetComponent<Score>().score >= cost)
        {
            infoText.transform.parent.gameObject.SetActive(false);
            score.GetComponent<Score>().score -= cost;
            hasBeenPressed = true;
            upgrades.Invoke(upgradeName, 0f);
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
