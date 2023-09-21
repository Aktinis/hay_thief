using UnityEngine;

public class DeathParticlePool : GenericPool<ParticleSystem>
{
    private ParticleSystem prefab = null;


    public DeathParticlePool(ParticleSystem prefab)
    {
        this.prefab = prefab;
    }

    protected override ParticleSystem Create()
    {
        return GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    protected override ParticleSystem OnGetFromPool(ParticleSystem obj)
    {
        obj.gameObject.SetActive(true);
        obj.Play();
        return obj;
    }

    protected override void OnReleaseToPool(ParticleSystem obj)
    {
        obj.Stop();
        obj.gameObject.SetActive(false);
    }
}