using UnityEngine;
using UnityEngine.UIElements;
using Autis.Editor.Telas;
using Autis.Editor.UI;
using Autis.Editor.Constantes;
using Autis.Editor.Utils;

public class TelaBemVindoBehaviour : JanelaEditor
{
    protected override string CaminhoTemplate => "Janelas/TelaBemVindo/TelaBemVindoTemplate.uxml";
    protected override string CaminhoStyle => "Janelas/TelaBemVindo/TelaBemVindoStyle.uss";

    private const string NOME_BOTAO_COMECAR = "botao-comecar";
    private Button botaoComecar;

    public static void ShowTelaBemVindo() {
        const string TITULO = "Bem-Vindo";

        TelaBemVindoBehaviour janela = GetWindow<TelaBemVindoBehaviour>();
        janela.titleContent = new GUIContent(TITULO);

        return;
    }

    protected override void OnRenderizarInterface() {
        // Declara os elementos visuais
        VisualElement root = rootVisualElement;
        ConfigurarBotaoCriarElementos();
    }

    private Image CriarIconeSeta() {
        return new() {
            image = Importador.ImportarImagem("seta-frente.png"),
        };
    }

    private void ConfigurarBotaoCriarElementos() {
        botaoComecar = root.Query<Button>(NOME_BOTAO_COMECAR);
        botaoComecar.RegisterCallback<ClickEvent>(AbreJanelaCriarFase);

        botaoComecar.Insert(1, CriarIconeSeta());

        return;
    }

    void AbreJanelaCriarFase(ClickEvent evento) {
        LayoutManager.CarregarLayout(ConstantesLayouts.NomeLayoutTelaInicial);
        return;
    }
}
