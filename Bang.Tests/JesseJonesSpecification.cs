using Bang.Characters;
using Bang.Game;
using Bang.Players;
using Bang.PlayingCards;
using Bang.Roles;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Bang.Game.GamePlayInitializer;

namespace Bang.Tests
{
	public class JesseJonesSpecification
	{
		#region Tests
		[Fact]
		public void Jesse_Jones_starts_game_with_4_life_points()
		{
			var gamePlay = InitGame();
			var jesse = SetCharacter(gamePlay, new JessyJones());

			jesse.LifePoints.Should().Be(4);
		}

		[Fact]
		 public void Other_player_looses_one_card_if_it_was_taken_by_Jesse_Jones()
		{
			var gamePlay = InitGame();
			var jesse = SetCharacter(gamePlay, new JessyJones());
			var victim = gamePlay.Players.First(x => x != jesse);
			var victimCountCards = victim.Hand.Count;
			// Steal card form the victim

			victim.Hand.Count.Should().Be(victimCountCards - 1);
		}

		#endregion

		#region Private Methods
		private Game.Gameplay InitGame() => InitGame(BangGameDeck());

		private Game.Gameplay InitGame(Deck<BangGameCard> deck)
		{
			var players = new List<Player>();
			for (int i = 0; i < 4; i++)
			{
				var player = new PlayerOnline(Guid.NewGuid().ToString());
				players.Add(player);
			}

			var gameplay = new Game.Gameplay(CharactersDeck(), deck);
			gameplay.Initialize(players);

			return gameplay;
		}

		private Player SetCharacter(Game.Gameplay gameplay, Character character)
		{
			var actor = gameplay.PlayerTurn;
			actor.SetInfo(gameplay, new Outlaw(), character);

			return actor;
		}

		#endregion
	}
}
