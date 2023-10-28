using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Autis.Runtime.DTOs;
using Autis.Editor.UI;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;
using Autis.Editor.Telas;

namespace Autis.Editor.Criadores
{
    public class CriadorInstrucoesBehaviour : Tela, IReiniciavel
    {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorInstrucoes/CriadorInstrucoesTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorInstrucoes/CriadorInstrucoesStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_TOOLTIP_INPUT_NOME = "Digite um nome para a Instrução. Cada componente deve ter um nome exclusivo (que não se repete em outro componente)";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_INSTRUCAO = "Forma que a instrução será apresentada. Opções: áudio, texto ou vídeo.";
        protected const string MENSAGEM_TOOLTIP_TITULO = "A instrução é um componente que pode ser usado para transmitir informações sobre a fase para o jogador.";

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_NOME = "regiao-input-nome";
        protected VisualElement regiaoCarregamenteInputNome;

        protected const string NOME_REGIAO_CARREGAMENTO_TIPO_INSTRUCAO = "regiao-carregamento-input-tipo-instrucao";
        protected VisualElement regiaoCarregamentoTipoInstrucao;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        protected VisualElement regiaoCarregamentoInputsVideo;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        protected VisualElement regiaoCarregamentoInputsAudio;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        protected VisualElement regiaoCarregamentoInputsTexto;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InputTexto campoNome;
        protected Dropdown dropdownTipoInstrucao;
        protected InputsComponenteVideo grupoInputsVideo;
        protected GrupoInputsAudio grupoInputsAudio;
        protected GrupoInputsTexto grupoInputsTexto;
        protected BotoesConfirmacao botoesConfirmacao;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected InterrogacaoToolTip tooltipTitulo;

        #endregion

        protected ManipuladorInstrucoes manipulador;

        public CriadorInstrucoesBehaviour() {
            manipulador = new ManipuladorInstrucoes();
            manipulador.Criar();

            ConfigurarTooltipTitulo();
            ConfigurarCampoNome();
            CarregarRegiaoInputsVideo();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsTexto();
            ConfigurarCampoTipoInstrucao();

            ConfigurarBotoesConfirmacao();

            OcultarCampos();

            return;
        }

        protected virtual void ConfigurarTooltipTitulo() {
            tooltipTitulo = new InterrogacaoToolTip(MENSAGEM_TOOLTIP_TITULO);

            regiaoCarregamentoTooltipTitulo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);
            regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

            return;
        }

        protected virtual void ConfigurarCampoNome() {
            campoNome = new InputTexto("Nome:", MENSAGEM_TOOLTIP_INPUT_NOME);

            campoNome.CampoTexto.AddToClassList("input-texto");

            campoNome.CampoTexto.RegisterCallback<ChangeEvent<string>>(evt => {
                manipulador.SetNome(evt.newValue);
            });

            regiaoCarregamenteInputNome = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_NOME);
            regiaoCarregamenteInputNome.Add(campoNome.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputsVideo() {
            grupoInputsVideo = new InputsComponenteVideo();
            grupoInputsVideo.VincularDados(manipulador.ComponenteVideo);

            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);
            regiaoCarregamentoInputsVideo.Add(grupoInputsVideo.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputsAudio() {
            grupoInputsAudio = new GrupoInputsAudio();
            //grupoInputsAudio.VincularDados(manipulador.ComponenteAudioSource);

            regiaoCarregamentoInputsAudio = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO);
            regiaoCarregamentoInputsAudio.Add(grupoInputsAudio.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputsTexto() {
            grupoInputsTexto = new GrupoInputsTexto();
            grupoInputsTexto.VincularDados(manipulador.ManipuladorTexto);

            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);
            regiaoCarregamentoInputsTexto.Add(grupoInputsTexto.Root);

            return;
        }

        protected virtual void ConfigurarCampoTipoInstrucao() {
            List<string> opcoes = new()
            {
                TiposIntrucoes.Audio.ToString(),
                TiposIntrucoes.Texto.ToString(),
                TiposIntrucoes.Video.ToString(),
            };

            dropdownTipoInstrucao = new Dropdown("Tipo de instrução:", MENSAGEM_TOOLTIP_DROPDOWN_TIPO_INSTRUCAO, opcoes);
            dropdownTipoInstrucao.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if (evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    manipulador.DesabilitarComponentes();
                    OcultarCampos();
                    return;
                }

                TiposIntrucoes novoTipo = Enum.Parse<TiposIntrucoes>(evt.newValue);

                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
                manipulador.AlterarTipo(novoTipo);
            });

            regiaoCarregamentoTipoInstrucao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TIPO_INSTRUCAO);
            regiaoCarregamentoTipoInstrucao.Add(dropdownTipoInstrucao.Root);

            return;
        }

        protected virtual void AlterarVisibilidadeCamposComBaseTipo(TiposIntrucoes tipo) {
            OcultarCampos();

            switch(tipo) {
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

        protected virtual void OcultarCampos() {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsVideo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposAudio() {
            OcultarCampos();

            regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposTexto() {
            OcultarCampos();

            regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposVideo() {
            OcultarCampos();

            regiaoCarregamentoInputsVideo.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        protected virtual void HandleBotaoConfirmarClick() {
            manipulador.Finalizar();

            Navigator.Instance.Voltar();
            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            manipulador.Cancelar();

            Navigator.Instance.Voltar();
            return;
        }

        public virtual void ReiniciarCampos() {
            campoNome.ReiniciarCampos();
            dropdownTipoInstrucao.ReiniciarCampos();

            grupoInputsVideo.ReiniciarCampos();
            grupoInputsAudio.ReiniciarCampos();
            grupoInputsTexto.ReiniciarCampos();

            OcultarCampos();

            return;
        }
    }
}