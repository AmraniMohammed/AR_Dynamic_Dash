using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using XCharts.Runtime;

public class RingChartGraph : MonoBehaviour
{
    public RectTransform ringChart;
    public RectTransform ringChartBackground;
    public RectTransform ringChartFilled;
    public RectTransform percentage;
    public string title = "Ring Chart";

    [SerializeField] public Color backgroundColor = new Color32(84, 112, 198, 50);
    [SerializeField] public Color ringBackgroundColor = new Color32(255, 255, 255, 35);
    public Color ringColor = new Color32(84, 112, 198, 255);

    public float percentageValue = 0.675f;

    private Coroutine fillCoroutine;
    float animationDuration = 1f;


    public void ShowGraphEditorMode()
    {
        if (!Application.isPlaying)
        {
            ringChartBackground = (RectTransform)transform.Find("RingChartBackground");
            ringChartFilled = (RectTransform)transform.Find("RingChartFilled");
            percentage = (RectTransform)transform.Find("Percentage");       

            Debug.Log("Editor mode");
            ShowGraph(percentageValue);
        }
    }


    private void Awake()
    {
        ringChartBackground = (RectTransform)transform.Find("RingChartBackground");
        ringChartFilled = (RectTransform)transform.Find("RingChartFilled");
        percentage = (RectTransform)transform.Find("Percentage");

        ShowGraph(percentageValue);
    }

    public void ShowGraph(float newPercentageValue)
    {
        percentageValue = newPercentageValue;
        // Ring background
        ringChartBackground.GetComponent<Image>().fillAmount = 1;

        // Cancel existing coroutine if it's running
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        // Start a new coroutine to smoothly update the fillAmount
        fillCoroutine = StartCoroutine(FillRingSmoothly(percentageValue, animationDuration));

        // Ring filled
        //ringChartFilled.GetComponent<Image>().fillAmount = percentageValue;
        //ringChartFilled.GetComponent<Image>().fillAmount = Mathf.Lerp(ringChartFilled.GetComponent<Image>().fillAmount, percentageValue, Time.deltaTime);


        // Percentage Text
        percentage.GetComponent<TextMeshProUGUI>().text = (System.Math.Round(percentageValue, 3) * 100).ToString() + "%";
        percentage.GetComponent<TextMeshProUGUI>().fontSize = 32;
        percentage.GetComponent<TextMeshProUGUI>().enableWordWrapping= false;


        
    }

    private IEnumerator FillRingSmoothly(float targetPercentage, float duration)
    {
        float startPercentage = ringChartFilled.GetComponent<Image>().fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            ringChartFilled.GetComponent<Image>().fillAmount = Mathf.Lerp(startPercentage, targetPercentage, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the fillAmount reaches the exact target value
        ringChartFilled.GetComponent<Image>().fillAmount = targetPercentage;

        fillCoroutine = null; // Reset the coroutine reference
    }

}
