namespace Bang.Characters.Visitors
{
    public interface ICharacterVisitor<T>
    {
        T DefaultValue { get; }
        T Visit(BartCassidy character) => DefaultValue;
        T Visit(BlackJack character) => DefaultValue;
        T Visit(CalamityJanet character) => DefaultValue;
        T Visit(ElGringo character) => DefaultValue;
        T Visit(JessyJones character) => DefaultValue;
        T Visit(Jourdonnais character) => DefaultValue;
        T Visit(KitCarlson character) => DefaultValue;
        T Visit(LuckyDuke character) => DefaultValue;
        T Visit(PaulRegret character) => DefaultValue;
        T Visit(PedroRamirez character) => DefaultValue;
        T Visit(RoseDoolan character) => DefaultValue;
        T Visit(SidKetchum character) => DefaultValue;
        T Visit(SlabTheKiller character) => DefaultValue;
        T Visit(SuzyLafayette character) => DefaultValue;
        T Visit(VultureSam character) => DefaultValue;
        T Visit(WillyTheKid character) => DefaultValue;
    }
}