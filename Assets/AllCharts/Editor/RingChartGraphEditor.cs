using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(RingChartGraph))]
[CanEditMultipleObjects]
public class pieChartGraphEditor : Editor
{
 

    RingChartGraph ringChartGraph;
    SerializedProperty title;


    void OnEnable()
    {
        // Get the target object as a WindowGraph instance
        ringChartGraph = (RingChartGraph)target;


        title = serializedObject.FindProperty("title");

        ringChartGraph.ShowGraphEditorMode();

    }


    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Percentage value");
        ringChartGraph.percentageValue = EditorGUILayout.Slider(ringChartGraph.percentageValue, 0, 1);    
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Background ring color");
        ringChartGraph.ringChartBackground.GetComponent<Image>().color = EditorGUILayout.ColorField(ringChartGraph.ringChartBackground.GetComponent<Image>().color);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Filled ring color");
        ringChartGraph.ringChartFilled.GetComponent<Image>().color = EditorGUILayout.ColorField(ringChartGraph.ringChartFilled.GetComponent<Image>().color);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Title");
        title.stringValue = EditorGUILayout.TextField(title.stringValue);
        EditorGUILayout.EndHorizontal();

        ringChartGraph.ringChartFilled.GetComponent<Image>().fillAmount = ringChartGraph.percentageValue;
        ringChartGraph.transform.Find("Percentage").GetComponent<TextMeshProUGUI>().text = (System.Math.Round(ringChartGraph.percentageValue, 3) * 100).ToString() + "%";
        ringChartGraph.transform.Find("Percentage").GetComponent<TextMeshProUGUI>().color = ringChartGraph.ringChartFilled.GetComponent<Image>().color;

        ringChartGraph.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = title.stringValue;

        serializedObject.ApplyModifiedProperties();

    }
}
