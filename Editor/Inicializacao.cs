using UnityEngine;
using UnityEditor;
using System.IO;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas {
    [InitializeOnLoad]
    public class Inicializacao : AssetPostprocessor {
        private const string NOME_ARQUIVO_CONFIGURACAO_PACOTE = "package.json";

        static Inicializacao() {
            return;
        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            string caminhoArquivoConfiguracaoPacote = Path.Combine(ConstantesProjeto.PastaRaizProjeto, NOME_ARQUIVO_CONFIGURACAO_PACOTE);

            foreach (string caminho in importedAssets) {
                if(caminhoArquivoConfiguracaoPacote == caminho) {
                    ConfigurarPacote();
                }
            }

            return;
        }

        private static void ConfigurarPacote() {
            Debug.Log("[LOG] Iniciar configuração");
            CopiarCenaPadrao();
            ConfigurarTags();
            ConfigurarLayers();
            return;
        }

        private static void CopiarCenaPadrao() {
            Debug.Log("[TODO] Copiar cena padrão para pasta de cenas");
            return;
        }

        private static void ConfigurarTags() {
            Debug.Log("[TODO] Criar layers de Editor, apoio, reforço, etc...");
            return;
        }

        private static void ConfigurarLayers() {
            Debug.Log("[TODO] Criar layers de Editor, etc...");
            return;
        }
    }
}