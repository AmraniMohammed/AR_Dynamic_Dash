using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class GraphContextMenu
{
    [MenuItem("GameObject/AllGraphs/Line Chart", false, 10)]
    static void CreateLineChartGraph(MenuCommand menuCommand)
    {
        CreateGraphObject<LineChartGraph>("Line Chart", menuCommand);
    }

    [MenuItem("GameObject/AllGraphs/Bar Chart", false, 10)]
    static void CreateBarChartGraph(MenuCommand menuCommand)
    {
        CreateGraphObject<BarChartGraph>("Bar Chart", menuCommand);
    }
    [MenuItem("GameObject/AllGraphs/Ring Chart", false, 10)]
    static void CreateRingChartGraph(MenuCommand menuCommand)
    {
        CreateGraphObject<RingChartGraph>("Ring Chart", menuCommand);
    }
    [MenuItem("GameObject/AllGraphs/Pie Chart", false, 10)]
    static void CreatepieChartGraph(MenuCommand menuCommand)
    {
        CreateGraphObject<PieChartGraph>("Pie Chart", menuCommand);
    }

    // Add more options as needed

    static void CreateGraphObject<T>(string name, MenuCommand menuCommand) where T : MonoBehaviour
    {
        RectTransform GraphRootPrefab;
        if (name == "Ring Chart") GraphRootPrefab = Resources.Load<RectTransform>("GrahsUI/RingChart");
        else if (name == "Pie Chart") GraphRootPrefab = Resources.Load<RectTransform>("GrahsUI/PieChart");
        else GraphRootPrefab = Resources.Load<RectTransform>("GrahsUI/GraphRoot");

        if (GraphRootPrefab != null)
        {
            // Find the Canvas component in the scene
            Canvas canvas = Object.FindObjectOfType<Canvas>();

            if (canvas == null)
            {
                // If Canvas is not found, create one dynamically
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
            }

            RectTransform GraphRoot = Object.Instantiate(GraphRootPrefab, canvas.transform);
            GraphRoot.name = name;
            GraphRoot.gameObject.AddComponent<T>();
            if(name == "Ring Chart" || name == "Pie Chart") GraphRoot.sizeDelta = new Vector2(300, 300);

            GameObjectUtility.SetParentAndAlign(GraphRoot.gameObject, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(GraphRoot.gameObject, "Create " + GraphRoot.gameObject.name);
            Selection.activeObject = GraphRoot.gameObject;
        }

        
    }
}
