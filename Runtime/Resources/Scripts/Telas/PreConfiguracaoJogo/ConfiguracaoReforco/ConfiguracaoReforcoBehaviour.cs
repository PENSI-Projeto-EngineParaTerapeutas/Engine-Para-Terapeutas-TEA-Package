using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;

namespace Autis.Runtime.UI {
    public class ConfiguracaoReforcoBehaviour : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoReforco/ConfiguracaoReforcoTemplate";
        protected override string CaminhoStyle => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoReforco/ConfiguracaoReforcoStyle";

        #region .: Elementos :.

        private const string NOME_REGIAO_SELECAO_REFORCOS = "regiao-selecao-reforcos";
        private readonly VisualElement regiaoSelecaoReforcos;

        private const string NOME_REGIAO_TELA_VAZIA = "tela-vazia";
        private readonly VisualElement regiaoTelaViza;

        #endregion

        private readonly List<GameObject> reforcos = new();

        public ConfiguracaoReforcoBehaviour() {
            reforcos = GameObject.FindGameObjectsWithTag(NomesTags.Reforcos).ToList();

            regiaoSelecaoReforcos = Root.Query<VisualElement>(NOME_REGIAO_SELECAO_REFORCOS);
            regiaoTelaViza = Root.Query<VisualElement>(NOME_REGIAO_TELA_VAZIA);
            PreencherSecaoSelecionarReforcos();

            return;
        }

        private void PreencherSecaoSelecionarReforcos() {
            if(reforcos.Count <= 0) {
                regiaoSelecaoReforcos.AddToClassList(NomesClassesPadroesStyle.DisplayNone);
                return;
            }
            
            regiaoTelaViza.AddToClassList(NomesClassesPadroesStyle.DisplayNone);

            int contadorGameObjects = 0;
            foreach(GameObject reforco in reforcos) {
                HabilitadorAtoresDinamico habilitadorAtoresDinamico = new(reforco);
                regiaoSelecaoReforcos.Add(habilitadorAtoresDinamico.Root);

                contadorGameObjects++;
            }

            return;
        }
    }
}