using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Autis.Runtime.Constantes;
using Autis.Runtime.ScriptableObjects;
using Autis.Editor.Constantes;

namespace Autis.Editor.Utils {
    public static class GerenciadorCenas {
        public static int QuantidadeCenas {
            get {
                int quantidadeCenas = 0;

                if(!Directory.Exists(ConstantesProjetoUnity.CaminhoUnityAssetsCenas)) {
                    Directory.CreateDirectory(ConstantesProjetoUnity.CaminhoUnityAssetsCenas);
                }

                string[] arquivos = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsCenas);
                foreach(string arquivo in arquivos) {
                    if(Path.GetExtension(arquivo) == ExtensoesEditor.Cena) {
                        quantidadeCenas++;
                    }
                }

                return quantidadeCenas;
            }
        }

        public static Cena CriarCena() {
            string dataHoraCraicao = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss-fff", CultureInfo.InvariantCulture);
            string nomeNovaCena = "Fase_" + dataHoraCraicao;

            string[] partesCaminhoPastaCenas = ConstantesProjetoUnity.CaminhoUnityAssetsCenas.Split(Path.AltDirectorySeparatorChar);
            string nomePastaCenas = partesCaminhoPastaCenas[^2];

            string caminhoCenaPadrao = Path.Combine(ConstantesEditor.CaminhoPastaEditor, nomePastaCenas, ConstantesEditor.NomeCenaPadrao);
            string caminhoNovaCena = Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsCenas, nomeNovaCena + ExtensoesEditor.Cena);

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

            AssetDatabase.CreateAsset(novaCena, Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas, nomeNovaCena + ExtensoesEditor.ScriptableObject));
            Salvamento.SalvarAssets();

            return novaCena;
        }

        public static void DeletarCena(Cena cena) {
            string caminhoScriptableObjectCenaAlvo = Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas, cena.NomeArquivo + ExtensoesEditor.ScriptableObject);
            string caminhoArquivoCenaAlvo = Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsCenas, cena.NomeArquivo + ExtensoesEditor.Cena);

            AssetDatabase.DeleteAsset(caminhoScriptableObjectCenaAlvo);
            AssetDatabase.DeleteAsset(caminhoArquivoCenaAlvo);
            AssetDatabase.Refresh();

            return;
        }

        public static List<Cena> GetTodasCenasCriadas() {
            if(!Directory.Exists(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas)) {
                Directory.CreateDirectory(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas);
                return new List<Cena>();
            }

            string[] arquivos = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsScriptableObjectsCenas);
            List<Cena> cenas = new();

            foreach(string arquivo in arquivos) {
                if(Path.GetExtension(arquivo) == ExtensoesEditor.ScriptableObject) {
                    Cena cena = AssetDatabase.LoadAssetAtPath<Cena>(arquivo);
                    cenas.Add(cena);
                }
            }

            return cenas;
        }
    }
}