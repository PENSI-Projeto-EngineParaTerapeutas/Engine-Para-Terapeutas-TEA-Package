using UnityEngine;
using UnityEngine.UIElements;

public class ConfiguracaoCenarioBehaviour : ElementoInterfaceJogo {
    #region .: Elementos :.

    private const string NOME_LABEL_PREVIEW_CENARIO = "label-preview-cenario";
    private readonly Label labelPreviewCenario;

    private const string NOME_PREVIEW_CENARIO = "preview-cenario";
    private readonly Image previewCenario;

    #endregion

    private GameObject cenario;
    private SpriteRenderer spriteCenario;

    public ConfiguracaoCenarioBehaviour() {
        ImportarTemplate("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoReforco/ConfiguracaoReforcoTemplate");
        ImportarStyle("Scripts/Telas/PreConfiguracaoJogo/ConfiguracaoReforco/ConfiguracaoReforcoStyle");

        labelPreviewCenario = Root.Query<Label>(NOME_LABEL_PREVIEW_CENARIO);
        previewCenario = Root.Query<Image>(NOME_PREVIEW_CENARIO);

        return;
    }
}