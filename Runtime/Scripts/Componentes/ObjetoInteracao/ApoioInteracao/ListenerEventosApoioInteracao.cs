using UnityEngine;
using Autis.Runtime.DTOs;
using Autis.Runtime.Eventos;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("AUTIS/Apoio Objeto Interação/Listener Apoio Objeto de Interação")]
    public class ListenerEventosApoioInteracao : MonoBehaviour {
        public TipoAcionamentoApoioObjetoInteracao TipoAcionamento { get => tipoAcionamento; set { tipoAcionamento = value; } }
        [SerializeField]
        private TipoAcionamentoApoioObjetoInteracao tipoAcionamento;

        [SerializeField]
        private EventoJogo eventoErroGeral;

        [SerializeField]
        private EventoInteiro eventoErroComOrdem;

        private IdentificadorTipoApoioObjetoInteracao tipoApoioObjetoInteracao;
        private VerificacaoGabaritoSelecao verificacaoGabaritoSelecao;
        private AudioSource audioSource;

        private void Awake() {
            tipoApoioObjetoInteracao = GetComponent<IdentificadorTipoApoioObjetoInteracao>();
            audioSource = GetComponent<AudioSource>();

            return;
        }

        private void Start() {
            verificacaoGabaritoSelecao = transform.parent.GetComponent<VerificacaoGabaritoSelecao>();

            if(tipoAcionamento == TipoAcionamentoApoioObjetoInteracao.Erro && verificacaoGabaritoSelecao.ordemImporta) {
                eventoErroComOrdem.AdicionarCallback(HandleEventoErroComOrdem);
            }
            else if(tipoAcionamento == TipoAcionamentoApoioObjetoInteracao.Erro) {
                eventoErroGeral.AdicionarCallback(HandleEventoErroPadrao);
            }

            tipoApoioObjetoInteracao.DesabilitarComponentes();

            return;
        }

        private void HandleEventoErroComOrdem(int numeroPassoAtual) {
            if(verificacaoGabaritoSelecao.numeroOrdemSelecao != numeroPassoAtual) {
                return;
            }

            AcionarComponentes();
            return;
        }

        private void HandleEventoErroPadrao() {
            AcionarComponentes();
            return;
        }

        public void AcionarComponentes() {
            tipoApoioObjetoInteracao.HabilitarComponentes();

            switch(tipoApoioObjetoInteracao.Tipo) {
                case(TiposApoiosObjetosInteracao.Audio): {
                    audioSource.Play();
                    tipoApoioObjetoInteracao.IniciarCorrotinaDesabilitarComponentes(audioSource.clip.length);
                    break;
                }
                case(TiposApoiosObjetosInteracao.Seta): {
                    if(audioSource.clip != null) {
                        audioSource.Play();
                        tipoApoioObjetoInteracao.IniciarCorrotinaDesabilitarComponentes(audioSource.clip.length);

                        break;
                    }

                    tipoApoioObjetoInteracao.IniciarCorrotinaDesabilitarComponentes();
                    break;
                }
                default: {
                    tipoApoioObjetoInteracao.IniciarCorrotinaDesabilitarComponentes();
                    break;
                }
            }

            return;
        }
    }
}