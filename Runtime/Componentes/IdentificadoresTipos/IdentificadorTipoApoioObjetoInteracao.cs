using UnityEngine;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Identificador Tipo/Identificador Tipo Apoio Objeto Interação")]
    public class IdentificadorTipoApoioObjetoInteracao : IdentificadorTipo<TiposApoiosObjetosInteracao> {
        private SpriteRenderer spriteRenderer;
        private AudioSource audioSource;
        private Texto texto;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            texto = GetComponent<Texto>();

            return;
        }

        #if UNITY_EDITOR
        public override void AlterarTipo(TiposApoiosObjetosInteracao novoTipo) {
            tipo = novoTipo;

            spriteRenderer = GetComponent<SpriteRenderer>();
            audioSource = GetComponent<AudioSource>();
            texto = GetComponent<Texto>();

            DesabilitarComponentes();
            HabilitarComponentes();

            return;
        }
        #endif

        public override void DesabilitarComponentes() {
            spriteRenderer.enabled = false;
            audioSource.enabled = false;

            texto.Desabilitar();
            texto.enabled = false;

            return;
        }

        public override void HabilitarComponentes(TiposApoiosObjetosInteracao tipo) {
            switch(tipo) {
                case(TiposApoiosObjetosInteracao.Audio): {
                    audioSource.enabled = true;
                    break;
                }
                case(TiposApoiosObjetosInteracao.Seta): {
                    spriteRenderer.enabled = true;
                    audioSource.enabled = true;

                    break;
                }
                case(TiposApoiosObjetosInteracao.Texto): {
                    texto.enabled = true;
                    texto.Habilitar();

                    break;
                }
            }

            return;
        }
    }
}