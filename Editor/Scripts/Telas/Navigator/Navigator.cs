using System.Linq;
using System.Collections.Generic;

namespace Autis.Editor.Telas {
    public class Navigator {
        private static readonly Tela TELA_PADRAO = new MenuPrincipalBehaviour();

        public static Navigator Instance { 
            get {
                if(instance == null) {
                    instance = new Navigator();
                }

                return instance;
            }
        }
        private static Navigator instance;
        
        private readonly static List<Tela> telas = new();

        public Tela TelaAtual { 
            get {
                if(telas.Count <= 0) {
                    return TELA_PADRAO;
                }

                return telas.Last();
            }
        }

        private Navigator() {}

        public void IrPara(Tela tela) {
            telas.Add(tela);
            return;
        }

        public void Voltar() {
            telas.RemoveAt(telas.Count - 1);
            return;
        }

        public void VoltarParaTelaInicial() {
            Tela telaInicial = telas.First();
            
            telas.Clear();
            telas.Add(telaInicial);

            return;
        }
    }
}