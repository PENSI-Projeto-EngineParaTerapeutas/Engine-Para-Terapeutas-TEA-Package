using UnityEditor;
using UnityEditor.SceneManagement;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.Build {
    public static class CompilacaoAutomatica {
        private static string NomeCompania { get => (ConstantesProjeto.NomeOrganizacao + " - " + ConstantesProjeto.NomeProjeto); }

        [MenuItem("Engine Para Terapeutas/Criar jogo")]
        public static void Compilar() {
            EngineParaTerapeutas.Utils.Utils.SalvarProjeto();

            PlayerSettings.companyName = NomeCompania;
            PlayerSettings.productName = "Teste"; // TODO: Pegar dinamicamente

            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.WebGL, PlayerSettings.productName);

            BuildPlayerOptions playerOptions = new() {
                scenes = new string[] { ConstantesProjeto.PastaCenasAssets + "/Fase1.unity" }, // TODO: Pegar dinamicamente
                locationPathName = ConstantesProjeto.PastaBuild,
                target = BuildTarget.WebGL,
                options = BuildOptions.None,
            };

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);

            BuildPipeline.BuildPlayer(playerOptions);

            return;
        }
    }
}