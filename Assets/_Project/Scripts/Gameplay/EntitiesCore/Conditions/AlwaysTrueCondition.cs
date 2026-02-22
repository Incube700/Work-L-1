namespace Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions
{
    public sealed class AlwaysTrueCondition : ICondition
    {
        public bool IsSatisfied(Entity entity) => true;
    }
}