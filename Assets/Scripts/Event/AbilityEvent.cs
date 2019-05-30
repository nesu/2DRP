using Assets.Scripts.Model;

namespace Assets.Scripts.Event
{
    public class AbilityEvent : GameEvent
    {
        public Ability Ability;
        public float Directed;

        public AbilityEvent(Ability ability, float direction)
        {
            Ability = ability;
            Directed = direction;
        }
    }
}
