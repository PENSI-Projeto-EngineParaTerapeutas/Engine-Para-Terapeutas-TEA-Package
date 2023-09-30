using System;
using UnityEngine;
using Autis.Editor.Criadores;
using Autis.Runtime.DTOs;
using Autis.Editor.Manipuladores;
using Autis.Runtime.ComponentesGameObjects;

namespace Autis.Editor.Telas {
    public class EditorPersonagemBehaviour : CriadorPersonagemBehaviour {
        public Action<GameObject> OnConfirmarEdicao { get; set; }

        private readonly GameObject objetoOriginal;
        private readonly GameObject objetoEditado;

        public EditorPersonagemBehaviour(GameObject instrucaoEditada) {
            objetoOriginal = instrucaoEditada;

            objetoEditado = GameObject.Instantiate(objetoOriginal);
            objetoEditado.name = objetoOriginal.name;

            objetoOriginal.SetActive(false);

            TiposPersonagem tipoPersonagem = objetoEditado.GetComponent<DadosPersonagem>().tipoPersonagem;
            switch(tipoPersonagem) {
                case(TiposPersonagem.Avatar): {
                    manipuladorPersonagem = new ManipuladorAvatar(prefabAvatar);
                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    manipuladorPersonagem = new ManipuladorBonecoPalito(prefabBonecoPalito);
                    break;
                }
                case(TiposPersonagem.Ludico): {
                    manipuladorPersonagem = new ManipuladorPersonagemLudico(prefabPersonagemLudico);
                    break;
                }
            }

            manipuladorPersonagem.Editar(objetoEditado);
            manipuladorPersonagem.CarregarAcoesControleIndireto();

            CarregarDados();

            return;
        }

        private void CarregarDados() {
            HabilitarCamposPersonagemSelecionado();

            inputNome.CampoTexto.SetValueWithoutNotify(manipuladorPersonagem.GetNome());

            grupoInputsPosicao.VincularDados(manipuladorPersonagem);
            inputTamanho.CampoNumerico.SetValueWithoutNotify(manipuladorPersonagem.GetTamanho().x);

            dropdownTipoPersonagem.Campo.SetValueWithoutNotify(manipuladorPersonagem.GetTipoPersonagem().ToString());

            switch(manipuladorPersonagem.GetTipoControle()) {
                case(TipoControle.Indireto): {
                    radioOpcaoControleDireto.SetValueWithoutNotify(false);
                    radioOpcaoControleIndireto.SetValueWithoutNotify(true);
                    botaoConfigurarControleIndireto.SetEnabled(true);
                    break;
                }
                case(TipoControle.Direto): {
                    radioOpcaoControleDireto.SetValueWithoutNotify(true);
                    radioOpcaoControleIndireto.SetValueWithoutNotify(false);
                    botaoConfigurarControleIndireto.SetEnabled(false);
                    break;
                }
            }

            return;
        }

        private void HabilitarCamposPersonagemSelecionado() {
            botaoPersonalizarPersonagem.SetEnabled(true);
            radioOpcaoControleDireto.SetEnabled(true);
            radioOpcaoControleIndireto.SetEnabled(true);
            botoesConfirmacao.Root.SetEnabled(true);

            return;
        }

        protected override void HandleBotaoConfirmarClick() {
            manipuladorPersonagem.Finalizar();
            GameObject.DestroyImmediate(objetoOriginal);

            OnConfirmarEdicao?.Invoke(objetoEditado);
            Navigator.Instance.Voltar();

            return;
        }

        protected override void HandleBotaoCancelarClick() {
            manipuladorPersonagem.CancelarEdicao();
            objetoOriginal.SetActive(true);

            Navigator.Instance.Voltar();

            return;
        }
    }
}