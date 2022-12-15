using UnityEngine;
using UnityEngine.UIElements;
using EngineParaTerapeutas.Constantes;
using EngineParaTerapeutas.UI;

namespace EngineParaTerapeutas.Criadores {
    public class HeaderCriadorBehaviour : ElementoInterfaceEditor, IVinculavel<GameObject>, IReiniciavel {
        private const string NOME_PADRAO_NOVO_ATOR = "Ator";

        #region .: Elementos :.

        private const string NOME_LABEL_TEXTO_NOME_OBJETO = "label-nome-ator";
        private const string NOME_CAMPO_TEXTO_NOME_OBJETO = "input-nome-ator";
        private TextField campoNomeObjeto;

        private const string NOME_REGIAO_CARREGAMENTO_INPUTS_POSICAO = "regiao-carregamento-inputs-posicao";
        private VisualElement regiaoCarregamentoInputsPosicao;

        private readonly InputsPosicao grupoInputsPosicao;

        #endregion

        private GameObject novoAtor;
        private string nomeAtor = NOME_PADRAO_NOVO_ATOR;

        public HeaderCriadorBehaviour() {
            grupoInputsPosicao = new InputsPosicao();

            ImportarTemplate("Telas/Criador/HeaderCriador/HeaderCriadorTemplate.uxml");
            ImportarStyle("Telas/Criador/HeaderCriador/HeaderCriadorStyle.uss");

            ConfigurarInputs();

            return;
        }

        private void ConfigurarInputs() {
            ConfigurarCampoNomeObjeto();
            CarregarInputsPosicao();

            return;
        }

        private void ConfigurarCampoNomeObjeto() {
            campoNomeObjeto = Root.Query<TextField>(NOME_CAMPO_TEXTO_NOME_OBJETO);

            campoNomeObjeto.labelElement.name = NOME_LABEL_TEXTO_NOME_OBJETO;
            campoNomeObjeto.labelElement.AddToClassList(NomesClassesPadroesEditorStyle.LabelInputPadrao);

            campoNomeObjeto.SetValueWithoutNotify(string.Empty);
            campoNomeObjeto.RegisterCallback<ChangeEvent<string>>(evt => {
                nomeAtor = evt.newValue;

                if(novoAtor != null) {
                    novoAtor.name = nomeAtor;
                }
            });

            return;
        }

        private void CarregarInputsPosicao() {
            regiaoCarregamentoInputsPosicao = Root.Query<VisualElement>(NOME_REGIAO_CARREGAMENTO_INPUTS_POSICAO);
            regiaoCarregamentoInputsPosicao.Add(grupoInputsPosicao.Root);

            return;
        }

        public void VincularDados(GameObject componente) {
            novoAtor = componente;
            grupoInputsPosicao.VincularDados(componente.transform);

            return;
        }

        public void ReiniciarCampos() {
            nomeAtor = NOME_PADRAO_NOVO_ATOR;

            campoNomeObjeto.SetValueWithoutNotify(string.Empty);
            grupoInputsPosicao.ReiniciarCampos();

            return;
        }
    }
}