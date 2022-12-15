using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;

public class ConfiguracaoApoioBehaviour : ElementoInterfaceJogo {
    #region .: Elementos :.

    private const string NOME_REGIAO_SELECAO_APOIOS = "regiao-selecao-apoios";
    private readonly VisualElement regiaoSelecaoApoios;

    private const string NOME_REGIAO_TELA_VAZIA = "tela-vazia";
    private readonly VisualElement regiaoTelaViza;

    #endregion

    private readonly List<GameObject> apoios = new();

    public ConfiguracaoApoioBehaviour() {
        apoios = GameObject.FindGameObjectsWithTag(NomesTags.Apoios).ToList();

        ImportarTemplate("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoApoio/ConfiguracaoApoioTemplate");
        ImportarStyle("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoApoio/ConfiguracaoApoioStyle");

        regiaoSelecaoApoios = Root.Query<VisualElement>(NOME_REGIAO_SELECAO_APOIOS);
        regiaoTelaViza = Root.Query<VisualElement>(NOME_REGIAO_TELA_VAZIA);
        PreencherSecaoSelecionarApoios();

        return;
    }

    private void PreencherSecaoSelecionarApoios() {
        Debug.Log("[LOG]: Quantidade de apoios: " + apoios.Count);
        if(apoios.Count <= 0) {
            regiaoSelecaoApoios.AddToClassList(NomesClassesPadroesStyle.DisplayNone);
            return;
        } 
        else {
            regiaoTelaViza.AddToClassList(NomesClassesPadroesStyle.DisplayNone);
        }

        int contadorGameObjects = 0;
        foreach(GameObject apoio in apoios) {
            Toggle selecionarAtivo = new() {
                name = ("selecionar-apoio-" + contadorGameObjects),
                label = apoio.name,
            };
            selecionarAtivo.AddToClassList("input-selecionar-apoio");

            selecionarAtivo.labelElement.name = ("label-selecionar-apoio-" + contadorGameObjects);
            selecionarAtivo.labelElement.AddToClassList("label-selecionar-apoio");

            selecionarAtivo.SetValueWithoutNotify(apoio.activeInHierarchy);
            selecionarAtivo.RegisterCallback<ChangeEvent<bool>>(evt => {
                apoio.SetActive(evt.newValue);
            });

            regiaoSelecaoApoios.Add(selecionarAtivo);
            contadorGameObjects++;
        }

        return;
    }
}