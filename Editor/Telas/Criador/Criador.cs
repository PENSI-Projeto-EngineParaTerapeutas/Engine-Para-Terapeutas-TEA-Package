using UnityEditor;
using UnityEngine;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Criadores {
    public abstract class Criador : ElementoInterfaceEditor, IReiniciavel {
        #region .: Elementos :.

        protected readonly HeaderCriadorBehaviour header;

        #endregion

        protected GameObject prefab;
        protected GameObject novoObjeto;

        protected Criador() {
            header = new HeaderCriadorBehaviour();
            return;
        }

        protected virtual void ImportarPrefab(string caminho) {
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ConstantesRuntime.CaminhoCompletoPastaResources + caminho); // TODO: Usar path
            return;
        }

        public virtual void IniciarCriacao() {
            novoObjeto = GameObject.Instantiate(prefab, new Vector3(), Quaternion.identity);
            //TODO: novoObjeto.hideFlags = HideFlags.HideInHierarchy;
            novoObjeto.tag = NomesTags.EditorOnly;
            novoObjeto.layer = LayersProjeto.EditorOnly.Index;

            header.VincularDados(novoObjeto);

            VincularCamposAoNovoObjeto();

            Selection.activeObject = novoObjeto;
            return;
        }

        protected abstract void VincularCamposAoNovoObjeto();

        public virtual void CancelarCriacao() {
            if(novoObjeto != null) {
                GameObject.DestroyImmediate(novoObjeto);
            }

            novoObjeto = null;
            ReiniciarPropriedadesNovoObjeto();

            header.ReiniciarCampos();
            ReiniciarCampos();

            return;
        }

        public abstract void FinalizarCriacao();

        protected abstract void ReiniciarPropriedadesNovoObjeto();

        public abstract void ReiniciarCampos();
    }
}