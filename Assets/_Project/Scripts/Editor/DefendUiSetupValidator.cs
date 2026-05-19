using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class DefendUiSetupValidator
{
    private const string _reportPath = "Docs/DefendUiSetupValidationReport.md";

    [MenuItem("Tools/Defend/UI/Validate Selected UI")]
    public static void ValidateSelectedUi()
    {
        List<string> findings = CollectFindingsForSelection();

        if (findings.Count == 0)
        {
            Debug.Log("Defend UI setup validation completed with no missing serialized references.");
            return;
        }

        for (int i = 0; i < findings.Count; i++)
        {
            Debug.LogError(findings[i]);
        }
    }

    [MenuItem("Tools/Defend/UI/Generate Selected UI Validation Report")]
    public static void GenerateSelectedUiValidationReport()
    {
        List<string> findings = CollectFindingsForSelection();
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("# Defend UI Setup Validation Report");
        builder.AppendLine();
        builder.AppendLine("Generated from the Unity Editor selection. This tool is read-only and does not modify vendor assets, scenes, or prefabs.");
        builder.AppendLine();

        if (Selection.gameObjects.Length == 0)
        {
            builder.AppendLine("No GameObjects were selected.");
        }
        else if (findings.Count == 0)
        {
            builder.AppendLine("No missing serialized object references were found on selected UI views.");
        }
        else
        {
            for (int i = 0; i < findings.Count; i++)
            {
                builder.AppendLine($"- {findings[i]}");
            }
        }

        Directory.CreateDirectory(Path.GetDirectoryName(_reportPath));
        File.WriteAllText(_reportPath, builder.ToString());
        AssetDatabase.Refresh();
        Debug.Log($"Defend UI setup validation report written to {_reportPath}.");
    }

    private static List<string> CollectFindingsForSelection()
    {
        List<string> findings = new List<string>();
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            findings.Add("No GameObjects selected. Select scene UI roots or UI prefabs before running validation.");
            return findings;
        }

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            CollectFindings(selectedObjects[i], findings);
        }

        return findings;
    }

    private static void CollectFindings(GameObject root, List<string> findings)
    {
        MonoBehaviour[] components = root.GetComponentsInChildren<MonoBehaviour>(true);

        for (int i = 0; i < components.Length; i++)
        {
            MonoBehaviour component = components[i];

            if (component == null)
            {
                findings.Add($"{root.name}: missing MonoBehaviour script under selected UI hierarchy.");
                continue;
            }

            if (IsDefendUiComponent(component) == false)
            {
                continue;
            }

            CollectMissingObjectReferences(component, findings);
        }
    }

    private static bool IsDefendUiComponent(MonoBehaviour component)
    {
        string typeName = component.GetType().Name;
        return typeName.EndsWith("View") || typeName == nameof(PopupLayer);
    }

    private static void CollectMissingObjectReferences(MonoBehaviour component, List<string> findings)
    {
        SerializedObject serializedObject = new SerializedObject(component);
        SerializedProperty property = serializedObject.GetIterator();
        bool enterChildren = true;

        while (property.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (property.name == "m_Script")
            {
                continue;
            }

            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                continue;
            }

            if (property.objectReferenceValue != null)
            {
                continue;
            }

            findings.Add($"{component.gameObject.name}: {component.GetType().Name} missing serialized reference '{property.propertyPath}'.");
        }
    }
}
