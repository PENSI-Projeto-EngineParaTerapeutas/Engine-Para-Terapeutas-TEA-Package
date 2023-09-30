using System.Linq;
using Autis.Editor.Manipuladores;

namespace Autis.Editor.Telas {
    public class EditarGabaritoSelecionavelBehaviour : GabaritoSelecionarBehaviour {
        public EditarGabaritoSelecionavelBehaviour() {
            CarregarDados();
            return;
        }

        private void CarregarDados() {
            if(manipuladorGabarito.GetOrdemSelecaoEhRelevante()) {
                toggleOrdem.SetValueWithoutNotify(true);
                listViewObjetosSelecionaveis.reorderable = true;
                manipuladorGabarito.OrdemEhRelevante = true;
            }

            IOrderedEnumerable<ManipuladorObjetoInteracao> elementosCorretosOrdenados = manipuladorGabarito.GetElementosOpcoesCorretas().OrderBy(manipulador => manipulador.GetOrdemSelecao());
            foreach(ManipuladorObjetoInteracao manipulador in elementosCorretosOrdenados) {
                ordemObjetosInteracao.Add(manipulador.GetNome());
                dropdownObjetos.Campo.choices.Remove(manipulador.GetNome());
            }

            listViewObjetosSelecionaveis.Rebuild();

            return;
        }
    }
}