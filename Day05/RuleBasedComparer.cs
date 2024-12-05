namespace Day05 {
    internal class RuleBasedComparer : IComparer<int> {
        private readonly List<(int, int)> _rules;


        public RuleBasedComparer(List<(int, int)> rules) {
            _rules = rules; 
        }


        public int Compare(int x, int y) {
            int ruleIndex = FindRuleIndex(x, y);
            if (ruleIndex < 0) return 0;

            return x == _rules[ruleIndex].Item1 ? -1 : 1;
        }

        private int FindRuleIndex(int x, int y) {
            for (int i = 0; i < _rules.Count; i++) {
                var rule = _rules[i];
                if (rule.Item1 == x && rule.Item2 == y || rule.Item1 == y && rule.Item2 == x) return i;
            }
            return -1;
        }
    }
}
