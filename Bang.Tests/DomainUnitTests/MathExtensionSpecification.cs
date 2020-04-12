using System;
using FluentAssertions;
using static Domain.MathExtension;

using Xunit;

namespace Bang.Tests.DomainUnitTests
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
        public void Mod_works_correctly(int number, int modulo, int expected)
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