using System;
using Autis.Editor.Manipuladores;
using Autis.Editor.UI;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Telas {
    public class EditarInformacoesCenaBehaviour : InformacoesCenaBehaviour {
        public Action<ManipuladorCena> OnConfirmarEdicao { get; set; }

        public EditarInformacoesCenaBehaviour() {
            CarregarDados();
            return;
        }

        private void CarregarDados() {
            inputVideo.VincularDados(manipuladorContexto.GetNomeArquivoVideo());
            
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

        protected override void HandleBotaoConfirmarClick() {
            base.HandleBotaoConfirmarClick();

            OnConfirmarEdicao?.Invoke(manipuladorCena);

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
    }
}