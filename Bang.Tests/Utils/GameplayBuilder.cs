using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;

namespace Bang.Tests
{
    public class GameplayBuilder
    {
        private int playersAmount;
        private Deck<BangGameCard> cardsDeck;
        private Deck<Character> characterDeck;

        private List<Character> charactersMustToBe = new List<Character>();
        
        public GameplayBuilder(int playersAmount = 4)
        {
            this.playersAmount = playersAmount;
            cardsDeck = GamePlayInitializer.BangGameDeck();
            Debug.WriteLine("Playing cards in the deck = " + cardsDeck.Count);
            characterDeck = GamePlayInitializer.CharactersDeck();
            Debug.WriteLine("Characters cards in the deck = " + characterDeck.Count);
        }

        public GameplayBuilder WithDeck(Deck<BangGameCard> deck)
        {
            var stack = new Stack<BangGameCard>();
            while (!deck.IsEmpty())
            {
                stack.Push(deck.Deal());
            }
            while(stack.Count > 0)
            {
                cardsDeck.Put(stack.Pop());
            }
            return this;
        }

        public GameplayBuilder WithCharacterDeck(Deck<Character> characterDeck)
        {
            this.characterDeck = characterDeck;
            return this;
        }

        public GameplayBuilder WithoutCharacter(Character character)
        {
            charactersMustToBe.Remove(character);
            
            var dropped = new Stack<Character>();

            Character person = characterDeck.Deal();
            while (person != null && person != character)
            {
                dropped.Push(person);
                person = characterDeck.Deal();
            }

            while (dropped.Count > 0)
            {
                characterDeck.Put(dropped.Pop());
            }
            
            return this;
        }

        public GameplayBuilder WithCharacter(Character character)
        {
            charactersMustToBe.Add(character);
            return this;
        }

        private void UpdateCharacterDeck()
        {
            if (!charactersMustToBe.Any()) return;

            var oldDeck = characterDeck;
            characterDeck = new Deck<Character>();

            foreach (var character in charactersMustToBe)
            {
                characterDeck.Put(character);
            }

            for (int i = 0; i < playersAmount - charactersMustToBe.Count; i++)
            {
                var character = oldDeck.Deal();

                if (charactersMustToBe.Contains(character))
                {
                    character = oldDeck.Deal();
                }
                
                characterDeck.Put(character);
            }
        }

        public Gameplay Build()
        {
            var players = new List<Player>();
            UpdateCharacterDeck();
            
            for (int i = 0; i < playersAmount; i++)
            {
                var player = new PlayerOnline(Guid.NewGuid().ToString());
                players.Add(player);
            }
            
            var gameplay = new Gameplay(characterDeck, cardsDeck);
            
            gameplay.Initialize(players);
            
            return gameplay;
        }
    }
}