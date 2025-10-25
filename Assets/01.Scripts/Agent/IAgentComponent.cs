namespace Project_Unorder.AgentSystem
{

    public interface IAgentComponent
    {
        public void Initialize(Agent owner);

        public void AfterInitialize();

        public void LateInitialize();

        public void Dispose();
    }
}