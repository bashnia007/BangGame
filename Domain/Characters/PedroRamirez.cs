﻿using System;
 using Domain.Characters.Visitors;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Pedro Ramirez (4 life points):
    /// during phase 1 of his turn, he may choose to draw the first card from the top of the discard pile or from the deck.
    /// Then, he draws the second card from the deck.
    /// </summary>
    [Serializable]
    public class PedroRamirez : Character
    {
        public override string Name => CardName.PedroRamirez;
        public override int LifePoints => 3;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(Character other) => other is PedroRamirez;

        protected override int GetHashCodeCore() => typeof(PedroRamirez).GetHashCode();
    }
}