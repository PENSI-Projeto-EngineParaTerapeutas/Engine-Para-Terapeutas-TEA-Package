using System.IO;
using UnityEditor;
using UnityEngine;
using Autis.Editor.Utils;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.Constantes;
using Autis.Editor.Constantes;

namespace Autis.Editor.Manipuladores {
    public class ManipuladorCenario : ManipuladorObjetos, IExcluir {
        private const string CAMINHO_PREFAB_CENARIO = "Cenarios/Cenario.prefab";
        private static Sprite IMAGEM_PADRAO_CENARIO;

        #region .: Componentes :.

        public SpriteRenderer ComponenteSpriteRenderer { get => componenteSpriteRenderer; }
        protected SpriteRenderer componenteSpriteRenderer;
        
        public CenarioResize ComponenteCenarioResize { get => componenteCenarioResize; }
        protected CenarioResize componenteCenarioResize;

        #endregion

        public ManipuladorSpriteRenderer ManipuladorComponenteSpriteRenderer { get => manipuladorComponenteSpriteRenderer; }
        private ManipuladorSpriteRenderer manipuladorComponenteSpriteRenderer;

        public ManipuladorCenario() {
            prefabObjeto = Importador.ImportarPrefab(CAMINHO_PREFAB_CENARIO);
            IMAGEM_PADRAO_CENARIO = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(ConstantesEditor.CaminhoPastaAssetsRuntime, "Imagens/Square.png"));

            return;
        }

        public ManipuladorCenario(GameObject prefabAtor) : base(prefabAtor) { }

        public override void Editar(GameObject objetoAlvo) {
            base.Editar(objetoAlvo);

            componenteCenarioResize = objeto.GetComponent<CenarioResize>();
            componenteSpriteRenderer = objeto.GetComponent<SpriteRenderer>();
            
            manipuladorComponenteSpriteRenderer = new ManipuladorSpriteRenderer(componenteSpriteRenderer);

            componenteCenarioResize.Resize();

            return;
        }

        public override void Finalizar() {
            objeto.tag = NomesTags.Cenario;
            objeto.layer = LayersProjeto.Default.Index;
            componenteSpriteRenderer.sortingOrder = OrdemRenderizacao.Cenario;

            RemoverVinculo();

            return;
        }

        public void Excluir() {
            GameObject.DestroyImmediate(objeto);
            RemoverVinculo();

            return;
        }

        protected void RemoverVinculo() {
            objeto = null;

            componenteCenarioResize = null;
            componenteSpriteRenderer = null;

            manipuladorComponenteSpriteRenderer = null;

            return;
        }

        public void SetCorSolida(Color cor) {
            if(objeto == null) {
                return;
            }
            
            manipuladorComponenteSpriteRenderer.SetImagem(IMAGEM_PADRAO_CENARIO);
            manipuladorComponenteSpriteRenderer.SetCor(cor);

            componenteCenarioResize.Resize();

            return;
        }

        public Color GetCor() {
            return componenteSpriteRenderer.color;
        }

        public void SetImagem(Sprite sprite) {
            if(objeto == null) {
                return;
            }

            manipuladorComponenteSpriteRenderer.SetImagem(sprite);
            manipuladorComponenteSpriteRenderer.SetCor(Color.white);
            componenteCenarioResize.Resize();

            return;
        }

        public Sprite GetImagem() {
            return componenteSpriteRenderer.sprite;
        }

        public bool EhCorSolida() {
            return componenteSpriteRenderer.sprite.name == IMAGEM_PADRAO_CENARIO.name;
        }

        public bool EhImagem() {
            return componenteSpriteRenderer.sprite.name != IMAGEM_PADRAO_CENARIO.name;
        }
    }
}