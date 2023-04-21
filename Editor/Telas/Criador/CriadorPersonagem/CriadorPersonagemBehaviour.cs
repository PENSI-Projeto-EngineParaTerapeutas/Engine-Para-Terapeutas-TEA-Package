using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.Utils;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.DTOs;

namespace Autis.Editor.Criadores {
    public class CriadorPersonagemBehaviour : Criador {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/CriadorPersonagemTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/CriadorPersonagemStyle.uss";
        
        private const string CAMINHO_PREFAB_BONECO_PALITO = "Personagens/Boneco_Palito_Redondo.prefab";
        private const string CAMINHO_PREFAB_PERSONAGEM_LUDICO = "Personagens/Ludico.prefab";
        private const string CAMINHO_PREFAB_AVATAR = "Personagens/Avatar.prefab";

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM = "[ERROR]: Não foi possível achar a referência para o Controlador de Animação para o Personagem. Certifique-se de que o arquivo {nome-controller} ControllerAvatar.controller está localizado em {local}.";

        private const string TOOLTIP_TITULO = "Tipo especial de objeto interativo, que interage com outros objetos do jogo. Os personagens devem ter uma representação visual com perspectiva em primeira ou terceira pessoa, e sua movimentação deve ser controlada pelo usuário por meio de manipulação direta ou indireta (e.g., após a ação do usuário é exibidauma animação com o personagem). Opcionalmente, o personagem pode ter acapacidade de se comunicar por meio de texto ou áudio.";
        private const string TOOLTIP_TIPO_CONTROLE = "Texto de teste.";
        private const string TOOLTIP_TIPO_PERSONAGEM = "Define o tipo de Personagem";

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_HEADER = "regiao-carregamento-header";
        private VisualElement regiaoCarregamentoHeader;

        private const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        private VisualElement regiaoCarregamentoTooltipTitulo;

        private const string NOME_REIGAO_CARREGAMENTO_DROPDOWN_TIPO_PERSONAGEM = "regiao-carregamento-tipo-personagem";
        private VisualElement regiaoCarregamentoDropdownTipoPersonagem;

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

        private Dropdown dropdownTipoPersonagem;

        #endregion

        private readonly GameObject prefabBonecoPalito;
        private readonly GameObject prefabPersonagemLudico;
        private readonly GameObject prefabAvatar;

        private TiposPersonagem tipoPersonagemAtual = TiposPersonagem.Nenhum;

        private IdentificadorTipoControle tipoControle;

        private readonly List<AcaoPersonagem> associacoesAcoes;

        public CriadorPersonagemBehaviour() {
            tooltipTitulo = new InterrogacaoToolTip();
            tooltipTipoControle = new InterrogacaoToolTip();
            associacoesAcoes = new List<AcaoPersonagem>();

            prefabBonecoPalito = Importador.ImportarPrefab(CAMINHO_PREFAB_BONECO_PALITO);
            prefabPersonagemLudico = Importador.ImportarPrefab(CAMINHO_PREFAB_PERSONAGEM_LUDICO);
            prefabAvatar = Importador.ImportarPrefab(CAMINHO_PREFAB_AVATAR);

            CarregarRegiaoHeader();
            CarregarTooltipTitulo();
            ConfigurarDropdownTipoPersonagem();
            ConfigurarImagemPersonagem();
            ConfigurarBotaoPersonalizarPersonagem();
            CarregarTooltipTipoControle();
            ConfigurarRadioButtons();
            ConfigurarBotoesConfirmacao();

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

            tooltipTitulo.SetTexto(TOOLTIP_TITULO);

            return;
        }

        private void CarregarTooltipTipoControle() {
            regiaoCarregamentoTooltipTipoControle = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TIPO_CONTROLE);
            regiaoCarregamentoTooltipTipoControle.Add(tooltipTipoControle.Root);

            tooltipTipoControle.SetTexto(TOOLTIP_TIPO_CONTROLE);

            return;
        }

        private void ConfigurarDropdownTipoPersonagem() {
            List<string> tiposPersonagem = new() {
                TiposPersonagem.Avatar.ToString(),
                TiposPersonagem.BonecoPalito.ToString(),
                TiposPersonagem.Ludico.ToString(),
            };
            dropdownTipoPersonagem = new Dropdown("Tipo de personagem:", TOOLTIP_TIPO_PERSONAGEM, tiposPersonagem);

            dropdownTipoPersonagem.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    CancelarCriacao();

                    tipoPersonagemAtual = TiposPersonagem.Nenhum;

                    radioOpcaoControleDireto.SetValueWithoutNotify(false);
                    radioOpcaoControleIndireto.SetValueWithoutNotify(false);

                    botaoPersonalizarPersonagem.SetEnabled(false);
                    radioOpcaoControleDireto.SetEnabled(false);
                    radioOpcaoControleIndireto.SetEnabled(false);
                    botaoConfigurarControleIndireto.SetEnabled(false);
                    botoesConfirmacao.BotaoConfirmar.SetEnabled(false);

                    return;
                }

                TiposPersonagem tipoSelecionado = Enum.Parse<TiposPersonagem>(evt.newValue);
                if(tipoPersonagemAtual == tipoSelecionado) {
                    return;
                }

                tipoPersonagemAtual = tipoSelecionado;
                botaoPersonalizarPersonagem.SetEnabled(true);

                CancelarCriacao();

                switch(tipoPersonagemAtual) {
                    case(TiposPersonagem.Avatar): {
                        prefab = prefabAvatar;

                        radioOpcaoControleDireto.SetValueWithoutNotify(false);
                        radioOpcaoControleIndireto.SetValueWithoutNotify(false);

                        radioOpcaoControleDireto.SetEnabled(true);
                        radioOpcaoControleIndireto.SetEnabled(true);
                        botaoConfigurarControleIndireto.SetEnabled(false);
                        botoesConfirmacao.BotaoConfirmar.SetEnabled(true);

                        break;
                    }
                    case(TiposPersonagem.BonecoPalito): {
                        prefab = prefabBonecoPalito;

                        radioOpcaoControleDireto.SetValueWithoutNotify(false);
                        radioOpcaoControleIndireto.SetValueWithoutNotify(false);

                        radioOpcaoControleDireto.SetEnabled(true);
                        radioOpcaoControleIndireto.SetEnabled(true);
                        botaoConfigurarControleIndireto.SetEnabled(false);
                        botoesConfirmacao.BotaoConfirmar.SetEnabled(true);

                        break;
                    }
                    case(TiposPersonagem.Ludico): {
                        prefab = prefabPersonagemLudico;

                        radioOpcaoControleDireto.SetValueWithoutNotify(false);
                        radioOpcaoControleIndireto.SetValueWithoutNotify(false);

                        radioOpcaoControleDireto.SetEnabled(false);
                        radioOpcaoControleIndireto.SetEnabled(false);
                        botaoConfigurarControleIndireto.SetEnabled(false);
                        botoesConfirmacao.BotaoConfirmar.SetEnabled(false);

                        break;
                    }
                }

                IniciarCriacao();
            });

            regiaoCarregamentoDropdownTipoPersonagem = Root.Query<VisualElement>(NOME_REIGAO_CARREGAMENTO_DROPDOWN_TIPO_PERSONAGEM);
            regiaoCarregamentoDropdownTipoPersonagem.Add(dropdownTipoPersonagem.Root);

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
            botaoPersonalizarPersonagem.SetEnabled(false);

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
                    Navigator.Instance.IrPara(new PersonalizarLudicoBehaviour(prefab, ReiniciarCriacaoComPrefab));
                    break;
                }
            }

            return;
        }

        private void ReiniciarCriacaoComPrefab(GameObject novoPrefab) {
            CancelarCriacao();

            radioOpcaoControleDireto.SetValueWithoutNotify(false);
            radioOpcaoControleIndireto.SetValueWithoutNotify(false);

            radioOpcaoControleDireto.SetEnabled(true);
            radioOpcaoControleIndireto.SetEnabled(true);
            botoesConfirmacao.BotaoConfirmar.SetEnabled(true);
            botaoConfigurarControleIndireto.SetEnabled(false);

            prefab = novoPrefab;

            IniciarCriacao();

            return;
        }

        private void ConfigurarRadioButtons() {
            radioOpcaoControleDireto = Root.Query<RadioButton>(NOME_RADIO_OPCAO_CONTROLE_DIRETO);
            radioOpcaoControleDireto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            radioOpcaoControleDireto.SetValueWithoutNotify(false);
            radioOpcaoControleDireto.RegisterValueChangedCallback(evt => {
                if(!evt.newValue) {
                    return;
                }

                tipoControle.AlterarTipo(TipoControle.Direto);
                botaoConfigurarControleIndireto.SetEnabled(false);
            });
            radioOpcaoControleDireto.SetEnabled(false);

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
            radioOpcaoControleIndireto.SetEnabled(false);

            botaoConfigurarControleIndireto = Root.Query<Button>(NOME_BOTAO_CONFIGURAR_CONTROLE_INDIRETO);
            botaoConfigurarControleIndireto.clicked += HandleBotaoConfigurarControleIndiretoClick;
            botaoConfigurarControleIndireto.SetEnabled(false);

            return;
        }

        private void HandleBotaoConfigurarControleIndiretoClick() {
            Navigator.Instance.IrPara(new ConfigurarControleIndiretoBehaviour(novoObjeto, tipoPersonagemAtual, associacoesAcoes));
            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            regiaoCarregamentoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            botoesConfirmacao.BotaoConfirmar.SetEnabled(false);
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            return;
        }

        protected override void HandleBotaoConfirmarClick() {
            CarregarControllerPersonagem();
            AtribuirVinculoAcoesControleIndireto();

            base.HandleBotaoConfirmarClick();

            return;
        }

        private void CarregarControllerPersonagem() {
            RuntimeAnimatorController controller = null;

            switch(tipoPersonagemAtual) {
                case(TiposPersonagem.Avatar): {
                    controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar, "ControllerAvatar.controller"));

                    if(controller == null) {
                        Debug.LogError(MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM.Replace("{nome-controller}", "ControllerAvatar.controller").Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar));
                    }

                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito, "ControllerPersonagemPalito.controller"));

                    if(controller == null) {
                        Debug.LogError(MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM.Replace("{nome-controller}", "ControllerPersonagemPalito.controller").Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito));
                    }

                    break;
                }
                case(TiposPersonagem.Ludico): {
                    string nomeTipoPersonagemLudico = novoObjeto.GetComponent<IdentificadorTipoPersonagemLudico>().tipoPersonagenLudicos.ToString();
                    controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico, nomeTipoPersonagemLudico, "ControllerPersonagemLudico.controller"));

                    if(controller == null) {
                        Debug.LogError(MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM.Replace("{nome-controller}", "ControllerPersonagemLudico.controller").Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico + nomeTipoPersonagemLudico));
                    }

                    break;
                }
            }

            novoObjeto.GetComponent<Animator>().runtimeAnimatorController = controller;

            return;
        }


        private void AtribuirVinculoAcoesControleIndireto() {
            if(radioOpcaoControleIndireto.value == false) {
                return;
            }

            foreach(AcaoPersonagem vinculoAcao in associacoesAcoes) {
                AcionadorAcaoPersonagem acionador = vinculoAcao.ObjetoGatilho.GetComponent<AcionadorAcaoPersonagem>();
                acionador.animacaoAcionada = vinculoAcao.Animacao;
            }

            return;
        }

        protected override void HandleBotaoCancelarClick() {
            CancelarCriacao();
            ReiniciarCampos();

            return;
        }

        public override void CancelarCriacao() {
            header.ReiniciarCampos();

            ReiniciarPropriedadesNovoObjeto();
            associacoesAcoes.Clear();

            if(novoObjeto != null) {
                GameObject.DestroyImmediate(novoObjeto);
            }

            novoObjeto = null;
            prefab = null;

            return;
        }

        protected override void ReiniciarPropriedadesNovoObjeto() {
            tipoControle = null;
            return;
        }

        protected override void VincularCamposAoNovoObjeto() {
            tipoControle = novoObjeto.GetComponent<IdentificadorTipoControle>();
            return;
        }

        public override void ReiniciarCampos() {
            tipoPersonagemAtual = TiposPersonagem.Nenhum;

            dropdownTipoPersonagem.ReiniciarCampos();
            botaoPersonalizarPersonagem.SetEnabled(false);

            radioOpcaoControleDireto.SetValueWithoutNotify(false);
            radioOpcaoControleIndireto.SetValueWithoutNotify(false);

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