﻿using Domain.PlayingCards.Visitors;
using System;

namespace Domain.PlayingCards
{
    
    /// Cards are played face up in front of player (exception: Jail).
    /// Blue cards in front of player are hence defined to be “in play”.
    /// The effect of these cards lasts until they are discarded or removed somehow, or a special event occurs.
    /// There is no limit on the cards player can have in front of him provided that they do not share the same name.
    [Serializable]
    public abstract class LongTermFeatureCardType : CardType
    {
        public override bool PlayAndDiscard => false;
        
        public abstract T Accept<T>(ILongTermCardTypeVisitor<T> visitor);
    }
}