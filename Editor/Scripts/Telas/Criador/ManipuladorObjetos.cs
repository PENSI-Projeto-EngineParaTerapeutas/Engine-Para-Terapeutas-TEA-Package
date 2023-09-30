using UnityEditor;
using UnityEngine;
using Autis.Runtime.Constantes;

namespace Autis.Editor.Manipuladores {
    public abstract class ManipuladorObjetos {
        #region .: Constantes :.

        protected static readonly Vector2 POSICAO_PADRAO_ATOR_TIPO_AUDIO = new(-9, 4);

        #endregion

        #region .: Mensagens :.

        private const string MENSAGEM_ERRO_CRIACAO_SEM_PREFAB = "[ERROR]: A criação de um objeto foi iniciada sem ter um prefab de referência.";

        #endregion

        public virtual GameObject ObjetoAtual { get => objeto; }
        protected GameObject objeto;

        public virtual GameObject PrefabObjeto { get => prefabObjeto; }
        protected GameObject prefabObjeto;

        public ManipuladorObjetos() { }

        public ManipuladorObjetos(GameObject prefabObjeto) {
            this.prefabObjeto = prefabObjeto;
            return;
        }

        public virtual void Criar() {
            if(prefabObjeto == null) {
                Debug.LogError(MENSAGEM_ERRO_CRIACAO_SEM_PREFAB);
            }

            objeto = GameObject.Instantiate(prefabObjeto, new Vector3(), Quaternion.identity);
            Editar(objeto);

            return;
        }

        public virtual void Editar(GameObject objetoAlvo) {
            objeto = objetoAlvo;

            objeto.tag = NomesTags.EditorOnly;
            objeto.layer = LayersProjeto.EditorOnly.Index;

            Selection.activeObject = objeto;

            return;
        }

        public virtual void SetObjeto(GameObject objeto) {
            this.objeto = objeto;
            return;
        }

        public virtual void AlterarPrefab(GameObject novoPrefab) {
            prefabObjeto = novoPrefab;

            Cancelar();
            Criar();

            return;
        }

        public virtual void Cancelar() {
            if(objeto != null) {
                GameObject.DestroyImmediate(objeto);
            }

            objeto = null;
            return;
        }

        public abstract void Finalizar();

        public void SetNome(string nome) {
            if(objeto == null) {
                return;
            }

            objeto.name = nome;
            return;
        }

        public string GetNome() {
            return objeto.name;
        }

        public void SetPosicao(Vector3 posicao) {
            if(objeto == null) {
                return;
            }

            objeto.transform.position = posicao;
            return;
        }

        public void SetPosicaoX(float posicaoX) {
            if(objeto == null) {
                return;
            }

            objeto.transform.position = new Vector3(posicaoX, objeto.transform.position.y);
            return;
        }

        public void SetPosicaoY(float posicaoY) {
            if(objeto == null) {
                return;
            }

            objeto.transform.position = new Vector3(objeto.transform.position.x, posicaoY);
            return;
        }

        public Vector3 GetPosicao() {
            return objeto.transform.position;
        }

        public void SetTamanho(float tamanho) {
            if(objeto == null) {
                return;
            }

            objeto.transform.localScale = new Vector3(tamanho, tamanho);
            return;
        }

        public void SetTamanhoX(float tamanhoX) {
            if(objeto == null) {
                return;
            }

            objeto.transform.localScale = new Vector3(tamanhoX, objeto.transform.localScale.y);
            return;
        }

        public void SetTamanhoY(float tamanhoY) {
            if(objeto == null) {
                return;
            }

            objeto.transform.localScale = new Vector3(objeto.transform.localScale.x, tamanhoY);
            return;
        }

        public Vector3 GetTamanho() {
            return objeto.transform.localScale;
        }
    }
}