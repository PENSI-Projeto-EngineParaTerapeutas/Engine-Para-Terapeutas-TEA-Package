using System.IO;

namespace EngineParaTerapeutas.Constantes {
    public static class ConstantesRuntime {
        public static string PastaRaiz { get => Path.Combine(ConstantesProjeto.PastaRaizProjeto, "Runtime/"); }
        public static string CaminhoCompletoPastaResources { get => Path.Combine(PastaRaiz, "Resources/"); }

        public static string CaminhoPastaAnimacoes { get => Path.Combine(CaminhoCompletoPastaResources, "Assets/Animacoes/"); }

        public static string NomePastaCenas { get => "Cenas/"; }

        public static string ExtensoesImagem { get => ".png, .jpg, .jpeg"; }
        public static string ExtensoesAudio { get => ".mp3, .wav"; }
    }
}