using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBullet : MonoBehaviour
{
    public Item bullet;
    public int quantity;
    public float timeDestroy;

    private void Start()
    {
        RandomQuantity();
        Destroy(gameObject, timeDestroy);
    }

    private void RandomQuantity()
    {
        quantity = Random.Range(0, bullet.quantity);
    }
}
