using Assets._Project.Scripts.Utilities.Conditions;

namespace Assets._Project.Scripts.Utilities.Conditions
{
    public interface ICompositeCondition : ICondition
    {
        ICompositeCondition Add(ICondition condition);
    }
}