using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;

namespace EngineParaTerapeutas.UI {
    public class ConfiguracaoReforcoBehaviour : ElementoInterfaceJogo {
        #region .: Elementos :.

        private const string NOME_REGIAO_SELECAO_REFORCOS = "regiao-selecao-reforcos";
        private readonly VisualElement regiaoSelecaoReforcos;

        private const string NOME_REGIAO_TELA_VAZIA = "tela-vazia";
        private readonly VisualElement regiaoTelaViza;

        #endregion

        private readonly List<GameObject> reforcos = new();

        public ConfiguracaoReforcoBehaviour() {
            reforcos = GameObject.FindGameObjectsWithTag(NomesTags.Reforcos).ToList();

            ImportarTemplate("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoReforco/ConfiguracaoReforcoTemplate");
            ImportarStyle("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoReforco/ConfiguracaoReforcoStyle");

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
            else {
                regiaoTelaViza.AddToClassList(NomesClassesPadroesStyle.DisplayNone);
            }

            int contadorGameObjects = 0;
            foreach(GameObject reforco in reforcos) {
                Toggle selecionarAtivo = new() {
                    name = ("selecionar-reforco-" + contadorGameObjects),
                    label = reforco.name,
                };
                selecionarAtivo.AddToClassList("input-selecionar-reforcos");

                selecionarAtivo.labelElement.name = ("label-selecionar-reforcos-" + contadorGameObjects);
                selecionarAtivo.labelElement.AddToClassList("label-selecionar-reforcos");

                selecionarAtivo.SetValueWithoutNotify(reforco.activeInHierarchy);
                selecionarAtivo.RegisterCallback<ChangeEvent<bool>>(evt => {
                    reforco.SetActive(evt.newValue);
                });

                regiaoSelecaoReforcos.Add(selecionarAtivo);
                contadorGameObjects++;
            }

            return;
        }
    }
}