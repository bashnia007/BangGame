using Bang.Characters;
using Gameplay.Characters;

namespace Bang.Characters.Visitors
{
    public interface ICharacterVisitor<T>
    {
        T Visit(BartCassidy character);
        T Visit(BlackJack character);
        T Visit(CalamityJanet character);
        T Visit(ElGringo character);
        T Visit(JessyJones character);
        T Visit(Jourdonnais character);
        T Visit(KitCarlson character);
        T Visit(LuckyDuke character);
        T Visit(PaulRegret character);
        T Visit(PedroRamirez character);
        T Visit(RoseDoolan character);
        T Visit(SidKetchum character);
        T Visit(SlabTheKiller character);
        T Visit(SuzyLafayette character);
        T Visit(VultureSam character);
        T Visit(WillyTheKid character);
    }
}