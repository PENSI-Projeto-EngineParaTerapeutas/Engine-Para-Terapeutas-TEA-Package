using System;
using UnityEngine;
using Autis.Editor.Criadores;
using Autis.Editor.Constantes;
using Autis.Editor.Excecoes;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Telas {
    public class EditorCenarioBehaviour : CriadorCenarioBehaviour {

        #region .: Eventos :.

        public Action<GameObject> OnConfirmarEdicao { get; set; }
        protected static EventoJogo eventoFinalizarEdicao;

        #endregion

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorCenarioBehaviour(GameObject cenarioEditado) {
            eventoFinalizarEdicao = Importador.ImportarEvento("EventoFinalizarEdicao");

            objetoOriginal = cenarioEditado;

            objetoEditado = GameObject.Instantiate(objetoOriginal);
            objetoEditado.name = objetoOriginal.name;

            objetoOriginal.SetActive(false);

            manipulador.Cancelar();
            manipulador.Editar(objetoEditado);

            CarregarDados();

            return;
        }

        private void CarregarDados() {
            if(manipulador.EhCorSolida()) {
                radioButtonCorUnica.SetValueWithoutNotify(true);
                inputCor.Root.SetEnabled(true);
                inputCor.CampoCor.SetValueWithoutNotify(manipulador.GetCor());
            }
            else {
                radioButtonImagem.SetValueWithoutNotify(true);
                inputImagem.Root.SetEnabled(true);
                inputImagem.CampoImagem.SetValueWithoutNotify(manipulador.GetImagem());
            }

            return;
        }

        protected override void HandleBotaoConfirmarClick() {
            try {
                VerificarCamposObrigatorios();
            }
            catch(ExcecaoCamposObrigatoriosVazios error) {
                PopupAvisoBehaviour.ShowPopupAviso(error.Message);
                return;
            }

            try {
                manipulador.Finalizar();
            }
            catch(ExcecaoObjetoDuplicado excecao) {
                PopupAvisoBehaviour.ShowPopupAviso(MensagensGerais.MENSAGEM_ATOR_DUPLICADO.Replace("{nome}", excecao.NomeObjetoDuplicado));
                return;
            }

            GameObject.DestroyImmediate(objetoOriginal);

            OnConfirmarEdicao?.Invoke(objetoEditado);
            eventoFinalizarEdicao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }

        protected override void HandleBotaoCancelarClick() {
            manipulador.Cancelar();
            objetoOriginal.SetActive(true);

            eventoFinalizarEdicao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }
    }
}