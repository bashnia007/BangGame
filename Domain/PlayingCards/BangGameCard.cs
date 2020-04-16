using System;

namespace Domain.PlayingCards
{
    public class BangGameCard : ValueObject<BangGameCard>, IShuffledCard
    {
        public CardType Type { get; }
        public Suite Suite { get; }
        public Rank Rank { get; }

        public string Description => Type.Description;
        public bool IsLongTermCard => Type is LongTermFeatureCardType;
        public bool IsWeaponCard => Type is WeaponCardType;
        
        public BangGameCard(CardType card, Suite suite, Rank rank)
        {
            Type = card ?? throw new ArgumentNullException(nameof(card));
            Suite = suite;
            Rank = rank;
        }

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
            return card.Type;
        }
    }
}