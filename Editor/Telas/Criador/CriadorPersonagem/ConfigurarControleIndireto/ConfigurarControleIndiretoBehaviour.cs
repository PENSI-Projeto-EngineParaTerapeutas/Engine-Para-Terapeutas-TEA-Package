using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Editor.DTOs;
using Autis.Runtime.Constantes;
using Autis.Editor.Constantes;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Telas {
    public class ConfigurarControleIndiretoBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/ConfigurarControleIndireto/ConfigurarControleIndiretoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/ConfigurarControleIndireto/ConfigurarControleIndiretoStyle.uss";

        private enum EstadoConfiguracao { Edicao, Criacao, }

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CARREGAR_ANIMACOES = "[ERROR]: Não foi possível carregar as animações do Personagem. Certifique-se de que as Animações estão localizadas em {local}.";

        private const string TOOLTIP_DROPDOWN_ANIMACAO = "Define o a animação que será exibida ao selecionar o Ator";
        private const string TOOLTIP_DROPDOWN_OBJETO_GATILHO = "Define o Ator que irá iniciar a animação";

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_OBJETO_GATILHO = "regiao-carregamento-input-objeto-interacao";
        private readonly VisualElement regiaoCarregamentoInputObjetoGatilho;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_ANIMACAO = "regiao-carregamento-input-animacao";
        private readonly VisualElement regiaoCarregamentoInputAnimacao;

        private const string NOME_BOTAO_ADICIONAR_ACAO = "botao-adicionar-acao";
        private readonly Button botaoAdicionarAcao;

        private const string NOME_REGIAO_LISTA_ANIMACOES = "regiao-lista-animacoes";
        private readonly ScrollView regiaoListaAnimacoes;

        private const string NOME_REGIAO_LISTA_ANIMACOES_VAZIA = "regiao-lista-animacoes-vazia";
        private readonly VisualElement regiaoListaAnimacoesVazia;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private readonly VisualElement regiaoBotoesConfirmacao;

        private Dropdown inputObjetoGatilho;
        private Dropdown inputAnimacao;
        private readonly BotoesConfirmacao botoesConfirmacao;

        #endregion

        private InformacoesAcao displayInformacoesAcaoAtual;
        private readonly List<AcaoPersonagem> associacoesAcoes;
        private readonly List<InformacoesAcao> displaysInformacoesAcao;

        private readonly List<GameObject> objetosInteracao;
        private readonly List<AnimationClip> clipsAnimacoes;

        private readonly GameObject personagem;

        private EstadoConfiguracao estadoAtual = EstadoConfiguracao.Criacao;

        public ConfigurarControleIndiretoBehaviour(GameObject personagem, TiposPersonagem tipoPersonagem, List<AcaoPersonagem> associacoesAcoes) {
            this.personagem = personagem;
            this.associacoesAcoes = associacoesAcoes;

            displaysInformacoesAcao = new List<InformacoesAcao>();
            displayInformacoesAcaoAtual = CriarDisplayInformacoesAcao();

            clipsAnimacoes = new List<AnimationClip>();
            CarregarAnimacoesComBaseTipoPersonagem(tipoPersonagem);

            objetosInteracao = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();

            botoesConfirmacao = new BotoesConfirmacao();

            regiaoCarregamentoInputObjetoGatilho = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_OBJETO_GATILHO);
            regiaoCarregamentoInputAnimacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_ANIMACAO);
            botaoAdicionarAcao = Root.Query<Button>(NOME_BOTAO_ADICIONAR_ACAO);
            regiaoListaAnimacoes = Root.Query<ScrollView>(NOME_REGIAO_LISTA_ANIMACOES);
            regiaoListaAnimacoesVazia = Root.Query<VisualElement>(NOME_REGIAO_LISTA_ANIMACOES_VAZIA);
            regiaoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);

            ConfigurarInputObjetoGatilho();
            ConfigurarInputAnimacao();
            ConfigurarBotaoAdicionarAcao();
            ConfigurarBoteosConfirmacao();

            CarregarAssociacoesJaEstabelecidas();

            return;
        }

        private InformacoesAcao CriarDisplayInformacoesAcao() {
            return new InformacoesAcao(new AcaoPersonagem()) {
                CallbackExcluirAcao = HandleExcluirAcao,
                CallbackEditarAcao = HandleEditarAcao,
            };
        }

        private void CarregarAnimacoesComBaseTipoPersonagem(TiposPersonagem tiposPersonagem) {
            List<string> caminhoArquivosPastaAnimacao = new();

            switch(tiposPersonagem) {
                case(TiposPersonagem.Avatar): {
                    caminhoArquivosPastaAnimacao = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar).ToList();

                    if(caminhoArquivosPastaAnimacao.Count <= 0) {
                        Debug.LogError(MENSAGEM_ERRO_CARREGAR_ANIMACOES.Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar));
                    }

                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    caminhoArquivosPastaAnimacao = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito).ToList();

                    if(caminhoArquivosPastaAnimacao.Count <= 0) {
                        Debug.LogError(MENSAGEM_ERRO_CARREGAR_ANIMACOES.Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito));
                    }

                    break;
                }
                case(TiposPersonagem.Ludico): {
                    string tipoPersonagemLudico = personagem.GetComponent<IdentificadorTipoPersonagemLudico>().tipoPersonagenLudicos.ToString();
                    caminhoArquivosPastaAnimacao = Directory.GetFiles(Path.Combine(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico, tipoPersonagemLudico)).ToList();

                    if(caminhoArquivosPastaAnimacao.Count <= 0) {
                        Debug.LogError(MENSAGEM_ERRO_CARREGAR_ANIMACOES.Replace("{local}", ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico + tipoPersonagemLudico));
                    }

                    break;
                }
            }

            foreach(string caminhoArquivo in caminhoArquivosPastaAnimacao) {
                if(Path.GetExtension(caminhoArquivo) == ExtensoesEditor.ClipAnimacao) {
                    AnimationClip clipAnimacao = AssetDatabase.LoadAssetAtPath<AnimationClip>(caminhoArquivo);
                    clipsAnimacoes.Add(clipAnimacao);
                }
            }

            return;
        }

        private void CarregarAssociacoesJaEstabelecidas() {
            foreach(AcaoPersonagem acaoAssociada in associacoesAcoes) {
                displayInformacoesAcaoAtual.AcaoVinculada.Animacao = acaoAssociada.Animacao;
                displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho = acaoAssociada.ObjetoGatilho;

                AdicionarAcaoAtual();

                displayInformacoesAcaoAtual = CriarDisplayInformacoesAcao();
            }

            return;
        }

        private void ConfigurarInputObjetoGatilho() {
            List<string> nomeObjetosInteracao = new();
            foreach(GameObject objetoInteracao in objetosInteracao) {
                nomeObjetosInteracao.Add(objetoInteracao.name);
            }

            inputObjetoGatilho = new Dropdown("Objeto:", TOOLTIP_DROPDOWN_OBJETO_GATILHO, nomeObjetosInteracao);

            inputObjetoGatilho.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(inputObjetoGatilho.Campo.value == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho = null;
                    botaoAdicionarAcao.SetEnabled(false);
                    return;
                }

                displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho = objetosInteracao.Find(objeto => objeto.name == inputObjetoGatilho.Campo.value);
                if(displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho != null && displayInformacoesAcaoAtual.AcaoVinculada.Animacao != null) {
                    botaoAdicionarAcao.SetEnabled(true);
                    return;
                }
            });

            regiaoCarregamentoInputObjetoGatilho.Add(inputObjetoGatilho.Root);

            return;
        }

        private void ConfigurarInputAnimacao() {
            List<string> nomeClipsAnimacoes = new();
            foreach(AnimationClip clip in clipsAnimacoes) {
                nomeClipsAnimacoes.Add(clip.name);
            }

            inputAnimacao = new Dropdown("Animação:", TOOLTIP_DROPDOWN_ANIMACAO, nomeClipsAnimacoes);

            inputAnimacao.Campo.RegisterCallback<ChangeEvent<string>>(evt => {
                if(inputAnimacao.Campo.value == Dropdown.VALOR_PADRAO_DROPDOWN) {
                    displayInformacoesAcaoAtual.AcaoVinculada.Animacao = null;
                    botaoAdicionarAcao.SetEnabled(false);
                    return;
                }

                displayInformacoesAcaoAtual.AcaoVinculada.Animacao = clipsAnimacoes.Find(clip => clip.name == inputAnimacao.Campo.value);
                if(displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho != null && displayInformacoesAcaoAtual.AcaoVinculada.Animacao != null) {
                    botaoAdicionarAcao.SetEnabled(true);
                    return;
                }
            });

            regiaoCarregamentoInputAnimacao.Add(inputAnimacao.Root);

            return;
        }

        private void ConfigurarBotaoAdicionarAcao() {
            botaoAdicionarAcao.clicked += HandleBotaoAdicionarAcaoClick;
            botaoAdicionarAcao.SetEnabled(false);
            return;
        }

        private void HandleBotaoAdicionarAcaoClick() {
            if(displayInformacoesAcaoAtual.AcaoVinculada.Animacao == null || displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho == null) {
                Debug.Log("[LOG]: Valores obrigatórios não preenchidos");
                return;
            }

            switch(estadoAtual) {
                case(EstadoConfiguracao.Criacao): {
                    AdicionarAcaoAtual();
                    break;
                }
                case(EstadoConfiguracao.Edicao): {
                    EditarAcaoAtual();
                    break;
                }
            }

            displayInformacoesAcaoAtual = CriarDisplayInformacoesAcao();
            ReiniciarCampos();

            return;
        }

        private void AdicionarAcaoAtual() {
            InformacoesAcao acaoRepetida = displaysInformacoesAcao.Find(displayInformacao => displayInformacao.AcaoVinculada.ObjetoGatilho == displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho);
            if(acaoRepetida != null) {
                acaoRepetida.AcaoVinculada.Animacao = displayInformacoesAcaoAtual.AcaoVinculada.Animacao;
                acaoRepetida.AtualizarInformacoesLabel();

                return;
            }

            displaysInformacoesAcao.Add(displayInformacoesAcaoAtual);
            regiaoListaAnimacoes.Add(displayInformacoesAcaoAtual.Root);

            displayInformacoesAcaoAtual.AtualizarInformacoesLabel();
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

        private void EditarAcaoAtual() {
            displayInformacoesAcaoAtual.AtualizarInformacoesLabel();
            estadoAtual = EstadoConfiguracao.Criacao;
            return;
        }

        private void HandleExcluirAcao(InformacoesAcao displayInformacoesAcao) {
            AcionadorAcaoPersonagem acionador = displayInformacoesAcao.AcaoVinculada.ObjetoGatilho.GetComponent<AcionadorAcaoPersonagem>();
            acionador.animacaoAcionada = null;

            displaysInformacoesAcao.Remove(displayInformacoesAcao);
            regiaoListaAnimacoes.Remove(displayInformacoesAcao.Root);

            AlterarVizibilidadeListaAnimacoes(displaysInformacoesAcao.Count > 0);

            return;
        }

        private void HandleEditarAcao(InformacoesAcao displayInformacoesAcao) {
            estadoAtual = EstadoConfiguracao.Edicao;

            displayInformacoesAcaoAtual = displayInformacoesAcao;

            inputAnimacao.Campo.SetValueWithoutNotify(displayInformacoesAcaoAtual.AcaoVinculada.Animacao.name);
            inputObjetoGatilho.Campo.SetValueWithoutNotify(displayInformacoesAcaoAtual.AcaoVinculada.ObjetoGatilho.name);

            return;
        }

        private void ConfigurarBoteosConfirmacao() {
            regiaoBotoesConfirmacao.Add(botoesConfirmacao.Root);
            botoesConfirmacao.BotaoConfirmar.clicked += HandleBotaoConfirmarClick;

            return;
        }

        private void HandleBotaoConfirmarClick() {
            associacoesAcoes.Clear();

            foreach(InformacoesAcao informacoesAcao in displaysInformacoesAcao) {
                associacoesAcoes.Add(informacoesAcao.AcaoVinculada);
            }

            return;
        }

        public void ReiniciarCampos() {
            inputObjetoGatilho.ReiniciarCampos();
            inputAnimacao.ReiniciarCampos();

            return;
        }
    }
}