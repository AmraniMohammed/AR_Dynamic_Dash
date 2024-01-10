using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(PieChartGraph))]
[CanEditMultipleObjects]
public class PieChartGraphEditor : Editor
{
    PieChartGraph pieChartGraph;
    SerializedProperty title;


    void OnEnable()
    {
        // Get the target object as a WindowGraph instance
        pieChartGraph = (PieChartGraph)target;


        title = serializedObject.FindProperty("title");

        pieChartGraph.ShowGraphEditorMode();

    }


    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Percentage value");
        pieChartGraph.percentageValue = EditorGUILayout.Slider(pieChartGraph.percentageValue, 0, 1);
        pieChartGraph.pieChartFilled.GetComponent<Image>().fillAmount = pieChartGraph.percentageValue;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Background ring color");
        pieChartGraph.pieChartBackground.GetComponent<Image>().color = EditorGUILayout.ColorField(pieChartGraph.pieChartBackground.GetComponent<Image>().color);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Filled ring color");
        pieChartGraph.pieChartFilled.GetComponent<Image>().color = EditorGUILayout.ColorField(pieChartGraph.pieChartFilled.GetComponent<Image>().color);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Title");
        title.stringValue = EditorGUILayout.TextField(title.stringValue);
        EditorGUILayout.EndHorizontal();

        pieChartGraph.transform.Find("Percentage").GetComponent<TextMeshProUGUI>().text = (System.Math.Round(pieChartGraph.percentageValue, 3) * 100).ToString() + "%";
        pieChartGraph.transform.Find("Percentage").GetComponent<TextMeshProUGUI>().color = pieChartGraph.pieChartFilled.GetComponent<Image>().color;

        pieChartGraph.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = title.stringValue;

        serializedObject.ApplyModifiedProperties();

    }
}
