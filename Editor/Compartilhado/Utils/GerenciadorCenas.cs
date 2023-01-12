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

                if(!AssetDatabase.IsValidFolder(ConstantesProjeto.PastaCenasAssets)) {
                    AssetDatabase.CreateFolder("Assets", "Cenas");
                }
                string[] arquivos = Directory.GetFiles(ConstantesProjeto.PastaCenasAssets);

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
            string caminhoCenaPadrao = Path.Combine(ConstantesProjeto.PastaRaizEditor, ConstantesProjeto.PastaCenas, ConstantesProjeto.NomeCenaPadrao);
            string caminhoNovaCena = Path.Combine(ConstantesProjeto.PastaCenasAssets, "Fase" + numeroFase + Extensoes.Cena);

            AssetDatabase.CopyAsset(caminhoCenaPadrao, caminhoNovaCena);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorSceneManager.OpenScene(caminhoNovaCena);
            Scene arquivoCena = EditorSceneManager.GetSceneByPath(caminhoNovaCena);

            Cena novaCena = ScriptableObject.CreateInstance<Cena>();
            novaCena.Arquivo = arquivoCena;
            novaCena.Nome = arquivoCena.name;

            AssetDatabase.CreateAsset(novaCena, Path.Combine(ConstantesProjeto.PastaCenasAssets, novaCena.Nome + Extensoes.ScriptableObject));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return novaCena;
        }

        public static void DeletarCena(Cena cena) {
            string caminhoScriptableObjectCenaAlvo = Path.Combine(ConstantesProjeto.PastaCenasAssets, cena.Nome + Extensoes.ScriptableObject);
            string caminhoArquivoCenaAlvo = Path.Combine(ConstantesProjeto.PastaCenasAssets, cena.Nome + Extensoes.Cena);

            AssetDatabase.DeleteAsset(caminhoScriptableObjectCenaAlvo);
            AssetDatabase.DeleteAsset(caminhoArquivoCenaAlvo);
            AssetDatabase.Refresh();

            return;
        }

        public static List<Cena> GetTodasCenasCriadas() {
            if(!AssetDatabase.IsValidFolder(ConstantesProjeto.PastaCenasAssets)) {
                AssetDatabase.CreateFolder("Assets", "Cenas");
                return new List<Cena>();
            }

            string[] arquivos = Directory.GetFiles(ConstantesProjeto.PastaCenasAssets);
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