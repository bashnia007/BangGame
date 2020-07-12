using System;
using System.Collections.Generic;
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
            characterDeck = GamePlayInitializer.CharactersDeck();
        }

        public GameplayBuilder WithDeck(Deck<BangGameCard> deck)
        {
            cardsDeck = deck;
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
                characterDeck.Put(oldDeck.Deal());
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