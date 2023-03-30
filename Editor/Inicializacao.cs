using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.UI;

namespace Autis {
    [InitializeOnLoad]
    public class Inicializacao : AssetPostprocessor {
        static Inicializacao() {}

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            const string NOME_ARQUIVO_CONFIGURACAO_PACOTE = "package.json";
            string caminhoArquivoConfiguracaoPacote = Path.Combine(ConstantesProjeto.CaminhoAssetDatabaseProjeto, NOME_ARQUIVO_CONFIGURACAO_PACOTE);

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
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoes,
            };

            foreach(string pasta in CAMINHO_PASTAS) {
                CriarPasta(pasta);
            }

            CopiarConteudoPastaAnimacoes();

            return;
        }

        private static void CriarPasta(string caminho) {
            if(Directory.Exists(caminho)) {
                return;
            }

            Directory.CreateDirectory(caminho);
            return;
        }

        private static void CopiarConteudoPastaAnimacoes() {
            string[] SUBPASTAS_ANIMACAO = {
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar,
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito,
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico,
            };

            foreach(string pasta in SUBPASTAS_ANIMACAO) {
                CriarPasta(pasta);
            }

            string[] pastasTiposAnimacaoPorPersonagem = Directory.GetDirectories(Path.Combine(ConstantesProjeto.CaminhoDinamicoPacote, "Runtime/Resources/Assets/Animacoes"));
            foreach(string pasta in pastasTiposAnimacaoPorPersonagem) {
                string caminnhoFormatado = pasta.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string nomePasta = caminnhoFormatado.Split(Path.AltDirectorySeparatorChar).Last();

                string[] arquivos = Directory.GetFiles(caminnhoFormatado);
                foreach(string arquivo in arquivos) {
                    string caminhoArquivoFormatado = arquivo.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    if(Path.GetExtension(caminhoArquivoFormatado) == ExtensoesEditor.Meta) {
                        continue;
                    }

                    string nomeArquivo = Path.GetFileName(caminhoArquivoFormatado);
                    FileUtil.CopyFileOrDirectory(caminhoArquivoFormatado, Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoes, nomePasta, nomeArquivo));
                }
            }

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