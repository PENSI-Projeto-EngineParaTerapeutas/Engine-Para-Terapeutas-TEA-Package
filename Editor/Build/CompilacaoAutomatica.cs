using UnityEditor;

namespace EngineParaTerapeutas.Build {
    public class CompilacaoAutomatica {
        private const string CAMINHO_NOME_CENA_PADRAO = "Assets/Cenas/Teste.unity"; // TODO: Pegar dinamicamente

        [MenuItem("Engine Para Terapeutas/Criar jogo")]
        public static void Compilar() {
            PlayerSettings.companyName = "EngineParaTerapeutas"; // TODO: Pegar dinamicamente
            PlayerSettings.productName = "Teste"; // TODO: Pegar dinamicamente

            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.WebGL, PlayerSettings.productName);

            BuildPlayerOptions playerOptions = new() {
                scenes = new string[] { CAMINHO_NOME_CENA_PADRAO },
                locationPathName = "Build/WebGL/",
                target = BuildTarget.WebGL,
                options = BuildOptions.None,
            };

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);

            BuildPipeline.BuildPlayer(playerOptions);

            return;
        }
    }
}