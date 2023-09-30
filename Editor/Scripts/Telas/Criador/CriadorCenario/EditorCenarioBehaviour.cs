using System;
using UnityEngine;
using Autis.Editor.Criadores;

namespace Autis.Editor.Telas {
    public class EditorCenarioBehaviour : CriadorCenarioBehaviour {
        public Action<GameObject> OnConfirmarEdicao { get; set; }

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorCenarioBehaviour(GameObject cenarioEditado) {
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
            // TODO: Adaptar a tela refatorada ao figma
            campoNome.CampoTexto.SetValueWithoutNotify(manipulador.ObjetoAtual.name);
            grupoInputsImagem.VincularDados(manipulador.ComponenteSpriteRenderer);
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