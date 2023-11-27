using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.UI;
using Autis.Runtime.Constantes;
using Autis.Editor.Editores;
using Autis.Editor.Manipuladores;
using Autis.Editor.Utils;
using Autis.Runtime.Eventos;
using UnityEditor;

namespace Autis.Editor.Telas {
    public class EditarElementoBehaviour : Tela {
        protected override string CaminhoTemplate => "Telas/EditarElemento/EditarElementoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/EditarElemento/EditarElementoStyle.uss";

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_TIPO_PERSONAGEM_NAO_ENCONTRADO = "[ERRO]: O tipo do personagem não encontrado.";

        #endregion

        #region .: Eventos :.

        protected static EventoJogo eventoIniciarEdicao;

        #endregion

        #region .: Elementos :.

        private const string NOME_REGIAO_CONTEUDO_PRINCIPAL = "regiao-conteudo-principal";
        private ScrollView scrollViewConteudoPrincipal;

        private const string NOME_REGIAO_CARREGAMENTO_LISTA_PERSONAGEM = "regiao-carregamento-lista-personagens";
        private VisualElement regiaoCarregamentoListaPersonagem;

        private const string NOME_LISTA_PERSONAGENS = "lista-personagens";
        private const string NOME_FOLDOUT_LISTA_PERSONAGENS = "foldout-lista-personagens";
        private ListaElementos listaPersonagens;

        private const string NOME_REGIAO_CARREGAMENTO_LISTA_CENARIO = "regiao-carregamento-lista-cenario";
        private VisualElement regiaoCarregamentoListaCenario;

        private const string NOME_LISTA_CENARIO = "lista-cenario";
        private const string NOME_FOLDOUT_LISTA_CENARIO = "foldout-lista-cenario";
        private ListaElementos listaCenario;

        private const string NOME_REGIAO_CARREGAMENTO_LISTA_INSTRUCOES = "regiao-carregamento-lista-instrucoes";
        private VisualElement regiaoCarregamentoListaInstrucoes;

        private const string NOME_LISTA_INSTRUCOES = "lista-instrucoes";
        private const string NOME_FOLDOUT_LISTA_INSTRUCOES = "foldout-lista-instrucoes";
        private ListaElementos listaInstrucoes;

        private const string NOME_REGIAO_CARREGAMENTO_LISTA_ELEMENTOS_INTERACAO = "regiao-carregamento-lista-elementos-interacao";
        private VisualElement regiaoCarregamentoListaElementosInteracao;

        private const string NOME_LISTA_ELEMENTOS_INTERACAO = "lista-elementos-interacao";
        private const string NOME_FOLDOUT_LISTA_ELEMENTOS_INTERACAO = "foldout-lista-elementos-interacao";
        private ListaElementos listaElementosInteracao;

        private const string NOME_REGIAO_CARREGAMENTO_LISTA_REFORCOS = "regiao-carregamento-lista-reforcos";
        private VisualElement regiaoCarregamentoListaReforcos;

        private const string NOME_LISTA_REFORCOS = "lista-reforcos";
        private const string NOME_FOLDOUT_LISTA_REFORCOS = "foldout-lista-reforcoes";
        private ListaElementos listaReforcos;

        private const string NOME_REGIAO_CARREGAMENTO_LISTA_APOIOS = "regiao-carregamento-lista-apoios";
        private VisualElement regiaoCarregamentoListaApoios;

        private const string NOME_LISTA_APOIOS = "lista-apoios";
        private const string NOME_FOLDOUT_LISTA_APOIOS = "foldout-lista-apoios";
        private ListaElementos listaApoios;

        private const string NOME_BOTAO_ESTRUTURA_FASE = "botao-estrutura-fase";
        private Button botaoEstruturaFase;

        private const string NOME_ICONE_BOTAO_ESTRUTURA_FASE = "icone-botao-estrutura-fase";
        private Image iconeBotaoEstruturaFase;

        private const string NOME_LABEL_ESTRUTURA_FASE = "label-nome-estrutura-fase";
        private Label labelEstruturaFase;

        #endregion

        private ManipuladorCena manipuladorCena;

        public EditarElementoBehaviour() {
            eventoIniciarEdicao = Importador.ImportarEvento("EventoIniciarEdicao");

            ConfigurarRegiaoConteudoPrincipal();
            ConfigurarBotaoEditarCenario();
            ConfigurarBotaoEditarPersonagem();
            ConfigurarListaApoios();
            ConfigurarListaElementosInteracao();
            ConfigurarListaInstrucoes();
            ConfigurarListaReforcos();
            ConfigurarBotaoEstruturaFase();

            return;
        }

        public override void OnEditorUpdate() {
            DefinirFerramenta();
            return;
        }

        protected virtual void DefinirFerramenta() {
            if(Tools.current != Tool.Rect) {
                Tools.current = Tool.Rect;
                return;
            }

            return;
        }

        private void ConfigurarRegiaoConteudoPrincipal() {
            scrollViewConteudoPrincipal = root.Query<ScrollView>(NOME_REGIAO_CONTEUDO_PRINCIPAL);
            scrollViewConteudoPrincipal.mode = ScrollViewMode.Vertical;
            scrollViewConteudoPrincipal.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            scrollViewConteudoPrincipal.nestedInteractionKind = ScrollView.NestedInteractionKind.StopScrolling;

            return;
        }

        private void ConfigurarBotaoEditarCenario() {
            GameObject cenario = GameObject.FindGameObjectWithTag(NomesTags.Cenario);
            
            List<GameObject> cenarios = new();
            if(cenario != null) {
                cenarios.Add(cenario);
            }

            listaCenario = new ListaElementos("Cenario:", cenarios);

            listaCenario.Foldout.name = NOME_FOLDOUT_LISTA_CENARIO;
            listaCenario.ListView.name = NOME_LISTA_CENARIO;

            void bindItem(VisualElement elemento, int index) {
                elemento.Clear();

                ItemListaElementosBehaviour item = new(listaCenario.Objetos[index]);
                
                item.OnEditarClick = (() => {
                    eventoIniciarEdicao.AcionarCallbacks();
                    Navigator.Instance.IrPara(new EditorCenarioBehaviour(listaCenario.Objetos[index]) {
                        OnConfirmarEdicao = ((novoObjeto) => {
                            listaCenario.Objetos[index] = novoObjeto;
                            item.AlterarObjetoVinculado(novoObjeto);
                        }),
                    });
                });

                item.OnExcluirClick = (() => {
                    ManipuladorCenario manipuladorCenario = new();
                    
                    manipuladorCenario.SetObjeto(item.ObjetoVinculado);
                    manipuladorCenario.Excluir();

                    listaCenario.RemoverItem(item.ObjetoVinculado);
                });

                elemento.Add(item.Root);
            }

            listaCenario.ListView.bindItem = bindItem;

            regiaoCarregamentoListaCenario = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_LISTA_CENARIO);
            regiaoCarregamentoListaCenario.Add(listaCenario.Root);

            return;
        }

        private void ConfigurarBotaoEditarPersonagem() {
            GameObject personagem = GameObject.FindGameObjectWithTag(NomesTags.Personagem);

            List<GameObject> personagens = new();
            if(personagem != null) {
                personagens.Add(personagem);
            }

            listaPersonagens = new ListaElementos("Personagem:", personagens);

            listaPersonagens.Foldout.name = NOME_FOLDOUT_LISTA_PERSONAGENS;
            listaPersonagens.ListView.name = NOME_LISTA_PERSONAGENS;

            void bindItem(VisualElement elemento, int index) {
                elemento.Clear();

                ItemListaElementosBehaviour item = new(listaPersonagens.Objetos[index]);
                
                item.OnEditarClick = (() => {
                    eventoIniciarEdicao.AcionarCallbacks();
                    Navigator.Instance.IrPara(new EditorPersonagemBehaviour(listaPersonagens.Objetos[index]) {
                        OnConfirmarEdicao = ((novoObjeto) => {
                            listaPersonagens.Objetos[index] = novoObjeto;
                            item.AlterarObjetoVinculado(novoObjeto);
                        }),
                    });
                });

                item.OnExcluirClick = (() => {
                    ManipuladorPersonagens manipuladorPersonagen = ManipuladorPersonagens.GetManipuladorPersonagem(item.ObjetoVinculado);

                    if(manipuladorPersonagen == null) {
                        Debug.Log(MENSAGEM_ERRO_TIPO_PERSONAGEM_NAO_ENCONTRADO);
                    }

                    manipuladorPersonagen.SetObjeto(item.ObjetoVinculado);
                    manipuladorPersonagen.Excluir();

                    listaPersonagens.RemoverItem(item.ObjetoVinculado);
                });

                elemento.Add(item.Root);
            }

            listaPersonagens.ListView.bindItem = bindItem;

            regiaoCarregamentoListaPersonagem = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_LISTA_PERSONAGEM);
            regiaoCarregamentoListaPersonagem.Add(listaPersonagens.Root);

            return;
        }

        private void ConfigurarListaInstrucoes() {
            List<GameObject> instrucoes = GameObject.FindGameObjectsWithTag(NomesTags.Instrucoes).ToList();
            listaInstrucoes = new ListaElementos("Instruções:", instrucoes);

            listaInstrucoes.Foldout.name = NOME_FOLDOUT_LISTA_INSTRUCOES;
            listaInstrucoes.ListView.name = NOME_LISTA_INSTRUCOES;

            void bindItem(VisualElement elemento, int index) {
                elemento.Clear();

                ItemListaElementosBehaviour item = new(listaInstrucoes.Objetos[index]);

                item.OnEditarClick = (() => {
                    eventoIniciarEdicao.AcionarCallbacks();
                    Navigator.Instance.IrPara(new EditorInstrucoesBehaviour(listaInstrucoes.Objetos[index]) {
                        OnConfirmarEdicao = ((novoObjeto) => {
                            listaInstrucoes.Objetos[index] = novoObjeto;
                            item.AlterarObjetoVinculado(novoObjeto);
                        }),
                    });
                });

                item.OnExcluirClick = (() => {
                    ManipuladorInstrucoes manipuladorInstrucoes = new();

                    manipuladorInstrucoes.SetObjeto(item.ObjetoVinculado);
                    manipuladorInstrucoes.Excluir();

                    listaInstrucoes.RemoverItem(item.ObjetoVinculado);
                });

                elemento.Add(item.Root);
            }

            listaInstrucoes.ListView.bindItem = bindItem;

            regiaoCarregamentoListaInstrucoes = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_LISTA_INSTRUCOES);
            regiaoCarregamentoListaInstrucoes.Add(listaInstrucoes.Root);

            return;
        }

        private void ConfigurarListaElementosInteracao() {
            List<GameObject> elementosInteracao = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();
            listaElementosInteracao = new ListaElementos("Elementos Interação:", elementosInteracao);

            listaElementosInteracao.Foldout.name = NOME_FOLDOUT_LISTA_ELEMENTOS_INTERACAO;
            listaElementosInteracao.ListView.name = NOME_LISTA_ELEMENTOS_INTERACAO;

            void bindItem(VisualElement elemento, int index) {
                elemento.Clear();

                ItemListaElementosBehaviour item = new(listaElementosInteracao.Objetos[index]);

                item.OnEditarClick = (() => {
                    eventoIniciarEdicao.AcionarCallbacks();
                    Navigator.Instance.IrPara(new EditorObjetoInteracaoBehaviour(listaElementosInteracao.Objetos[index]) {
                        OnConfirmarEdicao = ((novoObjeto) => {
                            listaElementosInteracao.Objetos[index] = novoObjeto;
                            item.AlterarObjetoVinculado(novoObjeto);
                        }),
                    });
                });

                item.OnExcluirClick = (() => {
                    // TODO: Sincronizar a exclusão para pegar os apoios também
                    ManipuladorObjetoInteracao manipuladorObjetoInteracao = new();
                    manipuladorObjetoInteracao.SetObjeto(item.ObjetoVinculado);

                    List<ManipuladorApoio> manipuladorApoios = manipuladorObjetoInteracao.ApoiosVinculados;
                    foreach(ManipuladorApoio manipuladorApoio in manipuladorApoios) {
                        listaApoios.RemoverItem(manipuladorApoio.ObjetoAtual);
                        manipuladorApoio.Excluir();
                    }

                    listaElementosInteracao.RemoverItem(item.ObjetoVinculado);
                    manipuladorObjetoInteracao.Excluir();

                    // TODO: Sincronizar para atualizar o máximo de acerto no gabarito
                });

                elemento.Add(item.Root);
            }


            listaElementosInteracao.ListView.bindItem = bindItem;

            regiaoCarregamentoListaElementosInteracao = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_LISTA_ELEMENTOS_INTERACAO);
            regiaoCarregamentoListaElementosInteracao.Add(listaElementosInteracao.Root);

            return;
        }

        private void ConfigurarListaReforcos() {
            List<GameObject> reforcos = GameObject.FindGameObjectsWithTag(NomesTags.Reforcos).ToList();
            listaReforcos = new ListaElementos("Reforços:", reforcos);

            listaReforcos.Foldout.name = NOME_FOLDOUT_LISTA_REFORCOS;
            listaReforcos.ListView.name = NOME_LISTA_REFORCOS;

            void bindItem(VisualElement elemento, int index) {
                elemento.Clear();

                ItemListaElementosBehaviour item = new(listaReforcos.Objetos[index]);

                item.OnEditarClick = (() => {
                    eventoIniciarEdicao.AcionarCallbacks();
                    Navigator.Instance.IrPara(new EditorReforcoBehaviour(listaReforcos.Objetos[index]) {
                        OnConfirmarEdicao = ((novoObjeto) => {
                            listaReforcos.Objetos[index] = novoObjeto;
                            item.AlterarObjetoVinculado(novoObjeto);
                        }),
                    });
                });

                item.OnExcluirClick = (() => {
                    ManipuladorReforco manipuladorReforco = new();

                    manipuladorReforco.SetObjeto(item.ObjetoVinculado);
                    manipuladorReforco.Excluir();

                    listaReforcos.RemoverItem(item.ObjetoVinculado);
                });

                elemento.Add(item.Root);
            }

            listaReforcos.ListView.bindItem = bindItem;

            regiaoCarregamentoListaReforcos = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_LISTA_REFORCOS);
            regiaoCarregamentoListaReforcos.Add(listaReforcos.Root);

            return;
        }

        private void ConfigurarListaApoios() {
            List<GameObject> apoios = GameObject.FindGameObjectsWithTag(NomesTags.Apoios).ToList();
            listaApoios = new ListaElementos("Apoios:", apoios);

            listaApoios.Foldout.name = NOME_FOLDOUT_LISTA_APOIOS;
            listaApoios.ListView.name = NOME_LISTA_APOIOS;

            void bindItem(VisualElement elemento, int index) {
                elemento.Clear();

                ItemListaElementosBehaviour item = new(listaApoios.Objetos[index]);

                item.OnEditarClick = (() => {
                    eventoIniciarEdicao.AcionarCallbacks();
                    Navigator.Instance.IrPara(new EditorApoioBehaviour(listaApoios.Objetos[index]) {
                        OnConfirmarEdicao = ((novoObjeto) => {
                            listaApoios.Objetos[index] = novoObjeto;
                            item.AlterarObjetoVinculado(novoObjeto);
                        }),
                    });
                });

                item.OnExcluirClick = (() => {
                    ManipuladorApoio manipuladorApoio = new();

                    manipuladorApoio.SetObjeto(item.ObjetoVinculado);
                    manipuladorApoio.Excluir();

                    listaApoios.RemoverItem(item.ObjetoVinculado);
                });

                elemento.Add(item.Root);
            }

            listaApoios.ListView.bindItem = bindItem;

            regiaoCarregamentoListaApoios = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_LISTA_APOIOS);
            regiaoCarregamentoListaApoios.Add(listaApoios.Root);

            return;
        }

        private void ConfigurarBotaoEstruturaFase() {
            manipuladorCena = new ManipuladorCena();

            iconeBotaoEstruturaFase = root.Query<Image>(NOME_ICONE_BOTAO_ESTRUTURA_FASE);
            iconeBotaoEstruturaFase.image = Importador.ImportarImagem("Cubo.png");

            labelEstruturaFase = root.Query<Label>(NOME_LABEL_ESTRUTURA_FASE);
            labelEstruturaFase.text = "Fase atual";
            // TODO: labelEstruturaFase.text = manipuladorCena.GetNome();

            botaoEstruturaFase = root.Query<Button>(NOME_BOTAO_ESTRUTURA_FASE);
            botaoEstruturaFase.clicked += HandleBotaoEstruturaFaseClick;

            return;
        }

        private void HandleBotaoEstruturaFaseClick() {
            eventoIniciarEdicao.AcionarCallbacks();
            Navigator.Instance.IrPara(new EditarInformacoesCenaBehaviour() {
                OnConfirmarEdicao = ((manipuladorCena) => {
                    labelEstruturaFase.text = manipuladorCena.GetNome();
                }),
            });

            return;
        }
    }
}