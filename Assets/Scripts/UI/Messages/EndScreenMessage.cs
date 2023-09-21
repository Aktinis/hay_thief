public class EndScreenMessage : BaseMessage
{
    private readonly bool hasWon = false;
    public bool HasWon => hasWon;   


    public EndScreenMessage(bool hasWon) 
    {
        this.hasWon = hasWon;
    }
}
