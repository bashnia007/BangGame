using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.GameEvents.CardEffects
{
    internal class ChangeWeaponHandler : CardActionHandler
    {
        public ChangeWeaponHandler(Gameplay gameplay, HandlerState state) : base(gameplay, state)
        {
        }

        public override HandlerState ApplyEffect(Player victim, BangGameCard card)
        {
            victim.PlayerTablet.ChangeWeapon(card);
            return new DoneState(state);
        }
    }
}