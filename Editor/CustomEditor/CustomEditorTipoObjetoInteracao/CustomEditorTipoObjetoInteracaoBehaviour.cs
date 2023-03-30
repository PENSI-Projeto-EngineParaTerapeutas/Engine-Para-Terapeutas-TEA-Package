using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Autis.Runtime.ComponentesGameObjects;
using Autis.Runtime.DTOs;
using Autis.Editor.Constantes;

namespace Autis.Editor.CustomEditorComponentesGameObjects {
    [CustomEditor(typeof(IdentificadorTipoObjetoInteracao))]
    public class CustomEditorTipoObjetoInteracaoBehaviour : CustomEditorBase {
        protected override string CaminhoTemplate => "CustomEditor/CustomEditorTipoObjetoInteracao/CustomEditorTipoObjetoInteracaoTemplate.uxml";
        protected override string CaminhoStyle => "CustomEditor/CustomEditorTipoObjetoInteracao/CustomEditorTipoObjetoInteracaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_LABEL_TIPO_OBJETO_INTERACAO = "label-tipo-obejto-interacao";
        private const string NOME_INPUT_TIPO_OBJETO_INTERACAO = "input-tipo-obejto-interacao";
        private EnumField campoTipoObjetoInteracao;

        #endregion

        private IdentificadorTipoObjetoInteracao componente;

        protected override void OnRenderizarInterface() {
            componente = target as IdentificadorTipoObjetoInteracao;

            ConfigurarInputTipoObjetoInteracao();

            return;
        }

        private void ConfigurarInputTipoObjetoInteracao() {
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