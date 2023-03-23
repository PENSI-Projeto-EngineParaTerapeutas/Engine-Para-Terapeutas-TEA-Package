using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoApoio))]
    public class CustomEditorIdentificadorTipoApoioBehaviour : CustomEditorBase {

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_APOIO = "label-tipo-apoio";
        private const string NOME_INPUT_TIPO_APOIO = "input-tipo-apoio";
        private EnumField campoTipoApoio;

        #endregion

        private IdentificadorTipoApoio componente;

        protected override void OnEnable() {
            base.OnEnable();

            componente = target as IdentificadorTipoApoio;

            ImportarTemplate("/CustomEditor/CustomEditorIdentificadorTipoApoio/CustomEditorIdentificadorTipoApoioTemplate.uxml");
            ImportarStyle("/CustomEditor/CustomEditorIdentificadorTipoApoio/CustomEditorIdentificadorTipoApoioStyle.uss");

            ConfigurarInputs();

            return;
        }

        protected override void ConfigurarInputs() {
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