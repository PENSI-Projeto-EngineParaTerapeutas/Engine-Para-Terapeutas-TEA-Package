using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoApoio))]
    public class CustomEditorTipoApoioBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTipoApoio/CustomEditorTipoApoioTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorTipoApoio/CustomEditorTipoApoioStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_APOIO = "label-tipo-apoio";
        private const string NOME_INPUT_TIPO_APOIO = "input-tipo-apoio";
        private EnumField campoTipoApoio;

        #endregion

        private IdentificadorTipoApoio componente;

        protected override void OnRenderizarInterface() {
            componente = target as IdentificadorTipoApoio;

            ConfigurarInputTipoApoio();

            return;
        }

        private void ConfigurarInputTipoApoio() {
            campoTipoApoio = root.Query<EnumField>(NOME_INPUT_TIPO_APOIO);

            campoTipoApoio.labelElement.name = NOME_LABEL_TIPO_APOIO;
            campoTipoApoio.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoApoio.Init(componente.Tipo);
            campoTipoApoio.SetValueWithoutNotify(componente.Tipo);

            campoTipoApoio.RegisterCallback<ChangeEvent<Enum>>(evt => {
                componente.AlterarTipo(Enum.Parse<TiposApoios>(campoTipoApoio.value.ToString()));
            });

            return;
        }
    }
}