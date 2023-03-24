using System.IO;
using UnityEditor;
using UnityEngine;

namespace EngineParaTerapeutas.Constantes {
    public static class ConstantesEditor {
        public static string CaminhoDinamicoPacote { // TODO: Arrumar localização
            get {
                if(!string.IsNullOrWhiteSpace(caminhoDinamicoPacote)) {
                    return caminhoDinamicoPacote;
                }

                string[] assets = AssetDatabase.FindAssets($"t:Script {nameof(Inicializacao)}");
                string caminhoArquivoInicializacao = AssetDatabase.GUIDToAssetPath(assets[0]);

                string caminhoCompletoProjetoUnity = Directory.GetParent(Application.dataPath).FullName;
                string caminhoCompletoPacoteEngineTEA = Directory.GetParent(Path.GetDirectoryName(Path.GetFullPath(caminhoArquivoInicializacao))).FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                caminhoDinamicoPacote = Path.GetRelativePath(caminhoCompletoProjetoUnity, caminhoCompletoPacoteEngineTEA);

                return caminhoDinamicoPacote;
            }
        }
        private static string caminhoDinamicoPacote = "";

        public static string PastaRaiz { get => Path.Combine(ConstantesProjeto.PastaRaizProjeto, "Editor/"); }
        public static string CaminhoCompletoPastaResources { get => Path.Combine(PastaRaiz, "Resources/"); }
        public static string CaminhoCompletoPastaImagens { get => Path.Combine(CaminhoCompletoPastaResources, "Imagens/"); }
        public static string CaminhoPastaBuild { get => "Build/WebGL/"; }

        public static string CaminhoArquivoClassesPadroesUSS { get => "Compartilhado/ClassesPadroesEditorStyle.uss"; }

        public static string NomePastaLayouts { get => "Layouts/";  }
        public static string NomeCenaPadrao { get => "CenaBasePadrao.unity"; }

        public static string ExtensoesImagem { get => "png,jpg,jpeg"; }
        public static string ExtensoesAudio { get => "mp3,wav"; }
        public static string ExtensoesVideo { get => "mp4"; }
    }
}