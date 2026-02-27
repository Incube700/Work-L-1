using System;

namespace Assets._Project.Scripts.Utilities.Conditions
{
    public sealed class FuncCondition : ICondition
    {
        private readonly Func<bool> _func;

        public FuncCondition(Func<bool> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public bool Evaluate()
        {
            return _func.Invoke();
        }
    }
}