namespace Domain.PlayingCards.Visitors
{
    public interface ILongTermCardTypeVisitor<out T>
    {
        T Visit(VolcanicCardType volcanicCardType);
        T Visit(SchofieldCardType schofieldCardType);
        T Visit(RemingtonCardType remingtonCardType);
        T Visit(CarabineCardType carabineCardType);
        T Visit(WinchesterCardType winchester);
        
        T Visit(BarrelCardType barrelCardType);
        T Visit(ScopeCardType scopeCardType);
        T Visit(MustangCardType mustangCardType);
        
        T Visit(JailCardType jailCardType);
        T Visit(DynamiteCardType dynamiteCardType);
    }
}