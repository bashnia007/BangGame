using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Game.Phases
{
    class DiscardPhase
    {
        private readonly Gameplay gameplay;
        public Player PlayerTurn { get; }
        
        public DiscardPhase(Gameplay gameplay, Player playerTurn)
        {
            this.gameplay = gameplay;
            PlayerTurn = playerTurn;
        }

        public void Do(BangGameCard card)
        {
            gameplay.Discard(card);
        }

        public Result<DrawCardsPhase> NextPhase()
        {
            if (PlayerTurn.Hand.Count <= PlayerTurn.LifePoints)
            {
                var nextPlayer = gameplay.GetNextPlayer();
                return Result<DrawCardsPhase>.Success(new DrawCardsPhase(gameplay, nextPlayer));
            }
            
            return Result<DrawCardsPhase>.Error($"{PlayerTurn.Name} exceeds hand-size limit");
        }
    }
}