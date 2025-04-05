using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static automator_baselib.Input;

namespace automator_baselib
{
    public class ActionItem
    {
        private string actionName;
        private string description;

        private Func<string, bool> isMatchPatternFunc;
        private Func<string, string, IEnumerable<string>> actionFunc;

        private string structure;

        public bool isMatchPattern(string pattern)
        {
            return isMatchPattern(pattern);
        }

        public IEnumerable<string> Action(string input, string cmd)
        {
            return actionFunc(input, cmd);
        }

        public string getStructure()
        {
            return this.structure;
        }

        public ActionItem(
            string actionName, string description,
            Func<string, bool> isMatchPatternFunc,
            Func<string, string, IEnumerable<string>> actionFunc,
            string structure)
        {

            this.actionName = actionName;
            this.description = description;

            this.isMatchPatternFunc = isMatchPatternFunc;
            this.actionFunc = actionFunc;

            this.structure = structure;
        }
    }
}
