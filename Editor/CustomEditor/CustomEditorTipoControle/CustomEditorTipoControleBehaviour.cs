using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;

namespace Autis.Editor.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoControle))]
    public class CustomEditorTipoControleBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTipoControle/CustomEditorTipoControleTemplate.uxml";

        protected override string CaminhoStyle => "CustomEditor/CustomEditorTipoControle/CustomEditorTipoControleStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_CONTROLE = "label-tipo-controle";
        private const string NOME_INPUT_TIPO_CONTROLE = "input-tipo-controle";
        private EnumField campoTipoControle;

        #endregion

        private IdentificadorTipoControle componente;

        protected override void OnRenderizarInterface() {
            componente = target as IdentificadorTipoControle;

            ConfigurarInputTipoReforco();

            return;
        }

        private void ConfigurarInputTipoReforco() {
            campoTipoControle = root.Query<EnumField>(NOME_INPUT_TIPO_CONTROLE);

            campoTipoControle.labelElement.name = NOME_LABEL_TIPO_CONTROLE;
            campoTipoControle.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoControle.Init(componente.Tipo);
            campoTipoControle.SetValueWithoutNotify(componente.Tipo);

            campoTipoControle.RegisterCallback<ChangeEvent<Enum>>(evt => {
                componente.AlterarTipo(Enum.Parse<TipoControle>(campoTipoControle.value.ToString()));
            });

            return;
        }
    }
}