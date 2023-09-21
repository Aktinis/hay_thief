public class QuestScreenMessage : BaseMessage
{
    public int Goal => goal;
    public int Progress => progress;
    private int goal = 0;
    private int progress = 0;


    public QuestScreenMessage() { }

    public QuestScreenMessage(int progress, int goal)
    {
        this.goal = goal;
        this.progress = progress;
    }
}
