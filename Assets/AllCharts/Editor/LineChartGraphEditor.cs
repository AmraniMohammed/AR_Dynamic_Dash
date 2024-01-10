using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LineChartGraph))]
[CanEditMultipleObjects]
public class LineChartGraphEditor : Editor
{
    SerializedProperty backgroundColor;
    SerializedProperty lineColor;
    SerializedProperty labelColor;
    SerializedProperty title;

    LineChartGraph lineChartGraph;

    private bool showDictionaryData = false;


    void OnEnable()
    {
        // Get the target object as a WindowGraph instance
        lineChartGraph = (LineChartGraph)target;

        // Setup the SerializedProperties.
        backgroundColor = serializedObject.FindProperty("backgroundColor");
        lineColor = serializedObject.FindProperty("lineColor");
        labelColor = serializedObject.FindProperty("labelColor");
        title = serializedObject.FindProperty("title");
        //chartsOptionsProp = serializedObject.FindProperty("chartsOptions");
        lineChartGraph.ShowGraphEditorMode();

    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max number of visble values");
        lineChartGraph.maxVisibleValues = EditorGUILayout.IntField(lineChartGraph.maxVisibleValues);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Title");
        title.stringValue = EditorGUILayout.TextField(title.stringValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Background color");
        backgroundColor.colorValue = EditorGUILayout.ColorField(backgroundColor.colorValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Line color");
        lineColor.colorValue = EditorGUILayout.ColorField(lineColor.colorValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Label color");
        labelColor.colorValue = EditorGUILayout.ColorField(labelColor.colorValue);
        EditorGUILayout.EndHorizontal();

        // Foldout for the dictionary data
        EditorGUILayout.Space();
        showDictionaryData = EditorGUILayout.Foldout(showDictionaryData, "Data", true);

        if (showDictionaryData)
        {
            EditorGUI.indentLevel++;

            // Create a list of keys before iterating over the dictionary
            List<object> keysList = new List<object>(lineChartGraph.DataTable.Keys);

            // Display the dictionary data with editable fields
            foreach (var key in keysList)
            {
                EditorGUILayout.BeginHorizontal();

                // Display and edit the key
                object newKey = DisplayKeyField(key);

                // Display and edit the value
                float newValue = EditorGUILayout.FloatField(lineChartGraph.DataTable[key]);

                EditorGUILayout.EndHorizontal();

                // Update the dictionary with the new key and value
                UpdateDictionaryEntry(key, newKey, newValue);
            }

            // Button to add a new entry
            if (GUILayout.Button("Add Entry"))
            {
                lineChartGraph.DataTable.Add(GetUniqueKey(), 0f); // Initial value is set to 0
            }

            EditorGUI.indentLevel--;
        }


        lineChartGraph.UpdateChangesEditor();

        serializedObject.ApplyModifiedProperties();
    }

    private object DisplayKeyField(object key)
    {
        EditorGUI.BeginChangeCheck();

        // Display and edit the key based on its type
        if (key is int)
        {
            key = EditorGUILayout.IntField((int)key);
        }
        else if (key is float)
        {
            key = EditorGUILayout.FloatField((float)key);
        }
        else
        {
            key = EditorGUILayout.TextField(key.ToString());
        }

        // Check for changes in the key
        if (EditorGUI.EndChangeCheck())
        {
            key = ValidateUniqueKey(key); // Ensure key is unique
        }

        return key;
    }

    private void UpdateDictionaryEntry(object oldKey, object newKey, float value)
    {
        if (!oldKey.Equals(newKey))
        {
            // Remove the old key and add the new key
            lineChartGraph.DataTable.Remove(oldKey);
            lineChartGraph.DataTable.Add(newKey, value);
        }
        else
        {
            // Update the value if the key remains the same
            lineChartGraph.DataTable[oldKey] = value;
        }
    }

    private object GetUniqueKey()
    {
        int counter = 1;

        // Generate a new unique key
        while (lineChartGraph.DataTable.ContainsKey("NewKey" + counter))
        {
            counter++;
        }

        return "NewKey" + counter;
    }

    private object ValidateUniqueKey(object key)
    {
        // Ensure the key is unique
        if (!lineChartGraph.DataTable.ContainsKey(key))
        {
            return key;
        }

        // If not unique, generate a new unique key
        return GetUniqueKey();
    }
}
