using System;
using System.Collections.Generic;

namespace Autis.Editor.Excecoes {
    public class ExcecaoCamposObrigatoriosVazios : Exception {
        public List<string> NomesCamposVazios { get; set; }

        public ExcecaoCamposObrigatoriosVazios() : base() {
            NomesCamposVazios = new List<string>();
            return;
        }

        public ExcecaoCamposObrigatoriosVazios(string mensagem) : base(mensagem) {
            NomesCamposVazios = new List<string>();
            return;
        }

        public ExcecaoCamposObrigatoriosVazios(string mensagem, Exception inner) : base(mensagem, inner) {
            NomesCamposVazios = new List<string>();
            return;
        }
    }
}