using System;
using UnityEngine;
using Autis.Editor.Criadores;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Telas {
    public class EditorInstrucoesBehaviour : CriadorInstrucoesBehaviour {
        public Action<GameObject> OnConfirmarEdicao { get; set; }

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorInstrucoesBehaviour(GameObject instrucaoEditada) {
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
            grupoInputsVideo.VincularDados(manipulador.ComponenteVideo);

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