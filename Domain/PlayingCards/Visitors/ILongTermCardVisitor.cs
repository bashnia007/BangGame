namespace Domain.PlayingCards.Visitors
{
    public interface ILongTermCardVisitor<out T>
    {
        T Visit(VolcanicCard volcanicCard);
        T Visit(SchofieldCard schofieldCard);
        T Visit(RemingtonCard remingtonCard);
        T Visit(CarabineCard carabineCard);
        T Visit(WinchesterCard winchester);
        
        T Visit(BarrelCard barrelCard);
        T Visit(ScopeCard scopeCard);
        T Visit(MustangCard mustangCard);
        
        T Visit(JailCard jailCard);
        T Visit(DynamiteCard dynamiteCard);
    }
}