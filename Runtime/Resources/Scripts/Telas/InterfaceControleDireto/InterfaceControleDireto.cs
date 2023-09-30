using UnityEngine;
using UnityEngine.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.Telas { 
    public class InterfaceControleDireto : TelaJogo {
        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_BOTOES = "regiao-botoes-selecao-parte";
        private ScrollView regiaoCarregamentoBoteoes;

        private const string NOME_BOTAO_ROTACIONAR_ESQUERDA = "botao-rotacionar-esquerda";
        private Button botaoRotacionarEsquerda;

        private const string NOME_BOTAO_ROTACIONAR_DIREITA = "botao-rotacionar-direita";
        private Button botaoRotacionarDireita;

        #endregion

        private GameObject personagem;
        private DadosPersonagem dados;

        private Transform parteSelecionada;
        private ControleDireto controlePersonagem;

        protected override void Awake() {
            base.Awake();

            personagem = GameObject.FindGameObjectWithTag(NomesTags.Personagem);
            if(personagem == null) {
                gameObject.SetActive(false);
                return;
            }

            dados = personagem.GetComponent<DadosPersonagem>();
            if(dados.tipoControle != TipoControle.Direto) {
                gameObject.SetActive(false);
                return;
            }

            regiaoCarregamentoBoteoes = Root.Query<ScrollView>(NOME_REGIAO_CARREGAMENTO_BOTOES);

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
            foreach(ParteControleDireto membro in controlePersonagem.PartesCorpo) {
                Button botao = new() {
                    name = membro.NomeDisplay,
                    text = membro.NomeDisplay,
                };

                botao.clicked += () => { parteSelecionada = membro.ParteCorpo; };

                regiaoCarregamentoBoteoes.Add(botao);
            }

            botaoRotacionarEsquerda.clicked += () => { controlePersonagem.RotacionarSentidoAntiHorario(parteSelecionada); };
            botaoRotacionarDireita.clicked += () => { controlePersonagem.RotacionarSentidoHorario(parteSelecionada); };

            return;
        }
    }
}