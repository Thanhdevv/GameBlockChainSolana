using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Health health;
    public GameObject weapon;
    public GameObject melee;
    public LayerMask layerGrounded;

    [Header("Setting")]
    public float moveSpeed;
    public float jumpForce;
    public int maxHealth;

    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private BoxCollider2D m_BoxCollider;

    private int currentHealth;
    private float currentSpeed;

    private bool isFacingRight;
    private bool isGrounded;

    private int currentWeapon;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_BoxCollider = GetComponent<BoxCollider2D>();
        isFacingRight = true;
        currentHealth = maxHealth;
        currentSpeed = moveSpeed;
        health.UpdateHealth(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != 1) { 
            melee.SetActive(true);
            weapon.SetActive(false);
            currentWeapon = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != 2)
        {
            melee.SetActive(false);
            weapon.SetActive(true);
            currentWeapon = 2;
        }

        if (GameManager.Instance.isPaused) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        float move = Input.GetAxisRaw("Horizontal");
        if (move > 0 && isFacingRight || move < 0 && !isFacingRight)
        {
            currentSpeed = moveSpeed;
        } else
        {
            currentSpeed = moveSpeed / 2;
        }

        m_Rigidbody.velocity = new Vector2(move * currentSpeed, m_Rigidbody.velocity.y);

        if (move != 0)
        {
            m_Animator.SetBool("isMoving",true);
        } else
        {
            m_Animator.SetBool("isMoving", false);
        }

        if ((isGrounded || IsGrounded()) && (Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Mouse ScrollWheel") != 0)) { 
            m_Rigidbody.velocity = Vector2.up * jumpForce;
        }
        if (transform.position.y < -4f)
        {
            Die();
        }
        RotateWeapon();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("ItemBullet"))
        {
            UpdateBullet(collision.gameObject.GetComponent<ItemBullet>());
            AudioManager.Instance.PlaySFXSelect();
        }
        if (collision.gameObject.CompareTag("ItemGem"))
        {
            Destroy(collision.gameObject);
            AudioManager.Instance.PlaySFXSelect();
        }
    }

    private void UpdateBullet(ItemBullet item)
    {
        transform.Find("Weapon").GetComponent<Weapon>().AddBullet(item.quantity);
        Destroy(item.gameObject);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(m_BoxCollider.bounds.center, m_BoxCollider.bounds.size, 0f, Vector2.down, 0.1f, layerGrounded);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            isGrounded = false;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        m_Animator.SetTrigger("hit");
        health.UpdateHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        GameManager.Instance.Die();
    }

    private void RotateWeapon()
    {
        float localScaleX = weapon.transform.localScale.x;
        if (localScaleX < 0)
        {
            localScaleX *= -1;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distance = mousePos - weapon.transform.position;
        distance.Normalize();
        float rotationZ = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;

        weapon.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        if (weapon.transform.eulerAngles.z > 90 && weapon.transform.eulerAngles.z < 270)
        {
            weapon.transform.localScale = new Vector3(-localScaleX, -localScaleX, 0);
        }
        else
        {
            weapon.transform.localScale = new Vector3(localScaleX, localScaleX, 0);
        }
        mousePos.z = 0f;
        Vector3 direction = mousePos - transform.position;
        if (direction.x < 0 && isFacingRight || direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
    }
}
