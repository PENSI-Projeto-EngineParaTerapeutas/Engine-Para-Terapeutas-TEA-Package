using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.Telas {
    public class ConfigurarControleIndiretoBehaviour : Tela, IReiniciavel {
        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/ConfigurarControleIndireto/ConfigurarControleIndiretoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/ConfigurarControleIndireto/ConfigurarControleIndiretoStyle.uss";

        private class Acao {
            public GameObject ObjetoInteracaoGatilho { get; set; }
            public AnimationClip AnimacaoAssociada { get; set; }
        }

        private const string VALOR_PADRAO_DROPDOWN = "Selecione";

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_OBJETO_GATILHO = "regiao-carregamento-input-objeto-interacao";
        private readonly VisualElement regiaoCarregamentoInputObjetoGatilho;

        private const string NOME_LABEL_OBJETO_GATILHO = "label-objeto-interacao-gatilho";
        private const string NOME_INPUT_OBJETO_GATILHO = "input-objeto-interacao-gatilho";
        private DropdownField inputObjetoGatilho;

        private const string NOME_REGIAO_CARREGAMENTO_INPUT_ANIMACAO = "regiao-carregamento-input-animacao";
        private readonly VisualElement regiaoCarregamentoInputAnimacao;

        private const string NOME_LABEL_ANIMACAO = "label-animacao";
        private const string NOME_INPUT_ANIMACAO = "input-animacao";
        private DropdownField inputAnimacao;

        private const string NOME_BOTAO_ADICIONAR_ACAO = "botao-adicionar-acao";
        private readonly Button botaoAdicionarAcao;

        private const string NOME_REGIAO_LISTA_ANIMACOES = "regiao-lista-animacoes";
        private readonly ScrollView regiaoListaAnimacoes;

        private const string NOME_REGIAO_LISTA_ANIMACOES_VAZIA = "regiao-lista-animacoes-vazia";
        private readonly VisualElement regiaoListaAnimacoesVazia;

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO = "regiao-carregamento-botoes-confirmacao";
        private readonly VisualElement regiaoBotoesConfirmacao;

        private readonly BotoesConfirmacao botoesConfirmacao;

        #endregion

        private readonly List<Acao> acoes;

        private readonly List<GameObject> objetosInteracao;
        private readonly List<AnimationClip> clipsAnimacoes;
        private Acao acaoAtual;

        public ConfigurarControleIndiretoBehaviour() {
            acoes = new List<Acao>();
            clipsAnimacoes = new List<AnimationClip>();
            objetosInteracao = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();
            CarregarAnimacoesComBaseTipoPersonagem(TiposPersonagem.BonecoPalito);

            acaoAtual = new Acao();

            botoesConfirmacao = new BotoesConfirmacao();

            regiaoCarregamentoInputObjetoGatilho = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_OBJETO_GATILHO);
            regiaoCarregamentoInputAnimacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUT_ANIMACAO);
            botaoAdicionarAcao = Root.Query<Button>(NOME_BOTAO_ADICIONAR_ACAO);
            regiaoListaAnimacoes = Root.Query<ScrollView>(NOME_REGIAO_LISTA_ANIMACOES);
            regiaoListaAnimacoesVazia = Root.Query<VisualElement>(NOME_REGIAO_LISTA_ANIMACOES_VAZIA);
            regiaoBotoesConfirmacao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_BOTOES_CONFIRMACAO);

            ConfigurarInputObjetoGatilho();
            ConfigurarAnimacao();
            ConfigurarBotaoAdicionarAcao();
            ConfigurarListaAnimacoes();
            ConfigurarBoteosConfirmacao();

            return;
        }

        private void ConfigurarInputObjetoGatilho() {
            List<string> nomeObjetosInteracao = new() { VALOR_PADRAO_DROPDOWN };
            foreach(GameObject objetoInteracao in objetosInteracao) {
                nomeObjetosInteracao.Add(objetoInteracao.name);
            }

            inputObjetoGatilho = new DropdownField("Objeto:", nomeObjetosInteracao, 0) {
                tooltip = "Define o Ator que irá iniciar a animação",
                name = NOME_INPUT_OBJETO_GATILHO,
            };
            inputObjetoGatilho.AddToClassList(NomesClassesPadroesEditorStyle.InputPadrao);

            inputObjetoGatilho.labelElement.name = NOME_LABEL_OBJETO_GATILHO;
            inputObjetoGatilho.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            inputObjetoGatilho.RegisterCallback<ChangeEvent<string>>(evt => {
                if(inputObjetoGatilho.value == VALOR_PADRAO_DROPDOWN) {
                    acaoAtual.ObjetoInteracaoGatilho = null;
                    return;
                }

                acaoAtual.ObjetoInteracaoGatilho = objetosInteracao.Find(objeto => objeto.name == inputObjetoGatilho.value);
            });

            regiaoCarregamentoInputObjetoGatilho.Add(inputObjetoGatilho);

            return;
        }

        private void ConfigurarAnimacao() {
            List<string> nomeClipsAnimacoes = new() { VALOR_PADRAO_DROPDOWN };
            foreach(AnimationClip clip in clipsAnimacoes) {
                nomeClipsAnimacoes.Add(clip.name);
            }

            inputAnimacao = new DropdownField("Animação:", nomeClipsAnimacoes, 0) {
                tooltip = "Define o a animação que será exibida ao selecionar o Ator",
                name = NOME_INPUT_ANIMACAO,
            };
            inputAnimacao.AddToClassList(NomesClassesPadroesEditorStyle.InputPadrao);

            inputAnimacao.labelElement.name = NOME_LABEL_ANIMACAO;
            inputAnimacao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            inputAnimacao.RegisterCallback<ChangeEvent<string>>(evt => {
                if(inputAnimacao.value == VALOR_PADRAO_DROPDOWN) {
                    acaoAtual.AnimacaoAssociada = null;
                    return;
                }

                acaoAtual.AnimacaoAssociada = clipsAnimacoes.Find(clip => clip.name == inputAnimacao.value);
            });

            regiaoCarregamentoInputAnimacao.Add(inputAnimacao);

            return;
        }

        private void CarregarAnimacoesComBaseTipoPersonagem(TiposPersonagem tiposPersonagem) {
            List<string> caminhoArquivosPastaAnimacao = new();

            switch(tiposPersonagem) {
                case(TiposPersonagem.Avatar): {
                    caminhoArquivosPastaAnimacao = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesAvatar).ToList();
                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    caminhoArquivosPastaAnimacao = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesBonecoPalito).ToList();
                    break;
                }
                case(TiposPersonagem.Ludico): {
                    caminhoArquivosPastaAnimacao = Directory.GetFiles(ConstantesProjetoUnity.CaminhoUnityAssetsAnimacoesPersonagemLudico).ToList();
                    break;
                }
            }

            foreach(string caminhoArquivo in caminhoArquivosPastaAnimacao) {
                if(Path.GetExtension(caminhoArquivo) == Extensoes.ClipAnimacao) {
                    AnimationClip clipAnimacao = AssetDatabase.LoadAssetAtPath<AnimationClip>(caminhoArquivo);
                    clipsAnimacoes.Add(clipAnimacao);
                }
            }

            return;
        }

        private void ConfigurarBotaoAdicionarAcao() {
            botaoAdicionarAcao.clicked += HandleBotaoAdicionarAcaoClick;
            return;
        }

        private void HandleBotaoAdicionarAcaoClick() {
            if(acaoAtual.AnimacaoAssociada == null || acaoAtual.ObjetoInteracaoGatilho == null) {
                Debug.Log("[LOG]: Valores obrigatórios não preenchidos");
                return;
            }

            acoes.Add(acaoAtual);
            Debug.Log(acoes.Count);
            AtualizarListaAnimacoes();

            acaoAtual = new Acao();
            ReiniciarCampos();
            return;
        }

        private void AtualizarListaAnimacoes() {
            Debug.Log("[TODO]: Atualizar lista animações");

            regiaoListaAnimacoesVazia.AddToClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            regiaoListaAnimacoes.RemoveFromClassList(NomesClassesPadroesEditorStyle.DisplayNone);
            InformacoesAcao teste = new(acaoAtual.ObjetoInteracaoGatilho.name, acaoAtual.AnimacaoAssociada.name);
            regiaoListaAnimacoes.Add(teste.Root);

            return;
        }

        private void ConfigurarListaAnimacoes() {
            return;
        }

        private void ConfigurarBoteosConfirmacao() {
            regiaoBotoesConfirmacao.Add(botoesConfirmacao.Root);
            return;
        }

        public void ReiniciarCampos() {
            inputObjetoGatilho.SetValueWithoutNotify(inputObjetoGatilho.choices.First());
            inputAnimacao.SetValueWithoutNotify(inputAnimacao.choices.First());

            return;
        }
    }
}