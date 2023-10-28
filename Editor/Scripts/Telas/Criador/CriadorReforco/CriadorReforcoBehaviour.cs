using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Criadores {
    public class CriadorReforcoBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorReforco/CriadorReforcoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorReforco/CriadorReforcoStyle.uss";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_TITULO = "Reforço é um feedback ou recompensa que serão apresentadas no jogo.";
        private const string MENSAGEM_TOOLTIP_INPUT_NOME = "Digite um nome para o Reforço. Cada componente deve ter um nome exclusivo (que não se repete em outro componente)";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_REFORCO = "Forma que o reforço será apresentado.";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_ACIONAMENTO = "Definição do que fará o reforço ser apresentado na fase do jogo.";
        protected const string MENSAGEM_TOOLTIP_DROPDOWN_TEMPO_EXIBICAO = "[TODO]: Adicionar tooltip";

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_NOME = "regiao-input-nome";
        protected VisualElement regiaoCarregamenteInputNome;

        protected const string NOME_REGIAO_CARREGAMENTO_DROPDOWN_TIPO = "regiao-carregamento-input-tipo-reforco";
        protected VisualElement regiaoCarregamentoDropdownTipo;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        protected VisualElement regiaoCarregamentoInputsImagem;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        protected VisualElement regiaoCarregamentoInputsAudio;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        protected VisualElement regiaoCarregamentoInputsTexto;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO = "regiao-carregamento-inputs-video";
        protected VisualElement regiaoCarregamentoInputsVideo;

        protected const string NOME_REGIAO_CARREGAMENTO_TEMPO_ACIONAMENTO = "regiao-tempo-acionamento";
        protected VisualElement regiaoCarregamentoTempoExibicao;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected const string NOME_REGIAO_CARREGAMENTO_TIPO_ACIONAMENTO = "regiao-carregamento-tipo-acionamento";
        protected VisualElement regiaoCarregamentoTipoAcionamento;

        protected const string NOME_REGIAO_OPCOES_AVANCADAS = "foldout-opcoes-avancadas";
        protected Foldout foldoutOpcoesAvancadas;

        protected InputTexto campoNome;

        protected InputImagem inputImagem;
        protected GrupoInputsAudio grupoInputsAudio;
        protected GrupoInputsTexto grupoInputsTexto;
        protected GrupoInputsVideo grupoInputsVideo;

        protected Dropdown dropdownTipoReforco;
        protected Dropdown dropdownTipoAcionamento;
        protected Dropdown dropdownTempoExibicao;

        protected GrupoInputsPosicao grupoInputsPosicao;
        protected GrupoInputsTamanho grupoInputsTamanho;

        protected BotoesConfirmacao botoesConfirmacao;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected InterrogacaoToolTip tooltipTitulo;

        #endregion

        protected ManipuladorReforco manipulador;

        protected Dictionary<string, TiposReforcos> associacaoValoresDropdownTipoReforocos;
        protected Dictionary<string, TipoAcionamentoReforco> associacoesOpcaoValorDropdown;
        protected Dictionary<string, float> associacoesTemposExibicao;

        public CriadorReforcoBehaviour() {
            manipulador = new ManipuladorReforco();
            manipulador.Criar();

            ConfigurarTooltipTitulo();
            ConfigurarCampoNome();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsAudio();
            CarregarRegiaoInputsTexto();
            CarregarRegiaoInputsVideo();
            ConfigurarCampoTipoReforco();
            ConfigurarCampoAcionamento();
            ConfigurarCampoTempoExibicao();
            ConfigurarRegiaoOpcoesAvancadas();
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
            campoNome.CampoTexto.RegisterCallback<ChangeEvent<string>>(evt => {
                manipulador.SetNome(evt.newValue);
            });

            regiaoCarregamenteInputNome = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_NOME);
            regiaoCarregamenteInputNome.Add(campoNome.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputsImagem() {
            inputImagem = new InputImagem();
            inputImagem.CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
                manipulador.ManipuladorComponenteSpriteRenderer.SetImagem(evt.newValue as Sprite);
            });
            
            regiaoCarregamentoInputsImagem = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM);
            regiaoCarregamentoInputsImagem.Add(inputImagem.Root);
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void CarregarRegiaoInputsAudio() {
            grupoInputsAudio = new GrupoInputsAudio();
            grupoInputsAudio.VincularDados(manipulador.ManipuladorComponenteAudioSource);

            regiaoCarregamentoInputsAudio = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO);
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsAudio.Add(grupoInputsAudio.Root);

            return;
        }

        protected virtual void CarregarRegiaoInputsTexto() {
            grupoInputsTexto = new GrupoInputsTexto();
            grupoInputsTexto.VincularDados(manipulador.ManipuladorComponenteTexto);

            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);
            regiaoCarregamentoInputsTexto.Add(grupoInputsTexto.Root);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void CarregarRegiaoInputsVideo() {
            grupoInputsVideo = new GrupoInputsVideo();
            grupoInputsVideo.VincularDados(manipulador.ManipuladorComponenteVideo);

            regiaoCarregamentoInputsVideo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_VIDEO);
            regiaoCarregamentoInputsVideo.Add(grupoInputsVideo.Root);
            regiaoCarregamentoInputsVideo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ConfigurarCampoTipoReforco() {
            associacaoValoresDropdownTipoReforocos = new() {
                { "Apresentar áudio", TiposReforcos.Audio },
                { "Apresentar Imagem", TiposReforcos.Imagem },
                { "Apresentar Texto", TiposReforcos.Texto },
                { "Apresentar Vídeo", TiposReforcos.Video },
            };

            List<string> opcoes = new();
            foreach(KeyValuePair<string, TiposReforcos> associacao in associacaoValoresDropdownTipoReforocos) {
                opcoes.Add(associacao.Key);
            }

            dropdownTipoReforco = new Dropdown("Tipo de reforço:", MENSAGEM_TOOLTIP_DROPDOWN_TIPO_REFORCO, opcoes);
            dropdownTipoReforco.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    manipulador.DesabilitarComponentes();
                    OcultarCampos();
                    return;
                }

                TiposReforcos novoTipo = associacaoValoresDropdownTipoReforocos[evt.newValue];
                
                manipulador.AlterarTipo(novoTipo);
                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
            });

            regiaoCarregamentoDropdownTipo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_DROPDOWN_TIPO);
            regiaoCarregamentoDropdownTipo.Add(dropdownTipoReforco.Root);

            return;
        }

        protected virtual void AlterarVisibilidadeCamposComBaseTipo(TiposReforcos tipo) {
            OcultarCampos();

            switch(tipo) {
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

        protected virtual void OcultarCampos() {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsVideo.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposAudio() {
            regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            regiaoCarregamentoTempoExibicao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsPosicao.Root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposImagem() {
            regiaoCarregamentoInputsImagem.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            regiaoCarregamentoTempoExibicao.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsPosicao.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposTexto() {
            regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            regiaoCarregamentoTempoExibicao.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsPosicao.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposVideo() {
            regiaoCarregamentoInputsVideo.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            regiaoCarregamentoTempoExibicao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsPosicao.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ConfigurarCampoAcionamento() {
            associacoesOpcaoValorDropdown = new() {
                { "Automático, em caso de acerto", TipoAcionamentoReforco.Acerto },
                { "Automático, em caso de erro", TipoAcionamentoReforco.Erro },
                { "Automático, no final da fase", TipoAcionamentoReforco.FimJogo },
            };

            List<string> opcoes = new();
            foreach(KeyValuePair<string, TipoAcionamentoReforco> associacao in associacoesOpcaoValorDropdown) {
                opcoes.Add(associacao.Key);
            }

            dropdownTipoAcionamento = new("Forma de acionamento:", MENSAGEM_TOOLTIP_DROPDOWN_TIPO_ACIONAMENTO, opcoes);
            dropdownTipoAcionamento.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                manipulador.SetTipoAcionamento(associacoesOpcaoValorDropdown[evt.newValue]);
            });

            regiaoCarregamentoTipoAcionamento = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TIPO_ACIONAMENTO);
            regiaoCarregamentoTipoAcionamento.Add(dropdownTipoAcionamento.Root);

            return;
        }

        protected virtual void ConfigurarRegiaoOpcoesAvancadas() {
            grupoInputsPosicao = new GrupoInputsPosicao();
            grupoInputsPosicao.VincularDados(manipulador);

            grupoInputsTamanho = new GrupoInputsTamanho();
            grupoInputsTamanho.VincularDados(manipulador);

            foldoutOpcoesAvancadas = root.Query<Foldout>(NOME_REGIAO_OPCOES_AVANCADAS);

            foldoutOpcoesAvancadas.Add(grupoInputsPosicao.Root);
            foldoutOpcoesAvancadas.Add(grupoInputsTamanho.Root);
            foldoutOpcoesAvancadas.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ConfigurarCampoTempoExibicao() {
            associacoesTemposExibicao = new() {
                { "5 segundos", 5f },
                { "7 segundos", 7f },
            };

            List<string> opcoes = new();
            foreach (KeyValuePair<string, float> associacao in associacoesTemposExibicao) {
                opcoes.Add(associacao.Key);
            }

            dropdownTempoExibicao = new Dropdown("Tempo de exibição:", MENSAGEM_TOOLTIP_DROPDOWN_TEMPO_EXIBICAO, opcoes);
            dropdownTempoExibicao.Root.RegisterCallback<ChangeEvent<string>>(evt => {
                float tempoEspera = associacoesTemposExibicao[evt.newValue];
                manipulador.SetTempoExibicao(tempoEspera);
            });

            regiaoCarregamentoTempoExibicao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TEMPO_ACIONAMENTO);
            regiaoCarregamentoTempoExibicao.Add(dropdownTempoExibicao.Root);
            regiaoCarregamentoTempoExibicao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

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

        public void ReiniciarCampos() {
            campoNome.ReiniciarCampos();

            inputImagem.ReiniciarCampos();
            grupoInputsAudio.ReiniciarCampos();
            grupoInputsTexto.ReiniciarCampos();
            grupoInputsVideo.ReiniciarCampos();

            dropdownTipoReforco.ReiniciarCampos();
            dropdownTipoAcionamento.ReiniciarCampos();
            dropdownTempoExibicao.ReiniciarCampos();

            grupoInputsPosicao.ReiniciarCampos();
            grupoInputsTamanho.ReiniciarCampos();

            OcultarCampos();

            return;
        }
    }
}