using System.IO;

namespace EngineParaTerapeutas.Constantes {
    public static class ConstantesRuntime {
        public static string PastaRaiz { get => Path.Combine(ConstantesProjeto.PastaRaizProjeto, "Runtime/"); }
        public static string CaminhoCompletoPastaResources { get => Path.Combine(PastaRaiz, "Resources/"); }
        public static string CaminhoPastaCenas { get => "Assets/Cenas/"; }
        public static string CaminhoPastaAnimacoes { get => "Assets/Animacoes"; }
        public static string CaminhoPastaAnimacoesBonecoPalito { get => "Assets/Animacoes/BonecoPalito"; }
        public static string CaminhoPastaAnimacoesPersonagemLudico { get => "Assets/Animacoes/PersonagemLudico"; }
        public static string CaminhoPastaAnimacoesAvatar { get => "Assets/Animacoes/Avatar"; }
        public static string CaminhoPastaImagens { get => "Assets/Resources/Imagens/"; }
        public static string CaminhoPastaSons { get => "Assets/Resources/Sons/"; }
        public static string CaminhoPastaScriptableObjectsCenas { get => "Assets/Resources/Cenas/"; }
        public static string CaminhoPastaStreamingAssets { get => "Assets/StreamingAssets/"; }

        public static string NomePastaCenas { get => "Cenas/"; }

        public static string ExtensoesImagem { get => ".png, .jpg, .jpeg"; }
        public static string ExtensoesAudio { get => ".mp3, .wav"; }
    }
}