using UnityEditor;
using UnityEditor.SceneManagement;

namespace EngineParaTerapeutas.Utils {
    public static class Utils {
        public static void SalvarCenas() {
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();

            return;
        }

        public static void SalvarProjeto() {
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("File/Save Project");

            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();

            return;
        }
    }
}