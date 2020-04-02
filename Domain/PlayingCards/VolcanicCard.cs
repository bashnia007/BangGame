namespace Domain.PlayingCards
{
    public class VolcanicCard : WeaponCard
    {
        public override string Description => CardName.Volcanic;
        public override int Distance => 1;
        public override bool MultipleBang => true;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is VolcanicCard;
        }
    }
}