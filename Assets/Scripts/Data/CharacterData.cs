using UnityEngine;

[CreateAssetMenu]
public sealed class CharacterData : ScriptableObject
{
    [SerializeField] private int health = 0;


    public int Health => health;
}
