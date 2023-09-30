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
        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CRIAR_LAYER = "[ERROR]: Não foi possível inserir a layer: {nome-layer}.";

        #endregion

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
                ConstantesProjetoUnity.CaminhoUnityAssetsStreamingAssets,
            };

            foreach(string pasta in CAMINHO_PASTAS) {
                CriarPasta(pasta);
            }

            CopiarPasta(Path.Combine(ConstantesEditor.CaminhoPastaAssetsRuntime, "Animacoes"), ConstantesProjetoUnity.CaminhoUnityAssets);

            return;
        }

        private static void CriarPasta(string caminho) {
            if(Directory.Exists(caminho)) {
                return;
            }

            Directory.CreateDirectory(caminho);
            return;
        }

        private static void CopiarPasta(string origem, string destino) {
            string nomePastaOrigem = origem.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Split(Path.AltDirectorySeparatorChar).Last();
            string caminhoCopia = Path.Combine(destino, nomePastaOrigem).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if(!Directory.Exists(caminhoCopia)) {
                Directory.CreateDirectory(caminhoCopia);
            }

            string[] subDiretorios = Directory.GetDirectories(origem);
            foreach(string subDiretorio in subDiretorios) {
                CopiarPasta(subDiretorio, caminhoCopia);
            }

            string[] arquivos = Directory.GetFiles(origem);
            foreach(string arquivo in arquivos) {
                string caminhoArquivoFormatado = arquivo.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                if(Path.GetExtension(caminhoArquivoFormatado) == ExtensoesEditor.Meta) {
                    continue;
                }

                string nomeArquivo = Path.GetFileName(caminhoArquivoFormatado);
                FileUtil.CopyFileOrDirectory(caminhoArquivoFormatado, Path.Combine(caminhoCopia, nomeArquivo));
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
                NomesTags.Gabarito,
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
                Debug.LogError(MENSAGEM_ERRO_CRIAR_LAYER.Replace("{nome-layer}", layer.Nome));
                return;
            }

            layerAlvo.stringValue = layer.Nome;
            return;
        }
    }
}