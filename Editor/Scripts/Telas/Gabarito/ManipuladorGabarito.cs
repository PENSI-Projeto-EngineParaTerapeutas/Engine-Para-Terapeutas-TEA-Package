using System.Collections.Generic;
using UnityEngine;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;

namespace Autis.Editor.Manipuladores {
    public abstract class ManipuladorGabarito : ManipuladorObjetos {
        #region .: Mensagens :.

        protected const string MENSAGEM_ERRO_GABARITO_NAO_ENCONTRADO = "[ERRO]: N�o foi poss�vel encontrar o Game Object do gabarito. Garanta que ele existe na hierarquia do projeto.";

        #endregion

        #region .: Componentes :.

        protected Gabarito componenteGabarito;

        #endregion

        public List<ManipuladorObjetoInteracao> ElementosInteracao { get => elementosInteracao; }
        protected List<ManipuladorObjetoInteracao> elementosInteracao = new();

        public ManipuladorGabarito() {
            EncontrarObjetoGabarito();
            return;
        }

        protected virtual void EncontrarObjetoGabarito() {
            objeto = GameObject.FindGameObjectWithTag(NomesTags.Gabarito);

            if(objeto == null) {
                Debug.LogError(MENSAGEM_ERRO_GABARITO_NAO_ENCONTRADO);
            }

            componenteGabarito = objeto.GetComponent<Gabarito>();

            return;
        }

        public override void Criar() {
            EncontrarObjetoGabarito();
            return;
        }

        public override void Editar(GameObject objetoAlvo) {
            EncontrarObjetoGabarito();
            return;
        }

        public void SetLimiteAcertos(bool possuiLimite) {
            componenteGabarito.possuiLimiteAcertos = possuiLimite;
            return;
        }

        public bool PossuiLimiteAcertos() {
            return componenteGabarito.possuiLimiteAcertos;
        }

        public void SetQuantidadeAcertos(int maximoAcertos) {
            if(maximoAcertos < 0) {
                return;
            }

            componenteGabarito.maximoAcertos = maximoAcertos;
            return;
        }

        public int GetQuantidadeAcertos() {
            return componenteGabarito.maximoAcertos;
        }
    }
}