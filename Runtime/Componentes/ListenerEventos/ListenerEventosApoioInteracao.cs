using UnityEngine;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Listener Eventos Apoio Intera��o")]
    public class ListenerEventosApoioInteracao : ListenerEventosBase {
        private IdentificadorTipoApoioObjetoInteracao tipoApoioObjetoInteracao;
        private AudioSource audioSource;

        protected override void Awake() {
            base.Awake();

            tipoApoioObjetoInteracao = GetComponent<IdentificadorTipoApoioObjetoInteracao>();
            audioSource = GetComponent<AudioSource>();

            return;
        }

        protected override void Start() {
            base.Start();
            tipoApoioObjetoInteracao.DesabilitarComponentes();

            return;
        }

        protected override void AcionarComponentes() {
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
