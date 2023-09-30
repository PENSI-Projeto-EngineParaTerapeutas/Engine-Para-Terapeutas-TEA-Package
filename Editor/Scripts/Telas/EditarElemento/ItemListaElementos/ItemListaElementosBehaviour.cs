using System;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {
    public class ItemListaElementosBehaviour : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "Telas/EditarElemento/ItemListaElementos/ItemListaElementosTemplate.uxml";
        protected override string CaminhoStyle => "Telas/EditarElemento/ItemListaElementos/ItemListaElementosStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_NOME_ELEMENTO = "label-nome-elemento";
        private Label labelNomeElemento;

        private const string NOME_ICONE_ELEMENTO = "icone-elemento";
        private Image iconeElemento;

        private const string NOME_BOTAO_EXCLUIR = "botao-excluir";
        private Button botaoExcluir;

        private const string NOME_ICONE_EXCLUIR = "icone-excluir";
        private Image iconeExcluir;

        #endregion

        public GameObject ObjetoVinculado { get => objetoVinculado; }
        private GameObject objetoVinculado;

        public Action OnEditarClick { get; set; }
        public Action OnExcluirClick { get; set; }

        public ItemListaElementosBehaviour(GameObject objetoVinculado) {
            this.objetoVinculado = objetoVinculado;

            ConfigurarLabelElemento();
            ConfigurarIconeElemento();
            ConfigurarIconeExcluir();
           
            return;
        }

        private void ConfigurarLabelElemento() {
            labelNomeElemento = root.Query<Label>(NOME_LABEL_NOME_ELEMENTO);
            labelNomeElemento.text = objetoVinculado.name;

            labelNomeElemento.RegisterCallback<ClickEvent>(HandleEventoSelecao);

            return;
        }

        private void ConfigurarIconeElemento() {
            iconeElemento = root.Query<Image>(NOME_ICONE_ELEMENTO);
            iconeElemento.image = Importador.ImportarImagem("Cubo.png");

            return;
        }

        private void ConfigurarIconeExcluir() {
            iconeExcluir = Root.Query<Image>(NOME_ICONE_EXCLUIR);
            iconeExcluir.image = Importador.ImportarImagem("icone-lixeira.png");

            botaoExcluir = Root.Query<Button>(NOME_BOTAO_EXCLUIR);
            botaoExcluir.clicked += HandleEventoExcluir;

            return;
        }

        public void AlterarObjetoVinculado(GameObject objetoVinculado) {
            this.objetoVinculado = objetoVinculado;
            labelNomeElemento.text = this.objetoVinculado.name;

            return;
        }

        private void HandleEventoSelecao(ClickEvent evt) {
            OnEditarClick?.Invoke();
            return;
        }

        private void HandleEventoExcluir() {
            OnExcluirClick?.Invoke();
            return;
        }
    }
}