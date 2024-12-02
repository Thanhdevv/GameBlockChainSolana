using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public TextMeshProUGUI bulletQuantity;

    public ItemWeapon itemWeapon;
    public Image iconWeapon;

    public GameObject weapon;
    public Transform firePos;
    public GameObject bulletPrefabs;

    [Header("Setting")]
    public float bulletForce;
    public float shotInterval;
    public float timeDestroyBullet;
    public int damage;
    public int maxQuantity;

    private int currentQuantity;
    private float currentShotInterval;

    private void Start()
    {
        if (itemWeapon != null)
        {
            bulletForce = itemWeapon.bulletForce;
            shotInterval = itemWeapon.shotInterval;
            damage = itemWeapon.damage;
            maxQuantity = itemWeapon.maxQuantity;
            weapon.GetComponent<SpriteRenderer>().sprite = itemWeapon.sprite;
        }
        currentQuantity = maxQuantity;
        currentShotInterval = shotInterval;
        bulletQuantity.text = currentQuantity + "/" + maxQuantity;
        FillIconWeapon();
    }

    private void FillIconWeapon()
    {
        iconWeapon.sprite = itemWeapon.sprite;
    }
    private void UpdateBulletQuantity()
    {
        currentQuantity--;
        bulletQuantity.text = currentQuantity + "/" + maxQuantity;
    }

    public void AddBullet(int quantity)
    {
        currentQuantity += quantity;
        if (currentQuantity > maxQuantity)
        {
            currentQuantity = maxQuantity;
        }
        bulletQuantity.text = currentQuantity + "/" + maxQuantity;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!weapon.activeSelf) return;
        currentShotInterval += Time.deltaTime;
        if (Input.GetMouseButton(0) && currentShotInterval > shotInterval)
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        if (currentQuantity <= 0) return;
        CreBullet();
        UpdateBulletQuantity();
    }

    private void CreBullet()
    {
        currentShotInterval = 0;
        AudioManager.Instance.PlaySFXFire();
        Quaternion newRotation = weapon.transform.rotation * Quaternion.Euler(0, 0, -90f);
        GameObject newBullet = Instantiate(bulletPrefabs, firePos.position, newRotation);
        newBullet.GetComponent<Bullet>().SetDamage(damage);
        newBullet.GetComponent<Rigidbody2D>().AddForce(weapon.transform.right * bulletForce, ForceMode2D.Impulse);
        StartCoroutine(DestroyBullet(newBullet));
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(timeDestroyBullet);
        Destroy(bullet);
    }
}
