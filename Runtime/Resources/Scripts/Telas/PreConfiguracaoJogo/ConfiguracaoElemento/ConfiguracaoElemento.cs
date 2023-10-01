using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.UI {
    public class ConfiguracaoElemento : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoElemento/ConfiguracaoElementoTemplate";
        protected override string CaminhoStyle => "Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoElemento/ConfiguracaoElementoStyle";

        #region .: Elementos :.

        private const string NOME_REGIAO_SELECAO_OBJETOS_INTERACAO = "regiao-selecao-objeto-interacao";
        private readonly VisualElement regiaoSelecaoObjetosInteracao;

        private const string NOME_REGIAO_TELA_VAZIA = "tela-vazia";
        private readonly VisualElement regiaoTelaVazia;

        #endregion

        private readonly List<GameObject> objetosInteracao = new();

        public ConfiguracaoElemento() {
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
                ModificadorImagemDinamico modificadorImagem = new(spriteObjetoInteracao.sprite, (novaImagem) => {
                    spriteObjetoInteracao.sprite = Sprite.Create(novaImagem, new Rect(0.0f, 0.0f, novaImagem.width, novaImagem.height), new Vector2(0.5f, 0.5f)); // TODO: Ajustar para não aumentar ou diminuir o tamanho do ator
                });

                regiaoSelecaoObjetosInteracao.Add(modificadorImagem.Root);

                contadorGameObjects++;
            }

            return;
        }
    }
}