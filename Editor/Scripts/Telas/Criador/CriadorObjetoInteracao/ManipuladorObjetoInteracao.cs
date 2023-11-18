using System.Collections.Generic;
using UnityEngine;
using Autis.Editor.Utils;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorObjetoInteracao : ManipuladorObjetos, IExcluir {
        private const string CAMINHO_PREFAB_OBJETO_INTERACAO = "ObjetosInteracao/ObjetoInteracao.prefab";

        #region .: Componentes :.

        public SpriteRenderer ComponenteSpriteRenderer { get => componenteSpriteRenderer; }
        private SpriteRenderer componenteSpriteRenderer;

        public Texto ComponenteTexto { get => componenteTexto; }
        private Texto componenteTexto;

        public Arrastavel ComponenteArrastavel { get => componenteArrastavel; }
        private Arrastavel componenteArrastavel;

        public VerificacaoGabaritoArrastar ComponenteVerificacaoGabaritoArrastar { get => componenteVerificacaoGabaritoArrastar; }
        private VerificacaoGabaritoArrastar componenteVerificacaoGabaritoArrastar;

        public AcionadorApoios ComponenteAcionadorApoios { get => componenteAcionadorApoios; }
        private AcionadorApoios componenteAcionadorApoios;

        public VerificacaoGabaritoSelecao ComponenteVerificacaoGabaritoSelecao { get => componenteVerificacaoGabaritoSelecao; }
        private VerificacaoGabaritoSelecao componenteVerificacaoGabaritoSelecao;

        public AcionadorAcaoPersonagem GatilhoAcaoPersonagem { get => gatilhoAcaoPersonagem; }
        private AcionadorAcaoPersonagem gatilhoAcaoPersonagem;

        #endregion

        #region .: Manipuladores Componentes :.

        public ManipuladorSpriteRenderer ManipuladorSpriteRenderer { get => manipuladorSpriteRenderer; }
        private ManipuladorSpriteRenderer manipuladorSpriteRenderer;

        public ManipuladorTexto ManipuladorTexto { get => manipuladorTexto; }
        private ManipuladorTexto manipuladorTexto;

        #endregion

        public List<ManipuladorApoio> ApoiosVinculados { get => apoiosVinculados; }
        private readonly List<ManipuladorApoio> apoiosVinculados = new();

        public ManipuladorObjetoInteracao() {
            prefabObjeto = Importador.ImportarPrefab(CAMINHO_PREFAB_OBJETO_INTERACAO);
            return;
        }

        public ManipuladorObjetoInteracao(GameObject prefabAtor) : base(prefabAtor) { }

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);

            CarregarComponentes();
            CarregarApoiosVinculados();

            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.EmCriacao;

            return;
        }

        private void CarregarApoiosVinculados() {
            foreach(Transform child in objeto.transform) {
                if(!child.CompareTag(NomesTags.Apoios)) {
                    continue;
                }

                ManipuladorApoio manipuladorApoio = new(child.gameObject);
                apoiosVinculados.Add(manipuladorApoio);
            }

            return;
        }

        private void CarregarComponentes() {
            componenteSpriteRenderer = objeto.GetComponent<SpriteRenderer>();
            componenteTexto = objeto.GetComponent<Texto>();

            componenteArrastavel = objeto.GetComponent<Arrastavel>();
            componenteVerificacaoGabaritoArrastar = objeto.GetComponent<VerificacaoGabaritoArrastar>();

            componenteAcionadorApoios = objeto.GetComponent<AcionadorApoios>();
            componenteVerificacaoGabaritoSelecao = objeto.GetComponent<VerificacaoGabaritoSelecao>();

            gatilhoAcaoPersonagem = objeto.GetComponent<AcionadorAcaoPersonagem>();

            manipuladorSpriteRenderer = new ManipuladorSpriteRenderer(componenteSpriteRenderer);
            manipuladorTexto = new ManipuladorTexto(componenteTexto);
            
            return;
        }

        public override void SetObjeto(GameObject objeto) {
            base.SetObjeto(objeto);

            CarregarComponentes();
            CarregarApoiosVinculados();

            return;
        }

        protected override void FinalizarInterno() {
            objeto.tag = NomesTags.ObjetosInteracao;
            objeto.layer = LayersProjeto.Default.Index;

            componenteSpriteRenderer.sortingOrder = (GetTipoInteracao() == TiposAcoes.Nenhuma) ? OrdemRenderizacao.ObjetoInteracaoSimples : OrdemRenderizacao.ObjetoInteracao;

            objeto = null;

            return;
        }

        public void Excluir() {
            RemoverApoiosVinculados();

            GameObject.DestroyImmediate(objeto);
            RemoverVinculo();

            // TODO: Atualizar o gabarito
            return;
        }

        protected void RemoverVinculo() {
            objeto = null;

            componenteSpriteRenderer = null;
            componenteTexto = null;

            componenteArrastavel = null;
            componenteVerificacaoGabaritoArrastar = null;

            componenteAcionadorApoios = null;
            componenteVerificacaoGabaritoSelecao = null;

            gatilhoAcaoPersonagem = null;

            manipuladorSpriteRenderer = null;
            manipuladorTexto = null;

            return;
        }

        private void RemoverApoiosVinculados() {
            if(objeto == null) {
                return;
            }

            foreach(ManipuladorApoio manipuladorApoio in apoiosVinculados) {
                manipuladorApoio.Excluir();
            }

            return;
        }

        public void SetTipoInteracao(TiposAcoes tipo) {
            if(objeto == null) {
                return;
            }

            DesabilitarInteracao();
            switch(tipo) {
                case(TiposAcoes.Arrastavel): {
                    HabilitarInteracaoArrastar();
                    break;
                }
                case(TiposAcoes.Selecionavel): {
                    HabilitarInteracaoSelecionar();
                    break;
                }
            }

            return;
        }

        public void DesabilitarInteracao() {
            componenteArrastavel.habilitado = false;
            componenteArrastavel.enabled = false;

            componenteVerificacaoGabaritoArrastar.habilitado = false;
            componenteVerificacaoGabaritoArrastar.enabled = false;

            componenteVerificacaoGabaritoSelecao.habilitado = false;
            componenteVerificacaoGabaritoSelecao.enabled = false;

            return;
        }

        private void HabilitarInteracaoArrastar() {
            componenteArrastavel.habilitado = true;
            componenteArrastavel.enabled = true;

            componenteVerificacaoGabaritoArrastar.habilitado = true;
            componenteVerificacaoGabaritoArrastar.enabled = true;

            return;
        }

        private void HabilitarInteracaoSelecionar() {
            componenteVerificacaoGabaritoSelecao.habilitado = true;
            componenteVerificacaoGabaritoSelecao.enabled = true;

            return;
        }

        public void HabilitarAcionamentoApoios(bool habilitado) {
            componenteAcionadorApoios.habilitado = habilitado;
            componenteAcionadorApoios.enabled = habilitado;

            return;
        }

        public TiposAcoes GetTipoInteracao() {
            if(componenteArrastavel.habilitado) {
                return TiposAcoes.Arrastavel;
            }

            if(componenteVerificacaoGabaritoSelecao.habilitado) {
                return TiposAcoes.Selecionavel;
            }

            return TiposAcoes.Nenhuma;
        }

        public void AlterarTipo(TiposObjetosInteracao tipo) {
            if(objeto == null) {
                return;
            }

            DesabilitarComponentes();
            switch(tipo) {
                case(TiposObjetosInteracao.Imagem): {
                    HabilitarComponentesImagem();
                    break;
                }
                case(TiposObjetosInteracao.Texto): {
                    HabilitarComponentesTexto();
                    break;
                }
            }

            return;
        }

        public void DesabilitarComponentes() {
            if(objeto == null) {
                return;
            }

            componenteSpriteRenderer.enabled = false;
            componenteTexto.Habilitado = false;

            return;
        }

        private void HabilitarComponentesImagem() {
            componenteSpriteRenderer.enabled = true;
            return;
        }

        private void HabilitarComponentesTexto() {
            componenteTexto.Habilitado = true;
            return;
        }

        public TiposObjetosInteracao GetTipo() {
            if(componenteTexto.Habilitado) {
                return TiposObjetosInteracao.Texto;
            }

            return TiposObjetosInteracao.Imagem;
        }

        public void SetGatilhoAcaoPersonagem(AnimationClip animacaoAcionada) {
            if(objeto == null) {
                return;
            }

            gatilhoAcaoPersonagem.animacaoAcionada = animacaoAcionada;
            return;
        }

        public bool EhGatilhoAcaoPersonagem() {
            return gatilhoAcaoPersonagem.animacaoAcionada != null;
        }

        public void SetOpcaoCorretaGabaritoSelecao(bool ehOpcaoCorreta) {
            componenteVerificacaoGabaritoSelecao.ehOpcaoCorreta = ehOpcaoCorreta;
            return;
        }

        public bool EhOpcaoCorretaSelecao() {
            return componenteVerificacaoGabaritoSelecao.ehOpcaoCorreta;
        }

        public void SetOrdemSelecaoEhRelevante(bool ordemEhRelevante) {
            componenteVerificacaoGabaritoSelecao.ordemImporta = ordemEhRelevante;
            return;
        }

        public bool GetOrdemSelecaoEhRelevante() {
            return componenteVerificacaoGabaritoSelecao.ordemImporta;
        }

        public void SetOrdemSelecao(int ordemSelecao) {
            componenteVerificacaoGabaritoSelecao.numeroOrdemSelecao = ordemSelecao;
            return;
        }

        public int GetOrdemSelecao() {
            return componenteVerificacaoGabaritoSelecao.numeroOrdemSelecao;
        }

        public void SetObjetoDestino(Transform objetoDestino) {
            componenteVerificacaoGabaritoArrastar.objetoDestinoCorreto = objetoDestino;
            return;
        }

        public Transform GetObjetoDestino() {
            return componenteVerificacaoGabaritoArrastar.objetoDestinoCorreto;
        }

        public void SetDeveDesfazerAcao(bool deveDesfazer) {
            componenteVerificacaoGabaritoArrastar.deveRetornarPosicaoInicial = deveDesfazer;
            return;
        }

        public bool DeveDesfazerAcao() {
            return componenteVerificacaoGabaritoArrastar.deveRetornarPosicaoInicial;
        }
    }
}