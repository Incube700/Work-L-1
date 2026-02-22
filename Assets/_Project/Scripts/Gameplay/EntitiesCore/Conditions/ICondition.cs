namespace Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions
{
    public interface ICondition
    {
        bool IsSatisfied(Entity entity);
    }
}