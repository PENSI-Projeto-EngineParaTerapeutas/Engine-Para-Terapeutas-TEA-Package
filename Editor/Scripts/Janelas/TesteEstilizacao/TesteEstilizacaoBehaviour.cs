using UnityEditor;
using UnityEngine;
using Autis.Editor.Telas;
using Autis.Editor.UI;
using UnityEngine.UIElements;


public class TesteEstilizacaoBehaviour : JanelaEditor
{
    protected override string CaminhoTemplate => "Janelas/TesteEstilizacao/TesteEstilizacaoTemplate.uxml";
    protected override string CaminhoStyle => "Janelas/TesteEstilizacao/TesteEstilizacaoStyle.uss";

    [MenuItem("AUTIS/Teste de Estilização")]
    public static void ShowTelaBemVindo() {
        const string TITULO = "Teste de Estilização";

        TesteEstilizacaoBehaviour janela = GetWindow<TesteEstilizacaoBehaviour>();
        janela.titleContent = new GUIContent(TITULO);

        return;
    }

    protected override void OnRenderizarInterface() {
        Dropdown dropdown = new Dropdown("TesteDropdown");
        root.Add(dropdown.Root);

        Botao botao = new Botao("TesteBotao");
        root.Add(botao.Root);

        Tooltip toolTip = new Tooltip();
        root.Add(toolTip.Root);

        Toggle toggle = new Toggle();
        root.Add(toggle);

        Label label = new Label("Teste label");
        label.AddToClassList("label-padrao");
        root.Add(label);

        Slider slider = new Slider("Teste slider");
        slider.AddToClassList("slider-padrao");
        root.Add(slider);

        RadioButton rb = new RadioButton("Teste rb");
        rb.AddToClassList("radio-button-padrao");
        root.Add(rb);

        Box containerInputsNumericos = new Box();
        containerInputsNumericos.AddToClassList("container-faixa-numerica");

        InputNumerico min = new InputNumerico("Min");
        containerInputsNumericos.Add(min.Root);

        InputNumerico max = new InputNumerico("Max");
        containerInputsNumericos.Add(max.Root);

        root.Add(containerInputsNumericos);
    }
}