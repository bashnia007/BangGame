using System;
using System.Data;
using Bang.Characters;
using Bang.Game;
using Bang.GameEvents.CardEffects;
using Bang.GameEvents.CardEffects.States;

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

    internal class GetHandlerVisitor : ICardTypeVisitor<Func<Gameplay, HandlerState, Character, CardActionHandler>>
    {
        public Func<Gameplay, HandlerState, Character, CardActionHandler> DefaultValue => null;

        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(MustangCardType card)
        {
            return (gameplay, state, character) => new LongTermFeatureCardHandler(gameplay, state);
        }

        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(ScopeCardType card) => 
            (gameplay, state, character) => new LongTermFeatureCardHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(BarrelCardType card) => 
            (gameplay, state, character) => new LongTermFeatureCardHandler(gameplay, state);

        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(StagecoachCardType card) => 
            (gameplay, state, character) => new StageCoachCardHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(WellsFargoCardType card) => 
            (gameplay, state, character) => new WellsFargoCoachCardHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(BangCardType card) => 
            (gameplay, state, character) => new BangCardHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(MissedCardType card) => 
            (gameplay, state, character) => character == new CalamityJanet() ? new BangCardHandler(gameplay, state) : null;
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(VolcanicCardType card) => 
            (gameplay, state, character) => new ChangeWeaponHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(SchofieldCardType card) => 
            (gameplay, state, character) => new ChangeWeaponHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(CarabineCardType card) => 
            (gameplay, state, character) => new ChangeWeaponHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(RemingtonCardType card) => 
            (gameplay, state, character) => new ChangeWeaponHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(WinchesterCardType card) => 
            (gameplay, state, character) => new ChangeWeaponHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(CatBalouCardType card) => 
        (gameplay, state, character) => new CatBalouHandler(gameplay, state);

        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(GatlingCardType card) => 
            (gameplay, state, character) => new GatlingActionHandler(gameplay, state);

        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(PanicCardType card) => 
            (gameplay, state, character) => new PanicHandler(gameplay, state);

        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(IndiansCardType card) => 
            (gameplay, state, character) => new IndiansActionHandler(gameplay, state);

        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(GeneralStoreCardType card) => 
            (gameplay, state, character) => new GeneralStoreActionHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(DuelCardType card) => 
            (gameplay, state, character) => new DuelActionHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(BeerCardType card) => 
            (gameplay, state, character) => new BeerActionHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(SaloonCardType card) => 
            (gameplay, state, character) => new SaloonCardHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(JailCardType card) => 
            (gameplay, state, character) => new JailActionHandler(gameplay, state);
        public Func<Gameplay, HandlerState, Character, CardActionHandler> Visit(DynamiteCardType card) => 
            (gameplay, state, character) => new DynamiteActionHandler(gameplay, state);
    }
}