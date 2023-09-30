using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Autis.Editor.Constantes;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;
using Autis.Runtime.ScriptableObjects;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorCena {
        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CARREGAR_CENA = "[ERROR]: Não foi possível carregar o ScriptableObject da Cena atual.";

        #endregion

        public Cena CenaVinculada { get => cenaVinculada; }
        private Cena cenaVinculada;

        public ManipuladorCena() {
            string nomeCenaAtual = SceneManager.GetActiveScene().name;
            CarregarCena(nomeCenaAtual);

            return;
        }

        public ManipuladorCena(string nomeCena) {
            CarregarCena(nomeCena);
            return;
        }

        public void CarregarCena(string nomeCena) {
            cenaVinculada = AssetDatabase.LoadAssetAtPath<Cena>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsCenas, nomeCena + ExtensoesEditor.ScriptableObject));
            if(cenaVinculada == null) {
                Debug.LogError(MENSAGEM_ERRO_CARREGAR_CENA);
            }

            return;
        }

        public void VincularCena(Scene scene) {
            cenaVinculada.nomeExibicao = scene.name;
            cenaVinculada.caminho = scene.path;
            cenaVinculada.buildIndex = scene.buildIndex;

            return;
        }

        public void SetNome(string nome) {
            cenaVinculada.nomeExibicao = nome;
            return;
        }

        public string GetNome() {
            return cenaVinculada.nomeExibicao;
        }

        public void SetDificuldade(NiveisDificuldade dificuldade) {
            cenaVinculada.nivelDificuldade = dificuldade;
        }

        public NiveisDificuldade GetDificuldade() {
            return cenaVinculada.nivelDificuldade;
        }

        public void SetFaixaEtaria(int faixaEtaria) {
            if(faixaEtaria < 0) {
                cenaVinculada.faixaEtaria = 0;
                return;
            }

            cenaVinculada.faixaEtaria = faixaEtaria;
            return;
        }

        public int GetFaixaEtaria() {
            return cenaVinculada.faixaEtaria;
        }

        public string GetCaminho() {
            return cenaVinculada.caminho;
        }

        public string GetNomeArquivo() {
            return cenaVinculada.nomeArquivo;
        }

        public int GetBuildIndex() {
            return cenaVinculada.buildIndex;
        }

        public void SetTipoGabarito(TipoGabarito tipo) {
            cenaVinculada.tipoGabarito = tipo;
            return;
        }

        public TipoGabarito GetTipoGabarito() {
            return cenaVinculada.tipoGabarito;
        }
    }
}