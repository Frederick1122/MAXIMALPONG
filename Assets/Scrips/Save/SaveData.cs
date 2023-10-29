public abstract class SaveData<T> where T : new()
{
    protected T data = new();
    public abstract void PreLoad();
    public abstract void Save(T obj);

    public T Load()
    {
        return data;
    }
}