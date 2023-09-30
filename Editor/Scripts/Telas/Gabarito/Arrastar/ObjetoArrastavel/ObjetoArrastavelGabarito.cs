using System;
using UnityEngine.UIElements;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {
    public class ObjetoArrastavelGabarito : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "Telas/Gabarito/Arrastar/ObjetoArrastavel/ObjetoArrastavelGabaritoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Gabarito/Arrastar/ObjetoArrastavel/ObjetoArrastavelGabaritoStyle.uss";

        #region .: Elementos :.

        private const string NOME_IMAGEM_ICONE_ARRASTAVEL = "imagem-icone-arrastavel";
        private Image iconeArrastavel;

        private const string NOME_LABEL_NOME_OBJETO_ARRASTAVEL = "label-nome-objeto-arrastavel";
        private Label labelNomeObjeto;

        private const string NOME_IMAGEM_ICONE_LIXEIRA = "imagem-icone-lixeira";
        private Image iconeLixeira;

        #endregion

        public Action<ObjetoArrastavelGabarito> CallbackExcluirObjeto { get => callbackExcluirObjeto; set => callbackExcluirObjeto = value; }
        private Action<ObjetoArrastavelGabarito> callbackExcluirObjeto;

        public string NomeObjetoOrigemAssociado { get => nomeObjetoOrigem; }
        private readonly string nomeObjetoOrigem = string.Empty;

        public string NomeObjetoDestinoAssociado { get => nomeObjetoDestino; }
        private readonly string nomeObjetoDestino = string.Empty;

        public ObjetoArrastavelGabarito(string nomeObjetoOrigem, string nomeObjetoDestino) {
            this.nomeObjetoOrigem = nomeObjetoOrigem;
            this.nomeObjetoDestino = nomeObjetoDestino;

            ConfigurarIconeArrastavel();
            ConfigurarLabelNomeObjeto();
            ConfigurarIconeLixeira();

            return;
        }
        
        private void ConfigurarIconeArrastavel() {
            iconeArrastavel = root.Query<Image>(NOME_IMAGEM_ICONE_ARRASTAVEL);
            iconeArrastavel.image = Importador.ImportarImagem("icone-arrastar-objeto.png");

            return;
        }

        private void ConfigurarLabelNomeObjeto() {
            labelNomeObjeto = root.Query<Label>(NOME_LABEL_NOME_OBJETO_ARRASTAVEL);
            labelNomeObjeto.text = (nomeObjetoOrigem + " - " + nomeObjetoDestino);

            return;
        }

        private void ConfigurarIconeLixeira() {
            iconeLixeira = root.Query<Image>(NOME_IMAGEM_ICONE_LIXEIRA);

            iconeLixeira.image = Importador.ImportarImagem("icone-lixeira.png");
            iconeLixeira.RegisterCallback<ClickEvent>(evt => {
                callbackExcluirObjeto?.Invoke(this);
            });

            return;
        }
    }
}