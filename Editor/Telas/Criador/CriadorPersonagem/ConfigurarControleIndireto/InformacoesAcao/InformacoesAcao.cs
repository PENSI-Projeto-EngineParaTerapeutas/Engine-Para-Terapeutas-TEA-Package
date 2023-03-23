using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EngineParaTerapeutas.Utils;

namespace EngineParaTerapeutas.UI {
    public class InformacoesAcao : ElementoInterfaceEditor {
        private const string IMAGEM_ANIMACAO = "icone-animacao.png";
        private const string IMAGEM_LIXEIRA = "icone-lixeira.png";

        protected override string CaminhoTemplate => "Telas/Criador/CriadorPersonagem/ConfigurarControleIndireto/InformacoesAcao/InformacoesAcaoTemplate.uxml";
        protected override string CaminhoStyle => "Telas/Criador/CriadorPersonagem/ConfigurarControleIndireto/InformacoesAcao/InformacoesAcaoStyle.uss";

        #region .: Elementos :.

        private const string NOME_IMAGEM_ICONE_ANIMACAO = "imagem-icone-animacao";
        private readonly Image iconeAnimacao;

        private const string NOME_LABEL_ASSOCIACAO_OBJETO_ANIMACAO = "label-associacao-objeto-animacao";
        private readonly Label associacaoObjetoAnimacao;

        private const string NOME_IMAGEM_ICONE_LIXEIRA = "imagem-icone-lixeira";
        private readonly Image iconeLixeira;

        #endregion

        public InformacoesAcao(string nomeObjeto, string nomeAnimacao) {
            iconeAnimacao = Root.Query<Image>(NOME_IMAGEM_ICONE_ANIMACAO);
            associacaoObjetoAnimacao = Root.Query<Label>(NOME_LABEL_ASSOCIACAO_OBJETO_ANIMACAO);
            iconeLixeira = Root.Query<Image>(NOME_IMAGEM_ICONE_LIXEIRA);

            ConfigurarImagens();
            ConfigurarLabel(nomeObjeto, nomeAnimacao);

            return;
        }

        private void ConfigurarImagens() {
            iconeAnimacao.image = Importador.ImportarImagem(IMAGEM_ANIMACAO);
            iconeLixeira.image = Importador.ImportarImagem(IMAGEM_LIXEIRA);

            return;
        }

        private void ConfigurarLabel(string nomeObjeto, string nomeAnimacao) {
            associacaoObjetoAnimacao.text = nomeObjeto + " - " + nomeAnimacao;
            return;
        }
    }
}