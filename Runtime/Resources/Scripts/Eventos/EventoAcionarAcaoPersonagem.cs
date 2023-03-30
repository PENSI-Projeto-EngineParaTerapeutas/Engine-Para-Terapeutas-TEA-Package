using UnityEngine;

namespace Autis.Runtime.Eventos {
    [CreateAssetMenu(fileName = "EventoAcionarAcaoPersonagem", menuName = "Engine Para Terapeutas/Scriptable Object/Eventos/Evento Acionar Ação Personagem")]
    public class EventoAcionarAcaoPersonagem : EventoJogoGenerico<AnimationClip> {}
}