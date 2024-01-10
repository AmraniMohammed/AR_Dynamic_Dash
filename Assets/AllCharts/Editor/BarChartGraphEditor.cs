using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(BarChartGraph))]
[CanEditMultipleObjects]
public class BarChartGraphEditor : Editor
{
    SerializedProperty backgroundColor;
    SerializedProperty barColor;
    SerializedProperty maxVisibleValues;
    SerializedProperty labelColor;
    SerializedProperty title;

    private bool showDictionaryData = false;

    BarChartGraph barChartGraph;


    void OnEnable()
    {
        // Get the target object as a WindowGraph instance
        barChartGraph = (BarChartGraph)target;

        // Setup the SerializedProperties.
        backgroundColor = serializedObject.FindProperty("backgroundColor");
        barColor = serializedObject.FindProperty("barColor");
        labelColor = serializedObject.FindProperty("labelColor");
        maxVisibleValues = serializedObject.FindProperty("maxVisibleValues");
        title = serializedObject.FindProperty("title");
        barChartGraph.ShowGraphEditorMode();

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Max number of visble values");
        maxVisibleValues.intValue = EditorGUILayout.IntField(maxVisibleValues.intValue);
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
        EditorGUILayout.LabelField("Bar color");
        barColor.colorValue = EditorGUILayout.ColorField(barColor.colorValue);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Label color");
        labelColor.colorValue = EditorGUILayout.ColorField(labelColor.colorValue);
        EditorGUILayout.EndHorizontal();

        // Foldout for the dictionary data
        EditorGUILayout.Space();
        showDictionaryData = EditorGUILayout.Foldout(showDictionaryData, "Data", false);

        if (showDictionaryData)
        {
            EditorGUI.indentLevel++;

            // Create a list of keys before iterating over the dictionary
            List<object> keysList = new List<object>(barChartGraph.dataTable.Keys);

            // Display the dictionary data with editable fields
            foreach (var key in keysList)
            {
                EditorGUILayout.BeginHorizontal();

                // Display and edit the key
                object newKey = DisplayKeyField(key);

                // Display and edit the value
                int newValue = EditorGUILayout.IntField(barChartGraph.dataTable[key]);

                EditorGUILayout.EndHorizontal();

                // Update the dictionary with the new key and value
                UpdateDictionaryEntry(key, newKey, newValue);
            }

            // Button to add a new entry
            if (GUILayout.Button("Add Entry"))
            {
                barChartGraph.dataTable.Add(GetUniqueKey(), 0); // Initial value is set to 0
            }

            EditorGUI.indentLevel--;
        }

        barChartGraph.UpdateChangesEditor();


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

    private void UpdateDictionaryEntry(object oldKey, object newKey, int value)
    {
        if (!oldKey.Equals(newKey))
        {
            // Remove the old key and add the new key
            barChartGraph.dataTable.Remove(oldKey);
            barChartGraph.dataTable.Add(newKey, value);
        }
        else
        {
            // Update the value if the key remains the same
            barChartGraph.dataTable[oldKey] = value;
        }
    }

    private object GetUniqueKey()
    {
        int counter = 1;

        // Generate a new unique key
        while (barChartGraph.dataTable.ContainsKey("NewKey" + counter))
        {
            counter++;
        }

        return "NewKey" + counter;
    }

    private object ValidateUniqueKey(object key)
    {
        // Ensure the key is unique
        if (!barChartGraph.dataTable.ContainsKey(key))
        {
            return key;
        }

        // If not unique, generate a new unique key
        return GetUniqueKey();
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
}
