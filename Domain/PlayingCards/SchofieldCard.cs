namespace Domain.PlayingCards
{
    public class SchofieldCard : WeaponCard
    {
        public override int Distance => 2;
        public override bool MultipleBang => false;
        protected override bool EqualsCore(PlayingCard other)
        {
            return other is SchofieldCard;
        }
    }
}