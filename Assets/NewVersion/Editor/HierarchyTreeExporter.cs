using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HierarchyTreeExporter
{
    [MenuItem("Tools/Export Hierarchy to Text")]
    public static void ExportHierarchy()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.IsValid())
        {
            EditorUtility.DisplayDialog("Erro", "Nenhuma cena ativa encontrada.", "OK");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Estrutura da Hierarchy com Componentes - Cena: {activeScene.name}");
        sb.AppendLine("==================================================================");

        GameObject[] rootObjects = activeScene.GetRootGameObjects();

        for (int i = 0; i < rootObjects.Length; i++)
        {
            bool isLastRoot = (i == rootObjects.Length - 1);
            PrintTree(rootObjects[i].transform, "", isLastRoot, sb);
        }

        string filePath = Path.Combine(Application.dataPath, "../estrutura_hierarchy_componentes.txt");
        File.WriteAllText(filePath, sb.ToString());
        
        EditorUtility.RevealInFinder(filePath);
        EditorUtility.DisplayDialog("Sucesso!", $"Árvore exportada com componentes para:\n{Path.GetFullPath(filePath)}", "OK");
    }

    private static void PrintTree(Transform current, string indent, bool isLast, StringBuilder sb)
    {
        string marker = isLast ? "└─ " : "├─ ";
        
        // Coleta os componentes do GameObject atual
        Component[] components = current.GetComponents<Component>();
        List<string> componentNames = new List<string>();

        foreach (Component comp in components)
        {
            // Ignora componentes nulos (scripts quebrados) e o Transform/RectTransform básico
            if (comp != null && !(comp is Transform))
            {
                componentNames.Add(comp.GetType().Name);
            }
        }

        // Formata a linha: NomeDoObjeto [Componente1, Componente2]
        string componentsText = componentNames.Count > 0 ? $" [{string.Join(", ", componentNames)}]" : "";
        sb.AppendLine($"{indent}{marker}{current.name}{componentsText}");

        string childIndent = indent + (isLast ? "   " : "│  ");

        for (int i = 0; i < current.childCount; i++)
        {
            bool isLastChild = (i == current.childCount - 1);
            PrintTree(current.GetChild(i), childIndent, isLastChild, sb);
        }
    }
}
