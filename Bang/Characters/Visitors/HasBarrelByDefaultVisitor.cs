namespace Bang.Characters.Visitors
{
    public class HasBarrelByDefaultVisitor : ICharacterVisitor<bool>
    {
        public bool DefaultValue => false;

        public bool Visit(Jourdonnais character) => true;
    }
}