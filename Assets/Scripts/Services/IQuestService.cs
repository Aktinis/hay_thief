public interface IQuestService
{
    void Initialize(int goal);

    void Increment();

    int GetProgress();

    int GetGoal();
}
