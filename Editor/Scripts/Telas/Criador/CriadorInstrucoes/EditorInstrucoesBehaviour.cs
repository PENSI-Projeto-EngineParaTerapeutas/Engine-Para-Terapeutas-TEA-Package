using System;
using UnityEngine;
using Autis.Editor.Criadores;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.Excecoes;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Telas {
    public class EditorInstrucoesBehaviour : CriadorInstrucoesBehaviour {

        #region .: Eventos :.

        public Action<GameObject> OnConfirmarEdicao { get; set; }
        protected static EventoJogo eventoFinalizarEdicao;

        #endregion

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorInstrucoesBehaviour(GameObject instrucaoEditada) {
            eventoFinalizarEdicao = Importador.ImportarEvento("EventoFinalizarEdicao");

            objetoOriginal = instrucaoEditada;

            objetoEditado = GameObject.Instantiate(objetoOriginal);
            objetoEditado.name = objetoOriginal.name;

            objetoOriginal.SetActive(false);

            manipulador.Cancelar();
            manipulador.Editar(objetoEditado);

            CarregarDados();

            return;
        }

        private void CarregarDados() {
            // TODO: Ajustar com base nas mudanças do Figma
            
            campoNome.CampoTexto.SetValueWithoutNotify(manipulador.ObjetoAtual.name);

            //grupoInputsAudio.VincularDados(manipulador.ComponenteAudioSource);
            grupoInputsTexto.VincularDados(manipulador.ManipuladorTexto);
            //grupoInputsVideo.VincularDados(manipulador.ComponenteVideo);

            dropdownTipoInstrucao.Campo.SetValueWithoutNotify(manipulador.GetTipo().ToString());

            switch(manipulador.GetTipo()) {
                case(TiposIntrucoes.Audio): {
                    ExibirCamposAudio();
                    break;
                }
                case(TiposIntrucoes.Texto): {
                    ExibirCamposTexto();
                    break;
                }
                case(TiposIntrucoes.Video): {
                    ExibirCamposVideo();
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