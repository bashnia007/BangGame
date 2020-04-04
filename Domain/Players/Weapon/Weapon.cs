using System;
using System.Collections.ObjectModel;
using Domain.PlayingCards;

namespace Domain.Players
{
    public abstract class Weapon : ValueObject<Weapon>
    {
        public abstract int Distance { get; }
        public abstract bool MultipleBang { get; }

        protected override int GetHashCodeCore()
        {
            return (Distance, MultipleBang).GetHashCode();
        }
    }

    public class Volcanic : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Volcanic;
        }

        public override int Distance => 1;
        public override bool MultipleBang => true;
    }

    public class Schofield : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Schofield;
        }

        public override int Distance => 2;
        public override bool MultipleBang => false;
    }

    public class Remington : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Remington;
        }

        public override int Distance => 3;
        public override bool MultipleBang => false;
    }

    public class Carabine : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Carabine;
        }

        public override int Distance => 4;
        public override bool MultipleBang => false;
    }

    public class Winchester : Weapon
    {
        protected override bool EqualsCore(Weapon other)
        {
            return other is Winchester;
        }

        public override int Distance => 5;
        public override bool MultipleBang => false;
    }

    public class Colt : Weapon
    {
        protected override bool EqualsCore(Weapon other) => other is Colt;

        public override int Distance => 1;
        public override bool MultipleBang => false;
    }

    public static class WeaponFactory
    {
        public static readonly Weapon DefaultWeapon = new Colt(); 
        
        public static Weapon Create(LongTermFeatureCard card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));
            
            var visitor = new CardToWeaponMatcher();
            var weapon = card.Accept(visitor);
            
            return weapon?? DefaultWeapon;
        }
    }

    public interface ICardVisitor
    {
        void Visit(VolcanicCard volcanicCard);
        void Visit(SchofieldCard schofieldCard);
        void Visit(RemingtonCard remingtonCard);
        void Visit(CarabineCard carabineCard);
        void Visit(WinchesterCard winchesterCard);

        void Visit(BarrelCard barrelCard);
        void Visit(ScopeCard scopeCard);
        void Visit(MustangCard mustangCard);
        
        void Visit(JailCard jailCard);
        void Visit(DynamiteCard dynamiteCard);
    }

    public interface ICardVisitor<out T>
    {
        T Visit(VolcanicCard volcanicCard);
        T Visit(SchofieldCard schofieldCard);
        T Visit(RemingtonCard remingtonCard);
        T Visit(CarabineCard carabineCard);
        T Visit(WinchesterCard winchester);
        
        T Visit(BarrelCard barrelCard);
        T Visit(ScopeCard scopeCard);
        T Visit(MustangCard mustangCard);
        
        T Visit(JailCard jailCard);
        T Visit(DynamiteCard dynamiteCard);
    }

    public class CardToWeaponMatcher : ICardVisitor<Weapon>
    {
        public Weapon Visit(VolcanicCard volcanicCard)
        {
            return new Volcanic();
        }

        public Weapon Visit(SchofieldCard schofieldCard)
        {
            return new Schofield();
        }

        public Weapon Visit(RemingtonCard remingtonCard)
        {
            return new Remington();
        }

        public Weapon Visit(CarabineCard carabineCard)
        {
            return new Carabine();
        }

        public Weapon Visit(WinchesterCard winchester)
        {
            return new Winchester();
        }

        public Weapon Visit(BarrelCard barrelCard) => null;
        public Weapon Visit(ScopeCard scopeCard) => null;
        public Weapon Visit(MustangCard mustangCard) => null;
        
        public Weapon Visit(JailCard jailCard) => null;
        public Weapon Visit(DynamiteCard dynamiteCard) => null;
    }
}