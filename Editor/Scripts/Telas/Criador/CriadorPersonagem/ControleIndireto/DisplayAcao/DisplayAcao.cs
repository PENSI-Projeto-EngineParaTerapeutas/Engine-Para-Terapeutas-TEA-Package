using System;
using UnityEngine.UIElements;
using Autis.Editor.DTOs;
using Autis.Editor.Utils;

namespace Autis.Editor.UI {
    public class DisplayAcao : ElementoInterfaceEditor {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/ControleIndireto/DisplayAcao/DisplayAcaoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/ControleIndireto/DisplayAcao/DisplayAcaoStyle.uss";

        public Action<DisplayAcao> CallbackExcluirAcao { get => callbackExcluirAcao; set => callbackExcluirAcao = value; }
        private Action<DisplayAcao> callbackExcluirAcao;

        public Action<DisplayAcao> CallbackEditarAcao { get => callbackEditarAcao; set => callbackEditarAcao = value; }
        private Action<DisplayAcao> callbackEditarAcao;

        public AcaoPersonagem AcaoVinculada { get => acaoVinculada; }
        private readonly AcaoPersonagem acaoVinculada;

        #region .: Elementos :.

        private const string NOME_IMAGEM_ICONE_ANIMACAO = "imagem-icone-animacao";
        private readonly Image iconeAnimacao;

        private const string NOME_LABEL_ASSOCIACAO_OBJETO_ANIMACAO = "label-associacao-objeto-animacao";
        private readonly Label associacaoObjetoAnimacao;

        private const string NOME_IMAGEM_ICONE_LIXEIRA = "imagem-icone-lixeira";
        private readonly Image iconeLixeira;

        #endregion

        public DisplayAcao(AcaoPersonagem acaoPersonagem) {
            acaoVinculada = acaoPersonagem;

            iconeAnimacao = Root.Query<Image>(NOME_IMAGEM_ICONE_ANIMACAO);
            associacaoObjetoAnimacao = Root.Query<Label>(NOME_LABEL_ASSOCIACAO_OBJETO_ANIMACAO);
            iconeLixeira = Root.Query<Image>(NOME_IMAGEM_ICONE_LIXEIRA);

            ConfigurarImagens();
            ConfigurarLabel();

            return;
        }

        private void ConfigurarImagens() {
            iconeAnimacao.image = Importador.ImportarImagem("icone-animacao.png");
            iconeLixeira.image = Importador.ImportarImagem("icone-lixeira.png");

            iconeLixeira.RegisterCallback<ClickEvent>(evt => {
                callbackExcluirAcao?.Invoke(this);
            });

            return;
        }

        private void ConfigurarLabel() {
            AtualizarInformacoesLabel();
            associacaoObjetoAnimacao.RegisterCallback<ClickEvent>(evt => {
                callbackEditarAcao?.Invoke(this);
            });

            return;
        }

        public void AtualizarInformacoesLabel() {
            if(acaoVinculada.ObjetoGatilho == null || acaoVinculada.Animacao == null) {
                associacaoObjetoAnimacao.text = " - ";
                return;
            }

            associacaoObjetoAnimacao.text = acaoVinculada.ObjetoGatilho.name + " - " + acaoVinculada.Animacao.name;
            return;
        }
    }
}