using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;

namespace Autis.Runtime.UI {
    public class ConfiguracaoApoioBehaviour : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoApoio/ConfiguracaoApoioTemplate";
        protected override string CaminhoStyle => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoApoio/ConfiguracaoApoioStyle";

        #region .: Elementos :.

        private const string NOME_REGIAO_SELECAO_APOIOS = "regiao-selecao-apoios";
        private readonly VisualElement regiaoSelecaoApoios;

        private const string NOME_REGIAO_TELA_VAZIA = "tela-vazia";
        private readonly VisualElement regiaoTelaViza;

        #endregion

        private readonly List<GameObject> apoios = new();

        public ConfiguracaoApoioBehaviour() {
            apoios = GameObject.FindGameObjectsWithTag(NomesTags.Apoios).ToList();

            regiaoSelecaoApoios = Root.Query<VisualElement>(NOME_REGIAO_SELECAO_APOIOS);
            regiaoTelaViza = Root.Query<VisualElement>(NOME_REGIAO_TELA_VAZIA);

            PreencherSecaoSelecionarApoios();

            return;
        }

        private void PreencherSecaoSelecionarApoios() {
            if(apoios.Count <= 0) {
                regiaoSelecaoApoios.AddToClassList(NomesClassesPadroesStyle.DisplayNone);
                return;
            }

            regiaoTelaViza.AddToClassList(NomesClassesPadroesStyle.DisplayNone);

            int contadorGameObjects = 0;
            foreach(GameObject apoio in apoios) {
                HabilitadorAtoresDinamico habilitadorAtoresDinamico = new(apoio);
                regiaoSelecaoApoios.Add(habilitadorAtoresDinamico.Root);

                contadorGameObjects++;
            }

            return;
        }
    }
}