using UnityEngine;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.ComponentesGameObjects {
    [AddComponentMenu("Engine Terapeutas TEA/Identificador Tipo/Identificador Tipo Objeto de Interação")]
    public class IdentificadorTipoObjetoInteracao : IdentificadorTipo<TiposObjetosInteracao> {
        private SpriteRenderer spriteRenderer;
        private Texto texto;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            texto = GetComponent<Texto>();

            return;
        }

        #if UNITY_EDITOR
        public override void AlterarTipo(TiposObjetosInteracao novoTipo) {
            tipo = novoTipo;

            spriteRenderer = GetComponent<SpriteRenderer>();
            texto = GetComponent<Texto>();

            DesabilitarComponentes();
            HabilitarComponentes();

            return;
        }
        #endif

        public override void DesabilitarComponentes() {
            spriteRenderer.enabled = false;
            texto.Desabilitar();
            texto.enabled = false;

            return;
        }

        public override void HabilitarComponentes(TiposObjetosInteracao tipo) {
            switch(tipo) {
                case(TiposObjetosInteracao.Imagem): {
                    spriteRenderer.enabled = true;
                    break;
                }
                case(TiposObjetosInteracao.Texto): {
                    texto.enabled = true;
                    texto.Habilitar();

                    break;
                }
            }

            return;
        }
    }
}