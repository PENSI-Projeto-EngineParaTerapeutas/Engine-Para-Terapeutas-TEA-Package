using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.Utils;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.Manipuladores;
using Autis.Editor.Excecoes;
using Autis.Runtime.Eventos;

namespace Autis.Editor.Criadores {
    public class CriadorPersonagemBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/CriadorPersonagemTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/CriadorPersonagemStyle.uss";
        
        protected const string CAMINHO_PREFAB_BONECO_PALITO = "Personagens/Boneco_Palito_Redondo.prefab";
        protected const string CAMINHO_PREFAB_PERSONAGEM_LUDICO = "Personagens/Ludico.prefab";
        protected const string CAMINHO_PREFAB_AVATAR = "Personagens/Avatar.prefab";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_TITULO = "Cada fase poderá ter um único personagem.";
        private const string MENSAGEM_TOOLTIP_INPUT_NOME = "Digite um nome para o Personagem. Cada componente deve ter um nome exclusivo (que não se repete em outro componente)";
        protected const string TOOLTIP_TIPO_CONTROLE = "Forma como o personagem será controlado:\n\n1) Direto: os movimentos corporais do personagem serão controlados pelo usuário.\n\n2) Indireto: serão apresentadas animações com o personagem, quando o usuário selecionar determinados Elementos do jogo.";
        protected const string TOOLTIP_TIPO_PERSONAGEM = "Forma do personagem. Opções: avatar, boneco palito ou personagem lúdico.";

        protected const string ERRO_TIPO_CONTROLE_NAO_SELECIONADO = "Selecione um tipo de controle para o Personagem.\n";

        #endregion

        #region .: Eventos :.

        protected static EventoJogo eventoFinalizarCriacao;

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_NOME = "regiao-carregamento-nome";
        protected VisualElement regiaoCarregamentoInputNome;

        protected const string NOME_REIGAO_CARREGAMENTO_DROPDOWN_TIPO_PERSONAGEM = "regiao-carregamento-tipo-personagem";
        protected VisualElement regiaoCarregamentoDropdownTipoPersonagem;

        protected const string NOME_IMAGEM_PERSONAGEM = "imagem-personagem";
        protected Image imagemPersonagem;

        protected const string NOME_BOTAO_PERSONALIZAR_PERSONAGEM = "botao-personalizar-personagem";
        protected Button botaoPersonalizarPersonagem;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TIPO_CONTROLE = "regiao-tooltip-forma-controle";
        protected VisualElement regiaoCarregamentoTooltipTipoControle;

        protected const string NOME_RADIO_OPCAO_CONTROLE_DIRETO = "radio-opcao-controle-direto";
        protected RadioButton radioOpcaoControleDireto;

        protected const string NOME_RADIO_OPCAO_CONTROLE_INDIRETO = "radio-opcao-controle-indireto";
        protected RadioButton radioOpcaoControleIndireto;

        protected const string NOME_BOTAO_CONFIGURAR_CONTROLE_INDIRETO = "botao-configurar-controle-indireto";
        protected Button botaoConfigurarControleIndireto;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_POSICAO = "regiao-carregamento-inputs-posicao";
        protected VisualElement regiaoCarregamentoInputsPosicao;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_TAMANHO = "regiao-carregamento-inputs-tamanho";
        protected VisualElement regiaoCarregamentoInputsTamanho;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InterrogacaoToolTip tooltipTipoControle;

        protected InputTexto inputNome;
        protected Dropdown dropdownTipoPersonagem;
        protected GrupoInputsPosicao grupoInputsPosicao;
        protected InputNumerico inputTamanho;

        protected BotoesConfirmacao botoesConfirmacao;

        protected const string NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO = "regiao-tooltip-titulo";
        protected VisualElement regiaoCarregamentoTooltipTitulo;

        protected InterrogacaoToolTip tooltipTitulo;

        #endregion

        protected GameObject prefabBonecoPalito;
        protected GameObject prefabPersonagemLudico;
        protected GameObject prefabAvatar;

        protected Dictionary<string, TiposPersonagem> associacaoTiposPersonagem;

        protected ManipuladorPersonagens manipuladorPersonagem;

        public CriadorPersonagemBehaviour() {
            eventoFinalizarCriacao = Importador.ImportarEvento("EventoFinalizarCriacao");

            CarregarPrefabs();

            ConfigurarTooltipTitulo();
            ConfigurarInputNome();
            ConfigurarDropdownTipoPersonagem();
            ConfigurarImagemPersonagem();
            ConfigurarBotaoPersonalizarPersonagem();
            CarregarTooltipTipoControle();
            ConfigurarRadioButtons();
            ConfigurarCampoPosicao();
            ConfigurarCampoTamanho();
            ConfigurarBotoesConfirmacao();

            return;
        }

        protected virtual void ConfigurarTooltipTitulo() {
            tooltipTitulo = new InterrogacaoToolTip(MENSAGEM_TOOLTIP_TITULO);

            regiaoCarregamentoTooltipTitulo = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TITULO);
            regiaoCarregamentoTooltipTitulo.Add(tooltipTitulo.Root);

            return;
        }

        public override void OnEditorUpdate() {
            if(Tools.current != Tool.Move) {
                Tools.current = Tool.Move;
            }

            if(Selection.activeObject != manipuladorPersonagem?.ObjetoAtual) {
                Selection.activeObject = manipuladorPersonagem?.ObjetoAtual;
            }

            return;
        }

        protected virtual void CarregarPrefabs() {
            prefabBonecoPalito = Importador.ImportarPrefab(CAMINHO_PREFAB_BONECO_PALITO);
            prefabPersonagemLudico = Importador.ImportarPrefab(CAMINHO_PREFAB_PERSONAGEM_LUDICO);
            prefabAvatar = Importador.ImportarPrefab(CAMINHO_PREFAB_AVATAR);

            return;
        }

        protected virtual void ConfigurarInputNome() {
            inputNome = new InputTexto("Nome", MENSAGEM_TOOLTIP_INPUT_NOME);
            inputNome.CampoTexto.AddToClassList("input-texto");

            inputNome.CampoTexto.RegisterCallback<ChangeEvent<string>>(evt => {
                manipuladorPersonagem?.SetNome(evt.newValue);
            });

            regiaoCarregamentoInputNome = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_NOME);
            regiaoCarregamentoInputNome.Add(inputNome.Root);

            return;
        }

        protected virtual void CarregarTooltipTipoControle() {
            tooltipTipoControle = new InterrogacaoToolTip();

            regiaoCarregamentoTooltipTipoControle = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TOOLTIP_TIPO_CONTROLE);
            regiaoCarregamentoTooltipTipoControle.Add(tooltipTipoControle.Root);

            tooltipTipoControle.SetTexto(TOOLTIP_TIPO_CONTROLE);

            return;
        }

        protected virtual void ConfigurarDropdownTipoPersonagem() {
            associacaoTiposPersonagem = new() {
                {"Avatar", TiposPersonagem.Avatar},
                {"Boneco Palito", TiposPersonagem.BonecoPalito},
                {"Personagem lúdico", TiposPersonagem.Ludico},
            };


            List<string> tiposPersonagem = new();
            foreach(KeyValuePair<string, TiposPersonagem> associacao in associacaoTiposPersonagem) {
                tiposPersonagem.Add(associacao.Key);
            }
            
            dropdownTipoPersonagem = new Dropdown("Tipo de personagem:", TOOLTIP_TIPO_PERSONAGEM, tiposPersonagem);
            dropdownTipoPersonagem.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                ReinicarCamposAlterarTipoPersonagem();

                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    manipuladorPersonagem.Cancelar();
                    manipuladorPersonagem = null;

                    AjustarEstadoDeselacaoPersonagem();

                    return;
                }

                TiposPersonagem tipoSelecionado = associacaoTiposPersonagem[evt.newValue];
                if(manipuladorPersonagem != null && manipuladorPersonagem.GetTipoPersonagem() == tipoSelecionado) {
                    return;
                }

                manipuladorPersonagem?.Cancelar();

                switch(tipoSelecionado) {
                    case(TiposPersonagem.Avatar): {
                        manipuladorPersonagem = new ManipuladorAvatar(prefabAvatar);
                        AlterarEstadoSelecaoPersonagemAvatar();

                        break;
                    }
                    case(TiposPersonagem.BonecoPalito): {
                        manipuladorPersonagem = new ManipuladorBonecoPalito(prefabBonecoPalito);
                        AlterarEstadoSelecaoPersonagemPalito();

                        break;
                    }
                    case(TiposPersonagem.Ludico): {
                        manipuladorPersonagem = new ManipuladorPersonagemLudico(prefabPersonagemLudico);
                        AlterarEstadoSelecaoPersonagemLudico();

                        break;
                    }
                }

                manipuladorPersonagem.Criar();
                manipuladorPersonagem.SetNome(inputNome.CampoTexto.value);
            });

            regiaoCarregamentoDropdownTipoPersonagem = Root.Query<VisualElement>(NOME_REIGAO_CARREGAMENTO_DROPDOWN_TIPO_PERSONAGEM);
            regiaoCarregamentoDropdownTipoPersonagem.Add(dropdownTipoPersonagem.Root);

            return;
        }

        protected virtual void ReinicarCamposAlterarTipoPersonagem() {
            radioOpcaoControleDireto.SetValueWithoutNotify(false);
            radioOpcaoControleIndireto.SetValueWithoutNotify(false);

            grupoInputsPosicao.ReiniciarCampos();
            inputTamanho.CampoNumerico.SetValueWithoutNotify(100f);

            return;
        }

        protected virtual void AlterarEstadoSelecaoPersonagemAvatar() {
            botaoPersonalizarPersonagem.SetEnabled(true);
            radioOpcaoControleDireto.SetEnabled(false);
            radioOpcaoControleIndireto.SetEnabled(false);
            botaoConfigurarControleIndireto.SetEnabled(false);
            botoesConfirmacao.BotaoConfirmar.SetEnabled(false);

            return;
        }

        protected virtual void AlterarEstadoSelecaoPersonagemPalito() {
            botaoPersonalizarPersonagem.SetEnabled(true);
            radioOpcaoControleDireto.SetEnabled(true);
            radioOpcaoControleIndireto.SetEnabled(true);
            botaoConfigurarControleIndireto.SetEnabled(false);
            botoesConfirmacao.BotaoConfirmar.SetEnabled(true);

            return;
        }

        protected virtual void AlterarEstadoSelecaoPersonagemLudico() {
            botaoPersonalizarPersonagem.SetEnabled(true);
            radioOpcaoControleDireto.SetEnabled(false);
            radioOpcaoControleIndireto.SetEnabled(false);
            botaoConfigurarControleIndireto.SetEnabled(false);
            botoesConfirmacao.BotaoConfirmar.SetEnabled(false);

            return;
        }

        protected virtual void AjustarEstadoDeselacaoPersonagem() {
            botaoPersonalizarPersonagem.SetEnabled(false);
            radioOpcaoControleDireto.SetEnabled(false);
            radioOpcaoControleIndireto.SetEnabled(false);
            botaoConfigurarControleIndireto.SetEnabled(false);
            botoesConfirmacao.BotaoConfirmar.SetEnabled(false);

            return;
        }

        protected virtual void ConfigurarImagemPersonagem() {
            imagemPersonagem = Root.Query<Image>(NOME_IMAGEM_PERSONAGEM);
            imagemPersonagem.image = Importador.ImportarImagem("imagem-personagem.png");

            return;
        }

        protected virtual void ConfigurarBotaoPersonalizarPersonagem() {
            botaoPersonalizarPersonagem = Root.Query<Button>(NOME_BOTAO_PERSONALIZAR_PERSONAGEM);

            botaoPersonalizarPersonagem.clicked += HandleBotaoPersonalizarPersonagemClick;
            botaoPersonalizarPersonagem.SetEnabled(false);

            return;
        }

        protected virtual void HandleBotaoPersonalizarPersonagemClick() {
            switch(manipuladorPersonagem.GetTipoPersonagem()) {
                case(TiposPersonagem.Avatar): {
                    Navigator.Instance.IrPara(new PersonalizacaoAvatarBehaviour(manipuladorPersonagem as ManipuladorAvatar) {
                        OnConfirmarCriacao = HabilitarCamposSelecaoControle,
                    });
                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    Navigator.Instance.IrPara(new PersonalizacaoBonecoPalitoBehaviour(manipuladorPersonagem as ManipuladorBonecoPalito));
                    break;
                }
                case(TiposPersonagem.Ludico): {
                    Navigator.Instance.IrPara(new PersonalizarLudicoBehaviour(manipuladorPersonagem as ManipuladorPersonagemLudico) {
                        OnConfirmarCriacao = HabilitarCamposSelecaoControle,
                    });
                    break;
                }
            }

            return;
        }

        protected virtual void HabilitarCamposSelecaoControle() {
            radioOpcaoControleDireto.SetEnabled(true);
            radioOpcaoControleIndireto.SetEnabled(true);
            botaoConfigurarControleIndireto.SetEnabled(false);
            botoesConfirmacao.BotaoConfirmar.SetEnabled(true);

            return;
        }

        protected virtual void ConfigurarRadioButtons() {
            radioOpcaoControleDireto = Root.Query<RadioButton>(NOME_RADIO_OPCAO_CONTROLE_DIRETO);
            radioOpcaoControleDireto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);
            radioOpcaoControleDireto.SetValueWithoutNotify(false);
            radioOpcaoControleDireto.RegisterValueChangedCallback(evt => {
                if(!evt.newValue) {
                    return;
                }

                manipuladorPersonagem.AlterarTipoControle(TipoControle.Direto);
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

                manipuladorPersonagem.AlterarTipoControle(TipoControle.Indireto);
                botaoConfigurarControleIndireto.SetEnabled(true);
            });
            radioOpcaoControleIndireto.SetEnabled(false);

            botaoConfigurarControleIndireto = Root.Query<Button>(NOME_BOTAO_CONFIGURAR_CONTROLE_INDIRETO);
            botaoConfigurarControleIndireto.clicked += HandleBotaoConfigurarControleIndiretoClick;
            botaoConfigurarControleIndireto.SetEnabled(false);

            return;
        }

        protected virtual void HandleBotaoConfigurarControleIndiretoClick() {
            Navigator.Instance.IrPara(new ControleIndiretoBehaviour(manipuladorPersonagem));
            return;
        }

        protected virtual void ConfigurarCampoPosicao() {
            grupoInputsPosicao = new GrupoInputsPosicao();

            grupoInputsPosicao.CampoPosicaoX.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                manipuladorPersonagem?.SetPosicaoX(evt.newValue);
            });

            grupoInputsPosicao.CampoPosicaoY.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                manipuladorPersonagem?.SetPosicaoY(evt.newValue);
            });

            regiaoCarregamentoInputsPosicao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_POSICAO);
            regiaoCarregamentoInputsPosicao.Add(grupoInputsPosicao.Root);

            return;
        }

        protected virtual void ConfigurarCampoTamanho() {
            inputTamanho = new InputNumerico("Tamanho (%):", "A porcentagem se refere ao tamanho do personagem. Obs: Nos casos que o personagem for controlado por controle indireto, através de animações, o tamanho do personagem pode ser diminuído na animação.");
            inputTamanho.CampoNumerico.SetValueWithoutNotify(100f);

            inputTamanho.CampoNumerico.RegisterCallback<ChangeEvent<float>>(evt => {
                manipuladorPersonagem?.SetTamanho(evt.newValue / 100);
            });

            regiaoCarregamentoInputsTamanho = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TAMANHO);
            regiaoCarregamentoInputsTamanho.Add(inputTamanho.Root);

            return;
        }

        protected virtual void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();
            botoesConfirmacao.BotaoConfirmar.SetEnabled(false);

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

            try {
                manipuladorPersonagem.Finalizar();
            }
            catch(ExcecaoObjetoDuplicado excecao) {
                PopupAvisoBehaviour.ShowPopupAviso(MensagensGerais.MENSAGEM_ATOR_DUPLICADO.Replace("{nome}", excecao.NomeObjetoDuplicado));
                return;
            }

            eventoFinalizarCriacao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }

        protected virtual void VerificarCamposObrigatorios() {
            string mensagem = string.Empty;

            if(inputNome.EstaVazio()) {
                mensagem += MensagensGerais.MENSAGEM_ERRO_CAMPO_NAO_PREENCHIDO.Replace("{nome_campo}", inputNome.LabelCampoTexto.text);
            }

            if(dropdownTipoPersonagem.EstaVazio()) {
                mensagem += MensagensGerais.MENSAGEM_ERRO_CAMPO_NAO_PREENCHIDO.Replace("{nome_campo}", dropdownTipoPersonagem.Label.text);
            }

            if(!radioOpcaoControleDireto.value && !radioOpcaoControleIndireto.value) {
                mensagem += ERRO_TIPO_CONTROLE_NAO_SELECIONADO;
            }

            if(mensagem != string.Empty) {
                throw new ExcecaoCamposObrigatoriosVazios(mensagem);
            }

            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            ReiniciarCampos();

            manipuladorPersonagem?.Cancelar();
            eventoFinalizarCriacao.AcionarCallbacks();
            Navigator.Instance.Voltar();

            return;
        }

        public void ReiniciarCampos() {
            inputNome.ReiniciarCampos();
            dropdownTipoPersonagem.ReiniciarCampos();

            radioOpcaoControleDireto.SetValueWithoutNotify(false);
            radioOpcaoControleIndireto.SetValueWithoutNotify(false);

            grupoInputsPosicao.ReiniciarCampos();
            inputTamanho.CampoNumerico.SetValueWithoutNotify(100f);

            AjustarEstadoDeselacaoPersonagem();

            return;
        }
    }
}