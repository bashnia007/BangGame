using Domain.PlayingCards;

namespace Domain.Messages
{
    public class ChangeWeaponMessage : Message
    {
        public BangGameCard WeaponCard { get; }

        public bool IsSuccess { get; set; }

        public ChangeWeaponMessage(BangGameCard card)
        {
            WeaponCard = card;
        }

        public override void Accept(IMessageProcessor visitor)
        {
            base.Accept(visitor);
        }
    }
}
