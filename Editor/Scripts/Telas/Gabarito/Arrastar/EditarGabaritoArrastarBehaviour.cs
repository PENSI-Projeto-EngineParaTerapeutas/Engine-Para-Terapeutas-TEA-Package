using System.Linq;
using Autis.Editor.Manipuladores;
using Autis.Editor.Telas;

namespace Autis.Editor.UI {
    public class EditarGabaritoArrastarBehaviour : GabaritoArrastarBehaviour {
        public EditarGabaritoArrastarBehaviour() {
            if(manipuladorGabaritoArrastar.ElementosInteracaoArrastaveis.Count > 0) {
                checkboxDesfazerAcao.SetValueWithoutNotify(manipuladorGabaritoArrastar.ElementosInteracaoArrastaveis.First().DeveDesfazerAcao());
            }

            return;
        }

        protected override void ConfigurarScrollviewAssociacoes() {
            base.ConfigurarScrollviewAssociacoes();

            foreach(AssociacaoArrastavel associacao in displaysAssociacoes) {
                ManipuladorObjetoInteracao manipuladorElemento = associacao.ObjetoOrigem;
                if(manipuladorElemento.GetObjetoDestino() == null) {
                    continue;
                }

                ManipuladorObjetoInteracao manipuladorElementoDestino = manipuladorGabaritoArrastar.ElementosInteracao.Find(elemento => elemento.GetNome() == manipuladorElemento.GetObjetoDestino().name);
                associacao.SetElementoDestino(manipuladorElementoDestino);
            }

            return;
        }
    }
}