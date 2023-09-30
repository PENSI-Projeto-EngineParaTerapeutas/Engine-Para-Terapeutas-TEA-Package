using Autis.Editor.Manipuladores;
using Autis.Editor.Telas;

namespace Autis.Editor.UI {
    public class EditarGabaritoArrastarBehaviour : GabaritoArrastarBehaviour {
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