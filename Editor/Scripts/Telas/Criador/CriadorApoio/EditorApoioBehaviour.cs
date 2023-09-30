using System;
using System.Collections.Generic;
using UnityEngine;
using Autis.Editor.Criadores;
using Autis.Editor.Telas;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Editores {
    public class EditorApoioBehaviour : CriadorApoioBehaviour {
        public Action<GameObject> OnConfirmarEdicao { get; set; }

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_APOIO_NAO_VINCULADO = "[ERRO]: O Apoio não foi vinculado a nenhum Elemento de Interação";

        #endregion

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorApoioBehaviour(GameObject apoioEditado) {
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
            manipulador.Finalizar();
            GameObject.DestroyImmediate(objetoOriginal);

            OnConfirmarEdicao?.Invoke(objetoEditado);
            Navigator.Instance.Voltar();

            return;
        }

        protected override void HandleBotaoCancelarClick() {
            manipulador.Cancelar();
            objetoOriginal.SetActive(true);

            Navigator.Instance.Voltar();

            return;
        }
    }
}