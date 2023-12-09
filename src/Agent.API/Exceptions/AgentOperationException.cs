namespace Agent.API.Exceptions {
    public class AgentOperationException(string message) : InvalidOperationException(message) {
    }
}
