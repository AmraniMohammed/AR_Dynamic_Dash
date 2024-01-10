using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BarChartGraph : MonoBehaviour
{
    public RectTransform graphContainer;
    public RectTransform labelTemplateX;
    public RectTransform labelTemplateY;
    public RectTransform dashTemplateX;
    public RectTransform dashTemplateY;

    [SerializeField] public Color backgroundColor = new Color32(255, 255, 255, 100);
    public Color barColor = new Color32(84, 112, 198, 255);
    public Color labelColor = new Color32(84, 112, 198, 255);

    [SerializeField] public string title = "Bar chart";

    public List<object> valueListX = new List<object>();
    public List<int> valueListY = new List<int>();

    [SerializeField]
    public Dictionary<object, int> dataTable = new Dictionary<object, int>
    {
        { "2010", 5 },
        { "2011", 10 },
        { "2012", 50 },
        { "2013", 17 },
        { "2014", 60 },
        { "2015", 88 },
        { "2016", 20 },
        { "2017", 7 },
        { "2018", 11 },
        { "2019", 25 },
        { "2020", 13 },
    };

    [SerializeField] public int maxVisibleValues = 5;

    [SerializeField] public List<GameObject> gameObjectList = new List<GameObject>();

    public void ShowGraphEditorMode()
    {
        if (!Application.isPlaying)
        {
            this.GetComponent<Image>().color = backgroundColor;
            graphContainer = (RectTransform)transform.Find("GraphContainer");
            labelTemplateX = Resources.Load<RectTransform>("GrahsUI/LabelTemplateX");
            labelTemplateY = Resources.Load<RectTransform>("GrahsUI/LabelTemplatey");
            dashTemplateX = Resources.Load<RectTransform>("GrahsUI/DashTemplateX");
            dashTemplateY = Resources.Load<RectTransform>("GrahsUI/DashTemplateY");

            valueListX.Clear();
            valueListY.Clear();

            foreach (var kvp in dataTable)
            {
                valueListX.Add(kvp.Key);
                valueListY.Add(kvp.Value);
            }

            //DestroyAllChildren(graphContainer.transform);

            ShowGraph(maxVisibleValues, (string xLabel) => xLabel, (int yLabel) => yLabel.ToString());
        }
    }

    private void Awake()
    {
        this.GetComponent<Image>().color = backgroundColor;
        graphContainer = (RectTransform)transform.Find("GraphContainer");
        labelTemplateX = Resources.Load<RectTransform>("GrahsUI/LabelTemplateX");
        labelTemplateY = Resources.Load<RectTransform>("GrahsUI/LabelTemplatey");
        dashTemplateX = Resources.Load<RectTransform>("GrahsUI/DashTemplateX");
        dashTemplateY = Resources.Load<RectTransform>("GrahsUI/DashTemplateY");

        valueListX.Clear();
        valueListY.Clear();

        foreach (var kvp in dataTable)
        {
            valueListX.Add(kvp.Key);
            valueListY.Add(kvp.Value);
        }

        //DestroyAllChildren(graphContainer.transform);

        ShowGraph(maxVisibleValues, (string xLabel) => xLabel, (int yLabel) => yLabel.ToString());
    }

    private void Start()
    {
        UpdateChangesEditor();
    }

    public void ShowGraph(int maxVisibleValues = -1, Func<string, string> getAxisLabelX = null, Func<int, string> getAxisLabelY = null)
    {
        valueListX.Clear();
        valueListY.Clear();

        foreach (var kvp in dataTable)
        {
            valueListX.Add(kvp.Key);
            valueListY.Add(kvp.Value);
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            DestroyImmediate(gameObject);
        }
        gameObjectList.Clear();

        DestroyAllChildren(graphContainer.transform);


        if (maxVisibleValues > valueListX.Count || maxVisibleValues <= 0) maxVisibleValues = valueListX.Count;

        if (getAxisLabelX == null)
        {
            getAxisLabelX = (string xLabel) => xLabel;
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = (int yLabel) => yLabel.ToString();
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        float yMax = valueListY[0];
        //float yMin = valueListY[0];
        float yMin = 0;

        for (int i = valueListY.Count - maxVisibleValues; i < valueListY.Count; i++)
        {
            int value = valueListY[i];
            if (value > yMax) yMax = value;
            //if (value < yMin) yMin = value;
        }

        float yDiff = yMax - yMin;
        if (yDiff <= 0) yDiff = 5;

        yMax = yMax + yDiff * 0.2f;
        //yMin = yMin - yDiff * 0.2f;
        yMin = 0;

        float xSize = graphWidth / (maxVisibleValues + 1);
        int xIndex = 0;


        for (int i = valueListX.Count - maxVisibleValues; i < valueListX.Count; i++)
        {
            float xPos = xSize + xIndex * xSize;
            float yPos = ((valueListY[i] - yMin) / (yMax - yMin)) * graphHeight;

            GameObject barGameObj = CreateBar(new Vector2(xPos, yPos), xSize);
            barGameObj.transform.SetSiblingIndex(1);
            gameObjectList.Add(barGameObj);

            // Crete X label
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPos - 2, -4);
            labelX.GetComponent<TextMeshProUGUI>().text = getAxisLabelX(valueListX[i].ToString());
            gameObjectList.Add(labelX.gameObject);

            // Create X dash
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPos, dashTemplateX.anchoredPosition.y);
            gameObjectList.Add(dashX.gameObject);
            dashX.SetSiblingIndex(0);

            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            // Crete Y label
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);

            float normalizedValue = i * 1f / separatorCount;

            labelY.anchoredPosition = new Vector2(-17, normalizedValue * graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = getAxisLabelY(Mathf.RoundToInt(yMin + (normalizedValue * (yMax - yMin))));

            gameObjectList.Add(labelY.gameObject);

            // Create Y dash
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(dashTemplateY.anchoredPosition.x, normalizedValue * graphHeight);
            gameObjectList.Add(dashY.gameObject);

            dashY.SetSiblingIndex(0);
        }
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
        rectTransform.pivot = new Vector2(0.5f, 0);

        return gameObject;
    }

    public void UpdateChangesEditor()
    {
        

        ShowGraph(maxVisibleValues, (string xLabel) => xLabel, (int yLabel) => yLabel.ToString());


        transform.Find("Title").GetComponent<TextMeshProUGUI>().text = title;
        transform.Find("Title").SetSiblingIndex(2);
        GetComponent<Image>().color = backgroundColor;

        List<Transform> labelX = FindChildrenByName(graphContainer.transform, "LabelTemplateY(Clone)");
        List<Transform> labelY = FindChildrenByName(graphContainer.transform, "LabelTemplateX(Clone)");

        // Find all children with the name "bar"
        List<Transform> children = FindChildrenByName(graphContainer.transform, "bar");

        // Change the color of the found children
        foreach (Transform child in children)
        {
            child.GetComponent<Image>().color = barColor;
        }

        // Change the color of the found children
        foreach (Transform child in labelX)
        {
            child.GetComponent<TextMeshProUGUI>().color = labelColor;
        }

        // Change the color of the found children
        foreach (Transform child in labelY)
        {
            child.GetComponent<TextMeshProUGUI>().color = labelColor;
        }
    }

    // Function to find children by name
    List<Transform> FindChildrenByName(Transform parent, string name)
    {
        // List to store matching children
        List<Transform> matchingChildren = new List<Transform>();

        // Iterate through each child of the parent
        foreach (Transform child in parent)
        {
            // Check if the child's name matches the desired name
            if (child.name == name)
            {
                // Add the matching child to the list
                matchingChildren.Add(child);
            }

            // Recursively call the function for nested children
            List<Transform> nestedChildren = FindChildrenByName(child, name);
            matchingChildren.AddRange(nestedChildren);
        }

        // Return the list of matching children
        return matchingChildren;
    }

    void DestroyAllChildren(Transform parent)
    {
        // Iterate through all children of the parent
        foreach (Transform child in parent)
        {
            // Destroy the child GameObject
            DestroyImmediate(child.gameObject);
        }
    }

}
