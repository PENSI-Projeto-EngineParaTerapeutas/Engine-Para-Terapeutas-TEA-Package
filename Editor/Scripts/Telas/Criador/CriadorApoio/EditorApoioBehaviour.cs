using System;
using System.Collections.Generic;
using UnityEngine;
using Autis.Editor.Criadores;
using Autis.Editor.Telas;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.Excecoes;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Editores {
    public class EditorApoioBehaviour : CriadorApoioBehaviour {

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_APOIO_NAO_VINCULADO = "[ERRO]: O Apoio não foi vinculado a nenhum Elemento de Interação";

        #endregion

        #region .: Eventos :.

        public Action<GameObject> OnConfirmarEdicao { get; set; }
        protected static EventoJogo eventoFinalizarEdicao;

        #endregion

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorApoioBehaviour(GameObject apoioEditado) {
            eventoFinalizarEdicao = Importador.ImportarEvento("EventoFinalizarEdicao");

            objetoOriginal = apoioEditado;

            objetoEditado = GameObject.Instantiate(objetoOriginal, objetoOriginal.transform.parent);
            objetoEditado.name = objetoOriginal.name;

            objetoOriginal.SetActive(false);

            manipulador.Cancelar();
            manipulador.Editar(objetoEditado);

            CarregarDados();

            return;
        }

        private void CarregarDados() {
            campoNome.CampoTexto.SetValueWithoutNotify(manipulador.GetNome());

            if(manipulador.GetObjetoPai() == null) {
                Debug.LogError(MENSAGEM_ERRO_APOIO_NAO_VINCULADO);
            }

            radioHabilitarSelecionarObjeto.SetValueWithoutNotify(true);
            dropdownObjetosInteracao.Root.SetEnabled(true);
            dropdownObjetosInteracao.Campo.SetValueWithoutNotify(manipulador.GetObjetoPai().name);

            foreach(KeyValuePair<string, TiposApoiosObjetosInteracao> associacao in associacaoValoresDropdownTipoApoios) {
                if(associacao.Value != manipulador.GetTipo()) {
                    continue;
                }

                dropdownTipoApoio.Campo.SetValueWithoutNotify(associacao.Key);
                break;
            }

            foreach(KeyValuePair<string, TipoAcionamentoApoioObjetoInteracao> associacao in associacaoOpcaoValorDropdownAcionamento) {
                if(associacao.Value != manipulador.GetTipoAcionamento()) {
                    continue;
                }

                dropdownTipoAcionamento.Campo.SetValueWithoutNotify(associacao.Key);
                break;
            }

            foreach(KeyValuePair<string, float> associacao in associacoesTemposExibicao) {
                if(associacao.Value != manipulador.GetTempoExibicao()) {
                    continue;
                }

                dropdownTempoExibicao.Campo.SetValueWithoutNotify(associacao.Key);
                break;
            }

            grupoInputsAudio.VincularDados(manipulador.ManipualdorComponenteAudioSource);
            grupoInputTexto.VincularDados(manipulador.ManipuladorComponenteTexto);
            grupoInputsAudio.VincularDados(manipulador.ManipualdorComponenteAudioSource);

            switch(manipulador.GetTipo()) {
                case(TiposApoiosObjetosInteracao.Audio): {
                    ExibirCamposApoioAudio();
                    break;
                }
                case(TiposApoiosObjetosInteracao.Texto): {
                    ExibirCamposApoioTexto();
                    break;
                }
                case(TiposApoiosObjetosInteracao.Seta): {
                    ExibirCamposApoioSeta();
                    break;
                }
            }

            grupoInputsPosicao.VincularDados(manipulador);
            grupoInputsTamanho.VincularDados(manipulador);

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