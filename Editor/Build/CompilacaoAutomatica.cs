using UnityEditor;
using Autis.Runtime.Constantes;
using Autis.Editor.Constantes;
using Autis.Editor.Utils;

namespace Autis.Editor.Build {
    public static class CompilacaoAutomatica {
        [MenuItem("Engine Para Terapeutas/Criar jogo")]
        public static void Compilar() {
            Salvamento.SalvarProjeto();

            PlayerSettings.companyName = ConstantesProjeto.NomeOrganizacao + " - " + ConstantesProjeto.NomeProjeto;
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