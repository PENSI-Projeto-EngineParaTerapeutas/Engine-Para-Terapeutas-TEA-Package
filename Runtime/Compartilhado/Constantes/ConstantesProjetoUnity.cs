using System.IO;

namespace Autis.Runtime.Constantes {
    public static class ConstantesProjetoUnity {
        public static string CaminhoUnityAssets => "Assets/";
        public static string CaminhoUnityAssetsCenas => Path.Combine(CaminhoUnityAssets, "Cenas/");
        public static string CaminhoUnityAssetsStreamingAssets => Path.Combine(CaminhoUnityAssets, "StreamingAssets/");

        public static string CaminhoUnityAssetsAnimacoes => Path.Combine(CaminhoUnityAssets, "Animacoes/");
        public static string CaminhoUnityAssetsAnimacoesBonecoPalito => Path.Combine(CaminhoUnityAssetsAnimacoes, "BonecoPalito/");
        public static string CaminhoUnityAssetsAnimacoesPersonagemLudico => Path.Combine(CaminhoUnityAssetsAnimacoes, "PersonagemLudico/");
        public static string CaminhoUnityAssetsAnimacoesAvatar => Path.Combine(CaminhoUnityAssetsAnimacoes, "Avatar/");

        public static string CaminhoUnityAssetsResources => Path.Combine(CaminhoUnityAssets, "Resources/");
        public static string CaminhoUnityAssetsImagens => Path.Combine(CaminhoUnityAssetsResources, "Imagens/");
        public static string CaminhoUnityAssetsSons => Path.Combine(CaminhoUnityAssetsResources, "Sons/");
        public static string CaminhoUnityAssetsScriptableObjectsCenas => Path.Combine(CaminhoUnityAssetsResources, "Cenas/");
    }
}