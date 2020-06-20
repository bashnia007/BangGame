using Bang.Characters.Visitors;
using Bang.Characters;

namespace Gameplay.Characters.Visitors
{
    public class DistanceFromPlayerVisitor : ICharacterVisitor<int>
    {
        public int DefaultValue => 0;

        public int Visit(RoseDoolan character) => -1;
    }
}