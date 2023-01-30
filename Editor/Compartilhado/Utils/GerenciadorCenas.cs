using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ScriptableObjects;
using System.Collections.Generic;
using System.Globalization;
using System;

namespace EngineParaTerapeutas.Utils {
    public static class GerenciadorCenas {
        public static int QuantidadeCenas {
            get {
                int quantidadeCenas = 0;

                if(!Directory.Exists(ConstantesRuntime.CaminhoPastaCenas)) {
                    Directory.CreateDirectory(ConstantesRuntime.CaminhoPastaCenas);
                }

                string[] arquivos = Directory.GetFiles(ConstantesRuntime.CaminhoPastaCenas);
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
            string caminhoNovaCena = Path.Combine(ConstantesRuntime.CaminhoPastaCenas, nomeNovaCena + Extensoes.Cena);

            AssetDatabase.CopyAsset(caminhoCenaPadrao, caminhoNovaCena);
            Salvamento.SalvarAssets();

            EditorSceneManager.OpenScene(caminhoNovaCena);
            Scene arquivoCena = EditorSceneManager.GetSceneByPath(caminhoNovaCena);

            Cena novaCena = ScriptableObject.CreateInstance<Cena>();
            novaCena.AssociarValores(arquivoCena);
            novaCena.NomeArquivo = nomeNovaCena;

            if(!Directory.Exists(ConstantesRuntime.CaminhoPastaScriptableObjectsCenas)) {
                Directory.CreateDirectory(ConstantesRuntime.CaminhoPastaScriptableObjectsCenas);
            }

            AssetDatabase.CreateAsset(novaCena, Path.Combine(ConstantesRuntime.CaminhoPastaScriptableObjectsCenas, nomeNovaCena + Extensoes.ScriptableObject));
            Salvamento.SalvarAssets();

            return novaCena;
        }

        public static void DeletarCena(Cena cena) {
            string caminhoScriptableObjectCenaAlvo = Path.Combine(ConstantesRuntime.CaminhoPastaScriptableObjectsCenas, cena.NomeArquivo + Extensoes.ScriptableObject);
            string caminhoArquivoCenaAlvo = Path.Combine(ConstantesRuntime.CaminhoPastaCenas, cena.NomeArquivo + Extensoes.Cena);

            AssetDatabase.DeleteAsset(caminhoScriptableObjectCenaAlvo);
            AssetDatabase.DeleteAsset(caminhoArquivoCenaAlvo);
            AssetDatabase.Refresh();

            return;
        }

        public static List<Cena> GetTodasCenasCriadas() {
            if(!AssetDatabase.IsValidFolder(ConstantesRuntime.CaminhoPastaScriptableObjectsCenas)) {
                AssetDatabase.CreateFolder("Assets", "Cenas");
                return new List<Cena>();
            }

            string[] arquivos = Directory.GetFiles(ConstantesRuntime.CaminhoPastaScriptableObjectsCenas);
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