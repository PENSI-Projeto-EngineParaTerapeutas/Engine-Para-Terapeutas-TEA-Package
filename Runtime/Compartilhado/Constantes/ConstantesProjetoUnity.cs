using System.IO;

namespace EngineParaTerapeutas.Constantes {
    public static class ConstantesProjetoUnity {
        public static string CaminhoUnityAssets { get => "Assets/"; }
        public static string CaminhoUnityAssetsCenas { get => Path.Combine(CaminhoUnityAssets, "Cenas/"); }
        public static string CaminhoUnityAssetsStreamingAssets { get => Path.Combine(CaminhoUnityAssets, "StreamingAssets/"); }

        public static string CaminhoUnityAssetsAnimacoes { get => Path.Combine(CaminhoUnityAssets, "Animacoes/"); }
        public static string CaminhoUnityAssetsAnimacoesBonecoPalito { get => Path.Combine(CaminhoUnityAssetsAnimacoes, "BonecoPalito/"); }
        public static string CaminhoUnityAssetsAnimacoesPersonagemLudico { get => Path.Combine(CaminhoUnityAssetsAnimacoes, "PersonagemLudico/"); }
        public static string CaminhoUnityAssetsAnimacoesAvatar { get => Path.Combine(CaminhoUnityAssetsAnimacoes, "Avatar/"); }

        public static string CaminhoUnityAssetsResources { get => Path.Combine(CaminhoUnityAssets, "Resources/"); }
        public static string CaminhoUnityAssetsImagens { get => Path.Combine(CaminhoUnityAssetsResources, "Imagens/"); }
        public static string CaminhoUnityAssetsSons { get => Path.Combine(CaminhoUnityAssetsResources, "Sons/"); }
        public static string CaminhoUnityAssetsScriptableObjectsCenas { get => Path.Combine(CaminhoUnityAssetsResources, "Cenas/"); }
    }
}