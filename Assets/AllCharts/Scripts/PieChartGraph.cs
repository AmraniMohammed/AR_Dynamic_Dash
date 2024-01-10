using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PieChartGraph : MonoBehaviour
{
    public RectTransform pieChart;
    public RectTransform pieChartBackground;
    public RectTransform pieChartFilled;
    public RectTransform percentage;
    public string title = "Pie Chart";

    [SerializeField] public Color backgroundColor = new Color32(84, 112, 198, 50);
    [SerializeField] public Color pieBackgroundColor = new Color32(255, 255, 255, 35);
    public Color pieColor = new Color32(84, 112, 198, 255);

    public float percentageValue = 0.675f;

    public void ShowGraphEditorMode()
    {
        if (!Application.isPlaying)
        {
            pieChartBackground = (RectTransform)transform.Find("PieChartBackground");
            pieChartFilled = (RectTransform)transform.Find("PieChartFilled");
            percentage = (RectTransform)transform.Find("Percentage");

            Debug.Log("Editor mode");
            ShowGraph(percentageValue);
        }
    }


    private void Awake()
    {
        pieChartBackground = (RectTransform)transform.Find("PieChartBackground");
        pieChartFilled = (RectTransform)transform.Find("PieChartFilled");
        percentage = (RectTransform)transform.Find("Percentage");

        ShowGraph(percentageValue);
    }

    public void ShowGraph(float percentageValue)
    {
        // Ring background
        pieChartBackground.GetComponent<Image>().fillAmount = 1;


        // Ring filled
        pieChartFilled.GetComponent<Image>().fillAmount = percentageValue;


        // Percentage Text
        percentage.GetComponent<TextMeshProUGUI>().text = (System.Math.Round(percentageValue, 3)).ToString() + "%";
        percentage.GetComponent<TextMeshProUGUI>().fontSize = 32;
        percentage.GetComponent<TextMeshProUGUI>().enableWordWrapping = false;



    }

}
