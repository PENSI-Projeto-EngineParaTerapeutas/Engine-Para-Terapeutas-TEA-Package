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
        static Inicializacao() {}

        [MenuItem("Engine Para Terapeutas/Teste")]
        public static void Teste() {
            string caminhoCompletoProjetoUnity = Directory.GetParent(Application.dataPath).FullName;
            string caminhoPacoteEngine = Directory.GetParent(GetCaminhoDiretorioAtual).FullName;

            Debug.Log(Path.GetRelativePath(caminhoCompletoProjetoUnity, caminhoPacoteEngine));
            return;
        }

        private static string GetCaminhoDiretorioAtual {
            get {
                string[] assets = AssetDatabase.FindAssets($"t:Script {nameof(Inicializacao)}");
                string caminhoLayoutManager = AssetDatabase.GUIDToAssetPath(assets[0]);

                return Path.GetDirectoryName(Path.GetFullPath(caminhoLayoutManager));
            }
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            const string NOME_ARQUIVO_CONFIGURACAO_PACOTE = "package.json";
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
            string[] CAMINHO_PASTAS = {
                ConstantesProjetoUnity.CaminhoUnityAssetsCenas,
                ConstantesProjetoUnity.CaminhoUnityAssetsImagens,
                ConstantesProjetoUnity.CaminhoUnityAssetsSons,
                ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas,
                ConstantesProjetoUnity.CaminhoUnityAssetsStreamingAssets,
            };

            foreach(string pasta in CAMINHO_PASTAS) {
                if(Directory.Exists(pasta)) {
                    continue;
                }

                Directory.CreateDirectory(pasta);
            }

            string caminhoCompletoProjetoUnity = Directory.GetParent(Application.dataPath).FullName;
            string caminhoCompletoPacoteEngine = Directory.GetParent(GetCaminhoDiretorioAtual).FullName;
            string caminhoRelativoProjeto = Path.GetRelativePath(caminhoCompletoProjetoUnity, caminhoCompletoPacoteEngine);

            Debug.Log("Copiando de: " + ConstantesRuntime.CaminhoPastaAnimacoes);
            Debug.Log("Destino da cópia: " + ConstantesProjetoUnity.CaminhoUnityAssets);
            FileUtil.CopyFileOrDirectory(ConstantesRuntime.CaminhoPastaAnimacoes, ConstantesProjetoUnity.CaminhoUnityAssets);
            
            return;
        }

        private static void ConfigurarProjectSettings() {
            string[] TAGS_ENGINE = {
                NomesTags.Apoios,
                NomesTags.Reforcos,
                NomesTags.Cenario,
                NomesTags.Contexto,
                NomesTags.Instrucoes,
                NomesTags.ObjetosInteracao,
                NomesTags.Personagem,
            };

            SerializedObject tagManager = new(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset").First());
            SerializedProperty tagsProperty = tagManager.FindProperty("tags");

            foreach(string tag in TAGS_ENGINE) {
                AdicionarTag(tagsProperty, tag);
            }

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