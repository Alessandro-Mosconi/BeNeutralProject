// Custom attribute for conditional hiding
using UnityEngine;

namespace Attributes
{
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public string conditionName;
        public bool showIfTrue;

        public ConditionalHideAttribute(string conditionName, bool showIfTrue = true)
        {
            this.conditionName = conditionName;
            this.showIfTrue = showIfTrue;
        }
    }
}