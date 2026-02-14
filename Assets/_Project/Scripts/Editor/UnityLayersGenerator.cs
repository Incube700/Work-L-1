using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class UnityLayersGenerator
{
    private const string OutputPath = "Assets/_Project/Scripts/Generated/UnityLayers.cs";

    [MenuItem("Tools/Generate Unity Layers")]
    public static void Generate()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("using UnityEngine;");
        sb.AppendLine();
        sb.AppendLine("public static class UnityLayers");
        sb.AppendLine("{");

        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);

            if (string.IsNullOrWhiteSpace(layerName))
                continue;

            string safeName = MakeSafeIdentifier(layerName);

            sb.AppendLine($"    public static readonly int {safeName} = LayerMask.NameToLayer(\"{layerName}\");");
        }

        sb.AppendLine();

        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);

            if (string.IsNullOrWhiteSpace(layerName))
                continue;

            string safeName = MakeSafeIdentifier(layerName);

            sb.AppendLine($"    public static readonly int Mask{safeName} = 1 << {safeName};");
        }

        sb.AppendLine("}");

        Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
        File.WriteAllText(OutputPath, sb.ToString());

        AssetDatabase.Refresh();
        Debug.Log($"UnityLayers generated: {OutputPath}");
    }

    private static string MakeSafeIdentifier(string name)
    {
        StringBuilder sb = new StringBuilder();
        bool upperNext = true;

        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];

            if (char.IsLetterOrDigit(c) == false)
            {
                upperNext = true;
                continue;
            }

            sb.Append(upperNext ? char.ToUpperInvariant(c) : c);
            upperNext = false;
        }

        string result = sb.ToString();

        if (string.IsNullOrEmpty(result))
            result = "Layer";

        if (char.IsDigit(result[0]))
            result = "_" + result;

        return result;
    }
}