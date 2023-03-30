using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Autis.Runtime.Constantes;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Telas;

namespace Autis.Editor.Criadores {
    public class CriadorObjetoInteracaoBehaviour : Criador {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorObjetoInteracao/CriadorObjetoInteracaoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorObjetoInteracao/CriadorObjetoInteracaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_LABEL_TIPO_OBJETO_INTERACAO = "label-tipo-obejto-interacao";
        private const string NOME_INPUT_TIPO_OBJETO_INTERACAO = "input-tipo-obejto-interacao";
        private EnumField campoTipoObjetoInteracao;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM = "regiao-carregamento-inputs-imagem";
        private VisualElement regiaoCarregamentoInputsImagem;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        private VisualElement regiaoCarregamentoInputsTexto;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO = "regiao-carregamento-inputs-tipo-acao";
        private VisualElement regiaoCarregamentoInputsTipoAcao;

        private const string NOME_RADIO_BUTTON_OPCAO_SIM = "radio-opcao-sim";
        private RadioButton radioOpcaoSim;

        private const string NOME_RADIO_BUTTON_OPCAO_NAO = "radio-opcao-nao";
        private RadioButton radioOpcaoNao;

        private const string NOME_BOTAO_COFIGURAR_APOIO_OBJETO_INTERACAO = "botao-configurar-apoio";
        private Button botaoConfigurarApoioObjetoInteracao;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly InputsComponenteImagem grupoInputsImagem;
        private readonly InputsComponenteTexto grupoInputsTexto;
        private readonly InputsIdentificadorTipoAcao grupoInputsTipoAcao;

        #endregion

        private SpriteRenderer sprite;
        private Texto texto;
        private IdentificadorTipoAcao identificadorTipoAcao;

        private readonly TiposObjetosInteracao tipoPadrao = TiposObjetosInteracao.Imagem;

        public CriadorObjetoInteracaoBehaviour() {
            grupoInputsImagem = new InputsComponenteImagem();
            grupoInputsTexto = new InputsComponenteTexto();
            grupoInputsTipoAcao = new InputsIdentificadorTipoAcao();

            ImportarPrefab("ObjetosInteracao/ObjetoInteracao.prefab");

            CarregarRegiaoHeader();
            CarregarRegiaoInputsImagem();
            CarregarRegiaoInputsTexto();

            ConfigurarCampoTipoObjetoInteracao();
            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            ConfigurarCampoTipoAcao();
            ConfigurarRadioButtons();

            ConfigurarBotoesConfirmacao();

            IniciarCriacao();

            return;
        }

        private void CarregarRegiaoHeader() {
            regiaoCarregamentoHeader = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_HEADER);
            regiaoCarregamentoHeader.Add(header.Root);

            return;
        }

        private void CarregarRegiaoInputsImagem() {
            regiaoCarregamentoInputsImagem = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_IMAGEM);
            regiaoCarregamentoInputsImagem.Add(grupoInputsImagem.Root);

            return;
        }

        private void CarregarRegiaoInputsTexto() {
            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);
            regiaoCarregamentoInputsTexto.Add(grupoInputsTexto.Root);

            return;
        }

        private void ConfigurarCampoTipoObjetoInteracao() {
            campoTipoObjetoInteracao = Root.Query<EnumField>(NOME_INPUT_TIPO_OBJETO_INTERACAO);

            campoTipoObjetoInteracao.labelElement.name = NOME_LABEL_TIPO_OBJETO_INTERACAO;
            campoTipoObjetoInteracao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoObjetoInteracao.Init(TiposObjetosInteracao.Imagem);
            campoTipoObjetoInteracao.SetValueWithoutNotify(TiposObjetosInteracao.Imagem);

            campoTipoObjetoInteracao.RegisterCallback<ChangeEvent<Enum>>(evt => {
                TiposObjetosInteracao novoTipo = Enum.Parse<TiposObjetosInteracao>(campoTipoObjetoInteracao.value.ToString());
                IdentificadorTipoObjetoInteracao tipoNovoObjeto = novoObjeto.GetComponent<IdentificadorTipoObjetoInteracao>();

                tipoNovoObjeto.AlterarTipo(novoTipo);
                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
            });

            return;
        }

        private void AlterarVisibilidadeCamposComBaseTipo(TiposObjetosInteracao tipo) {
            regiaoCarregamentoInputsImagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            switch(tipo) {
                case(TiposObjetosInteracao.Imagem): {
                    regiaoCarregamentoInputsImagem.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
                case(TiposObjetosInteracao.Texto): {
                    regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                    break;
                }
            }

            return;
        }

        private void ConfigurarCampoTipoAcao() {
            regiaoCarregamentoInputsTipoAcao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TIPO_ACAO);
            regiaoCarregamentoInputsTipoAcao.Add(grupoInputsTipoAcao.Root);

            return;
        }

        private void ConfigurarRadioButtons() {
            radioOpcaoSim = Root.Query<RadioButton>(NOME_RADIO_BUTTON_OPCAO_SIM);
            radioOpcaoSim.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            radioOpcaoSim.SetValueWithoutNotify(false);
            radioOpcaoSim.RegisterValueChangedCallback(evt => {
                if(!evt.newValue) {
                    return;
                }

                botaoConfigurarApoioObjetoInteracao.SetEnabled(true);
            });

            radioOpcaoNao = Root.Query<RadioButton>(NOME_RADIO_BUTTON_OPCAO_NAO);
            radioOpcaoNao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            radioOpcaoNao.SetValueWithoutNotify(true);
            radioOpcaoNao.RegisterValueChangedCallback(evt => {
                if(!evt.newValue) {
                    return;
                }

                botaoConfigurarApoioObjetoInteracao.SetEnabled(false);
            });

            botaoConfigurarApoioObjetoInteracao = Root.Query<Button>(NOME_BOTAO_COFIGURAR_APOIO_OBJETO_INTERACAO);
            botaoConfigurarApoioObjetoInteracao.SetEnabled(false);
            botaoConfigurarApoioObjetoInteracao.clicked += HandleBotaoConfigurarApoioObjetoInteracaoClick;

            return;
        }

        private void HandleBotaoConfigurarApoioObjetoInteracaoClick() {
            Navigator.Instance.IrPara(new CriadorApoioObjetoInteracaoBehaviour(novoObjeto));
            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            sprite = novoObjeto.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = OrdemRenderizacao.EmCriacao;
            grupoInputsImagem.VincularDados(sprite);

            texto = novoObjeto.GetComponent<Texto>();
            grupoInputsTexto.VincularDados(texto);

            IdentificadorTipoObjetoInteracao tipoObjetoInteracao = novoObjeto.GetComponent<IdentificadorTipoObjetoInteracao>();
            tipoObjetoInteracao.AlterarTipo(tipoPadrao);

            identificadorTipoAcao = novoObjeto.GetComponent<IdentificadorTipoAcao>();
            grupoInputsTipoAcao.VincularDados(identificadorTipoAcao);

            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.ObjetosInteracao;
            novoObjeto.layer = LayersProjeto.Default.Index;
            sprite.sortingOrder = OrdemRenderizacao.ObjetoInteracao;

            base.FinalizarCriacao();

            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            sprite = null;
            texto = null;
            identificadorTipoAcao = null;

            return;
        }

        public override void ReiniciarCampos() {
            grupoInputsImagem.ReiniciarCampos();
            grupoInputsTexto.ReiniciarCampos();

            campoTipoObjetoInteracao.SetValueWithoutNotify(tipoPadrao);
            AlterarVisibilidadeCamposComBaseTipo(tipoPadrao);

            grupoInputsTipoAcao.ReiniciarCampos();

            radioOpcaoSim.SetValueWithoutNotify(false);
            radioOpcaoNao.SetValueWithoutNotify(true);
            botaoConfigurarApoioObjetoInteracao.SetEnabled(false);

            return;
        }
    }
}