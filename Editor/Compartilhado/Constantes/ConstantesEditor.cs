using System.IO;

namespace Autis.Editor.Constantes {
    public static class ConstantesEditor {
        public static string CaminhoPastaEditor => Path.Combine(ConstantesProjeto.CaminhoAssetDatabaseProjeto, "Editor/");
        public static string CaminhoPastaResourcesEditor => Path.Combine(CaminhoPastaEditor, "Resources/");
        public static string CaminhoPastaImagensEditor => Path.Combine(CaminhoPastaResourcesEditor, "Imagens/");
        public static string CaminhoPastaEventosEditor => Path.Combine(CaminhoPastaEditor, "Eventos/");

        public static string CaminhoPastaRuntime => Path.Combine(ConstantesProjeto.CaminhoAssetDatabaseProjeto, "Runtime/");
        public static string CaminhoPastaResourcesRuntime => Path.Combine(CaminhoPastaRuntime, "Resources/");

        public static string CaminhoArquivoClassesPadroesUSS => "Compartilhado/ClassesPadroesEditorStyle.uss";

        public static string CaminhoPastaBuild => "Build/WebGL/";
        public static string NomePastaLayouts => "Layouts/";
        public static string NomeCenaPadrao => "CenaBasePadrao.unity";
    }
}