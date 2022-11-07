public abstract class CardCreature : Card{
    public int BaseHealth = 4;
    public int BaseDefense = 0;
    public int BaseAttack = 2;

    public override CardType Type{
        get{ return CardType.Creature; }
    }
}