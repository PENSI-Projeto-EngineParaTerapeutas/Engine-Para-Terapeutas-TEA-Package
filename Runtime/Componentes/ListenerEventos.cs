using UnityEngine;
using EngineParaTerapeutas.Eventos;

public class ListenerEventos : MonoBehaviour {

    private EventoJogo eventoErro;
    private EventoJogo eventoAcerto;
    private EventoJogo eventoFimJogo;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private TipoAcionamento tipoAcionamento;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        tipoAcionamento = GetComponent<IdentificadorTipoAcionamento>().TipoAcionamento;

        spriteRenderer.enabled = false;
        audioSource.enabled = false;

        eventoErro = Resources.Load<EventoJogo>("ScriptableObjects/EventoErro");
        eventoAcerto = Resources.Load<EventoJogo>("ScriptableObjects/EventoAcerto");
        eventoFimJogo = Resources.Load<EventoJogo>("ScriptableObjects/EventoFimJogo");

        eventoErro.AdicionarCallback(HandleEventoErro);
        eventoAcerto.AdicionarCallback(HandleEventoAcerto);
        eventoFimJogo.AdicionarCallback(HandleEventoFimJogo);

        return;
    }

    public virtual void HandleEventoErro() {
        if(tipoAcionamento != TipoAcionamento.Erro) {
            return;
        }

        HabilitarComponentes();
        return;
    }

    private void HabilitarComponentes() {
        spriteRenderer.enabled = true;
        audioSource.enabled = true;

        audioSource.Play();

        return;
    }

    public virtual void HandleEventoAcerto() {
        if(tipoAcionamento != TipoAcionamento.Acerto) {
            return;
        }

        HabilitarComponentes();
        return;
    }

    public virtual void HandleEventoFimJogo() {
        if(tipoAcionamento != TipoAcionamento.FimJogo) {
            return;
        }

        HabilitarComponentes();
        return;
    }
}