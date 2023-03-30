using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.Utils;
using Autis.Editor.UI;
using Autis.Editor.Telas;

namespace Autis.Editor.Criadores {
    public class CriadorPersonagemBehaviour : Criador {
        private const string CAMINHO_PREFAB_BONECO_PALITO = "Personagens/Boneco_Palito_Redondo.prefab";
        private const string CAMINHO_PREFAB_PERSONAGEM_LUDICO = "Personagens/Ludico.prefab";
        private const string CAMINHO_PREFAB_AVATAR = "Personagens/Avatar.prefab";

        private const TiposPersonagem TIPO_PADRAO = TiposPersonagem.BonecoPalito;

        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/CriadorPersonagemTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/CriadorPersonagemStyle.uss";

        #region .: Elementos :.

        private const string NOME_REGIAO_CONFIGURACAO_PERSONAGEM = "regiao-configurar-personagem";
        private readonly VisualElement regiaoConfigurarPersonagem;

        private const string NOME_REGIAO_AVISO_PERSONAGEM_JA_CRIADO = "regiao-aviso-personagem-ja-existente";
        private readonly VisualElement regiaoAvisoPersonagemJaCriado;

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_LABEL_TIPO_PERSONAGEM = "label-tipo-objeto-interacao";
        private const string NOME_ENUM_TIPO_PERSONAGEM = "input-tipo-objeto-interacao";
        private EnumField campoTipoPersonagem;

        private const string NOME_IMAGEM_PERSONAGEM = "imagem-personagem";
        private Image imagemPersonagem;

        private const string NOME_BOTAO_PERSONALIZAR_PERSONAGEM = "botao-personalizar-personagem";
        private Button botaoPersonalizarPersonagem;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TIPO_CONTROLE = "regiao-tooltip-forma-controle";
        private VisualElement regiaoCarregamentoTooltipTipoControle;

        private const string NOME_RADIO_OPCAO_CONTROLE_DIRETO = "radio-opcao-controle-direto";
        private RadioButton radioOpcaoControleDireto;

        private const string NOME_RADIO_OPCAO_CONTROLE_INDIRETO = "radio-opcao-controle-indireto";
        private RadioButton radioOpcaoControleIndireto;

        private const string NOME_BOTAO_CONFIGURAR_CONTROLE_INDIRETO = "botao-configurar-controle-indireto";
        private Button botaoConfigurarControleIndireto;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private VisualElement regiaoCarregamentoBotoesConfirmacao;

        private readonly InterrogacaoToolTip tooltipTitulo;
        private readonly InterrogacaoToolTip tooltipTipoControle;

        #endregion

        private readonly GameObject prefabBonecoPalito;
        private readonly GameObject prefabPersonagemLudico;
        private readonly GameObject prefabAvatar;

        private TiposPersonagem tipoPersonagemAtual;

        private IdentificadorTipoControle tipoControle;

        public CriadorPersonagemBehaviour() {
            regiaoConfigurarPersonagem = Root.Query<VisualElement>(NOME_REGIAO_CONFIGURACAO_PERSONAGEM);
            regiaoAvisoPersonagemJaCriado = Root.Query<VisualElement>(NOME_REGIAO_AVISO_PERSONAGEM_JA_CRIADO);

            bool jaCriouPersonagem = GameObject.FindGameObjectWithTag(NomesTags.Personagem) != null;
            if(jaCriouPersonagem) {
                regiaoAvisoPersonagemJaCriado.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                regiaoConfigurarPersonagem.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

                regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
                regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

                return;
            }

            regiaoConfigurarPersonagem.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoAvisoPersonagemJaCriado.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            tooltipTitulo = new InterrogacaoToolTip();
            tooltipTipoControle = new InterrogacaoToolTip();

            prefabBonecoPalito = Importador.ImportarPrefab(CAMINHO_PREFAB_BONECO_PALITO);
            prefabPersonagemLudico = Importador.ImportarPrefab(CAMINHO_PREFAB_PERSONAGEM_LUDICO);
            prefabAvatar = Importador.ImportarPrefab(CAMINHO_PREFAB_AVATAR);

            prefab = prefabBonecoPalito;
            tipoPersonagemAtual = TIPO_PADRAO;

            CarregarRegiaoHeader();
            CarregarTooltipTitulo();
            ConfigurarImagemPersonagem();
            ConfigurarBotaoPersonalizarPersonagem();
            CarregarTooltipTipoControle();
            ConfigurarCampoTipoPersonagem();
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

        private void CarregarTooltipTitulo() {
            regiaoCarregamentoTooltipTitulo = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);
            regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

            tooltipTitulo.SetTexto("Tipo especial de objeto interativo, que interage com outros objetos do jogo. Os personagens devem ter uma representação visual com perspectiva em primeira ou terceira pessoa, e sua movimentação deve ser controlada pelo usuário por meio de manipulação direta ou indireta (e.g., após a ação do usuário é exibidauma animação com o personagem). Opcionalmente, o personagem pode ter acapacidade de se comunicar por meio de texto ou áudio.");

            return;
        }

        private void CarregarTooltipTipoControle() {
            regiaoCarregamentoTooltipTipoControle = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TIPO_CONTROLE);
            regiaoCarregamentoTooltipTipoControle.Add(tooltipTipoControle.Root);

            tooltipTipoControle.SetTexto("Texto de teste.");

            return;
        }

        private void ConfigurarImagemPersonagem() {
            imagemPersonagem = Root.Query<Image>(NOME_IMAGEM_PERSONAGEM);
            imagemPersonagem.image = Importador.ImportarImagem("imagem-personagem.png");

            return;
        }

        private void ConfigurarBotaoPersonalizarPersonagem() {
            botaoPersonalizarPersonagem = Root.Query<Button>(NOME_BOTAO_PERSONALIZAR_PERSONAGEM);
            botaoPersonalizarPersonagem.clicked += HandleBotaoPersonalizarPersonagemClick;

            return;
        }

        private void HandleBotaoPersonalizarPersonagemClick() {
            switch(tipoPersonagemAtual) {
                case(TiposPersonagem.Avatar): {
                    Debug.Log("[TODO] Implementar");
                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    Navigator.Instance.IrPara(new PersonalizacaoBonecoPalitoBehaviour(novoObjeto));
                    break;
                }
                case(TiposPersonagem.Ludico): {
                    Debug.Log("[TODO] Implementar");
                    break;
                }
            }

            return;
        }

        private void ConfigurarCampoTipoPersonagem() {
            campoTipoPersonagem = Root.Query<EnumField>(NOME_ENUM_TIPO_PERSONAGEM);

            campoTipoPersonagem.labelElement.name = NOME_LABEL_TIPO_PERSONAGEM;
            campoTipoPersonagem.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoPersonagem.Init(TIPO_PADRAO);
            campoTipoPersonagem.SetValueWithoutNotify(TIPO_PADRAO);

            campoTipoPersonagem.RegisterCallback<ChangeEvent<Enum>>(evt => {
                TiposPersonagem tipoSelecionado = Enum.Parse<TiposPersonagem>(campoTipoPersonagem.value.ToString());
                if(tipoPersonagemAtual == tipoSelecionado) {
                    return;
                }

                tipoPersonagemAtual = tipoSelecionado;
                CancelarCriacao();

                switch(tipoPersonagemAtual) {
                    case(TiposPersonagem.Avatar): {
                        prefab = prefabAvatar;
                        break;
                    }
                    case(TiposPersonagem.BonecoPalito): {
                        prefab = prefabBonecoPalito;
                        break;
                    }
                    case(TiposPersonagem.Ludico): {
                        prefab = prefabPersonagemLudico;
                        break;
                    }
                }

                IniciarCriacao();
            });

            return;
        }

        private void ConfigurarRadioButtons() {
            radioOpcaoControleDireto = Root.Query<RadioButton>(NOME_RADIO_OPCAO_CONTROLE_DIRETO);
            radioOpcaoControleDireto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            radioOpcaoControleDireto.SetValueWithoutNotify(true);
            radioOpcaoControleDireto.RegisterValueChangedCallback(evt => {
                if(!evt.newValue) {
                    return;
                }

                tipoControle.AlterarTipo(TipoControle.Direto);
                botaoConfigurarControleIndireto.SetEnabled(false);
            });

            radioOpcaoControleIndireto = Root.Query<RadioButton>(NOME_RADIO_OPCAO_CONTROLE_INDIRETO);
            radioOpcaoControleIndireto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            radioOpcaoControleIndireto.SetValueWithoutNotify(false);
            radioOpcaoControleIndireto.RegisterValueChangedCallback(evt => {
                if(!evt.newValue) {
                    return;
                }

                tipoControle.AlterarTipo(TipoControle.Indireto);
                botaoConfigurarControleIndireto.SetEnabled(true);
            });

            botaoConfigurarControleIndireto = Root.Query<Button>(NOME_BOTAO_CONFIGURAR_CONTROLE_INDIRETO);
            botaoConfigurarControleIndireto.SetEnabled(false);
            botaoConfigurarControleIndireto.clicked += HandleBotaoConfigurarControleIndiretoClick;

            return;
        }

        private void HandleBotaoConfigurarControleIndiretoClick() {
            Navigator.Instance.IrPara(new ConfigurarControleIndiretoBehaviour());
            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            return;
        }

        protected override void HandleBotaoConfirmarClick() {
            RuntimeAnimatorController controller = null;
            switch(tipoPersonagemAtual) {
                case(TiposPersonagem.Avatar): {
                    controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar, "ControllerAvatar.controller"));
                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito, "ControllerPersonagemPalito.controller"));
                    break;

                }
                case(TiposPersonagem.Ludico): {
                    controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico, "ControllerPersonagemLudico.controller"));
                    break;
                }
            }

            novoObjeto.GetComponent<Animator>().runtimeAnimatorController = controller;

            base.HandleBotaoConfirmarClick();

            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            tipoControle = novoObjeto.GetComponent<IdentificadorTipoControle>();
            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            tipoControle = null;
            return;
        }

        public override void ReiniciarCampos() {
            campoTipoPersonagem.SetValueWithoutNotify(TiposPersonagem.BonecoPalito);
            campoTipoPersonagem.SendEvent(new ChangeEvent<TiposPersonagem>());

            radioOpcaoControleDireto.SetValueWithoutNotify(true);
            radioOpcaoControleDireto.SendEvent(new ChangeEvent<bool>());

            radioOpcaoControleIndireto.SetValueWithoutNotify(false);
            radioOpcaoControleIndireto.SendEvent(new ChangeEvent<bool>());
            return;
        }

        public override void FinalizarCriacao() {
            novoObjeto.tag = NomesTags.Personagem;
            novoObjeto.layer = LayersProjeto.Default.Index;

            SpriteRenderer[] spriteRenderers = novoObjeto.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.sortingOrder = OrdemRenderizacao.Personagem;
            }
            
            base.FinalizarCriacao();

            return;
        }
    }
}