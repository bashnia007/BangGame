using static System.Math;

namespace Domain
{
    public static class MathExtension
    {
        /// <summary>
        /// Always returns non-negative remainder
        /// </summary>
        /// <param name="number"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static int Mod(int number, int modulo)
        {
            int absModulo = Abs(modulo);
            return (number % absModulo + absModulo) % absModulo;
        }
    }
}