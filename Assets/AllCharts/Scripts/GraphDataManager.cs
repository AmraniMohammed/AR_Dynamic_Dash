using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GraphDataManager : MonoBehaviour
{
    public GraphData LoadCSV(string path)
    {
        // Read the CSV file
        string[] lines = System.IO.File.ReadAllLines(path);

        // Split the lines into columns
        List<object> xValues = new List<object>();
        List<object> yValues = new List<object>();

        foreach (var line in lines.Skip(1)) // Skip the header line
        {
            var values = line.Split(',');
            xValues.Add(values[0]);
            yValues.Add(values[1]); // It could be a string or float
        }

        // Create a GraphData object
        GraphData graphData = new GraphData
        {
            xValues = xValues,
            yValues = yValues
        };

        return graphData;
    }
}
public class GraphData
{
    public List<object> xValues;
    public List<object> yValues;
}
