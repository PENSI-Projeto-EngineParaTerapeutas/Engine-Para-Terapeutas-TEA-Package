using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas {
    [InitializeOnLoad]
    public class Inicializacao : AssetPostprocessor {
        private const string NOME_ARQUIVO_CONFIGURACAO_PACOTE = "package.json";

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            string caminhoArquivoConfiguracaoPacote = Path.Combine(ConstantesProjeto.PastaRaizProjeto, NOME_ARQUIVO_CONFIGURACAO_PACOTE);

            foreach(string caminho in importedAssets) {
                if(caminhoArquivoConfiguracaoPacote == caminho) {
                    ConfigurarPacote();
                }
            }

            return;
        }

        private static void ConfigurarPacote() {
            CriarPastasProjeto();
            ConfigurarProjectSettings();

            LayoutLoader.CarregarTelaInicial();
            return;
        }

        private static void CriarPastasProjeto() {
            if(!Directory.Exists(ConstantesRuntime.CaminhoPastaCenas)) {
                Directory.CreateDirectory(ConstantesRuntime.CaminhoPastaCenas);
            }

            if(!Directory.Exists(ConstantesRuntime.CaminhoPastaImagens)) {
                Directory.CreateDirectory(ConstantesRuntime.CaminhoPastaImagens);
            }

            if(!Directory.Exists(ConstantesRuntime.CaminhoPastaSons)) {
                Directory.CreateDirectory(ConstantesRuntime.CaminhoPastaSons);
            }

            if(!Directory.Exists(ConstantesRuntime.CaminhoPastaVideos)) {
                Directory.CreateDirectory(ConstantesRuntime.CaminhoPastaVideos);
            }

            return;
        }

        private static void ConfigurarProjectSettings() {
            SerializedObject tagManager = new(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset").First());
            SerializedProperty tagsProperty = tagManager.FindProperty("tags");

            AdicionarTag(tagsProperty, NomesTags.Apoios);
            AdicionarTag(tagsProperty, NomesTags.Reforcos);
            AdicionarTag(tagsProperty, NomesTags.Cenario);

            SerializedProperty layersProperty = tagManager.FindProperty("layers");

            AdicionarLayer(layersProperty, LayersProjeto.EditorOnly);

            tagManager.ApplyModifiedProperties();
            return;
        }

        private static void AdicionarTag(SerializedProperty tagsProperty, string novaTag) {
            for(int i = 0; i < tagsProperty.arraySize; i++) {
                SerializedProperty tag = tagsProperty.GetArrayElementAtIndex(i);

                if(tag.stringValue == novaTag) {
                    return;
                }
            }

            tagsProperty.InsertArrayElementAtIndex(0);
            SerializedProperty novaTagProperty = tagsProperty.GetArrayElementAtIndex(0);
            novaTagProperty.stringValue = novaTag;

            return;
        }

        private static void AdicionarLayer(SerializedProperty layersProperty, LayerInfo layer) {
            SerializedProperty layerAlvo = layersProperty.GetArrayElementAtIndex(layer.Index);
            if(layerAlvo == null) {
                Debug.LogError("[ERRO]: Não foi possível inserir a layer: " + layer.Nome);
                return;
            }

            layerAlvo.stringValue = layer.Nome;
            return;
        }
    }
}