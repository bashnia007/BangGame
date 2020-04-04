namespace Domain.PlayingCards
{
    public class WinchesterCard : WeaponCard
    {
        public override string Description => CardName.Winchester;
        
        public override int Distance => 5;
        public override bool MultipleBang => false;


        protected override bool EqualsCore(PlayingCard other)
        {
            return other is WinchesterCard;
        }
    }
}