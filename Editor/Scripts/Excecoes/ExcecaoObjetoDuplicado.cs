using System;

namespace Autis.Editor.Excecoes {
    public class ExcecaoObjetoDuplicado : Exception {
        public string NomeObjetoDuplicado { get; set; }

        public ExcecaoObjetoDuplicado() : base() { }
        public ExcecaoObjetoDuplicado(string mensagem) : base(mensagem) { }
        public ExcecaoObjetoDuplicado(string mensagem, Exception inner) : base(mensagem, inner) { }
    }
}