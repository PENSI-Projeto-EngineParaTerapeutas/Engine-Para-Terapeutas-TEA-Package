using UnityEngine;

public class IdentificadorTipoAcionamento : MonoBehaviour {

    public TipoAcionamento TipoAcionamento { get => tipoAcionamento; set { tipoAcionamento = value; } }

    [SerializeField]
    private TipoAcionamento tipoAcionamento;
}