using TMPro;
using UnityEngine;
using Autis.Runtime.ComponentesGameObjects;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorTexto {
        #region .: Componentes :.

        public Texto ComponenteTexto { get => componenteTexto; set { componenteTexto = value; } }
        protected Texto componenteTexto;

        #endregion

        public ManipuladorTexto(Texto componenteTexto) {
            this.componenteTexto = componenteTexto;
            return;
        }

        public void SetTexto(string texto) {
            if(componenteTexto == null) {
                return;
            }

            componenteTexto.TextMesh.text = texto;
            return;
        }

        public string GetTexto() {
            return componenteTexto.TextMesh.text;
        }

        public void SetFontSize(float tamanho) {
            if(tamanho < 0 || componenteTexto == null) {
                return;
            }

            componenteTexto.TextMesh.fontSize = tamanho;
            return;
        }

        public float GetFontSize() {
            return componenteTexto.TextMesh.fontSize;
        }

        public void SetFontStyle(bool ativo, FontStyles fontStyle) {
            if(componenteTexto == null) {
                return;
            }

            if(ativo) {
                componenteTexto.TextMesh.fontStyle |= fontStyle;
                return;
            }

            componenteTexto.TextMesh.fontStyle ^= fontStyle;

            return;
        }

        public bool FontStyleEstaAtivo(FontStyles fontStyle) {
            return (componenteTexto.TextMesh.fontStyle & fontStyle) != 0;
        }

        public void SetCor(Color cor) {
            if(componenteTexto == null) {
                return;
            }

            componenteTexto.TextMesh.color = cor;
            return;
        }

        public Color GetCor() {
            return componenteTexto.TextMesh.color;
        }
    }
}