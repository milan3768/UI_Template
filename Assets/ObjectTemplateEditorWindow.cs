using UnityEngine;
using UnityEditor;
using System.IO;

public class ObjectTemplateEditorWindow : EditorWindow
{
    private string jsonFilePath = "UITemplate.json"; // Adjust the file path as needed
    private string jsonString = "";
    private Vector2 scrollPosition;

    [MenuItem("UI Template/Object Template Editor")]
    public static void ShowWindow()
    {
        ObjectTemplateEditorWindow window = GetWindow<ObjectTemplateEditorWindow>();
        window.titleContent = new GUIContent("Object Template Editor");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Object Template Editor", EditorStyles.boldLabel);

        // Load JSON data
        if (GUILayout.Button("Load JSON Data"))
        {
            LoadJSONData();
        }

        // Display JSON data
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUI.BeginDisabledGroup(true); // Make the text read-only
        jsonString = EditorGUILayout.TextArea(jsonString, GUILayout.Height(position.height - 100));
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndScrollView();

        // Show UI Template
        if (GUILayout.Button("Show UI Template"))
        {
            FindObjectOfType<JSONReader>().ShowUITemplate();
        }

        // Save JSON data
        if (GUILayout.Button("Save JSON Data"))
        {
            SaveJSONData();
        }
    }

    private void LoadJSONData()
    {
        if (File.Exists(jsonFilePath))
        {
            jsonString = File.ReadAllText(jsonFilePath);
        }
        else
        {
            Debug.LogError("JSON file does not exist.");
        }
    }

    private void SaveJSONData()
    {
        File.WriteAllText(jsonFilePath, jsonString);
        Debug.Log("JSON data saved.");
    }
}
