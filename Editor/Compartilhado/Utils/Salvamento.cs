using UnityEditor;
using UnityEditor.SceneManagement;

namespace Autis.Editor.Utils {
    public static class Salvamento {
        public static void SalvarProjeto() {
            SalvarAssets();
            SalvarCenas();

            EditorApplication.ExecuteMenuItem("File/Save Project");

            return;
        }

        public static void SalvarAssets() {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return;
        }

        public static void SalvarCenas() {
            EditorSceneManager.MarkAllScenesDirty();
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();

            return;
        }
    }
}