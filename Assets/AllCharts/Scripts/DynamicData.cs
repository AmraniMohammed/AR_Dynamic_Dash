using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DynamicData : MonoBehaviour
{

    public LineChartGraph meanTime;
    public RingChartGraph quality;
    public RingChartGraph availability;
    public RingChartGraph performance;
    public RingChartGraph oee;
    public RectTransform greenAlert;
    public RectTransform redAlert;
    private string dayPos = "PM";
    int cnt = 2;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("AddDataPeriodicallyLineChart", 0f, 15f);

        if(quality != null && availability != null && performance != null && oee != null) InvokeRepeating("AddDataPeriodicallyOEE", 0f, 8f);

        if (meanTime != null) InvokeRepeating("AddDataPeriodicallyLineChart", 0f, 5f);
    }

    private void Update()
    {
        if(oee.percentageValue < 0.6)
        {
            greenAlert.gameObject.SetActive(false);
            redAlert.gameObject.SetActive(true);

            //redAlert.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "OEE < 60%";
        }
        else
        {
            greenAlert.gameObject.SetActive(true);
            redAlert.gameObject.SetActive(false);

            //greenAlert.transform.Find("Message").GetComponent<TextMeshProUGUI>().text = "OEE >= 60%";
        }
    }

    // Function to be called every 30 seconds
    void AddDataPeriodicallyLineChart()
    {
        // Generate a random value between 0 and 50
        float randomValue = Random.Range(0f, 50f);

        if(cnt > 12)
        {
            if (dayPos == "PM") dayPos = "AM";
            else dayPos = "PM";

            cnt = 1;
        }

        meanTime.AddData(cnt + " " + dayPos, randomValue);
        cnt++;

        //Canvas.ForceUpdateCanvases();
        //Selection.activeGameObject = meanTime.gameObject;
    }
    
    // Function to be called every 30 seconds
    void AddDataPeriodicallyOEE()
    {
        //Selection.activeGameObject = this.gameObject;
        // Generate a random value between 0 and 50
        float randomValueA = Random.Range(0.65f, 0.99f);
        float randomValueP = Random.Range(0.65f, 0.99f);
        float randomValueQ = Random.Range(0.65f, 0.99f);

        quality.ShowGraph(randomValueQ);
        performance.ShowGraph(randomValueP);
        availability.ShowGraph(randomValueA);
        oee.ShowGraph(randomValueA * randomValueP * randomValueQ);

    }

    IEnumerator RepeatDataPeriodicallyLineChart(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            AddDataPeriodicallyLineChart();
        }
    }
}
