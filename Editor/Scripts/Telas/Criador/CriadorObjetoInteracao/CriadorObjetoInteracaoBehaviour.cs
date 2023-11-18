using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using UnityEditor;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.Manipuladores;
using Autis.Editor.Excecoes;
using Autis.Runtime.Eventos;
using Autis.Editor.Utils;

namespace Autis.Editor.Criadores {
    public class CriadorObjetoInteracaoBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorObjetoInteracao/CriadorObjetoInteracaoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorObjetoInteracao/CriadorObjetoInteracaoStyle.uss";

        public Action<GameObject> OnFinalizarCriacao { get; set; }

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_TITULO = "Elemento (imagem ou texto) que podem ser adicionados na fase do jogo. Um elemento poderá ser estático OU o usuário poderá interagir com ele.";
        protected const string MENSAGEM_TOOLTIP_INPUT_NOME = "Digite um nome para o Elemento. Cada componente deve ter um nome exclusivo (que não se repete em outro componente)";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_OBJETO_INTERACAO = "Forma que o Elemento será representado na fase.";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_ACOES = "Forma que o jogador poderá interagir com o Elemento durante o jogo.";

        #endregion

        #region .: Eventos :.

        protected static EventoJogo eventoFinalizarCriacao;

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_NOME = "regiao-input-nome";
        protected VisualElement regiaoCarregamenteInputNome;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_TIPO = "regiao-carregamento-input-tipo";
        protected VisualElement regiaoCarregamentoInputTipo;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        protected VisualElement regiaoCarregamentoInputsImagem;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        protected VisualElement regiaoCarregamentoInputsTexto;

        protected const string NOME_REGIAO_CARREGAMENTO_TIPO_ACAO = "regiao-carregamento-tipo-acao";
        protected VisualElement regiaoCarregamentoTipoAcao;

        protected const string NOME_REGIAO_OPCOES_AVANCADAS = "foldout-opcoes-avancadas";
        protected Foldout foldoutOpcoesAvancadas;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InputTexto campoNome;

        protected InputImagem inputsImagem;
        protected GrupoInputsTexto grupoInputsTexto;

        protected Dropdown dropdownTipo;
        protected Dropdown dropdownTiposAcoes;

        protected GrupoInputsPosicao grupoInputsPosicao;
        protected GrupoInputsTamanho grupoInputsTamanho;

        protected BotoesConfirmacao botoesConfirmacao;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected InterrogacaoToolTip tooltipTitulo;

        #endregion

        protected Dictionary<string, TiposAcoes> associacaoValoresDropdownTipoAcoes;
        private readonly TiposObjetosInteracao tipoPadrao = TiposObjetosInteracao.Imagem;

        protected ManipuladorObjetoInteracao manipulador;

        public CriadorObjetoInteracaoBehaviour() {
            manipulador = new ManipuladorObjetoInteracao();
            manipulador.Criar();

            eventoFinalizarCriacao = Importador.ImportarEvento("EventoFinalizarCriacao");

            ConfigurarTooltipTitulo();
            ConfigurarCampoNome();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsTexto();

            ConfigurarCampoTipoObjetoInteracao();
            ConfigurarCampoTipoAcao();
            ConfigurarRegiaoOpcoesAvancadas();

            ConfigurarBotoesConfirmacao();

            manipulador.AlterarTipo(tipoPadrao);

            return;
        }

        public override void OnEditorUpdate() {
            DefinirFerramenta();

            if(Selection.activeObject != manipulador?.ObjetoAtual) {
                Selection.activeObject = manipulador?.ObjetoAtual;
            }

            return;
        }

        private void DefinirFerramenta() {
            if(manipulador?.ObjetoAtual && manipulador?.GetTipo() == TiposObjetosInteracao.Texto) {
                if(Tools.current != Tool.Move) {
                    Tools.current = Tool.Move;
                }

                return;
            }

            if (Tools.current != Tool.Rect) {
                Tools.current = Tool.Rect;
                return;
            }

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

        protected virtual void CarregarRegiaoInputsImagem() {
            inputsImagem = new InputImagem();
            inputsImagem.CampoImagem.RegisterCallback<ChangeEvent<Object>>(evt => {
                manipulador.ManipuladorSpriteRenderer.SetImagem(evt.newValue as Sprite);
            });

            regiaoCarregamentoInputsImagem = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM);
            regiaoCarregamentoInputsImagem.Add(inputsImagem.Root);
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void CarregarRegiaoInputsTexto() {
            grupoInputsTexto = new GrupoInputsTexto();
            grupoInputsTexto.VincularDados(manipulador.ManipuladorTexto);

            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);
            regiaoCarregamentoInputsTexto.Add(grupoInputsTexto.Root);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ConfigurarCampoTipoObjetoInteracao() {
            List<string> opcoes = new() {
                TiposObjetosInteracao.Imagem.ToString(),
                TiposObjetosInteracao.Texto.ToString(),
            };

            dropdownTipo = new Dropdown("Representação Visual:", MENSAGEM_TOOLTIP_DROPDOWN_TIPO_OBJETO_INTERACAO, opcoes );
            dropdownTipo.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    OcultarCampos();
                    manipulador.DesabilitarComponentes();
                    return;
                }

                TiposObjetosInteracao novoTipo = Enum.Parse<TiposObjetosInteracao>(evt.newValue);

                manipulador.AlterarTipo(novoTipo);
                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
            });

            regiaoCarregamentoInputTipo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_TIPO);
            regiaoCarregamentoInputTipo.Add(dropdownTipo.Root);

            return;
        }

        protected virtual void AlterarVisibilidadeCamposComBaseTipo(TiposObjetosInteracao tipo) {
            ReiniciarAtributos();
            OcultarCampos();

            switch(tipo) {
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

        protected void OcultarCampos() {
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected void ExibirCamposImagem() {
            regiaoCarregamentoInputsImagem.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            foldoutOpcoesAvancadas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            grupoInputsPosicao.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected void ExibirCamposTexto() {
            regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            foldoutOpcoesAvancadas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            grupoInputsPosicao.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ConfigurarCampoTipoAcao() {
            associacaoValoresDropdownTipoAcoes = new() {
                { "Arrastar", TiposAcoes.Arrastavel },
                { "Selecionar", TiposAcoes.Selecionavel },
            };

            List<string> opcoes = new();
            foreach(KeyValuePair<string, TiposAcoes> associacao in associacaoValoresDropdownTipoAcoes) {
                opcoes.Add(associacao.Key);
            }

            dropdownTiposAcoes = new Dropdown("Ação (opcional)", MENSAGEM_TOOLTIP_DROPDOWN_TIPO_ACOES, opcoes);
            dropdownTiposAcoes.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    manipulador.SetTipoInteracao(TiposAcoes.Nenhuma);
                    return;
                }

                manipulador.SetTipoInteracao(associacaoValoresDropdownTipoAcoes[evt.newValue]);
            });

            regiaoCarregamentoTipoAcao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TIPO_ACAO);
            regiaoCarregamentoTipoAcao.Add(dropdownTiposAcoes.Root);

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

        protected virtual void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        protected virtual void HandleBotaoConfirmarClick() {
            try {
                VerificarCamposObrigatorios();
            }
            catch(ExcecaoCamposObrigatoriosVazios excecao) {
                PopupAvisoBehaviour.ShowPopupAviso(excecao.Message);
                return;
            }

            GameObject objetoCriado;

            try {
                objetoCriado = manipulador.ObjetoAtual;
                manipulador.Finalizar();
            }
            catch(ExcecaoObjetoDuplicado excecao) {
                PopupAvisoBehaviour.ShowPopupAviso(MensagensGerais.MENSAGEM_ATOR_DUPLICADO.Replace("{nome}", excecao.NomeObjetoDuplicado));
                return;
            }

            OnFinalizarCriacao?.Invoke(objetoCriado);
            eventoFinalizarCriacao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }

        protected virtual void VerificarCamposObrigatorios() {
            string mensagem = string.Empty;

            if(campoNome.EstaVazio()) {
                mensagem += MensagensGerais.MENSAGEM_ERRO_CAMPO_NAO_PREENCHIDO.Replace("{nome_campo}", campoNome.LabelCampoTexto.text);
            }

            if(dropdownTipo.EstaVazio()) {
                mensagem += MensagensGerais.MENSAGEM_ERRO_CAMPO_NAO_PREENCHIDO.Replace("{nome_campo}", dropdownTipo.Label.text);
                throw new ExcecaoCamposObrigatoriosVazios(mensagem);
            }

            if(manipulador.GetTipo() == TiposObjetosInteracao.Imagem) {
                mensagem += VerificarCamposObrigatoriosImagem();
            }

            if(mensagem != string.Empty) {
                throw new ExcecaoCamposObrigatoriosVazios(mensagem);
            }

            return;
        }

        protected virtual string VerificarCamposObrigatoriosImagem() {
            string mensagem = string.Empty;
            
            if(inputsImagem.EstaVazio()) {
                mensagem += MensagensGerais.MENSAGEM_ERRO_CAMPO_ARQUIVO_IMAGEM_NAO_PREENCHIDO;
            }

            return mensagem;
        }

        protected virtual void HandleBotaoCancelarClick() {
            manipulador.Cancelar();

            eventoFinalizarCriacao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }

        public void ReiniciarCampos() {
            campoNome.ReiniciarCampos();
            inputsImagem.ReiniciarCampos();
            grupoInputsTexto.ReiniciarCampos();

            dropdownTiposAcoes.ReiniciarCampos();
            dropdownTipo.ReiniciarCampos();

            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            return;
        }

        protected virtual void ReiniciarAtributos() {
            grupoInputsPosicao.ReiniciarCampos();
            manipulador.SetPosicao(Vector3.zero);

            grupoInputsTamanho.ReiniciarCampos();
            manipulador.SetTamanho(1);

            return;
        }
    }
}