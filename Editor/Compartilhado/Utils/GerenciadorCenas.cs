using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ScriptableObjects;
using System.Collections.Generic;

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
            int numeroFase = QuantidadeCenas + 1;
            string caminhoCenaPadrao = Path.Combine(ConstantesEditor.PastaRaiz, ConstantesRuntime.NomePastaCenas, ConstantesEditor.NomeCenaPadrao);
            string caminhoNovaCena = Path.Combine(ConstantesRuntime.CaminhoPastaCenas, "Fase" + numeroFase + Extensoes.Cena);

            AssetDatabase.CopyAsset(caminhoCenaPadrao, caminhoNovaCena);
            Salvamento.SalvarAssets();

            EditorSceneManager.OpenScene(caminhoNovaCena);
            Scene arquivoCena = EditorSceneManager.GetSceneByPath(caminhoNovaCena);

            Cena novaCena = ScriptableObject.CreateInstance<Cena>();
            novaCena.AssociarValores(arquivoCena);

            AssetDatabase.CreateAsset(novaCena, Path.Combine(ConstantesRuntime.CaminhoPastaCenas, novaCena.Nome + Extensoes.ScriptableObject));
            Salvamento.SalvarAssets();

            return novaCena;
        }

        public static void DeletarCena(Cena cena) {
            string caminhoScriptableObjectCenaAlvo = Path.Combine(ConstantesRuntime.CaminhoPastaCenas, cena.Nome + Extensoes.ScriptableObject);
            string caminhoArquivoCenaAlvo = Path.Combine(ConstantesRuntime.CaminhoPastaCenas, cena.Nome + Extensoes.Cena);

            AssetDatabase.DeleteAsset(caminhoScriptableObjectCenaAlvo);
            AssetDatabase.DeleteAsset(caminhoArquivoCenaAlvo);
            AssetDatabase.Refresh();

            return;
        }

        public static List<Cena> GetTodasCenasCriadas() {
            if(!AssetDatabase.IsValidFolder(ConstantesRuntime.CaminhoPastaCenas)) {
                AssetDatabase.CreateFolder("Assets", "Cenas");
                return new List<Cena>();
            }

            string[] arquivos = Directory.GetFiles(ConstantesRuntime.CaminhoPastaCenas);
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