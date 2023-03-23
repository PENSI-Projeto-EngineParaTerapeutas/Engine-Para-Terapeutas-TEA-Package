using UnityEditor;
using UnityEngine;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.UI;
using EngineParaTerapeutas.Telas;

namespace EngineParaTerapeutas.Criadores {
    public abstract class Criador : Tela, IReiniciavel, ICamposAtualizaveis {
        #region .: Elementos :.

        protected readonly HeaderCriadorBehaviour header;
        protected readonly BotoesConfirmacao botoesConfirmacao;

        #endregion

        protected GameObject prefab;
        protected GameObject novoObjeto;

        protected Criador() {
            header = new HeaderCriadorBehaviour();
            botoesConfirmacao = new BotoesConfirmacao();

            return;
        }

        protected virtual void ImportarPrefab(string caminho) {
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(ConstantesRuntime.CaminhoCompletoPastaResources + caminho); // TODO: Usar path
            return;
        }

        public virtual void IniciarCriacao() {
            novoObjeto = GameObject.Instantiate(prefab, new Vector3(), Quaternion.identity);
            novoObjeto.tag = NomesTags.EditorOnly;
            novoObjeto.layer = LayersProjeto.EditorOnly.Index;

            header.VincularDados(novoObjeto);

            VincularCamposAoNovoObjeto();

            Selection.activeObject = novoObjeto; // TODO: Failed creating toolbar element from ID "Tools/Builtin Tools".
            return;
        }

        protected virtual void HandleBotaoConfirmarClick() {
            FinalizarCriacao();
            return;
        }

        public virtual void FinalizarCriacao() {
            novoObjeto = null;

            ReiniciarPropriedadesNovoObjeto();

            header.ReiniciarCampos();
            ReiniciarCampos();

            return;
        }

        protected virtual void HandleBotaoCancelarClick() {
            CancelarCriacao();
            return;
        }

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

        public override void OnEditorUpdate() {
            AtualizarCampos();
            return;
        }

        public virtual void AtualizarCampos() {
            header.AtualizarCampos();
            return;
        }

        public override void OnEditorPlay() {
            if(novoObjeto == null) {
                return;
            }

            CancelarCriacao();
            return;
        }

        protected abstract void VincularCamposAoNovoObjeto();

        protected abstract void ReiniciarPropriedadesNovoObjeto();

        public abstract void ReiniciarCampos();
    }
}