using System;
using System.Collections.Generic;
using Autis.Editor.Constantes;
using Autis.Editor.Criadores;
using Autis.Runtime.DTOs;
using UnityEngine;

namespace Autis.Editor.Telas {
    public class EditorObjetoInteracaoBehaviour : CriadorObjetoInteracaoBehaviour {
        public Action<GameObject> OnConfirmarEdicao { get; set; }

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorObjetoInteracaoBehaviour(GameObject reforcoEditado) {
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

                if(associacao.Value == TiposAcoes.Arrastavel) {
                    checkboxDesfazerAcao.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    checkboxDesfazerAcao.SetValueWithoutNotify(manipulador.DeveDesfazerAcao());
                }
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