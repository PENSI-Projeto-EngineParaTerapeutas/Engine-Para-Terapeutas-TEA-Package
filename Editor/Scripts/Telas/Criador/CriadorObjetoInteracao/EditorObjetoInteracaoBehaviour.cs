using System;
using System.Collections.Generic;
using UnityEngine;
using Autis.Editor.Criadores;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.Excecoes;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Telas {
    public class EditorObjetoInteracaoBehaviour : CriadorObjetoInteracaoBehaviour {

        #region .: Eventos :.

        public Action<GameObject> OnConfirmarEdicao { get; set; }
        protected static EventoJogo eventoFinalizarEdicao;

        #endregion

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorObjetoInteracaoBehaviour(GameObject reforcoEditado) {
            eventoFinalizarEdicao = Importador.ImportarEvento("EventoFinalizarEdicao");

            objetoOriginal = reforcoEditado;

            objetoEditado = GameObject.Instantiate(objetoOriginal);
            objetoEditado.name = objetoOriginal.name;

            objetoOriginal.SetActive(false);

            manipulador.Cancelar();
            manipulador.Editar(objetoEditado);

            CarregarDados();

            return;
        }

        private void CarregarDados() {
            campoNome.CampoTexto.SetValueWithoutNotify(manipulador.ObjetoAtual.name);

            inputsImagem.VincularDados(manipulador.ManipuladorSpriteRenderer.GetImagem());
            grupoInputsTexto.VincularDados(manipulador.ManipuladorTexto);

            dropdownTipo.Campo.SetValueWithoutNotify(manipulador.GetTipo().ToString());

            foreach(KeyValuePair<string, TiposAcoes> associacao in associacaoValoresDropdownTipoAcoes) {
                if(associacao.Value != manipulador.GetTipoInteracao()) {
                    continue;
                }

                dropdownTiposAcoes.Campo.SetValueWithoutNotify(associacao.Key);
            }

            switch(manipulador.GetTipo()) {
                case(TiposObjetosInteracao.Imagem): {
                    ExibirCamposImagem();
                    break;
                }
                case(TiposObjetosInteracao.Texto): {
                    ExibirCamposTexto();
                    break;
                }
            }

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