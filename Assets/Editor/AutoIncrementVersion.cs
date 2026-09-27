using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;

public class AutoIncrementVersion : IPostprocessBuildWithReport
{
    // Define a ordem de execução do script (0 é o padrão)
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        // Verifica se a build foi feita para Android e se terminou com sucesso
        if (report.summary.platform == BuildTarget.Android && report.summary.result == BuildResult.Succeeded)
        {
            // --- 1. Incremento do Bundle Version Code ---
            int currentVersionCode = PlayerSettings.Android.bundleVersionCode;
            int newVersionCode = currentVersionCode + 1;
            PlayerSettings.Android.bundleVersionCode = newVersionCode;

            // --- 2. Incremento do Version Name (ex: 1.0.4 -> 1.0.5) ---
            string currentVersion = PlayerSettings.bundleVersion;
            string newVersion = IncrementPatchVersion(currentVersion);
            PlayerSettings.bundleVersion = newVersion;

            Debug.Log($"[AutoIncrement] Build para Android bem-sucedida!\n" +
                      $"Bundle Version Code atualizado de {currentVersionCode} para {newVersionCode}.\n" +
                      $"Version Name atualizado de {currentVersion} para {newVersion}.");
        }
    }

    private string IncrementPatchVersion(string version)
    {
        // Divide a versão pelos pontos (Ex: "1.0.4" vira ["1", "0", "4"])
        string[] parts = version.Split('.');

        // Garante que existam pelo menos 3 partes (Major.Minor.Patch)
        if (parts.Length < 3)
        {
            Array.Resize(ref parts, 3);
            if (string.IsNullOrEmpty(parts[0])) parts[0] = "1";
            if (string.IsNullOrEmpty(parts[1])) parts[1] = "0";
            parts[2] = "0";
        }

        // Pega o último número (Patch), tenta converter e soma +1
        if (int.TryParse(parts[parts.Length - 1], out int patch))
        {
            patch++;
            parts[parts.Length - 1] = patch.ToString();
        }
        else
        {
            // Caso o último campo não seja um número puro, redefine o patch para 1
            parts[parts.Length - 1] = "1";
        }

        // Junta tudo de volta com os pontos
        return string.Join(".", parts);
    }
}
