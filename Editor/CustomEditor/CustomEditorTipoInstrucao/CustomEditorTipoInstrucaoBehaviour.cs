using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EngineParaTerapeutas.DTOs;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.ComponentesGameObjects;

namespace EngineParaTerapeutas.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoInstrucao))]
    public class CustomEditorTipoInstrucaoBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTipoInstrucao/CustomEditorTipoInstrucaoTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorTipoInstrucao/CustomEditorTipoInstrucaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_INSTRUCAO = "label-tipo-instrucao";
        private const string NOME_INPUT_TIPO_INSTRUCAO = "input-tipo-instrucao";
        private EnumField campoTipoInstrucao;

        #endregion

        private IdentificadorTipoInstrucao componente;

        protected override void OnRenderizarInterface() {
            componente = target as IdentificadorTipoInstrucao;

            ConfigurarInputTipoInstrucao();

            return;
        }

        private void ConfigurarInputTipoInstrucao() {
            campoTipoInstrucao = root.Query<EnumField>(NOME_INPUT_TIPO_INSTRUCAO);

            campoTipoInstrucao.labelElement.name = NOME_LABEL_TIPO_INSTRUCAO;
            campoTipoInstrucao.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoTipoInstrucao.Init(componente.Tipo);
            campoTipoInstrucao.SetValueWithoutNotify(componente.Tipo);

            campoTipoInstrucao.RegisterCallback<ChangeEvent<Enum>>(evt => {
                componente.AlterarTipo(Enum.Parse<TiposIntrucoes>(campoTipoInstrucao.value.ToString()));
            });

            return;
        }
    }
}