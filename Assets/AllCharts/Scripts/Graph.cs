using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] public Sprite dotSprite;
    [SerializeField] public RectTransform graphContainer;
    [SerializeField] public RectTransform labelTemplateX;
    [SerializeField] public RectTransform labelTemplateY;
    [SerializeField] public RectTransform dashTemplateX;
    [SerializeField] public RectTransform dashTemplateY;

    [SerializeField] public List<int> valueList = new List<int>() { 4, 11, 8, 30, 80, 9, 23, 66, 99, 40, 30, 20, 55 };

    [SerializeField] public int maxVisibleValues = 5;

    private List<GameObject> gameObjectList;

    public string[] chartsOptions;
    public int chartsOptionsIndex = 0;
    private int selctedGhraphIndex = 0;


    private void Awake()
    {
        gameObjectList = new List<GameObject>();

        chartsOptions = new string[] { "Line Chart", "Bar Chart", "Ring Chart", "Pie Chart" };

        selctedGhraphIndex = chartsOptionsIndex;

        ShowGraph(valueList, maxVisibleValues, (int _i) => "Day " + (_i + 1), (float _f) => "$" + (Mathf.RoundToInt(_f)));

    }

    private void OnDrawGizmos()
    {
        ShowGraph(valueList, maxVisibleValues, (int _i) => "Day " + (_i + 1), (float _f) => "$" + (Mathf.RoundToInt(_f)));
    }

    private void Update()
    {
        Debug.Log("chartsOptions[selctedGhraphIndex]: " + chartsOptions[selctedGhraphIndex]);
        Debug.Log("chartsOptions: " + chartsOptions);
    }

    

    public void ShowGraph(List<int> valueList, int maxVisibleValues = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        if (maxVisibleValues > valueList.Count || maxVisibleValues <= 0) maxVisibleValues = valueList.Count;

        if (getAxisLabelX == null)
        {
            getAxisLabelX = (int _i) => { return _i.ToString(); };
        }
        if(getAxisLabelY == null)
        {
            getAxisLabelY = (float _f) => { return Mathf.RoundToInt(_f).ToString(); };
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            DestroyImmediate(gameObject);
        }
        gameObjectList.Clear();


        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        float yMax = valueList[0];
        float yMin = valueList[0];

        for (int i = valueList.Count - maxVisibleValues; i < valueList.Count; i++)
        {
            int value = valueList[i];
            if(value > yMax) yMax = value;
            if(value < yMin) yMin = value;
        }

        float yDiff = yMax - yMin;
        if(yDiff <= 0) yDiff = 5;

        yMax = yMax + yDiff * 0.2f;
        yMin = yMin - yDiff * 0.2f;

        float xSize = graphWidth / (maxVisibleValues + 1);
        int xIndex = 0;

        GameObject lastDotGameObj = null;

        for (int i = valueList.Count - maxVisibleValues; i < valueList.Count; i++)
        {
            float xPos = xSize + xIndex * xSize;
            float yPos = ((valueList[i] - yMin) / (yMax - yMin)) * graphHeight;

            

            if (chartsOptions[selctedGhraphIndex] == "Line Chart")
            {
                GameObject dotGameObj = CreateDot(new Vector2(xPos, yPos));
                dotGameObj.transform.SetSiblingIndex(2);
                gameObjectList.Add(dotGameObj);

                if (lastDotGameObj != null)
                {
                    GameObject dotConGameObj = CreteDotConnection(lastDotGameObj.GetComponent<RectTransform>().anchoredPosition, dotGameObj.GetComponent<RectTransform>().anchoredPosition);
                    gameObjectList.Add(dotConGameObj);
                }

                lastDotGameObj = dotGameObj;
            }
            else
            {
                GameObject barGameObj = CreateBar(new Vector2(xPos, yPos), xSize);
                barGameObj.transform.SetSiblingIndex(2);
                gameObjectList.Add(barGameObj);
            }


            // Crete X label
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition= new Vector2(xPos, -4);
            labelX.GetComponent<TextMeshProUGUI>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            // Create X dash
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPos, dashTemplateX.anchoredPosition.y);
            gameObjectList.Add(dashX.gameObject);
            dashX.SetSiblingIndex(1);

            xIndex++;
        }

        int separatorCount = 10;
        for(int i = 0; i <= separatorCount; i++)
        {
            // Crete Y label
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);

            float normalizedValue = i * 1f / separatorCount;

            labelY.anchoredPosition = new Vector2(-17, normalizedValue * graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = getAxisLabelY(yMin + (normalizedValue * (yMax - yMin)));

            gameObjectList.Add(labelY.gameObject);

            // Create Y dash
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(dashTemplateY.anchoredPosition.x, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);

            dashY.SetSiblingIndex(1);
        }
    }

    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer.transform, false);
        gameObject.GetComponent<Image>().sprite = dotSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);

        return gameObject;
    }

    private GameObject CreteDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject gameObj = new GameObject("dotConnection", typeof(Image));
        gameObj.transform.SetParent(graphContainer, false);
        gameObj.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = gameObj.GetComponent<RectTransform>();
        Vector2 dir = (dotPosB - dotPosA).normalized;
        float distance  = Vector2.Distance(dotPosA, dotPosB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3);
        rectTransform.anchoredPosition = dotPosA + dir * distance * 0.5f; // Set the position in the middle of the two dots
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg); // Set the rotation to point in the direction of the line

        return gameObj;
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.transform.SetParent(graphContainer.transform, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0);
        rectTransform.sizeDelta = new Vector2(barWidth * 0.95f, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot= new Vector2(0.5f, 0);

        return gameObject;
    }
}
