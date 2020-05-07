using Bang.Characters.Visitors;
using Bang.Characters;

namespace Gameplay.Characters.Visitors
{
    public class DistanceFromPlayerVisitor : ICharacterVisitor<int>
    {
        public int Visit(BartCassidy character) => 0;

        public int Visit(BlackJack character) => 0;

        public int Visit(CalamityJanet character) => 0;


        public int Visit(ElGringo character) => 0;

        public int Visit(JessyJones character) => 0;

        public int Visit(Jourdonnais character) => 0;

        public int Visit(KitCarlson character) => 0;

        public int Visit(LuckyDuke character) => 0;

        public int Visit(PaulRegret character) => 0;

        public int Visit(PedroRamirez character) => 0;

        public int Visit(RoseDoolan character) => -1;

        public int Visit(SidKetchum character) => 0;

        public int Visit(SlabTheKiller character) => 0;

        public int Visit(SuzyLafayette character) => 0;

        public int Visit(VultureSam character) => 0;

        public int Visit(WillyTheKid character) => 0;
    }
}