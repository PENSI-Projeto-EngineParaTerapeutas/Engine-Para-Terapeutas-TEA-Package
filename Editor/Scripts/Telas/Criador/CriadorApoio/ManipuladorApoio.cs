using UnityEngine;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Runtime.DTOs;
using Autis.Editor.Utils;

namespace Autis.Editor.Manipuladores { 
    public class ManipuladorApoio : ManipuladorObjetos, IExcluir {
        private const string CAMINHO_PREFAB_APOIO = "ObjetosInteracao/ApoioObjetoInteracao.prefab";
        private readonly static Vector3 POSICAO_PADRAO_EM_RELACAO_PAI = new(1.5f, 1.5f, 0);

        #region .: Componente :.

        public SpriteRenderer ComponenteSpriteRenderer { get => componenteSpriteRenderer; }
        protected SpriteRenderer componenteSpriteRenderer;

        public AudioSource ComponenteAudioSource { get => componenteAudioSource; }
        protected AudioSource componenteAudioSource;

        public ListenerEventosApoioInteracao ComponenteListenerEventos { get => componenteListenerEventos; }
        protected ListenerEventosApoioInteracao componenteListenerEventos;

        public Texto ComponenteTexto { get => componenteTexto; }
        protected Texto componenteTexto;

        public IdentificadorTipoApoioObjetoInteracao ComponenteIdentificadorTipo { get => componenteIdentificadorTipo; }
        protected IdentificadorTipoApoioObjetoInteracao componenteIdentificadorTipo;

        public ManipuladorObjetoInteracao ManipuladorElementoInteracaoVinculado { get => manipuladorElementoInteracaoVinculado; }
        protected ManipuladorObjetoInteracao manipuladorElementoInteracaoVinculado;

        #endregion

        #region .: Manipuladores :.

        public ManipuladorTexto ManipuladorComponenteTexto { get => manipuladorComponenteTexto; }
        protected ManipuladorTexto manipuladorComponenteTexto;

        public ManipuladorAudioSource ManipualdorComponenteAudioSource { get => manipualdorComponenteAudioSource; }
        protected ManipuladorAudioSource manipualdorComponenteAudioSource;

        #endregion

        public ManipuladorApoio() {
            prefabObjeto = Importador.ImportarPrefab(CAMINHO_PREFAB_APOIO);
            return;
        }

        public ManipuladorApoio(GameObject prefabObjeto) : base(prefabObjeto) { }

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);
            
            CarregarComponentes();
            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.EmCriacao;

            return;
        }

        private void CarregarComponentes() {
            componenteIdentificadorTipo = objeto.GetComponent<IdentificadorTipoApoioObjetoInteracao>();
            componenteSpriteRenderer = objeto.GetComponent<SpriteRenderer>();
            componenteAudioSource = objeto.GetComponent<AudioSource>();
            componenteListenerEventos = objeto.GetComponent<ListenerEventosApoioInteracao>();
            componenteTexto = objeto.GetComponent<Texto>();

            manipuladorComponenteTexto = new ManipuladorTexto(componenteTexto);
            manipualdorComponenteAudioSource = new ManipuladorAudioSource(componenteAudioSource);

            return;
        }

        public override void SetObjeto(GameObject objeto) {
            base.SetObjeto(objeto);
            CarregarComponentes();

            return;
        }

        public override void Finalizar() {
            objeto.tag = NomesTags.Apoios;
            objeto.layer = LayersProjeto.Default.Index;
            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.Apoio;
            
            RemoverVinculo();

            return;
        }

        public void Excluir() {
            if(manipuladorElementoInteracaoVinculado != null) {
                DesvincularObjetoInteracao();
            }

            GameObject.DestroyImmediate(objeto);
            RemoverVinculo();

            return;
        }

        protected void RemoverVinculo() {
            objeto = null;

            componenteIdentificadorTipo = null;
            componenteSpriteRenderer = null;
            componenteAudioSource = null;
            componenteListenerEventos = null;
            componenteTexto = null;

            manipuladorComponenteTexto = null;
            manipualdorComponenteAudioSource = null;
            manipuladorElementoInteracaoVinculado = null;

            return;
        }

        public void AlterarTipo(TiposApoiosObjetosInteracao tipo) {
            if(objeto == null) {
                return;
            }

            DesabilitarComponentes();

            switch(tipo) {
                case(TiposApoiosObjetosInteracao.Seta): {
                    HabilitarComponenteSeta();
                    break;
                }
                case(TiposApoiosObjetosInteracao.Audio): {
                    HabilitarComponenteAudio();
                    break;
                }
                case(TiposApoiosObjetosInteracao.Texto): {
                    HabilitarComponenteTexto();
                    break;
                }
            }

            return;
        }

        private void HabilitarComponenteTexto() {
            componenteTexto.Habilitado = true;
            componenteListenerEventos.enabled = true;

            componenteIdentificadorTipo.tipo = TiposApoiosObjetosInteracao.Texto;

            return;
        }

        private void HabilitarComponenteAudio() {
            componenteAudioSource.enabled = true;
            objeto.transform.position = POSICAO_PADRAO_ATOR_TIPO_AUDIO;
            componenteListenerEventos.enabled = true;

            componenteIdentificadorTipo.tipo = TiposApoiosObjetosInteracao.Audio;

            return;
        }

        private void HabilitarComponenteSeta() {
            componenteAudioSource.enabled = true;
            componenteSpriteRenderer.enabled = true;
            componenteListenerEventos.enabled = true;

            componenteIdentificadorTipo.tipo = TiposApoiosObjetosInteracao.Seta;

            return;
        }

        public void DesabilitarComponentes() {
            if(objeto == null) {
                return;
            }

            componenteSpriteRenderer.enabled = false;
            componenteAudioSource.enabled = false;
            componenteListenerEventos.enabled = false;
            componenteTexto.Habilitado = false;

            return;
        }

        public TiposApoiosObjetosInteracao GetTipo() {
            if(componenteSpriteRenderer.enabled) {
                return TiposApoiosObjetosInteracao.Seta;
            }
            
            if(componenteTexto.Habilitado) {
                return TiposApoiosObjetosInteracao.Texto;
            }

            return TiposApoiosObjetosInteracao.Audio;
        }

        public void VincularObjetoInteracao(GameObject objetoInteracao) {
            if(objeto == null) {
                return;
            }

            ManipuladorObjetoInteracao manipulador = new();
            manipulador.SetObjeto(objetoInteracao);

            manipuladorElementoInteracaoVinculado = manipulador;

            objeto.transform.SetParent(objetoInteracao.transform);
            objeto.transform.localPosition = POSICAO_PADRAO_EM_RELACAO_PAI;

            manipuladorElementoInteracaoVinculado.HabilitarAcionamentoApoios(true);

            return;
        }

        public void VincularObjetoInteracao(ManipuladorObjetoInteracao manipulador) {
            if(objeto == null) {
                return;
            }

            manipuladorElementoInteracaoVinculado = manipulador;

            objeto.transform.SetParent(manipulador.ObjetoAtual.transform);
            objeto.transform.localPosition = POSICAO_PADRAO_EM_RELACAO_PAI;

            manipuladorElementoInteracaoVinculado.HabilitarAcionamentoApoios(true);

            return;
        }

        public void DesvincularObjetoInteracao() {
            if(objeto == null) {
                return;
            }

            objeto.transform.SetParent(null);
            manipuladorElementoInteracaoVinculado.HabilitarAcionamentoApoios(false);

            manipuladorElementoInteracaoVinculado = null;

            return;
        }

        public Transform GetObjetoPai() {
            return objeto.transform.parent;
        }

        public void SetTipoAcionamento(TipoAcionamentoApoioObjetoInteracao tipoAcionamento) {
            if(objeto == null) {
                return;
            }

            componenteListenerEventos.TipoAcionamento = tipoAcionamento;
            return;
        }

        public TipoAcionamentoApoioObjetoInteracao GetTipoAcionamento() {
            return componenteListenerEventos.TipoAcionamento;
        }

        public void SetTempoExibicao(float tempoExibicao) {
            componenteIdentificadorTipo.tempoEspera = tempoExibicao;
            return;
        }

        public float GetTempoExibicao() {
            return componenteIdentificadorTipo.tempoEspera;
        }
    }
}