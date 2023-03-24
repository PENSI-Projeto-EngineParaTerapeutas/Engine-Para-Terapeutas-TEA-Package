using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ScriptableObjects;

namespace EngineParaTerapeutas.Utils {
    public static class GerenciadorCenas {
        public static int QuantidadeCenas {
            get {
                int quantidadeCenas = 0;

                if(!Directory.Exists(ConstantesProjetoUnity.CaminhoUnityAssetsCenas)) {
                    Directory.CreateDirectory(ConstantesProjetoUnity.CaminhoUnityAssetsCenas);
                }

                string[] arquivos = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsCenas);
                foreach(string arquivo in arquivos) {
                    if(Path.GetExtension(arquivo) == Extensoes.Cena) {
                        quantidadeCenas++;
                    }
                }

                return quantidadeCenas;
            }
        }

        public static Cena CriarCena() {
            string dataHoraCraicao = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss-fff", CultureInfo.InvariantCulture);
            string nomeNovaCena = "Fase_" + dataHoraCraicao;

            string caminhoCenaPadrao = Path.Combine(ConstantesEditor.PastaRaiz, ConstantesRuntime.NomePastaCenas, ConstantesEditor.NomeCenaPadrao);
            string caminhoNovaCena = Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsCenas, nomeNovaCena + Extensoes.Cena);

            AssetDatabase.CopyAsset(caminhoCenaPadrao, caminhoNovaCena);
            Salvamento.SalvarAssets();

            EditorSceneManager.OpenScene(caminhoNovaCena);
            Scene arquivoCena = EditorSceneManager.GetSceneByPath(caminhoNovaCena);

            Cena novaCena = ScriptableObject.CreateInstance<Cena>();
            novaCena.AssociarValores(arquivoCena);
            novaCena.NomeArquivo = nomeNovaCena;

            if(!Directory.Exists(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas)) {
                Directory.CreateDirectory(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas);
            }

            AssetDatabase.CreateAsset(novaCena, Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas, nomeNovaCena + Extensoes.ScriptableObject));
            Salvamento.SalvarAssets();

            return novaCena;
        }

        public static void DeletarCena(Cena cena) {
            string caminhoScriptableObjectCenaAlvo = Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas, cena.NomeArquivo + Extensoes.ScriptableObject);
            string caminhoArquivoCenaAlvo = Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsCenas, cena.NomeArquivo + Extensoes.Cena);

            AssetDatabase.DeleteAsset(caminhoScriptableObjectCenaAlvo);
            AssetDatabase.DeleteAsset(caminhoArquivoCenaAlvo);
            AssetDatabase.Refresh();

            return;
        }

        public static List<Cena> GetTodasCenasCriadas() {
            if(!AssetDatabase.IsValidFolder(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas)) { // TODO: Alterar para Directory
                AssetDatabase.CreateFolder("Assets", "Cenas");
                return new List<Cena>();
            }

            string[] arquivos = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas);
            List<Cena> cenas = new();

            foreach(string arquivo in arquivos) {
                if(Path.GetExtension(arquivo) == Extensoes.ScriptableObject) {
                    Cena cena = AssetDatabase.LoadAssetAtPath<Cena>(arquivo);
                    cenas.Add(cena);
                }
            }

            return cenas;
        }
    }
}