using UnityEngine;

[CreateAssetMenu(fileName = "New ItemWeapon", menuName = "ItemWeapon/Create New Item Weapon")]
public class ItemWeapon : ScriptableObject
{
    public int id;
    public Sprite sprite;
    public float bulletForce;
    public float shotInterval;
    public int damage;
    public int maxQuantity;
}
