using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(Graph))]
[CanEditMultipleObjects]
public class GraphEditor : Editor
{
    SerializedProperty dotSpriteProp;
    SerializedProperty graphContainerProp;
    SerializedProperty labelTemplateXProp;
    SerializedProperty labelTemplateYProp;
    SerializedProperty dashTemplateXProp;
    SerializedProperty dashTemplateYProp;
    Graph graph;

    void OnEnable()
    {
        // Get the target object as a WindowGraph instance
        graph = (Graph)target;

        // Setup the SerializedProperties.
        dotSpriteProp = serializedObject.FindProperty("dotSprite");
        graphContainerProp = serializedObject.FindProperty("graphContainer");
        labelTemplateXProp = serializedObject.FindProperty("labelTemplateX");
        labelTemplateYProp = serializedObject.FindProperty("labelTemplateY");
        dashTemplateXProp = serializedObject.FindProperty("dashTemplateX");
        dashTemplateYProp = serializedObject.FindProperty("dashTemplateY");
        //chartsOptionsProp = serializedObject.FindProperty("chartsOptions");

    }


    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        // Show the custom GUI controls.
        EditorGUILayout.ObjectField(dotSpriteProp, typeof(Sprite));
        //EditorGUILayout.IntSlider(damageProp, 0, 100, new GUIContent("Damage"));

        EditorGUILayout.ObjectField(graphContainerProp, typeof(RectTransform));
        EditorGUILayout.ObjectField(labelTemplateXProp, typeof(RectTransform));
        EditorGUILayout.ObjectField(labelTemplateYProp, typeof(RectTransform));
        EditorGUILayout.ObjectField(dashTemplateXProp, typeof(RectTransform));
        EditorGUILayout.ObjectField(dashTemplateYProp, typeof(RectTransform));

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max number of visble values");
        graph.maxVisibleValues = EditorGUILayout.IntField(graph.maxVisibleValues);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Graph type");
        graph.chartsOptionsIndex = EditorGUILayout.Popup(graph.chartsOptionsIndex, new string[] { "Line Chart", "Bar Chart", "Ring Chart", "Pie Chart" });
        EditorGUILayout.EndHorizontal();


        //// Only show the damage progress bar if all the objects have the same damage value:
        //if (!damageProp.hasMultipleDifferentValues)
        //    ProgressBar(damageProp.intValue / 100.0f, "Damage");

        //EditorGUILayout.IntSlider(armorProp, 0, 100, new GUIContent("Armor"));

        //// Only show the armor progress bar if all the objects have the same armor value:
        //if (!armorProp.hasMultipleDifferentValues)
        //    ProgressBar(armorProp.intValue / 100.0f, "Armor");

        //EditorGUILayout.PropertyField(gunProp, new GUIContent("Gun Object"));

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();

        //base.OnInspectorGUI();

        //if (GUILayout.Button("Show Graph"))
        //{
        //    graph.ShowGraph(graph.valueList, graph.maxVisibleValues, (int _i) => "Day " + (_i + 1), (float _f) => "$" + (Mathf.RoundToInt(_f))); ;
        //}
    }

    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}