using System;
using Autis.Editor.Excecoes;
using Autis.Editor.Manipuladores;
using Autis.Editor.UI;
using Autis.Editor.Utils;
using Autis.Runtime.DTOs;
using Autis.Runtime.Eventos;

namespace Autis.Editor.Telas {
    public class EditarInformacoesCenaBehaviour : InformacoesCenaBehaviour {

        #region .: Eventos :.

        public Action<ManipuladorCena> OnConfirmarEdicao { get; set; }
        protected static EventoJogo eventoFinalizarEdicao;

        #endregion

        public EditarInformacoesCenaBehaviour() {
            eventoFinalizarEdicao = Importador.ImportarEvento("EventoFinalizarEdicao");
            CarregarDados();

            return;
        }

        private void CarregarDados() {
            campoNome.CampoTexto.SetValueWithoutNotify("Fase atual"); // TODO: Remover mock
            // TODO: campoNome.CampoTexto.SetValueWithoutNotify(manipuladorCena.GetNome());
            campoVideoContexto.VincularDados(manipuladorContexto.GetNomeArquivoVideo());

            dropdownDificuldade.Campo.SetValueWithoutNotify(manipuladorCena.GetDificuldade().ToString());
            /* TODO: Implementar faixa
            faixaEtariaInferior.CampoNumerico.SetValueWithoutNotify();
            faixaEtariaSuperior.CampoNumerico.SetValueWithoutNotify();*/

            CarregarOpcaoGabarito();

            return;
        }

        protected override void HandleBotaoConfirmarClick() {
            try {
                VerificarCamposObrigatorios();
            } 
            catch(ExcecaoCamposObrigatoriosVazios excecao) {
                PopupAvisoBehaviour.ShowPopupAviso(excecao.Message);
                return;
            }

            eventoFinalizarEdicao.AcionarCallbacks();
            OnConfirmarEdicao?.Invoke(manipuladorCena);
            Navigator.Instance.Voltar();

            return;
        }

        protected override void HandleBotaoCriarGabaritoClick() {
            if(opcaoRadioArrastar.value) {
                Navigator.Instance.IrPara(new EditarGabaritoArrastarBehaviour());
                return;
            }

            if(opcaoRadioSelecionar.value) {
                Navigator.Instance.IrPara(new EditarGabaritoSelecionavelBehaviour());
                return;
            }

            return;
        }

        protected override void CarregarOpcaoGabarito() {
            if(manipuladorCena.GetTipoGabarito() == TipoGabarito.Selecionar) {
                opcaoRadioSelecionar.SetValueWithoutNotify(true);
                botaoCriarGabarito.SetEnabled(true);
            }
            else if(manipuladorCena.GetTipoGabarito() == TipoGabarito.Arrastar) {
                opcaoRadioArrastar.SetValueWithoutNotify(true);
                botaoCriarGabarito.SetEnabled(true);
            }

            return;
        }

        protected override void HandleBotaoCancelarClick() {
            eventoFinalizarEdicao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }
    }
}