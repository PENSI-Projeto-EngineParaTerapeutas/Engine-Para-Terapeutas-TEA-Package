using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(Imagem))]
    public class CustomEditorImagemBehaviour : CustomEditor<Imagem, SpriteRenderer> {
        private readonly List<Material> componenteOriginalMaterial = new();

        #region .: Elementos :.

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_IMAGEM = "regiao-carregamento-inputs-padroes-imagem";
        private VisualElement regiaoCarregamentoInputsPadroesImagem;

        private InputsImagem grupoInputsImagem;

        #endregion

        public override void OnEnable() {
            base.OnEnable();
            grupoInputsImagem = new InputsImagem();

            componenteOriginal.GetSharedMaterials(componenteOriginalMaterial);
            AlterarVisibilidadeComponenteOriginal(HideFlags.HideInInspector);

            ImportarTemplate("/CustomEditor/CustomEditorImagem/CustomEditorImagemTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorImagem/CustomEditorImagemStyle.uss");
            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            regiaoCarregamentoInputsPadroesImagem = root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_PADROES_IMAGEM);
            regiaoCarregamentoInputsPadroesImagem.Add(grupoInputsImagem.Root);

            grupoInputsImagem.VincularDados(componenteOriginal);

            return;
        }

        protected override void AlterarVisibilidadeComponenteOriginal(HideFlags flag) {
            base.AlterarVisibilidadeComponenteOriginal(flag);

            for(int i = 0; i < componenteOriginalMaterial.Count; i++) {
                componenteOriginalMaterial[i].hideFlags = flag;
            }

            return;
        }
    }
}