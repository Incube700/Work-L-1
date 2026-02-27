using System;
using System.Collections.Generic;
using Assets._Project.Scripts.Utilities.Conditions;

namespace Assets._Project.Scripts.Utilities.Conditions
{
    public sealed class CompositeCondition : ICompositeCondition
    {
        private readonly List<ICondition> _conditions;
        private readonly LogicOperations _operation;

        public CompositeCondition(LogicOperations operation = LogicOperations.And)
        {
            _operation = operation;
            _conditions = new List<ICondition>();
        }

        public ICompositeCondition Add(ICondition condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            _conditions.Add(condition);
            return this;
        }

        public bool Evaluate()
        {
            if (_conditions.Count == 0)
                return true;

            if (_operation == LogicOperations.And)
                return EvaluateAnd();

            return EvaluateOr();
        }

        private bool EvaluateAnd()
        {
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (_conditions[i].Evaluate() == false)
                    return false;
            }

            return true;
        }

        private bool EvaluateOr()
        {
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (_conditions[i].Evaluate())
                    return true;
            }

            return false;
        }
    }
}