using UnityEngine;
using UnityEngine.UIElements;

namespace Autis.Runtime.UI {
    public class HabilitadorAtoresDinamico : ElementoInterfaceJogo {
        protected override string CaminhoTemplate => "Scripts/ElementosUI/HabilitadorAtoresDinamico/HabilitadorAtoresDinamicoTemplate";
        protected override string CaminhoStyle => "Scripts/ElementosUI/HabilitadorAtoresDinamico/HabilitadorAtoresDinamicoStyle";

        #region .: Elementos :.

        public Label InputHabilitarAtor { get => InputHabilitarAtor; }

        private string NOME_INPUT_HABILITAR_ATOR = "input-habilitar-ator";
        private readonly Toggle inputHabilitarAtor;

        #endregion

        private readonly GameObject atorVinculado;

        public HabilitadorAtoresDinamico(GameObject ator) {
            atorVinculado = ator;

            inputHabilitarAtor = Root.Query<Toggle>(NOME_INPUT_HABILITAR_ATOR);

            ConfigurarToggleComBaseAtor();

            return;
        }

        private void ConfigurarToggleComBaseAtor() {
            NOME_INPUT_HABILITAR_ATOR = ("selecionar-apoio-" + atorVinculado.name);

            inputHabilitarAtor.label = atorVinculado.name;
            inputHabilitarAtor.name = NOME_INPUT_HABILITAR_ATOR;
            inputHabilitarAtor.AddToClassList("input-habilitar-ator");

            inputHabilitarAtor.labelElement.name = ("label-habilitar-ator-" + atorVinculado.name);
            inputHabilitarAtor.labelElement.AddToClassList("label-selecionar-ator");

            inputHabilitarAtor.SetValueWithoutNotify(atorVinculado.activeInHierarchy);
            inputHabilitarAtor.RegisterCallback<ChangeEvent<bool>>(evt => {
                atorVinculado.SetActive(inputHabilitarAtor.value);
            });

            return;
        }
    }
}