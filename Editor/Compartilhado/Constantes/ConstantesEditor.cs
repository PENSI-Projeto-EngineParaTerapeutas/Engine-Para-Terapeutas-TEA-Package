using System.IO;

namespace EngineParaTerapeutas.Constantes {
    public static class ConstantesEditor {
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