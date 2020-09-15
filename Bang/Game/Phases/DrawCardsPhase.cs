using Bang.Characters.Visitors;
using Bang.GameEvents;
using Bang.GameEvents.CardEffects.States;
using Bang.GameEvents.Enums;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game.Phases
{
    class DrawCardsPhase
    {
        private HandlerState state;
        private readonly Gameplay gameplay;
        public Player PlayerTurn { get; }

        public DrawCardsPhase(Gameplay gameplay, Player playerTurn)
        {
            this.gameplay = gameplay;
            PlayerTurn = playerTurn;
            state = new DoneState(gameplay);
        }
        
        public Response Do()
        {
            var nextState = PlayerTurn
                .Character
                .Accept(new DrawCardsCharacterVisitor())
                .Invoke(gameplay, PlayerTurn);

            if (!nextState.IsError)
                state = nextState;

            return state.SideEffect;
        }

        public Response Reply(BangGameCard card)
        {
            state = state.ApplyCardEffect(PlayerTurn, card);

            return state.SideEffect;
        }

        public Response ApplyDrawOption(DrawOptions drawOption)
        {
            state = state.ApplyDrawOption(PlayerTurn, drawOption);

            return state.SideEffect;
        }

        public Result<PlayCardPhase> NextPhase()
        {
            if (state.IsFinalState)
                return Result<PlayCardPhase>.Success(new PlayCardPhase(gameplay, PlayerTurn));
            
            return Result<PlayCardPhase>.Error($"{PlayerTurn.Name} hasn't finished draw phase");
        }
    }
}