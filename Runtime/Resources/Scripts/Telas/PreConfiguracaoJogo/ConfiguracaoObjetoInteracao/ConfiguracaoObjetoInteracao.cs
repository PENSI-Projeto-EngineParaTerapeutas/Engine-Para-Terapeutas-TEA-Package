using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.UI {
    public class ConfiguracaoObjetoInteracao : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoObjetoInteracao/ConfiguracaoObjetoInteracaoTemplate";
        protected override string CaminhoStyle => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoObjetoInteracao/ConfiguracaoObjetoInteracaoStyle";

        #region .: Elementos :.

        private const string NOME_REGIAO_SELECAO_OBJETOS_INTERACAO = "regiao-selecao-objeto-interacao";
        private readonly VisualElement regiaoSelecaoObjetosInteracao;

        private const string NOME_REGIAO_TELA_VAZIA = "tela-vazia";
        private readonly VisualElement regiaoTelaVazia;

        #endregion

        private readonly List<GameObject> objetosInteracao = new();

        public ConfiguracaoObjetoInteracao() {
            objetosInteracao = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();

            regiaoSelecaoObjetosInteracao = Root.Query<VisualElement>(NOME_REGIAO_SELECAO_OBJETOS_INTERACAO);
            regiaoTelaVazia = Root.Query<VisualElement>(NOME_REGIAO_TELA_VAZIA);
            PreencherSecaoSelecionarObjetosInteracao();

            return;
        }

        private void PreencherSecaoSelecionarObjetosInteracao() {
            if(objetosInteracao.Count <= 0) {
                regiaoSelecaoObjetosInteracao.AddToClassList(NomesClassesPadroesStyle.DisplayNone);
                return;
            }

            regiaoTelaVazia.AddToClassList(NomesClassesPadroesStyle.DisplayNone);

            int contadorGameObjects = 0;
            foreach(GameObject objetoInteracao in objetosInteracao) {
                HabilitadorAtoresDinamico habilitadorAtoresDinamico = new(objetoInteracao);
                regiaoSelecaoObjetosInteracao.Add(habilitadorAtoresDinamico.Root);

                IdentificadorTipoObjetoInteracao tipoObjetoInteracao = objetoInteracao.GetComponent<IdentificadorTipoObjetoInteracao>();
                if(tipoObjetoInteracao.Tipo != TiposObjetosInteracao.Imagem) {
                    contadorGameObjects++;
                    continue;
                }

                SpriteRenderer spriteObjetoInteracao = objetoInteracao.GetComponent<SpriteRenderer>();
                ModificadorImagemDinamico modificadorImagem = new(spriteObjetoInteracao);

                regiaoSelecaoObjetosInteracao.Add(modificadorImagem.Root);

                contadorGameObjects++;
            }

            return;
        }
    }
}