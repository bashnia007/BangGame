using System;
using FluentAssertions;
using Xunit;

using static Bang.MathExtension;

namespace Bang.Tests
{
    public class MathExtensionSpecification
    {
        [Theory]
        [InlineData(-3, 3, 0)]
        [InlineData(-2, 3, 1)]
        [InlineData(-1, 3, 2)]
        [InlineData(0, 3, 0)]
        [InlineData(1, 3, 1)]
        [InlineData(2, 3, 2)]
        [InlineData(3, 3, 0)]
        
        [InlineData(-1, -3, 2)]
        [InlineData(2, -3, 2)]
        public void Mod_is_always_non_negative(int number, int modulo, int expected)
        {
            Mod(number, modulo).Should().Be(expected);
        }

        [Fact]
        public void Modulo_can_not_be_zero()
        {
            Assert.Throws<DivideByZeroException>(() => Mod(1, 0));
        }
    }
}