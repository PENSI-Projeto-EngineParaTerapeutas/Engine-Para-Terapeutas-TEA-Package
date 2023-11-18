using System.Collections.Generic;
using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Editor.DTOs;
using Autis.Editor.Constantes;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Telas {
    public class ControleIndiretoBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/ControleIndireto/ControleIndiretoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/ControleIndireto/ControleIndiretoStyle.uss";

        #region .: Elementos :.

        private const string NOME_BOTAO_ADICIONAR_ACAO = "botao-adicionar-acao";
        private Button botaoAdicionarAcao;

        private const string NOME_REGIAO_LISTA_ANIMACOES = "regiao-lista-animacoes";
        private ScrollView regiaoListaAnimacoes;

        private const string NOME_REGIAO_LISTA_ANIMACOES_VAZIA = "regiao-lista-animacoes-vazia";
        private VisualElement regiaoListaAnimacoesVazia;

        protected const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        protected VisualElement regiaoCarregamentoBotoesConfirmacao;
        protected BotoesConfirmacao botoesConfirmacao;

        #endregion

        private readonly ManipuladorPersonagens manipuladorPersonagem;
        private readonly List<DisplayAcao> displaysInformacoesAcao = new();

        private DisplayAcao displayAcaoEditada = null;

        public ControleIndiretoBehaviour(ManipuladorPersonagens manipuladorPersonagens) {
            manipuladorPersonagem = manipuladorPersonagens;

            regiaoListaAnimacoes = Root.Query<ScrollView>(NOME_REGIAO_LISTA_ANIMACOES);
            regiaoListaAnimacoesVazia = Root.Query<VisualElement>(NOME_REGIAO_LISTA_ANIMACOES_VAZIA);

            ConfigurarBotaoAdicionarAcao();
            ConfigurarBotoesConfirmacao();

            CarregarAssociacoesJaEstabelecidas();

            return;
        }

        private void CarregarAssociacoesJaEstabelecidas() {
            foreach(AcaoPersonagem acaoAssociada in manipuladorPersonagem.AssociacoesAcoesControleIndireto) {
                DisplayAcao displayAcao = new(acaoAssociada) {
                    CallbackEditarAcao = HandleEditarAcao,
                    CallbackExcluirAcao = HandleExcluirAcao,
                };

                displayAcao.AtualizarInformacoesLabel();

                displaysInformacoesAcao.Add(displayAcao);
                regiaoListaAnimacoes.Add(displayAcao.Root);
            }

            AlterarVizibilidadeListaAnimacoes(displaysInformacoesAcao.Count > 0);

            return;
        }

        private void ConfigurarBotaoAdicionarAcao() {
            botaoAdicionarAcao = Root.Query<Button>(NOME_BOTAO_ADICIONAR_ACAO);
            botaoAdicionarAcao.clicked += HandleBotaoAdicionarAcaoClick;

            return;
        }

        private void HandleBotaoAdicionarAcaoClick() {
            Navigator.Instance.IrPara(new AdicionarAcaoBehaviour(manipuladorPersonagem, displaysInformacoesAcao) {
                OnFinalizarCriacao = HandleConfirmarAdicionarAcao,
            });

            return;
        }

        private void HandleConfirmarAdicionarAcao(AcaoPersonagem acaoAdicionada) {
            DisplayAcao acaoRepetida = displaysInformacoesAcao.Find(displayInformacao => displayInformacao.AcaoVinculada.ObjetoGatilho == acaoAdicionada.ObjetoGatilho);
            if(acaoRepetida != null) {
                acaoRepetida.AcaoVinculada.Animacao = acaoAdicionada.Animacao;
                acaoRepetida.AtualizarInformacoesLabel();

                return;
            }

            DisplayAcao novoDisplayAcao = new(acaoAdicionada) {
                CallbackExcluirAcao = HandleExcluirAcao,
                CallbackEditarAcao = HandleEditarAcao,
            };

            displaysInformacoesAcao.Add(novoDisplayAcao);
            regiaoListaAnimacoes.Add(novoDisplayAcao.Root);

            novoDisplayAcao.AtualizarInformacoesLabel();
            AlterarVizibilidadeListaAnimacoes(displaysInformacoesAcao.Count > 0);

            return;
        }

        private void AlterarVizibilidadeListaAnimacoes(bool deveExibir) {
            if(deveExibir) {
                regiaoListaAnimacoesVazia.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                regiaoListaAnimacoes.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }
            else {
                regiaoListaAnimacoesVazia.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
                regiaoListaAnimacoes.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            }

            return;
        }

        private void HandleExcluirAcao(DisplayAcao displayInformacoesAcao) {
            manipuladorPersonagem.RemoverAcaoControleIndireto(displayInformacoesAcao.AcaoVinculada);

            displaysInformacoesAcao.Remove(displayInformacoesAcao);
            regiaoListaAnimacoes.Remove(displayInformacoesAcao.Root);

            AlterarVizibilidadeListaAnimacoes(displaysInformacoesAcao.Count > 0);

            return;
        }

        private void HandleEditarAcao(DisplayAcao displayInformacoesAcao) {
            displayAcaoEditada = displayInformacoesAcao;
            Navigator.Instance.IrPara(new AdicionarAcaoBehaviour(manipuladorPersonagem, displaysInformacoesAcao, displayInformacoesAcao.AcaoVinculada) {
                OnFinalizarCriacao = HandleFinalizarEdicao,
            });

            return;
        }

        private void HandleFinalizarEdicao(AcaoPersonagem acaoEditada) {
            displayAcaoEditada.AcaoVinculada.Animacao = acaoEditada.Animacao;
            displayAcaoEditada.AcaoVinculada.ObjetoGatilho = acaoEditada.ObjetoGatilho;

            displayAcaoEditada.AtualizarInformacoesLabel();

            displayAcaoEditada = null;

            return;
        }

        private void ConfigurarBotoesConfirmacao() {
            botoesConfirmacao = new();

            botoesConfirmacao.BotaoConfirmar.Clear();
            botoesConfirmacao.BotaoConfirmar.text = "Salvar Configuração\r\ndo Controle Indireto";
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;

            botoesConfirmacao.BotaoCancelar.Clear();
            botoesConfirmacao.BotaoCancelar.text = "Cancelar Configuração do Controle Indireto";
            botoesConfirmacao.BotaoCancelar.clicked += HandleBotaoCancelarClick;

            regiaoCarregamentoBotoesConfirmacao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);
            regiaoCarregamentoBotoesConfirmacao.Add(botoesConfirmacao.Root);

            return;
        }

        private void HandleBotaoConfirmarClick() {
            manipuladorPersonagem.LimparAcoesControleIndireto();
            foreach(DisplayAcao informacoesAcao in displaysInformacoesAcao) {
                manipuladorPersonagem.AdicionarAcaoControleIndireto(informacoesAcao.AcaoVinculada);
            }

            Navigator.Instance.Voltar();

            return;
        }

        private void HandleBotaoCancelarClick() {
            Navigator.Instance.Voltar();
            return;
        }
    }
}