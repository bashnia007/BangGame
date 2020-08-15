using Bang.Game;
using Bang.GameEvents.CardEffects.States;
using Bang.GameEvents.Enums;
using Bang.Players;
using Bang.PlayingCards;
using System;

namespace Bang.GameEvents.CharacterEffects.States
{
    internal class WaitingForDrawOptionState : HandlerState
    {
        public WaitingForDrawOptionState(Gameplay gameplay) : base(gameplay)
        { }

        public override HandlerState ApplyCardEffect(Player player, BangGameCard card)
        {
            throw new NotImplementedException();
        }

        public override HandlerState ApplyDrawOption(Player player, DrawOptions drawOption)
        {
            switch (drawOption)
            {
                case DrawOptions.FromDeck:
                    player.TakeCards(2);
                    break;
                case DrawOptions.FromDiscard:
                    player.AddCardToHand(gameplay.DealCardFromDiscarded());
                    player.TakeCards(1);
                    break;
                default:
                    throw new NotImplementedException();

            }
            return new DoneState(gameplay);
        }
    }
}
