using UnityEngine;

public class AICharacterPool : GenericPool<AICharacter>
{
    private AICharacter prefab = null;


    public AICharacterPool(AICharacter prefab)
    {
        this.prefab = prefab;
    }

    protected override AICharacter Create()
    {
        return GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    protected override AICharacter OnGetFromPool(AICharacter obj)
    {
        obj.gameObject.SetActive(true);
        return obj;
    }

    protected override void OnReleaseToPool(AICharacter obj)
    {
        obj.gameObject.SetActive(false);
    }
}
