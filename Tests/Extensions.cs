using CodersStrikeBack;
using FluentAssertions;

namespace Tests
{
    public static class Extensions
    {
        public static void ShouldBeApproximately(this Vector v1, Vector expected, double precision)
        {
            v1.X.Should().BeApproximately(expected.X, precision);
            v1.Y.Should().BeApproximately(expected.Y, precision);


        }
    }
}