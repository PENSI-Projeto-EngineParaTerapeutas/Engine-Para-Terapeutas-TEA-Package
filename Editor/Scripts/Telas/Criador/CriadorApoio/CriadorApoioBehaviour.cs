using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.UI;
using Autis.Editor.Telas;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Criadores {
    public class CriadorApoioBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorApoio/CriadorApoioTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorApoio/CriadorApoioStyle.uss";

        #region .: Mensagens :.

        protected const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_APOIO = "[TODO]: Adicionar tooltip";
        protected const string MENSAGEM_TOOLTIP_DROPDOWN_TIPO_ACIONAMENTO = "[TODO]: Adicionar tooltip";
        protected const string MENSAGEM_TOOLTIP_DROPDOWN_TEMPO_EXIBICAO = "[TODO]: Adicionar tooltip";

        #endregion

        #region .: Elementos :.

        protected const string NOME_REGIAO_CARREGAMENTO_INPUT_NOME = "regiao-input-nome";
        protected VisualElement regiaoCarregamenteInputNome;

        protected const string NOME_REGIAO_CARREGAMENTO_DROPDOWN_OBJETO_PAI = "regiao-carregamento-selecao-objeto-pai";
        protected VisualElement regiaoCarregamentoDropdownObjetoPai;

        protected const string NOME_BOTAO_CRIAR_OBJETO_INTERACAO = "botao-criar-elemento";
        protected Button botaoCriarObjetoInteracao;

        protected const string NOME_BOTAO_RADIO_SELECIONAR_OBJETO = "radio-opcao-selecionar-objeto-existente";
        protected RadioButton radioHabilitarSelecionarObjeto;

        protected const string NOME_BOTAO_RADIO_CRIAR_NOVO_OBJETO = "radio-opcao-criar-novo-objeto";
        protected RadioButton radioHabilitarCriarNovoObjeto;

        protected const string NOME_REGIAO_CARREGAMENTO_TIPO_APOIO = "regiao-carregamento-tipo-apoio";
        protected VisualElement regiaoCarregamentoTipoApoio;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO = "regiao-carregamento-inputs-audio";
        protected VisualElement regiaoCarregamentoInputsAudio;

        protected const string NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO = "regiao-carregamento-inputs-texto";
        protected VisualElement regiaoCarregamentoInputsTexto;

        protected const string NOME_REGIAO_CARREGAMENTO_TIPO_ACIONAMENTO = "regiao-carregamento-tipo-acionamento";
        protected VisualElement regiaoCarregamentoTipoAcionamento;

        protected const string NOME_REGIAO_CARREGAMENTO_TEMPO_ACIONAMENTO = "regiao-tempo-acionamento";
        protected VisualElement regiaoCarregamentoTempoExibicao;

        protected const string NOME_REGIAO_OPCOES_AVANCADAS = "foldout-opcoes-avancadas";
        protected Foldout foldoutOpcoesAvancadas;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;

        protected InputTexto campoNome;

        protected Dropdown dropdownTipoApoio;
        protected Dropdown dropdownObjetosInteracao;
        protected Dropdown dropdownTipoAcionamento;
        protected Dropdown dropdownTempoExibicao;

        protected GrupoInputsPosicao grupoInputsPosicao;
        protected GrupoInputsTamanho grupoInputsTamanho;
        protected GrupoInputsAudio grupoInputsAudio;
        protected GrupoInputsTexto grupoInputTexto;
        
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected readonly ManipuladorApoio manipulador;

        protected List<string> opcoesObjetosInteracao;
        protected Dictionary<string, GameObject> associacaoObjetosInteracaoNome;
        protected Dictionary<string, TiposApoiosObjetosInteracao> associacaoValoresDropdownTipoApoios;
        protected Dictionary<string, TipoAcionamentoApoioObjetoInteracao> associacaoOpcaoValorDropdownAcionamento;
        protected Dictionary<string, float> associacoesTemposExibicao;

        public CriadorApoioBehaviour() {
            manipulador = new ManipuladorApoio();
            manipulador.Criar();

            PreencherAssociacaoObjetosIteracao();
            ConfigurarCampoNome();
            ConfigurarDropdownAssociacaoObjetoInteracao();
            ConfigurarBotaoCriacaoObjetoInteracao();
            ConfigurarCampoTipoApoio();
            ConfigurarInputAudio();
            ConfigurarInputTexto();
            ConfigurarInputAcionamento();
            ConfigurarCampoTempoExibicao();
            ConfigurarRegiaoOpcoesAvancadas();

            ConfigurarBotoesConfirmacao();

            return;
        }

        protected virtual void PreencherAssociacaoObjetosIteracao() {
            opcoesObjetosInteracao = new();
            associacaoObjetosInteracaoNome = new();

            List<GameObject> objetosInteracao = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();
            foreach(GameObject objetoInteracao in objetosInteracao) {
                opcoesObjetosInteracao.Add(objetoInteracao.name);
                associacaoObjetosInteracaoNome.Add(objetoInteracao.name, objetoInteracao);
            }

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

        protected virtual void ConfigurarDropdownAssociacaoObjetoInteracao() {
            dropdownObjetosInteracao = new Dropdown(string.Empty, opcoesObjetosInteracao);
            dropdownObjetosInteracao.Root.SetEnabled(false);

            dropdownObjetosInteracao.Root.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    manipulador.DesvincularObjetoInteracao();
                    return;
                }

                manipulador.VincularObjetoInteracao(associacaoObjetosInteracaoNome[evt.newValue]);
            });

            regiaoCarregamentoDropdownObjetoPai = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_DROPDOWN_OBJETO_PAI);
            regiaoCarregamentoDropdownObjetoPai.Add(dropdownObjetosInteracao.Root);

            return;
        }

        protected virtual void ConfigurarBotaoCriacaoObjetoInteracao() {
            botaoCriarObjetoInteracao = root.Query<Button>(NOME_BOTAO_CRIAR_OBJETO_INTERACAO);
            botaoCriarObjetoInteracao.SetEnabled(false);
            botaoCriarObjetoInteracao.clicked += HandleBotaoCriarObjetoInteracaoClick;

            radioHabilitarCriarNovoObjeto = root.Query<RadioButton>(NOME_BOTAO_RADIO_CRIAR_NOVO_OBJETO);
            radioHabilitarCriarNovoObjeto.RegisterCallback<ChangeEvent<bool>>(evt => {
                botaoCriarObjetoInteracao.SetEnabled(evt.newValue);
            });

            radioHabilitarSelecionarObjeto = root.Query<RadioButton>(NOME_BOTAO_RADIO_SELECIONAR_OBJETO);
            radioHabilitarSelecionarObjeto.RegisterCallback<ChangeEvent<bool>>(evt => {
                dropdownObjetosInteracao.Root.SetEnabled(evt.newValue);
            });

            return;
        }

        protected virtual void HandleBotaoCriarObjetoInteracaoClick() {
            Navigator.Instance.IrPara(new CriadorObjetoInteracaoBehaviour() {
                OnFinalizarCriacao = HandleFinalizarCriacaoObjetoInteracao,
            });

            return;
        }

        protected virtual void HandleFinalizarCriacaoObjetoInteracao(GameObject objetoInteracao) {
            radioHabilitarCriarNovoObjeto.value = false;
            radioHabilitarSelecionarObjeto.value = true;

            botaoCriarObjetoInteracao.SetEnabled(false);

            dropdownObjetosInteracao.Root.SetEnabled(true);
            dropdownObjetosInteracao.Campo.choices.Add(objetoInteracao.name);
            dropdownObjetosInteracao.Campo.SetValueWithoutNotify(objetoInteracao.name);

            manipulador.VincularObjetoInteracao(objetoInteracao);

            opcoesObjetosInteracao.Add(objetoInteracao.name);
            associacaoObjetosInteracaoNome.Add(objetoInteracao.name, objetoInteracao);

            return;
        }

        protected virtual void ConfigurarCampoTipoApoio() {
            associacaoValoresDropdownTipoApoios = new() {
                { "Evidenciar Elemento com seta e áudio", TiposApoiosObjetosInteracao.Seta },
                { "Apresentar áudio", TiposApoiosObjetosInteracao.Audio },
                { "Apresentar texto", TiposApoiosObjetosInteracao.Texto },
            };

            List<string> tiposApoio = new();
            foreach(KeyValuePair<string, TiposApoiosObjetosInteracao> associacao in associacaoValoresDropdownTipoApoios) {
                tiposApoio.Add(associacao.Key);
            }

            dropdownTipoApoio = new Dropdown("Tipo de apoio:", MENSAGEM_TOOLTIP_DROPDOWN_TIPO_APOIO, tiposApoio);
            regiaoCarregamentoTipoApoio = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TIPO_APOIO);
            regiaoCarregamentoTipoApoio.Add(dropdownTipoApoio.Root);

            dropdownTipoApoio.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(evt.newValue == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    manipulador.DesabilitarComponentes();
                    OcultarCampos();
                    return;
                }

                TiposApoiosObjetosInteracao novoTipo = associacaoValoresDropdownTipoApoios[evt.newValue];

                AlterarVisibilidadeCamposComBaseTipo(novoTipo);
                manipulador.AlterarTipo(novoTipo);
            });

            return;
        }

        protected virtual void AlterarVisibilidadeCamposComBaseTipo(TiposApoiosObjetosInteracao tipo) {
            ReiniciarAtributos();

            switch(tipo) {
                case(TiposApoiosObjetosInteracao.Seta): {
                    ExibirCamposApoioSeta();
                    break;
                }
                case(TiposApoiosObjetosInteracao.Audio): {
                    ExibirCamposApoioAudio();
                    break;
                }
                case(TiposApoiosObjetosInteracao.Texto): {
                    ExibirCamposApoioTexto();
                    break;
                }
            }

            return;
        }

        protected virtual void ReiniciarAtributos() {
            grupoInputsPosicao.ReiniciarCampos();
            manipulador.SetPosicao(Vector3.zero);

            grupoInputsTamanho.ReiniciarCampos();
            manipulador.SetTamanho(1);

            grupoInputsAudio.ReiniciarCampos();
            manipulador.ManipualdorComponenteAudioSource.SetVolume(1);
            manipulador.ManipualdorComponenteAudioSource.SetAudioClip(null);

            grupoInputTexto.ReiniciarCampos();
            manipulador.ManipuladorComponenteTexto.SetTexto(grupoInputTexto.CampoConteudoTexto.value);
            manipulador.ManipuladorComponenteTexto.SetFontSize(1);

            return;
        }

        protected virtual void OcultarCampos() {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposApoioSeta() {
            regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            
            dropdownTempoExibicao.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposApoioAudio() {
            regiaoCarregamentoInputsAudio.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            
            dropdownTempoExibicao.Root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ExibirCamposApoioTexto() {
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            
            dropdownTempoExibicao.Root.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            foldoutOpcoesAvancadas.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            grupoInputsTamanho.Root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);

            return;
        }

        protected virtual void ConfigurarInputAcionamento() {
            associacaoOpcaoValorDropdownAcionamento = new() {
                { "Automático, em caso de erro", TipoAcionamentoApoioObjetoInteracao.Erro },
                { "Quando o Elemento é selecionado", TipoAcionamentoApoioObjetoInteracao.Selecao },
            };

            List<string> opcoes = new();
            foreach(KeyValuePair<string, TipoAcionamentoApoioObjetoInteracao> associaca in associacaoOpcaoValorDropdownAcionamento) {
                opcoes.Add(associaca.Key);
            }

            dropdownTipoAcionamento = new Dropdown("Forma de acionamento:", MENSAGEM_TOOLTIP_DROPDOWN_TIPO_ACIONAMENTO, opcoes);
            dropdownTipoAcionamento.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                manipulador.SetTipoAcionamento(associacaoOpcaoValorDropdownAcionamento[evt.newValue]);
            });

            regiaoCarregamentoTipoAcionamento = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TIPO_ACIONAMENTO);
            regiaoCarregamentoTipoAcionamento.Add(dropdownTipoAcionamento.Root);

            return;
        }

        protected virtual void ConfigurarCampoTempoExibicao() {
            associacoesTemposExibicao = new() {
                { "5 segundos", 5f},
                { "7 segundos", 7f}, 
            };

            List<string> opcoes = new();
            foreach(KeyValuePair<string, float> associacao in associacoesTemposExibicao) {
                opcoes.Add(associacao.Key);
            }

            dropdownTempoExibicao = new Dropdown("Tempo de exibição:", MENSAGEM_TOOLTIP_DROPDOWN_TEMPO_EXIBICAO, opcoes);
            dropdownTempoExibicao.Root.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            dropdownTempoExibicao.Root.RegisterCallback<ChangeEvent<string>>(evt => {
                float tempoEspera = associacoesTemposExibicao[evt.newValue];
                manipulador.SetTempoExibicao(tempoEspera);
            });

            regiaoCarregamentoTempoExibicao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_TEMPO_ACIONAMENTO);
            regiaoCarregamentoTempoExibicao.Add(dropdownTempoExibicao.Root);

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
            manipulador.Finalizar();

            Navigator.Instance.Voltar();

            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            manipulador.Cancelar();

            Navigator.Instance.Voltar();

            return;
        }

        protected virtual void ConfigurarInputAudio() {
            grupoInputsAudio = new GrupoInputsAudio();

            grupoInputsAudio.VincularDados(manipulador.ManipualdorComponenteAudioSource);

            regiaoCarregamentoInputsAudio = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_AUDIO);
            regiaoCarregamentoInputsAudio.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsAudio.Add(grupoInputsAudio.Root);

            return;
        }

        protected virtual void ConfigurarInputTexto() {
            grupoInputTexto = new GrupoInputsTexto();

            grupoInputTexto.VincularDados(manipulador.ManipuladorComponenteTexto);

            regiaoCarregamentoInputsTexto = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_TEXTO);
            regiaoCarregamentoInputsTexto.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoCarregamentoInputsTexto.Add(grupoInputTexto.Root);

            return;
        }

        public void ReiniciarCampos() {
            grupoInputsAudio.ReiniciarCampos();
            grupoInputsPosicao.ReiniciarCampos();
            grupoInputsTamanho.ReiniciarCampos();
            grupoInputTexto.ReiniciarCampos();
            
            dropdownTipoApoio.ReiniciarCampos();
            dropdownTipoAcionamento.ReiniciarCampos();

            OcultarCampos();

            return;
        }
    }
}