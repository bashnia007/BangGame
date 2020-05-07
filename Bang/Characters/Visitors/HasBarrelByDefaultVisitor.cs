using Bang.Characters;

namespace Bang.Characters.Visitors
{
    public class HasBarrelByDefaultVisitor : ICharacterVisitor<bool>
    {
        public bool Visit(BartCassidy character) => false;
        public bool Visit(BlackJack character) => false;
        public bool Visit(CalamityJanet character) => false;
        public bool Visit(ElGringo character) => false;
        public bool Visit(JessyJones character) => false;
        public bool Visit(Jourdonnais character) => true;
        public bool Visit(KitCarlson character) => false;
        public bool Visit(LuckyDuke character) => false;
        public bool Visit(PaulRegret character) => false;
        public bool Visit(PedroRamirez character) => false;
        public bool Visit(RoseDoolan character) => false;
        public bool Visit(SidKetchum character) => false;
        public bool Visit(SlabTheKiller character) => false;
        public bool Visit(SuzyLafayette character) => false;
        public bool Visit(VultureSam character) => false;
        public bool Visit(WillyTheKid character) => false;
    }
}