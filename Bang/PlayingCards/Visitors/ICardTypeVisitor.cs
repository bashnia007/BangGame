using Bang.GameEvents.CardEffects;
using Bang.Weapons;

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

    public class IsLongTermCardTypeVisitor : ICardTypeVisitor<bool>
    {
        public bool DefaultValue => false;
        public bool Visit(BarrelCardType card) => true;
        public bool Visit(CarabineCardType card) => true;
        public bool Visit(DynamiteCardType card) => true;
        public bool Visit(JailCardType card) => true;
        public bool Visit(MustangCardType card) => true;
        public bool Visit(RemingtonCardType card) => true;
        public bool Visit(SchofieldCardType card) => true;
        public bool Visit(ScopeCardType card) => true;
        public bool Visit(VolcanicCardType card) => true;
        public bool Visit(WinchesterCardType card) => true;
    }
    
    public class IsWeaponCardVisitor : ICardTypeVisitor<bool>
    {
        public bool DefaultValue => false;

        public bool Visit(VolcanicCardType card) => true;
        public bool Visit(SchofieldCardType card) => true;
        public bool Visit(CarabineCardType card) => true;
        public bool Visit(RemingtonCardType card) => true;
        public bool Visit(WinchesterCardType card) => true;
    }
    
    public class CanBePlayedToAnotherPlayer : ICardTypeVisitor<bool>
    {
        public bool DefaultValue => false;

        public bool Visit(CatBalouCardType card) => true;
        public bool Visit(PanicCardType card) => true;
        public bool Visit(BangCardType card) => true;
        public bool Visit(JailCardType card) => true;
    }
    
    public class CardToWeaponVisitor : ICardTypeVisitor<Weapon>
    {
        public Weapon DefaultValue => null;
        public Weapon Visit(VolcanicCardType card) => new Volcanic();
        public Weapon Visit(SchofieldCardType card) => new Schofield();
        public Weapon Visit(CarabineCardType card) => new Carabine(); 
        public Weapon Visit(RemingtonCardType card) => new Remington(); 
        public Weapon Visit(WinchesterCardType card) => new Winchester();
    }

    internal class GetHandlerVisitor : ICardTypeVisitor<CardActionHandler>
    {
        public CardActionHandler DefaultValue => null;

        public CardActionHandler Visit(MustangCardType card) => new LongTermFeatureCardHandler();
        public CardActionHandler Visit(ScopeCardType card) => new LongTermFeatureCardHandler();
        public CardActionHandler Visit(BarrelCardType card) => new LongTermFeatureCardHandler();

        public CardActionHandler Visit(StagecoachCardType card) => new StageCoachCardHandler();
        public CardActionHandler Visit(WellsFargoCardType card) => new WellsFargoCoachCardHandler();

        public CardActionHandler Visit(VolcanicCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(SchofieldCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(CarabineCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(RemingtonCardType card) => new ChangeWeaponHandler();
        public CardActionHandler Visit(WinchesterCardType card) => new ChangeWeaponHandler();
    }
}