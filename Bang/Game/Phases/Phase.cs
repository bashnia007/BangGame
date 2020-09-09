using Bang.Players;

namespace Bang.Game.Phases
{
    interface IPhase<T>
    {
        public Player PlayerTurn { get; }

        public Result<T> NextPhase();
    }
}