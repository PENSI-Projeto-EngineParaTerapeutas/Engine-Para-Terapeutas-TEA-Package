using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using Autis.Runtime.Constantes;
using Autis.Editor.DTOs;
using Autis.Runtime.DTOs;
using Autis.Runtime.ComponentesGameObjects;

namespace Autis.Editor.Manipuladores {
    public abstract class ManipuladorPersonagens : ManipuladorObjetos, IExcluir {
        #region .: Mensagens :.

        protected const string MENSAGEM_ERRO_CARREGAR_CONTROLLER_PERSONAGEM = "[ERROR]: Não foi possível achar a referência para o Controlador de Animação para o Personagem. Certifique-se de que o arquivo {nome-controller} ControllerAvatar.controller está localizado em {local}.";
        protected const string MENSAGEM_ERRO_CARREGAR_ANIMACOES = "[ERROR]: Não foi possível carregar as animações do Personagem. Certifique-se de que as Animações estão localizadas em {local}.";

        #endregion

        #region .: Componentes :.

        public List<SpriteRenderer> SpritesPersonagem { get => spritesPersonagem; }
        protected List<SpriteRenderer> spritesPersonagem;

        public DadosPersonagem Dados { get => dados; }
        protected DadosPersonagem dados;

        public ControleDireto ControleDiretoComponente { get => controleDiretoComponente; }
        protected ControleDireto controleDiretoComponente;

        public ControleIndireto ControleIndiretoComponente { get => controleIndiretoComponente; }
        protected ControleIndireto controleIndiretoComponente;

        public List<AcaoPersonagem> AssociacoesAcoesControleIndireto { get => associacoesAcoesControleIndireto; }
        protected readonly List<AcaoPersonagem> associacoesAcoesControleIndireto;

        public Animator AnimatorComponente { get => animatorComponente; }
        protected Animator animatorComponente;

        public Animation AnimationComponente { get => animationComponente; }
        protected Animation animationComponente;

        public SortingGroup SortingGroupComponente { get => sortingGroupComponente; }
        protected SortingGroup sortingGroupComponente;

        #endregion

        protected static readonly List<AcaoPersonagem> associacaoAcoesOriginal = new();

        public static ManipuladorPersonagens GetManipuladorPersonagem(GameObject objeto) {
            ManipuladorPersonagens manipuladorPersonagem = null;

            TiposPersonagem tipoPersonagem = objeto.GetComponent<DadosPersonagem>().tipoPersonagem;
            switch(tipoPersonagem) {
                case(TiposPersonagem.Avatar): {
                    manipuladorPersonagem = new ManipuladorAvatar();
                    break;
                }
                case(TiposPersonagem.BonecoPalito): {
                    manipuladorPersonagem = new ManipuladorBonecoPalito();
                    break;
                }
                case(TiposPersonagem.Ludico): {
                    manipuladorPersonagem = new ManipuladorPersonagemLudico();
                    break;
                }
            }

            return manipuladorPersonagem;
        }

        public ManipuladorPersonagens() {
            associacoesAcoesControleIndireto = new List<AcaoPersonagem>();
            return;
        }

        public ManipuladorPersonagens(GameObject prefabObjeto) : base(prefabObjeto) {
            associacoesAcoesControleIndireto = new List<AcaoPersonagem>();
            return;
        }

        public void Excluir() {
            ExcluirInterno();
            // TODO: Remover objetos de interação com ações do controle indireto
            return;
        }

        protected abstract void ExcluirInterno();

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);
            objetoAlvo.name = "Personagem";

            dados = objeto.GetComponent<DadosPersonagem>();
            
            controleDiretoComponente = objeto.GetComponent<ControleDireto>();
            controleIndiretoComponente = objeto.GetComponent<ControleIndireto>();
            
            spritesPersonagem = objeto.GetComponentsInChildren<SpriteRenderer>().ToList();

            animatorComponente = objeto.GetComponent<Animator>();
            animationComponente = objeto.GetComponent<Animation>();

            sortingGroupComponente = objeto.GetComponent<SortingGroup>();
            sortingGroupComponente.sortingOrder = OrdemRenderizacao.EmCriacao;

            foreach(Transform parte in objeto.transform) {
                SceneVisibilityManager.instance.DisablePicking(parte.gameObject, true);
            }

            return;
        }

        public override void Cancelar() {
            base.Cancelar();
            
            LimparAcoesControleIndireto();

            return;
        }

        public void CancelarEdicao() {
            base.Cancelar();

            foreach(AcaoPersonagem vinculoAcao in associacaoAcoesOriginal) {
                AcionadorAcaoPersonagem acionador = vinculoAcao.ObjetoGatilho.GetComponent<AcionadorAcaoPersonagem>();
                acionador.animacaoAcionada = vinculoAcao.Animacao;
            }

            return;
        }

        protected override void FinalizarInterno() {
            objeto.tag = NomesTags.Personagem;
            objeto.layer = LayersProjeto.Default.Index;
            sortingGroupComponente.sortingOrder = OrdemRenderizacao.Personagem;

            CarregarController();
            AtribuirVinculoAcoesControleIndireto();

            objeto = null;

            return;
        }

        protected abstract void CarregarController();

        private void AtribuirVinculoAcoesControleIndireto() {
            if(!PossuiPersonagemSelecionado() || dados.tipoControle != TipoControle.Indireto) {
                return;
            }

            foreach(AcaoPersonagem vinculoAcao in associacoesAcoesControleIndireto) {
                AcionadorAcaoPersonagem acionador = vinculoAcao.ObjetoGatilho.GetComponent<AcionadorAcaoPersonagem>();
                acionador.animacaoAcionada = vinculoAcao.Animacao;
            }

            return;
        }

        public void AlterarTipoControle(TipoControle tipoControle) {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            dados.tipoControle = tipoControle; 
            DesabilitarComponentesControle();

            switch(tipoControle) {
                case(TipoControle.Direto): {
                    controleDiretoComponente.enabled = true;
                    break;
                }
                case(TipoControle.Indireto): {
                    controleIndiretoComponente.enabled = true;
                    animatorComponente.enabled = true;
                    animationComponente.enabled = true;
                    break;
                }
            }

            return;
        }

        public void DesabilitarComponentesControle() {
            if(!PossuiPersonagemSelecionado()) {
                return;
            }

            controleDiretoComponente.enabled = false;
            controleIndiretoComponente.enabled = false;
            animatorComponente.enabled = false;
            animationComponente.enabled = false;

            return;
        }

        public void CarregarAcoesControleIndireto() {
            associacaoAcoesOriginal.Clear();

            List<GameObject> elementosInteracao = GameObject.FindGameObjectsWithTag(NomesTags.ObjetosInteracao).ToList();

            foreach(GameObject elementoInteracao in elementosInteracao) {
                AcionadorAcaoPersonagem acionador = elementoInteracao.GetComponent<AcionadorAcaoPersonagem>();
                if(acionador.animacaoAcionada == null) {
                    continue;
                }

                associacoesAcoesControleIndireto.Add(new AcaoPersonagem() {
                    Animacao = acionador.animacaoAcionada,
                    ObjetoGatilho = acionador.gameObject,
                });

                associacaoAcoesOriginal.Add(new AcaoPersonagem() {
                    Animacao = acionador.animacaoAcionada,
                    ObjetoGatilho = acionador.gameObject,
                });
            }

            return;
        }

        public void AdicionarAcaoControleIndireto(AcaoPersonagem acao) {
            associacoesAcoesControleIndireto.Add(acao);
            return;
        }

        public void RemoverAcaoControleIndireto(AcaoPersonagem acao) {
            AcaoPersonagem acaoAlvo = associacoesAcoesControleIndireto.Find(associacao => associacao.Animacao == acao.Animacao && associacao.ObjetoGatilho == associacao.ObjetoGatilho);
            if(acaoAlvo == null) {
                return;
            }

            associacoesAcoesControleIndireto.Remove(acaoAlvo);

            AcionadorAcaoPersonagem acionador = acaoAlvo.ObjetoGatilho.GetComponent<AcionadorAcaoPersonagem>();
            acionador.animacaoAcionada = null;

            return;
        }

        public void LimparAcoesControleIndireto() {
            foreach(AcaoPersonagem acao in associacoesAcoesControleIndireto) {
                AcionadorAcaoPersonagem acionador = acao.ObjetoGatilho.GetComponent<AcionadorAcaoPersonagem>();
                acionador.animacaoAcionada = null;
            }

            associacoesAcoesControleIndireto.Clear();
            return;
        }

        public bool PossuiPersonagemSelecionado() {
            return controleDiretoComponente.PartesCorpo.Count > 0;
        }

        public TipoControle GetTipoControle() {
            return dados.tipoControle;
        }

        public TiposPersonagem GetTipoPersonagem() {
            return dados.tipoPersonagem;
        }

        public abstract List<AnimationClip> GetAnimacoes();
    }
}