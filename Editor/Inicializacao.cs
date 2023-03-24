using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.UI;
using Codice.CM.Client.Differences;

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
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoes,
            };

            foreach(string pasta in CAMINHO_PASTAS) {
                if(Directory.Exists(pasta)) {
                    continue;
                }

                Directory.CreateDirectory(pasta);
            }

            /*string caminhoCompletoProjetoUnity = Directory.GetParent(Application.dataPath).FullName;
            string caminhoCompletoPacoteEngine = Directory.GetParent(GetCaminhoDiretorioAtual).FullName;
            string caminhoRelativoProjeto = Path.GetRelativePath(caminhoCompletoProjetoUnity, caminhoCompletoPacoteEngine);

            Debug.Log("Copiando de: " + Path.Combine(caminhoRelativoProjeto, "Runtime/Resources/Assets/Animacoes"));
            Debug.Log("Destino da cópia: " + ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoes);*/
            //FileUtil.CopyFileOrDirectory(Path.Combine(caminhoRelativoProjeto, "Runtime/Resources/Assets/Animacoes"), ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoes);

            CopiarConteudoPastaAnimacoes();

            return;
        }

        private static void CopiarConteudoPastaAnimacoes() {
            string[] SUBPASTAS_ANIMACAO = {
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar,
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito,
                ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico,
            };

            foreach(string pasta in SUBPASTAS_ANIMACAO) {
                Debug.Log(pasta);
                if(Directory.Exists(pasta)) {
                    continue;
                }

                Directory.CreateDirectory(pasta);
            }
            // To copy all the files in one directory to another directory.
            // Get the files in the source folder. (To recursively iterate through
            // all subfolders under the current directory, see
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously
            //       in this code example.
            /*string[] pastasAnimacoesPorTipoPersonagem = Directory.GetDirectories(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoes);

            foreach(string pastaAnimacao in pastasAnimacoesPorTipoPersonagem) {
                string[] animacoes = Directory.GetFiles(pastaAnimacao);

                foreach(string arquivoAnimacao in animacoes) {
                    string nomeArquivo = Path.GetFileName(arquivoAnimacao);

                    AssetDatabase.CopyAsset(arquivoAnimacao, );
                }
            }
            if (System.IO.Directory.Exists(sourcePath)) {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files) {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }*/

            string caminhoCompletoProjetoUnity = Directory.GetParent(Application.dataPath).FullName;
            string caminhoCompletoPacoteEngine = Directory.GetParent(GetCaminhoDiretorioAtual).FullName;
            string caminhoRelativoProjeto = Path.GetRelativePath(caminhoCompletoProjetoUnity, caminhoCompletoPacoteEngine);

            string[] animacoes = Directory.GetFiles(Path.Combine(caminhoRelativoProjeto, "Runtime/Resources/Assets/Animacoes/BonecoPalito"));
            foreach(string animacao in animacoes) {
                Debug.Log(animacao);
                AssetDatabase.CopyAsset(animacao, ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico);
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