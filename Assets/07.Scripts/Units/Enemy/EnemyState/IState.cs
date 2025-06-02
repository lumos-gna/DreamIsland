public interface IState<T>
{
    void Enter(T obj);
    void Update(T obj);
    void Exit(T obj);
}
