using Bang.GameEvents;
using Bang.GameEvents.CardEffects.States;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game.Phases
{
    class PlayCardPhase : IPhase<DiscardPhase>
    {
        public Player PlayerTurn { get; }
        private HandlerState state;
        private Gameplay gameplay;
        
        public PlayCardPhase(Gameplay gameplay, Player playerTurn)
        {
            this.gameplay = gameplay;
            PlayerTurn = playerTurn;
            state = new DoneState(gameplay);
        }

        public Response ApplyCardEffect(Player target)
        {
            state = state.ApplyCardEffect(target);
            return state.SideEffect;
        }
        
        public Response ApplyCardEffect(BangGameCard card, Player target)
        {
            state = state.ApplyCardEffect(target?? PlayerTurn, card);

            return state.SideEffect;
        }
        
        public Response ApplyCardEffect(BangGameCard card, BangGameCard secondCard, Player target)
        {
            state = state.ApplyCardEffect(target, card, secondCard);

            return state.SideEffect;
        }

        public Result<DiscardPhase> NextPhase()
        {
            return Result<DiscardPhase>.Success(new DiscardPhase(gameplay, PlayerTurn));
        }
    }
}