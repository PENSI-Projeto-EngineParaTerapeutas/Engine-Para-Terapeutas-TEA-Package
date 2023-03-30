using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.Telas { 
    public class ControleDiretoPersonagem : TelaJogo {
        #region .: Elementos :.

        private const string NOME_BOTAO_BRACO_ESQUERDO = "botao-braco-esquerdo";
        private Button botaoBracoEsquerdo;

        private const string NOME_BOTAO_ANTEBRACO_ESQUERDO = "botao-antebraco-esquerdo";
        private Button botaoAntebracoEsquerdo;

        private const string NOME_BOTAO_BRACO_DIREITO = "botao-braco-direito";
        private Button botaoBracoDireito;

        private const string NOME_BOTAO_ANTEBRACO_DIREITO = "botao-antebraco-direito";
        private Button botaoAntebracoDireito;

        private const string NOME_BOTAO_PERNA_ESQUERDA = "botao-perna-esquerda";
        private Button botaoPernaEsquerda;

        private const string NOME_BOTAO_PERNA_INFERIOR_ESQUERDA = "botao-perna-inferior-esquerda";
        private Button botaoPernaInferiorEsquerda;

        private const string NOME_BOTAO_PERNA_DIREITA = "botao-perna-direita";
        private Button botaoPernaDireita;

        private const string NOME_BOTAO_PERNA_INFERIOR_DIREITA = "botao-perna-inferior-direita";
        private Button botaoPernaInferiorDireita;

        private const string NOME_BOTAO_ROTACIONAR_ESQUERDA = "botao-rotacionar-esquerda";
        private Button botaoRotacionarEsquerda;

        private const string NOME_BOTAO_ROTACIONAR_DIREITA = "botao-rotacionar-direita";
        private Button botaoRotacionarDireita;

        #endregion

        private GameObject personagem;
        private IdentificadorTipoControle tipoControle;

        private Transform parteSelecionada;
        private ControleDireto controlePersonagem;

        private void Awake() {
            personagem = GameObject.FindGameObjectWithTag(NomesTags.Personagem);
            tipoControle = personagem.GetComponent<IdentificadorTipoControle>();

            if(personagem == null || tipoControle.Tipo != TipoControle.Direto) {
                gameObject.SetActive(false);
                return;
            }

            Root.styleSheets.Add(style);

            botaoBracoEsquerdo = Root.Query<Button>(NOME_BOTAO_BRACO_ESQUERDO);
            botaoAntebracoEsquerdo = Root.Query<Button>(NOME_BOTAO_ANTEBRACO_ESQUERDO);

            botaoBracoDireito = Root.Query<Button>(NOME_BOTAO_BRACO_DIREITO);
            botaoAntebracoDireito = Root.Query<Button>(NOME_BOTAO_ANTEBRACO_DIREITO);

            botaoPernaEsquerda = Root.Query<Button>(NOME_BOTAO_PERNA_ESQUERDA);
            botaoPernaInferiorEsquerda = Root.Query<Button>(NOME_BOTAO_PERNA_INFERIOR_ESQUERDA);

            botaoPernaDireita = Root.Query<Button>(NOME_BOTAO_PERNA_DIREITA);
            botaoPernaInferiorDireita = Root.Query<Button>(NOME_BOTAO_PERNA_INFERIOR_DIREITA);

            botaoRotacionarEsquerda = Root.Query<Button>(NOME_BOTAO_ROTACIONAR_ESQUERDA);
            botaoRotacionarDireita = Root.Query<Button>(NOME_BOTAO_ROTACIONAR_DIREITA);

            return;
        }

        private void Start() {
            controlePersonagem = personagem.GetComponent<ControleDireto>();
            ConfigurarBotoes();

            return;
        }

        private void ConfigurarBotoes() {
            botaoBracoEsquerdo.clicked += () => { parteSelecionada = controlePersonagem.BracoEsquerdo; };
            botaoAntebracoEsquerdo.clicked += () => { parteSelecionada = controlePersonagem.AntebracoEsquerdo;  };

            botaoBracoDireito.clicked += () => { parteSelecionada = controlePersonagem.BracoDireito; };
            botaoAntebracoDireito.clicked += () => { parteSelecionada = controlePersonagem.AntebracoDireito; };

            botaoPernaEsquerda.clicked += () => { parteSelecionada = controlePersonagem.PernaEsquerda; };
            botaoPernaInferiorEsquerda.clicked += () => { parteSelecionada = controlePersonagem.PernaInferiorEsquerda; };

            botaoPernaDireita.clicked += () => { parteSelecionada = controlePersonagem.PernaDireita; };
            botaoPernaInferiorDireita.clicked += () => { parteSelecionada = controlePersonagem.PernaInferiorDireita; };

            botaoRotacionarEsquerda.clicked += () => { controlePersonagem.RotacionarSentidoAntiHorario(parteSelecionada); };
            botaoRotacionarDireita.clicked += () => { controlePersonagem.RotacionarSentidoHorario(parteSelecionada); };

            return;
        }
    }
}