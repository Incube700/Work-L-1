namespace Assets._Project.Scripts.Gameplay.EntitiesCore.Conditions
{
    public sealed class AllConditions : ICondition
    {
        private readonly ICondition[] _conditions;

        public AllConditions(params ICondition[] conditions)
        {
            _conditions = conditions;
        }

        public bool IsSatisfied(Entity entity)
        {
            for (int i = 0; i < _conditions.Length; i++)
            {
                if (_conditions[i].IsSatisfied(entity) == false)
                    return false;
            }

            return true;
        }
    }
}