namespace Bang.Characters.Visitors
{
    public class DistanceToPlayerVisitor : ICharacterVisitor<int>
    {
        public int DefaultValue => 0;

        public int Visit(PaulRegret character) => 1;
    }
}