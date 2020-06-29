namespace Bang.Characters.Visitors
{
    public class DistanceFromPlayerVisitor : ICharacterVisitor<int>
    {
        public int DefaultValue => 0;

        public int Visit(RoseDoolan character) => -1;
    }
}