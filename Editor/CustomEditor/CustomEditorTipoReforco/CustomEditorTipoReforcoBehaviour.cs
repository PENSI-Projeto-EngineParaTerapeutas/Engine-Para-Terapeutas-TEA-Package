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
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTipoReforco/CustomEditorTipoReforcoTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorTipoReforco/CustomEditorTipoReforcoStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_REFORCO = "label-tipo-reforco";
        private const string NOME_INPUT_TIPO_REFORCO = "input-tipo-reforco";
        private EnumField campoTipoReforco;

        #endregion

        private IdentificadorTipoReforco componente;

        protected override void OnRenderizarInterface() {
            componente = target as IdentificadorTipoReforco;

            ConfigurarInputTipoReforco();

            return;
        }

        private void ConfigurarInputTipoReforco() {
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