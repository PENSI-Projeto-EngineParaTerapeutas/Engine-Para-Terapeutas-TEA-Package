using UnityEngine;
using Autis.Runtime.DTOs;

namespace Autis.Runtime.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Identificador Tipo/Identificador Tipo Instrução")]
    public class IdentificadorTipoInstrucao : IdentificadorTipo<TiposIntrucoes> {
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private Video video;
        private Texto texto;

        #if UNITY_EDITOR
        public override void AlterarTipo(TiposIntrucoes novoTipo) {
            tipo = novoTipo;

            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            video = GetComponent<Video>();
            texto = GetComponent<Texto>();

            DesabilitarComponentes();
            HabilitarComponentes();
            
            return;
        }
        #endif

        public override void DesabilitarComponentes() {
            spriteRenderer.enabled = false;
            audioSource.enabled = false;

            video.Player.enabled = false;
            video.enabled = false;

            texto.Desabilitar();
            texto.enabled = false;

            return;
        }

        public override void HabilitarComponentes(TiposIntrucoes tipo) {
            switch(tipo) {
                case(TiposIntrucoes.Audio): {
                    audioSource.enabled = true;
                    break;
                }
                case(TiposIntrucoes.Texto): {
                    texto.enabled = true;
                    texto.Habilitar();

                    break;
                }
                case(TiposIntrucoes.Video): {
                    spriteRenderer.enabled = true;
                    audioSource.enabled = true;

                    video.Player.enabled = true;
                    video.enabled = true;

                    break;
                }
            }

            return;
        }
    }
}