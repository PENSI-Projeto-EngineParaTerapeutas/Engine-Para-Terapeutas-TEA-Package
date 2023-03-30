using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;

namespace Autis.Runtime.UI {
    public class ConfiguracaoInstrucao : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoInstrucao/ConfiguracaoInstrucaoTemplate";
        protected override string CaminhoStyle => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoInstrucao/ConfiguracaoInstrucaoStyle";

        #region .: Elementos :.

        private const string NOME_REGIAO_SELECAO_INSTRUCOES = "regiao-selecao-instrucoes";
        private readonly VisualElement regiaoSelecaoInstrucoes;

        private const string NOME_REGIAO_TELA_VAZIA = "tela-vazia";
        private readonly VisualElement regiaoTelaVazia;

        #endregion

        private readonly List<GameObject> instrucoes = new();

        public ConfiguracaoInstrucao() {
            instrucoes = GameObject.FindGameObjectsWithTag(NomesTags.Instrucoes).ToList();

            regiaoSelecaoInstrucoes = Root.Query<VisualElement>(NOME_REGIAO_SELECAO_INSTRUCOES);
            regiaoTelaVazia = Root.Query<VisualElement>(NOME_REGIAO_TELA_VAZIA);

            PreencherSecaoSelecionarInstrucoes();

            return;
        }

        private void PreencherSecaoSelecionarInstrucoes() {
            if(instrucoes.Count <= 0) {
                regiaoSelecaoInstrucoes.AddToClassList(NomesClassesPadroesStyle.DisplayNone);
                return;
            }

            regiaoTelaVazia.AddToClassList(NomesClassesPadroesStyle.DisplayNone);

            int contadorGameObjects = 0;
            foreach(GameObject instrucao in instrucoes) {
                HabilitadorAtoresDinamico habilitadorAtoresDinamico = new(instrucao);
                regiaoSelecaoInstrucoes.Add(habilitadorAtoresDinamico.Root);

                contadorGameObjects++;
            }

            return;
        }
    }
}