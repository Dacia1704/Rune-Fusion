public abstract class State
{
    protected MonsterBase monster;
    public State(MonsterBase monster)
    {
        this.monster = monster;
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}