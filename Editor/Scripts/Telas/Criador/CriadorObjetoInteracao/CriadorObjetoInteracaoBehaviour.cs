using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.Manipuladores;
using Object = UnityEngine.Object;

namespace Autis.Editor.Criadores {
    public class CriadorObjetoInteracaoBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorObjetoInteracao/CriadorObjetoInteracaoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorObjetoInteracao/CriadorObjetoInteracaoStyle.uss";

        public Action<GameObject> OnFinalizarCriacao { get; set; }

        #region .: Mensagens :.

        private const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_OBJETO_INTERACAO = "[TODO]: Adicionar";
        private const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_ACOES = "[TODO]: Adicionar";

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

        protected const string NOME_CHECKBOX_DESFAZER_ACAO = "input-desfazer-acao";
        protected Toggle checkboxDesfazerAcao;

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

        #endregion

        protected Dictionary<string, TiposAcoes> associacaoValoresDropdownTipoAcoes;
        private readonly TiposObjetosInteracao tipoPadrao = TiposObjetosInteracao.Imagem;

        protected ManipuladorObjetoInteracao manipulador;

        public CriadorObjetoInteracaoBehaviour() {
            manipulador = new ManipuladorObjetoInteracao();
            manipulador.Criar();

            ConfigurarCampoNome();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsTexto();

            ConfigurarCampoTipoObjetoInteracao();
            ConfigurarCampoTipoAcao();
            ConfigurarCheckboxDesfazerAcao();
            ConfigurarRegiaoOpcoesAvancadas();

            ConfigurarBotoesConfirmacao();

            manipulador.AlterarTipo(tipoPadrao);

            return;
        }

        protected virtual void ConfigurarCampoNome() {
            campoNome = new InputTexto("Nome:");

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
                AlterarVisibilidadeCamposComBaseTipoAcao(associacaoValoresDropdownTipoAcoes[evt.newValue]);
            });

            regiaoCarregamentoTipoAcao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TIPO_ACAO);
            regiaoCarregamentoTipoAcao.Add(dropdownTiposAcoes.Root);

            return;
        }

        protected virtual void AlterarVisibilidadeCamposComBaseTipoAcao(TiposAcoes tipo) {
            if(tipo == TiposAcoes.Arrastavel) {
                checkboxDesfazerAcao.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                return;
            }

            checkboxDesfazerAcao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            return;
        }

        protected virtual void ConfigurarCheckboxDesfazerAcao() {
            checkboxDesfazerAcao = root.Query<Toggle>(NOME_CHECKBOX_DESFAZER_ACAO);

            checkboxDesfazerAcao.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            checkboxDesfazerAcao.RegisterCallback<ChangeEvent<bool>>(evt => {
                manipulador.SetDeveDesfazerAcao(evt.newValue);
            });

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
            OnFinalizarCriacao?.Invoke(manipulador.ObjetoAtual);
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