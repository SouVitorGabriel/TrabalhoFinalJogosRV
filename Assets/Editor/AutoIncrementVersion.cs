using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AutoIncrementVersion : IPostprocessBuildWithReport
{
    // Define a ordem de execução do script (0 é o padrão)
    public int callbackOrder => 0;

    public void OnPostprocessBuild(BuildReport report)
    {
        // Verifica se a build foi feita para Android e se terminou com sucesso
        if (report.summary.platform == BuildTarget.Android && report.summary.result == BuildResult.Succeeded)
        {
            // Obtém o código de versão atual do Android
            int currentVersionCode = PlayerSettings.Android.bundleVersionCode;
            
            // Incrementa o número em +1
            int newVersionCode = currentVersionCode + 1;
            
            // Aplica o novo número nas configurações do projeto
            PlayerSettings.Android.bundleVersionCode = newVersionCode;

            Debug.Log($"[AutoIncrement] Build para Android bem-sucedida! Bundle Version Code atualizado de {currentVersionCode} para {newVersionCode}.");
        }
    }
}
