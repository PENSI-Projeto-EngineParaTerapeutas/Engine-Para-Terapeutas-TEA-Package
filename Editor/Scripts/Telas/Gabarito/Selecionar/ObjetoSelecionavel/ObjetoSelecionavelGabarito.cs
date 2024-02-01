using System;
using UnityEngine.UIElements;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {
    public class ObjetoSelecionavelGabarito : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "Telas/Gabarito/Selecionar/ObjetoSelecionavel/ObjetoSelecionavelGabaritoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Gabarito/Selecionar/ObjetoSelecionavel/ObjetoSelecionavelGabaritoStyle.uss";

        #region .: Elementos :.

        private const string NOME_ICONE_SELECIONAVEL = "imagem-icone-selecionavel";
        private Image iconeSelecionavel;

        private const string NOME_LABEL_NOME_OBJETO = "label-nome-objeto-selecionavel";
        private Label nomeObjetoSelecionavel;

        private const string NOME_ICONE_EXCLUIR = "imagem-icone-lixeira";
        private Image iconeExcluir;

        #endregion

        public Action<ObjetoSelecionavelGabarito> CallbackExcluirObjeto { get => callbackExcluirObjeto; set => callbackExcluirObjeto = value; }
        private Action<ObjetoSelecionavelGabarito> callbackExcluirObjeto;

        public string NomeObjetoAssociado { get => nomeObjeto; }
        private readonly string nomeObjeto = string.Empty;

        public ObjetoSelecionavelGabarito(string nomeObjeto) {
            this.nomeObjeto = nomeObjeto;

            ConfigurarIconeSelecionavel();
            ConfigurarLabelNomeObjeto();
            ConfigurarIconeExcluir();

            return;
        }

        private void ConfigurarIconeSelecionavel() {
            iconeSelecionavel = root.Query<Image>(NOME_ICONE_SELECIONAVEL);
            iconeSelecionavel.image = Importador.ImportarImagem("icone-selecao-objeto.png");

            return;
        }

        private void ConfigurarLabelNomeObjeto() {
            nomeObjetoSelecionavel = root.Query<Label>(NOME_LABEL_NOME_OBJETO);
            nomeObjetoSelecionavel.text = nomeObjeto;

            return;
        }

        private void ConfigurarIconeExcluir() {
            iconeExcluir = root.Query<Image>(NOME_ICONE_EXCLUIR);
            
            iconeExcluir.image = Importador.ImportarImagem("icone-lixeira.png");
            iconeExcluir.RegisterCallback<ClickEvent>(evt => {
                callbackExcluirObjeto?.Invoke(this);
            });

            return;
        }
    }
}