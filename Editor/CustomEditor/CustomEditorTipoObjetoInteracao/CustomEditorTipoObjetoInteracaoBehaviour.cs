using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoObjetoInteracao))]
    public class CustomEditorTipoObjetoInteracaoBehaviour : CustomEditorBase {

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_OBJETO_INTERACAO = "label-tipo-obejto-interacao";
        private const string NOME_INPUT_TIPO_OBJETO_INTERACAO = "input-tipo-obejto-interacao";
        private EnumField campoTipoObjetoInteracao;

        #endregion

        private IdentificadorTipoObjetoInteracao componente;

        protected override void OnEnable() {
            base.OnEnable();

            componente = target as IdentificadorTipoObjetoInteracao;

            ImportarTemplate("/CustomEditor/CustomEditorTipoObjetoInteracao/CustomEditorTipoObjetoInteracaoTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorTipoObjetoInteracao/CustomEditorTipoObjetoInteracaoStyle.uss");

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            campoTipoObjetoInteracao = root.Query<EnumField>(NOME_INPUT_TIPO_OBJETO_INTERACAO);

            campoTipoObjetoInteracao.labelElement.name = NOME_LABEL_TIPO_OBJETO_INTERACAO;
            campoTipoObjetoInteracao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoObjetoInteracao.Init(componente.Tipo);
            campoTipoObjetoInteracao.SetValueWithoutNotify(componente.Tipo);

            campoTipoObjetoInteracao.RegisterCallback<ChangeEvent<Enum>>(evt => {
                componente.AlterarTipo(Enum.Parse<TiposObjetosInteracao>(campoTipoObjetoInteracao.value.ToString()));
            });

            return;
        }
    }
}