using System;
using System.Linq;
using Bang.Game;
using Bang.GameEvents;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;

namespace Bang.Tests
{
    public static class PlayerExtensions
    {
        public static void WithOneLifePoint(this Player player)
        {
            player.LoseLifePoint(player.LifePoints - 1);
        }
        
        public static void Die(this Player player, Player becauseOf = null)
        {
            player.LoseLifePoint(becauseOf, player.MaximumLifePoints);
        }
        
        public static Player AsSheriff(this Player player, Gameplay gameplay)
        {
            var previousSheriff = gameplay.Players.First(p => p.Role is Sheriff);
            previousSheriff.SetInfo(gameplay, player.Role, previousSheriff.Character);

            player.SetInfo(gameplay, new Sheriff(), player.Character);
            return player;
        }
        
        public static Player AsDeputy(this Player player, Gameplay gameplay)
        {
            player.SetInfo(gameplay, new Deputy(), player.Character);
            return player;
        }
        
        public static Player AsRenegade(this Player player, Gameplay gameplay)
        {
            var previousRenegade = gameplay.Players.First(p => p.Role is Renegade);
            previousRenegade.SetInfo(gameplay, player.Role, previousRenegade.Character);
            
            
            player.SetInfo(gameplay, new Renegade(), player.Character);
            return player;
        }
        
        public static Player AsOutlaw(this Player player, Gameplay gameplay)
        {
            player.SetInfo(gameplay, new Outlaw(), player.Character);
            return player;
        }

        public static Response PlayDuel(this Player player, Gameplay gameplay, Player opponent)
        {
            var duelCard = player.Hand.FirstOrDefault(c => c == new DuelCardType());
            if (duelCard == null)
                throw new ArgumentException("Player doesn't have a duel card");

            return player.PlayCard(gameplay, duelCard, opponent);
        }

        public static Response PlayStageCoach(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new StagecoachCardType());
        public static Response PlayWellsFargo(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new WellsFargoCardType());
        public static Response PlayScope(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new ScopeCardType());
        public static Response PlayMustang(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new MustangCardType());
        public static Response PlayBarrel(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new BarrelCardType());
        
        public static Response PlayIndians(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new IndiansCardType());
        
        public static Response PlayGatling(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new GatlingCardType());

        public static Response PlayDynamite(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new DynamiteCardType());

        public static Response PlayBang(this Player player, Gameplay gameplay, Player target) => player.PlayCard(
            gameplay, new BangCardType(),
            target);
        
        public static Response PlayMissed(this Player player, Gameplay gameplay, Player target) => player.PlayCard(
            gameplay, new MissedCardType(),
            target);
        
        public static Response PlayBeer(this Player player, Gameplay gameplay) => player.PlayCard(
            gameplay, new BeerCardType());
        
        public static Response PlayGeneralStore(this Player player, Gameplay gameplay) => player.PlayCard(
            gameplay, new GeneralStoreCardType());
        
        public static Response PlaySaloon(this Player player, Gameplay gameplay) => player.PlayCard(
            gameplay, new SaloonCardType());
        
        public static Response PlayPanic(this Player player, Gameplay gameplay, Player target) => player.PlayCard(
            gameplay, new PanicCardType(),
            target);
        
        public static Response PlayCatBalou(this Player player, Gameplay gameplay, Player target) => player.PlayCard(
            gameplay, new CatBalouCardType(),
            target);
        
        public static Response PlayJail(this Player player, Gameplay gameplay, Player target) => player.PlayCard(
            gameplay, new JailCardType(),
            target);
        
        public static Response PlayVolcanic(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new VolcanicCardType());

        public static Response PlaySchofield(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new SchofieldCardType());
        
        public static Response PlayRemington(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new RemingtonCardType());
        
        public static Response PlayCarabine(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new CarabineCardType());
        
        public static Response PlayWinchester(this Player player, Gameplay gameplay) => player.PlayCard(gameplay, new WinchesterCardType());
        
        public static Response PlayCard(this Player player, Gameplay gameplay, CardType cardType, Player target = null)
        {
            var card = player.Hand.FirstOrDefault(c => c == cardType);
            if (card == null)
                throw new ArgumentException($"Player {player.Name} doesn't have a {cardType}");
            

            return gameplay.Handle(new PlayCardMessage(player, card, target));;
        }

        public static Response PlayTwoCardsAsOne(this Player player, Gameplay gameplay, BangGameCard firstCard,
            BangGameCard secondCard)
        {
            return gameplay.Handle(new PlayCardMessage(player, firstCard, secondCard));
        }

        public static Response ForceToDropCard(this Player player, Gameplay gameplay, Player victim, BangGameCard card)
        {
            return gameplay.Handle(
                    new ReplyActionMessage(player)
                    {
                        Response = new ForcePlayerToDropCardResponse(card)
                        {
                            Player = victim,
                        } 
                    });
        }
        
        public static Response ForceToDropRandomCard(this Player player, Gameplay gameplay, Player victim)
        {
            return gameplay.Handle(
                new ReplyActionMessage(player)
                {
                    Response = new ForcePlayerToDropCardResponse()
                    {
                        Player = victim,
                    } 
                });
        }
        
        public static Response DrawCardFromPlayer(this Player player, Gameplay gameplay, Player victim)
        {
            return gameplay.Handle(
                new ReplyActionMessage(player)
                {
                    Response = new DrawCardFromPlayerResponse()
                    {
                        Player = victim,
                    } 
                });
        }
        
        public static Response DrawCardFromPlayer(this Player player, Gameplay gameplay, Player victim, BangGameCard card)
        {
            var message = new ReplyActionMessage(player)
                    {
                        Response = new DrawCardFromPlayerResponse(card)
                        {
                            Player = victim,
                        } 
                    };
            return Handle(gameplay, message);
        }

        public static Response DefenseAgainstBang(this Player player, Gameplay gameplay, BangGameCard card, BangGameCard secondCard = null)
        {
            var message = new ReplyActionMessage(player)
            {
                Response = new DefenceAgainstBang()
                {
                    Player = player,
                    FirstCard = card,
                    SecondCard = secondCard
                } 
            };
            return Handle(gameplay, message);
        }
        
        public static Response NotDefenseAgainstBang(this Player player, Gameplay gameplay)
        {
            return DefenseAgainstBang(player, gameplay, null);
        }
        
        public static Response DefenseAgainstDuel(this Player player, Gameplay gameplay, BangGameCard card)
        {
            var message = new ReplyActionMessage(player)
            {
                Response = new DefenceAgainstDuel()
                {
                    Player = player,
                    Card = card
                } 
            };
            return Handle(gameplay, message);
        }
        
        public static Response LoseDuel(this Player player, Gameplay gameplay)
        {
            return DefenseAgainstDuel(player, gameplay, null);
        }
        
        public static Response DefenseAgainstGatling(this Player player, Gameplay gameplay, BangGameCard card)
        {
            var message = new ReplyActionMessage(player)
            {
                Response = new DefenceAgainstBang()
                {
                    Player = player,
                    FirstCard = card
                } 
            };
            return Handle(gameplay, message);
        }

        public static Response DefenseAgainstIndians(this Player player, Gameplay gameplay, BangGameCard card)
        {
            var message = new ReplyActionMessage(player)
            {
                Response = new DefenceAgainstIndians()
                {
                    Player = player,
                    Card = card
                } 
            };
            return Handle(gameplay, message);
        }
        
        public static Response NotDefenseAgainstIndians(this Player player, Gameplay gameplay)
        {
            return DefenseAgainstIndians(player, gameplay, null);
        }

        private static Response Handle(Gameplay gameplay, BangGameMessage message)
        {
            return gameplay.Handle(message);
        }

        public static Response ChooseCardAfterGeneralStore(this Player player, Gameplay gameplay, BangGameCard card)
        {
            var message = new ReplyActionMessage(player)
            {
                Response = new TakeCardAfterGeneralStoreResponse(player, card)
                {
                    
                } 
            };
            return Handle(gameplay, message);
        }
        
        public static Response ChooseCardToReturn(this Player player, Gameplay gameplay, BangGameCard card)
        {
            var message = new ReplyActionMessage(player)
            {
                Response = new ChooseCardToReturnResponse()
                {
                    CardToReturn = card,
                    Player = player
                } 
            };
            return Handle(gameplay, message);
        }

        public static void FinishTurn(this Player player)
        {
            var result = player.EndTurn();
            while (result is NotAllowedOperation)
            {
                player.DropCard(player.Hand[0]);
                result = player.EndTurn();
            }
        }
    }
}