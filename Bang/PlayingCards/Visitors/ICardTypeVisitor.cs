using Bang.GameEvents.CardEffects;

namespace Bang.PlayingCards.Visitors
{
    public interface ICardTypeVisitor<T>
    {
        T DefaultValue { get; }
        T Visit(BangCardType card) => DefaultValue;
        T Visit(BarrelCardType card) => DefaultValue;
        T Visit(BeerCardType card) => DefaultValue;
        T Visit(CarabineCardType card) => DefaultValue;
        T Visit(CatBalouCardType card) => DefaultValue;
        T Visit(DuelCardType card) => DefaultValue;
        T Visit(DynamiteCardType card) => DefaultValue;
        T Visit(GatlingCardType card) => DefaultValue;
        T Visit(GeneralStoreCardType card) => DefaultValue;
        T Visit(IndiansCardType card) => DefaultValue;
        T Visit(JailCardType card) => DefaultValue;
        T Visit(MissedCardType card) => DefaultValue;
        T Visit(MustangCardType card) => DefaultValue;
        T Visit(PanicCardType card) => DefaultValue;
        T Visit(RemingtonCardType card) => DefaultValue;
        T Visit(SaloonCardType card) => DefaultValue;
        T Visit(SchofieldCardType card) => DefaultValue;
        T Visit(ScopeCardType card) => DefaultValue;
        T Visit(StagecoachCardType card) => DefaultValue;
        T Visit(VolcanicCardType card) => DefaultValue;
        T Visit(WellsFargoCardType card) => DefaultValue;
        T Visit(WinchesterCardType card) => DefaultValue;
    }

    internal class GetHandlerVisitor : ICardTypeVisitor<CardActionHandler>
    {
        public CardActionHandler DefaultValue => null;

        public CardActionHandler Visit(MustangCardType card) => new LongTermFeatureCardHandler();
        public CardActionHandler Visit(ScopeCardType card) => new LongTermFeatureCardHandler();
        public CardActionHandler Visit(BarrelCardType card) => new LongTermFeatureCardHandler();

        public CardActionHandler Visit(StagecoachCardType card) => new StageCoachCardHandler();
        public CardActionHandler Visit(WellsFargoCardType card) => new WellsFargoCoachCardHandler();
        public CardActionHandler Visit(BangCardType card) => new BangCardHandler();

        public CardActionHandler Visit(VolcanicCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(SchofieldCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(CarabineCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(RemingtonCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(WinchesterCardType card) => new ChangeWeaponHandler();

        public CardActionHandler Visit(CatBalouCardType card) => new CatBalouHandler();

        public CardActionHandler Visit(GatlingCardType card) => new GatlingActionHandler();

        public CardActionHandler Visit(PanicCardType card) => new PanicHandler();

        public CardActionHandler Visit(IndiansCardType card) => new IndiansActionHandler();

        public CardActionHandler Visit(GeneralStoreCardType card) => new GeneralStoreActionHandler();
        public CardActionHandler Visit(DuelCardType card) => new DuelActionHandler();
        public CardActionHandler Visit(BeerCardType card) => new BeerActionHandler();
    }
}