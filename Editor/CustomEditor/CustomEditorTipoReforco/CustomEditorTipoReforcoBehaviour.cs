using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;
using EngineParaTerapeutas.DTOs;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoReforco))]
    public class CustomEditorTipoReforcoBehaviour : CustomEditorBase {

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_REFORCO = "label-tipo-reforco";
        private const string NOME_INPUT_TIPO_REFORCO = "input-tipo-reforco";
        private EnumField campoTipoReforco;

        #endregion

        private IdentificadorTipoReforco componente;

        protected override void OnEnable() {
            base.OnEnable();

            componente = target as IdentificadorTipoReforco;

            ImportarTemplate("/CustomEditor/CustomEditorTipoReforco/CustomEditorTipoReforcoTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorTipoReforco/CustomEditorTipoReforcoStyle.uss");

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
            campoTipoReforco = root.Query<EnumField>(NOME_INPUT_TIPO_REFORCO);
            
            campoTipoReforco.labelElement.name = NOME_LABEL_TIPO_REFORCO;
            campoTipoReforco.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoReforco.Init(componente.Tipo);
            campoTipoReforco.SetValueWithoutNotify(componente.Tipo);

            campoTipoReforco.RegisterCallback<ChangeEvent<Enum>>(evt => {
                componente.AlterarTipo(Enum.Parse<TiposReforcos>(campoTipoReforco.value.ToString()));
            });

            return;
        }
    }
}