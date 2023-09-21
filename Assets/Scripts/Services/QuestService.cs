public sealed class QuestService : IQuestService
{
    private int goal = 0;
    private int progress = 0;


    public int GetGoal()
    {
        return goal;
    }

    public int GetProgress()
    {
        return progress;
    }

    public void Initialize(int goal)
    {
        this.goal = goal;
        progress = 0;
    }

    public void Increment()
    {
        progress++;
    }
}
