using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bang.Characters;
using Bang.Characters.Visitors;
using Bang.Exceptions;
using Bang.Game;
using Bang.PlayingCards;
using Bang.Roles;
using Bang.GameEvents;
using Bang.Weapons;

namespace Bang.Players
{
    [DebuggerDisplay("Name = {Name}, Character = {this.Character}")]
    [Serializable]
    public abstract class Player : Entity
    {
        public override string Id { get; protected set; }
        public string Name { get; set; }
        public Role Role { get; private set; }
        public PlayerTablet PlayerTablet { get; private set; }
        private readonly List<BangGameCard> hand;
        public IReadOnlyList<BangGameCard> Hand => hand;
        public virtual bool IsReadyToPlay { get; set; }

        public IReadOnlyList<BangGameCard> ActiveCards => PlayerTablet.ActiveCards;
        public Character Character => PlayerTablet.Character;
        public int MaximumLifePoints => PlayerTablet.MaximumHealth;
        public int LifePoints => PlayerTablet.Health;
        public bool IsAlive => PlayerTablet.IsAlive;
        public Weapon Weapon => PlayerTablet.Weapon;

        private Gameplay gamePlay;

        public Player()
        {
            hand = new List<BangGameCard>();
        }

        // TODO it would be much better add constructor with this parameters
        public void SetInfo(Gameplay gamePlay, Role role, Character character)
        {
            this.gamePlay = gamePlay?? throw new ArgumentNullException();
            Role = role?? throw new ArgumentNullException();
            PlayerTablet = new PlayerTablet(character, role is Sheriff);
        }

        public void AddCardToHand(BangGameCard card)
        {
            if (card == null) throw new ArgumentNullException(nameof(card));
            
            hand.Add(card);
        }

        public void DiscardActiveCard(BangGameCard card)
        {
            PlayerTablet.RemoveCard(card);
            
            gamePlay.Discard(card);
        }

        /// <summary>
        /// Drops card as discard phase
        /// </summary>
        /// <param name="cardsToDrop"></param>
        public void DropCards(List<BangGameCard> cardsToDrop)
        {
            gamePlay.DropCardsFromHand(cardsToDrop);
            foreach (var card in cardsToDrop)
            {
                hand.Remove(card);
            }
        }

        public void DropPlayedCard(BangGameCard card)
        {
            gamePlay.DropPlayedCard(this, card);
        }

        public void DropCard(BangGameCard cardToDrop)
        {
            DropCards(new List<BangGameCard> { cardToDrop });
        }
        
        public void PutCardOnDeck(BangGameCard card)
        {
            gamePlay.PutCardOnDeck(card);
            hand.Remove(card);
        }

        public void TakeCards(byte amount)
        {
            for (int i = 0; i < amount; i++)
            {
                var newCard = gamePlay.DealCard();
                hand.Add(newCard);
            }
        }

        public void RegainLifePoint()
        {
            PlayerTablet.Health++;
        }

        public void LoseLifePoint(int loseLifeAmount = 1) => LoseLifePoint(null, loseLifeAmount);
        
        public void LoseLifePoint(Player responsible, int loseLifeAmount = 1)
        {
            if(loseLifeAmount <= 0)
                throw new ArgumentOutOfRangeException();
            
            if (!IsAlive)
                throw new InvalidOperationException($"Player {Name} is already dead");

            for (int i = 0; i < loseLifeAmount; i++)
            {
                PlayerTablet.Health--;
            }

            if (IsAlive)
            {
                var visitor = new LoseLifePointCharacterVisitor();
                var action = Character.Accept(visitor);

                var damageInfo = new DamageInfo{Damage = (byte) loseLifeAmount, Hitter = responsible};
                action(this, damageInfo);
            }
            else
            {
                PlayerEliminator.Eliminate(this, responsible, gamePlay.AlivePlayers);
            }
        }
        
        /// <summary>
        /// Remove card (but don't put it on discard pile)
        /// </summary>
        /// <param name="card"></param>
        public void LoseCard(BangGameCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            if (Hand.Any(c => c == card))
            {
                hand.Remove(card);
            }
            else
            {
                PlayerTablet.RemoveCard(card);
            }
        }

        public void DrawPlayerActiveCard(Player victim, BangGameCard card)
        {
            victim.PlayerTablet.RemoveCard(card);
            AddCardToHand(card);
        }

        internal void DrawCardFromPlayer(Player victim)
        {
            if (victim == null)
                throw new ArgumentNullException(nameof(victim));
                        
            var card = RandomCardChooser.ChooseCard(victim.Hand);
            if (card != null)
            {
                victim.LoseCard(card);
                AddCardToHand(card);
            }

            if (victim.Character is SuzyLafayette && victim.Hand.Count == 0)
            {
                victim.AddCardToHand(gamePlay.DealCard());
            }
        }

        /// <summary>
        /// Drops all cards (active and hand) to discard pile without changing the phase
        /// </summary>
        public void DropAllCards()
        {
            foreach (var card in Hand.ToList())
            {
                LoseCard(card);
                gamePlay.Discard(card);
            }
            
            foreach (var activeCard in ActiveCards.ToList())
            {
                LoseCard(activeCard);
                gamePlay.Discard(activeCard);
            }
        }

        // TODO remove it
        public Response EndTurn()
        {
            return gamePlay.EndTurn();
        }

        public override string ToString()
        {
            return $"{Name}({Character})";
        }
    }
}
