using UnityEditor;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.Build {
    public static class CompilacaoAutomatica {
        private static string NomeCompania { get => (ConstantesProjeto.NomeOrganizacao + " - " + ConstantesProjeto.NomeProjeto); }

        [MenuItem("Engine Para Terapeutas/Criar jogo")]
        public static void Compilar() {
            Salvamento.SalvarProjeto();

            PlayerSettings.companyName = NomeCompania;
            PlayerSettings.productName = "Teste"; // TODO: Pegar dinamicamente

            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;

            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.WebGL, PlayerSettings.productName);

            BuildPlayerOptions playerOptions = new() {
                scenes = new string[] { ConstantesProjetoUnity.CaminhoUnityAssetsCenas + "/Fase1.unity" }, // TODO: Pegar dinamicamente
                locationPathName = ConstantesEditor.CaminhoPastaBuild,
                target = BuildTarget.WebGL,
                options = BuildOptions.None,
            };

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);

            BuildPipeline.BuildPlayer(playerOptions);

            return;
        }
    }
}