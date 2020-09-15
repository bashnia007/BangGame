using System;
using System.Diagnostics;
using Bang.Players;

namespace Bang.Game.Phases
{
    // TODO create a proper name   
    internal class PhaseTransition
    {
        internal DrawCardsPhase DrawCardsPhase { get; private set; }
        internal  PlayCardPhase PlayCardPhase { get; private set; }
        internal DiscardPhase DiscardPhase { get; private set; }
        
        public Player PlayerTurn =>
            DrawCardsPhase?.PlayerTurn ??
            PlayCardPhase?.PlayerTurn ??
            DiscardPhase?.PlayerTurn;

        public PhaseTransition(Gameplay gameplay, Player player)
        {
            DrawCardsPhase = new DrawCardsPhase(gameplay, player); 
        }

        public Result<DrawCardsPhase> ToDrawCardsPhase()
        {
            if (PlayCardPhase != null)
            {
                var playCardPhaseResult = PlayCardPhase.NextPhase();
                if (playCardPhaseResult.IsSuccess)
                {
                    DiscardPhase = playCardPhaseResult.Value;
                    PlayCardPhase = null;
                }
                else
                    return Result<DrawCardsPhase>.Error(playCardPhaseResult.Reason);
            }

            var result = DiscardPhase.NextPhase();
            if (result.IsSuccess)
            {
                DrawCardsPhase = result.Value;
                DiscardPhase = null;
                return Result<DrawCardsPhase>.Success(DrawCardsPhase);
            }
                
            return Result<DrawCardsPhase>.Error(result.Reason);
        }

        public Result<PlayCardPhase> ToPlayCardsPhase()
        {
            if (DrawCardsPhase != null)
            {
                var result = DrawCardsPhase.NextPhase();
                if (result.IsSuccess)
                {
                    PlayCardPhase = result.Value;
                    DrawCardsPhase = null;
                }
                else
                    return Result<PlayCardPhase>.Error(result.Reason);
            }
            
            Debug.Assert(PlayCardPhase != null);
            
            return Result<PlayCardPhase>.Success(PlayCardPhase);
        }

        public Result<DiscardPhase> ToDiscardPhase()
        {
            if (DrawCardsPhase != null)
            {
                var result = ToPlayCardsPhase();
                if (!result.IsSuccess)
                    return Result<DiscardPhase>.Error(result.Reason);
            }
            
            if (PlayCardPhase != null)
            {
                var result = PlayCardPhase.NextPhase();
                
                Debug.Assert(result.IsSuccess);
                DiscardPhase = result.Value;
                PlayCardPhase = null;
            }
            
            if (DiscardPhase == null) throw new InvalidOperationException();

            return Result<DiscardPhase>.Success(DiscardPhase);
        }

        public void SkipTurn(Gameplay gameplay, Player nextPlayerTurn)
        {
            DrawCardsPhase = new DrawCardsPhase(gameplay, nextPlayerTurn);
        }
    }
}