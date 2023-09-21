using System.Collections.Generic;

public abstract class GenericPool<T>
{
    private List<T> pooledObjects = new List<T>();
    private List<bool> pooledObjectStatus = new List<bool>();


    public GenericPool() { }

    public void Setup(int size)
    {
        for (int i = 0; i < size; i++)
        {
            var createdObj = Create();
            pooledObjects.Add(createdObj);
            pooledObjectStatus.Add(false);
            OnReleaseToPool(createdObj);
        }
    }

    public T GetFromPool()
    {
        int inactiveElementIndex = pooledObjectStatus.FindIndex(x => !x);
        if (inactiveElementIndex < 0)
        {
            var createdObj = Create();
            pooledObjects.Add(createdObj);
            pooledObjectStatus.Add(true);
            return createdObj;
        }
        else
        {
            pooledObjectStatus[inactiveElementIndex] = true;
            return OnGetFromPool(pooledObjects[inactiveElementIndex]);
        }
    }

    public void ReleaseToPool(T obj)
    {
        var index = pooledObjects.FindIndex(x => x.Equals(obj));
        pooledObjectStatus[index] = false;
        OnReleaseToPool(obj);
    }

    protected abstract T Create();

    protected abstract void OnReleaseToPool(T obj);

    protected abstract T OnGetFromPool(T obj);
}
