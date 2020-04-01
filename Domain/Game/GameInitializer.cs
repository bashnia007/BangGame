using Domain.Players;
using Domain.PlayingCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Domain.Game
{
    public class GameInitializer
    {
        public void CreateGameSet()
        {
            var roles = Assembly
                .GetAssembly(typeof(Role.Role))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Role.Role)))
                //
                .ToList();
            List<Role.Role> roles2 = new List<Role.Role>();
            /*foreach(var r in roles)
            {
                Type typeArgument = r.GetType();
                var rr = new Role.RoleFactory<Role.Deputy>();
                var role = new Role.RoleFactory<typeArgument>();
                roles2.Add(role.GetInstance());
            }*/
                
            var characters = Assembly
                .GetAssembly(typeof(Character.Character))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Character.Character)))
                //.Cast<Character.Character>()
                .ToList();

            var playingCards = Assembly
                .GetAssembly(typeof(PlayingCard))
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(PlayingCard)))
                //.Cast<PlayingCard>()
                .ToList();

        }

        public Game CreateGame(List<Player> players)
        {
            return null;
        }
    }
}
