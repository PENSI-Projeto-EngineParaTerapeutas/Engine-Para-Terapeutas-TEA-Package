using System.IO;

namespace Autis.Editor.Constantes {
    public static class ConstantesEditor {
        public static string CaminhoPastaEditor => Path.Combine(ConstantesProjeto.CaminhoAssetDatabaseProjeto, "Editor/");
        public static string CaminhoPastaScriptsEditor => Path.Combine(CaminhoPastaEditor, "Scripts/");
        public static string CaminhoPastaAssetsEditor => Path.Combine(CaminhoPastaEditor, "Assets/");
        public static string CaminhoPastaImagensEditor => Path.Combine(CaminhoPastaAssetsEditor, "Imagens/");
        public static string CaminhoPastaCenasEditor => Path.Combine(CaminhoPastaAssetsEditor, "Cenas/");
        public static string CaminhoPastaEventosEditor => Path.Combine(CaminhoPastaEditor, "Eventos/");
        public static string CaminhoPastaFontesEditor => Path.Combine(CaminhoPastaAssetsEditor, "Fontes/");

        public static string CaminhoPastaRuntime => Path.Combine(ConstantesProjeto.CaminhoAssetDatabaseProjeto, "Runtime/");
        public static string CaminhoPastaAssetsRuntime => Path.Combine(CaminhoPastaRuntime, "Assets/");

        public static string CaminhoArquivoClassesPadroesUSS => "Compartilhado/ClassesPadroesEditorStyle.uss";

        public static string CaminhoPastaBuild => "Build/WebGL/";
        public static string NomeCenaPadrao => "CenaBasePadrao.unity";
    }
}