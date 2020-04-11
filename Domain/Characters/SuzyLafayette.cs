﻿using System;
 using Domain.Characters.Visitors;

 namespace Domain.Characters
{
    
    /// <summary>
    /// Suzy Lafayette (4 life points):
    /// as soon as she has no cards in her hand, she draws a card from the draw pile.
    /// </summary>
    [Serializable]
    public class SuzyLafayette : Character
    {
        public override string Name => CardName.SuzyLafayette;
        public override int LifePoints => 4;
        internal override T Accept<T>(ICharacterVisitor<T> visitor) => visitor.Visit(this);

        protected override bool EqualsCore(Character other) => other is SuzyLafayette;

        protected override int GetHashCodeCore() => typeof(SuzyLafayette).GetHashCode();
    }
}