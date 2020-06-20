using Bang.Characters.Visitors;
using Bang.Characters;

namespace Gameplay.Characters.Visitors
{
    public class DistanceToPlayerVisitor : ICharacterVisitor<int>
    {
        public int DefaultValue => 0;

        public int Visit(PaulRegret character) => 1;
    }
}