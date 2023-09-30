using System;
using UnityEngine;
using Autis.Editor.Criadores;
using System.Collections.Generic;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Telas {
    public class EditorReforcoBehaviour : CriadorReforcoBehaviour {
        public Action<GameObject> OnConfirmarEdicao { get; set; }

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorReforcoBehaviour(GameObject reforcoEditado) {
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

            grupoInputsAudio.VincularDados(manipulador.ManipuladorComponenteAudioSource);
            grupoInputsImagem.VincularDados(manipulador.ComponenteSpriteRenderer);
            grupoInputsTexto.VincularDados(manipulador.ManipuladorComponenteTexto);
            grupoInputsVideo.VincularDados(manipulador.ManipuladorComponenteVideo);

            foreach(KeyValuePair<string, TiposReforcos> associacao in associacaoValoresDropdownTipoReforocos) {
                if(associacao.Value != manipulador.GetTipo()) {
                    continue;
                }

                dropdownTipoReforco.Campo.SetValueWithoutNotify(associacao.Key);
            }

            foreach (KeyValuePair<string, TipoAcionamentoReforco> associacao in associacoesOpcaoValorDropdown) {
                if(associacao.Value != manipulador.GetTipoAcionamento()) {
                    continue;
                }

                dropdownTipoAcionamento.Campo.SetValueWithoutNotify(associacao.Key);
            }

            foreach(KeyValuePair<string, float> associacao in associacoesTemposExibicao) {
                if(associacao.Value != manipulador.GetTempoExibicao()) {
                    continue;
                }

                dropdownTempoExibicao.Campo.SetValueWithoutNotify(associacao.Key);
                break;
            }

            OcultarCampos();

            switch(manipulador.GetTipo()) {
                case(TiposReforcos.Audio): {
                    ExibirCamposAudio();
                    break;
                }
                case(TiposReforcos.Imagem): {
                    ExibirCamposImagem();
                    break;
                }
                case(TiposReforcos.Texto): {
                    ExibirCamposTexto();
                    break;
                }
                case(TiposReforcos.Video): {
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