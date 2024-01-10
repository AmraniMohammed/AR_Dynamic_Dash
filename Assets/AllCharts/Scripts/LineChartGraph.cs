using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineChartGraph : MonoBehaviour
{
    public Sprite dotSprite;
    public RectTransform graphContainer;
    public RectTransform labelTemplateX;
    public RectTransform labelTemplateY;
    public RectTransform dashTemplateX;
    public RectTransform dashTemplateY;

    [SerializeField] public Color backgroundColor = new Color32(255, 255, 255, 100);
    public Color lineColor = new Color32(84, 112, 198, 255);
    public Color labelColor = new Color32(84, 112, 198, 255);

    [SerializeField] public string title = "Line chart";

    public List<object> valueListX = new List<object>();
    public List<float> valueListY = new List<float>();

    private Dictionary<object, float> dataTable = new Dictionary<object, float>
    {
        { "1 AM", 5.0f },
        { "2 AM", 10.0f },
        { "3 AM", 30.0f },
        { "4 AM", 15.0f },
        { "5 AM", 45.0f },
        { "6 AM", 7.0f },
        { "7 AM", 2.0f },
        { "8 AM", 50.0f },
        { "9 AM", 35.0f },
        { "10 AM", 25.0f },
        { "11 AM", 14.0f },
        { "12 AM", 34.0f },
        { "1 PM", 11.0f },
    };

    public int maxVisibleValues = 5;

    private List<GameObject> gameObjectList = new List<GameObject>();

    // Public property to get and set the private variable
    public Dictionary<object, float> DataTable
    {
        get { return dataTable; }
        set { dataTable = value; }
    }

    public void ShowGraphEditorMode()
    {
        if (!Application.isPlaying)
        {
            GetComponent<Image>().color = backgroundColor;
            graphContainer = (RectTransform)transform.Find("GraphContainer");
            dotSprite = Resources.Load<Sprite>("GrahsUI/Circle");
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

            ShowGraph(maxVisibleValues, (string xLabel) => xLabel, (float yLabel) => yLabel.ToString());
        }
    }

    private void Awake()
    {
        GetComponent<Image>().color = backgroundColor;
        graphContainer = (RectTransform)transform.Find("GraphContainer");
        dotSprite = Resources.Load<Sprite>("GrahsUI/Circle");
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


        ShowGraph(maxVisibleValues, (string xLabel) => xLabel, (float yLabel) => yLabel.ToString());
    }

    private void Start()
    {
        dataTable = new Dictionary<object, float>
    {
        { "1 AM", 5.0f },
        { "2 AM", 10.0f },
        { "3 AM", 30.0f },
        { "4 AM", 15.0f },
        { "5 AM", 45.0f },
        { "6 AM", 7.0f },
        { "7 AM", 2.0f },
        { "8 AM", 50.0f },
        { "9 AM", 35.0f },
        { "10 AM", 25.0f },
        { "11 AM", 14.0f },
        { "12 AM", 34.0f },
        { "1 PM", 11.0f },
    };

        UpdateChangesEditor();
    }

    public void ShowGraph(int maxVisibleValues = -1, Func<string, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null)
    {
        foreach (GameObject gameObject in gameObjectList)
        {
            DestroyImmediate(gameObject);
        }
        gameObjectList.Clear();

        DestroyAllChildren(graphContainer.transform);

        valueListX.Clear();
        valueListY.Clear();

        foreach (var kvp in dataTable)
        {
            valueListX.Add(kvp.Key);
            valueListY.Add(kvp.Value);
        }


        if (maxVisibleValues > valueListX.Count || maxVisibleValues <= 0) maxVisibleValues = valueListX.Count;

        if (getAxisLabelX == null)
        {
            getAxisLabelX = (string xLabel) => xLabel;
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = (float yLabel) => yLabel.ToString();
        }

        float graphHeight = graphContainer.sizeDelta.y;
        float graphWidth = graphContainer.sizeDelta.x;

        float yMax = valueListY[0];
        //float yMin = valueListY[0];
        float yMin = 0;

        for (int i = valueListY.Count - maxVisibleValues; i < valueListY.Count; i++)
        {
            float value = valueListY[i];
            if (value > yMax) yMax = value;
            //if (value < yMin) yMin = value;
        }

        float yDiff = yMax - yMin;
        if (yDiff <= 0) yDiff = 5;

        yMax = yMax + yDiff * 0.2f; // yMin = 0 => yDiff = yMax
        //yMin = yMin - yDiff * 0.2f;
        yMin = 0;

        float xSize = graphWidth / (maxVisibleValues + 1);
        int xIndex = 0;


        GameObject lastDotGameObj = null;


        for (int i = valueListX.Count - maxVisibleValues; i < valueListX.Count; i++)
        {
            float xPos = xSize + xIndex * xSize;
            float yPos = (((float)valueListY[i] - yMin) / (yMax - yMin)) * graphHeight;

            GameObject dotGameObj = CreateDot(new Vector2(xPos, yPos));
            dotGameObj.transform.SetParent(graphContainer, false);
            dotGameObj.transform.SetSiblingIndex(2);
            gameObjectList.Add(dotGameObj);

            if (lastDotGameObj != null)
            {
                GameObject dotConGameObj = CreteDotConnection(lastDotGameObj.GetComponent<RectTransform>().anchoredPosition, dotGameObj.GetComponent<RectTransform>().anchoredPosition);
                dotConGameObj.transform.SetParent(graphContainer, false);
                gameObjectList.Add(dotConGameObj);
            }

            lastDotGameObj = dotGameObj;

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

            labelY.anchoredPosition = new Vector2(-18, (normalizedValue * graphHeight) + 2);
            labelY.GetComponent<TextMeshProUGUI>().text = getAxisLabelY(Mathf.RoundToInt(normalizedValue * yMax));

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

    private GameObject CreateDot(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("dot", typeof(Image));
        gameObject.transform.SetParent(graphContainer.transform, false);
        gameObject.GetComponent<Image>().sprite = dotSprite;
        gameObject.GetComponent<Image>().color = lineColor;
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
        gameObj.GetComponent<Image>().color = lineColor;
        RectTransform rectTransform = gameObj.GetComponent<RectTransform>();
        Vector2 dir = (dotPosB - dotPosA).normalized;
        float distance = Vector2.Distance(dotPosA, dotPosB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3);
        rectTransform.anchoredPosition = dotPosA + dir * distance * 0.5f; // Set the position in the middle of the two dots
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg); // Set the rotation to point in the direction of the line

        return gameObj;
    }

    public void UpdateChangesEditor()
    {
        ShowGraph(maxVisibleValues, (string xLabel) => xLabel, (float yLabel) => yLabel.ToString());

        // Force update canvases to ensure layout changes are applied
        Canvas.ForceUpdateCanvases();

        GetComponent<Image>().color = backgroundColor;

        transform.Find("Title").GetComponent<TextMeshProUGUI>().text = title;
        transform.Find("Title").SetSiblingIndex(2);

        List<Transform> dotsConnections = FindChildrenByName(graphContainer.transform, "dotConnection");
        List<Transform> dots = FindChildrenByName(graphContainer.transform, "dot");
        List<Transform> labelX = FindChildrenByName(graphContainer.transform, "LabelTemplateY(Clone)");
        List<Transform> labelY = FindChildrenByName(graphContainer.transform, "LabelTemplateX(Clone)");

        // Change the color of the found children
        foreach (Transform child in dotsConnections)
        {
            child.GetComponent<Image>().color = lineColor;
        }

        // Change the color of the found children
        foreach (Transform child in dots)
        {
            child.GetComponent<Image>().color = lineColor;
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

    // Function to add data to the dictionary
    public void AddData(object key, float value)
    {
        if (dataTable.ContainsKey(key))
        {
            dataTable.Add(key, value);
        }
        else
        {
            // If the key already exists, update the value
            dataTable[key] = value;
        }

        Debug.Log("Add data function");

        UpdateChangesEditor();
    }
}
