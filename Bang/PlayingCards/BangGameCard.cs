using System;
using Bang.PlayingCards.Visitors;

namespace Bang.PlayingCards
{
    public class BangGameCard : ValueObject<BangGameCard>
    {
        public CardType Type { get; }
        public Suite Suite { get; }
        public Rank Rank { get; }

        public string Description => Type.Description;

        public bool IsLongTerm => Type.Accept(new IsLongTermCardTypeVisitor());
        public bool IsWeapon => Type.Accept(new IsWeaponCardVisitor());
        public bool CanBePlayedToAnotherPlayer => Type.Accept(new CanBePlayedToAnotherPlayer());
        
        public BangGameCard(CardType card, Suite suite, Rank rank)
        {
            Type = card ?? throw new ArgumentNullException(nameof(card));
            Suite = suite;
            Rank = rank;
        }

        public T Accept<T>(ICardTypeVisitor<T> visitor) => Type.Accept(visitor);

        protected override bool EqualsCore(BangGameCard other)
        {
            return Type == other.Type && Suite == other.Suite && Rank == other.Rank;
        }

        protected override int GetHashCodeCore()
        {
            return HashCode.Combine(Type, Suite, Rank);
        }
        
        public static implicit operator CardType(BangGameCard card)
        {
            return card?.Type;
        }

        public override string ToString()
        {
            return $"{Type.Description}, {Rank}, {Suite}";
        }
    }
}