using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class DataUpdateManager : MonoBehaviour
{
    public pieChart oee;
    public pieChart availability;
    public pieChart performance;
    public pieChart quality;
    public BarChart defectDistribution;
    [SerializeField]
    public LineChart meanTime;
    public int index = 0;
    public string serieName = "serie0";



    private void Update()
    {
        //Debug.Log(oee.GetData(0, 1, 2));
        //oee.UpdateData("serie0", 1, 50);
        Debug.Log(oee.GetSerie(serieName).GetSerieData(index).data[index]);
        Debug.Log(oee.GetSerie(serieName).GetSerieData(index + 1).data[index + 1]);
    }
}
