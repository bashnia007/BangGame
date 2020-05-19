using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;
using Gameplay.Players;

namespace Bang.GameEvents.CardEffects
{
    internal class ChangeWeaponHandler : CardActionHandler
    {
        public override HandlerState ApplyEffect(Game.Gameplay gameplay, Player victim, BangGameCard card)
        {
            victim.PlayerTablet.ChangeWeapon(card);
            return new DoneState();
        }
    }
}