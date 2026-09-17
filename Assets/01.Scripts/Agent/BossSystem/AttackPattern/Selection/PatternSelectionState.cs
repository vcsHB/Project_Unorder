namespace Project_Unorder.AgentSystem.BossSystem.AttackPattern.Selection
{
    public class PatternSelectionState
    {
        public int LastIndex { get; private set; } = -1;
        public int SelectCount { get; private set; }

        public void Record(int index)
        {
            LastIndex = index;
            SelectCount++;
        }

        public void Reset()
        {
            LastIndex = -1;
            SelectCount = 0;
        }
    }
}
