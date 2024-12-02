using System.Collections;

using UnityEngine;


public class EnemyController : MonoBehaviour
{
    public Canvas canvasHealth;
    public Transform body;
    public Health health;
    [Header("List Item")]
    public GameObject itemBulletPrefabs;
    public GameObject itemGemPrefabs;

    [Header("Setting")]
    public float moveSpeed;
    public float distaceTarget;
    public int damage;
    public int maxHealth;
    public int experience;
    public int minScore;
    public int maxScore;
    public float attackInterval;

    [Header("Drop Rate")]
    public float dropRateBullet;
    public float dropRateGem;


    public Transform player;

    private float currentAttackInterval;
    private int currentHealth;
    private bool isFacingRight;
    private bool isDie;

    private Animator m_Animator;

    private Vector3 defPos;
    private Vector2 direction;

    private void Start()
    {
        isDie = false;
        m_Animator = GetComponent<Animator>();
        isFacingRight = true;
        defPos = transform.position;
        currentHealth = maxHealth;
        health.UpdateHealth(currentHealth, maxHealth);
        currentAttackInterval = attackInterval;
    }

    private void Update()
    {
        if (isDie) return;
        currentAttackInterval += Time.deltaTime;
        player = GameObject.Find("Player") != null ? GameObject.Find("Player").transform:null;
        if (player == null)
        {
            ReturnDefPos(); 
            return;
        }
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < distaceTarget) {
            direction = player.transform.position - transform.position;
            m_Animator.SetBool("isMoving", true);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        } else
        {
            ReturnDefPos();
        }


        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        } else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void ReturnDefPos()
    {
        m_Animator.SetBool("isMoving", true);
        direction = defPos - transform.position;
        if (Vector2.Distance(transform.position, defPos) <= 0.2)
        {
            m_Animator.SetBool("isMoving", false);
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, defPos, moveSpeed * Time.deltaTime);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Quaternion quaternion;
        quaternion = isFacingRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
        body.rotation = quaternion;
    }

    private void Attack()
    {
        if (player == null) return;
        if (currentAttackInterval > attackInterval)
        {
            currentAttackInterval = 0;
            player.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
    public void TakeDamage(int damage)
    {
        if (isDie) return;
        currentHealth -= damage;
        m_Animator.SetTrigger("hit");
        AudioManager.Instance.PlaySFXHit();
        health.UpdateHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDie = true;
        AudioManager.Instance.PlaySFXDead();
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<CapsuleCollider2D>());
        canvasHealth.transform.gameObject.SetActive(false);
        m_Animator.SetTrigger("die");
        ExpManager.Instance.UpdateExp(experience);
        int randomScore = Random.Range(minScore, maxScore);
        ScoreManager.Instance.UpdateScore(randomScore);
        ScoreManager.Instance.UpdateKill();
        DropItem();
        StartCoroutine(DestroyEnemy());
    }

    private void DropItem()
    {
        DropItemPrefabs(itemBulletPrefabs, dropRateBullet, "ItemBullet");
        DropItemPrefabs(itemGemPrefabs, dropRateGem, "ItemGem");
    }

    private void DropItemPrefabs(GameObject item,float dropRate,string name)
    {
        if (item == null) return;
        int randomValue = Random.Range(0, 101);
        if (randomValue < dropRate)
        {
            if (name == "ItemGem")
            {
                for (int i = 0; i < Random.Range(2,5); i++)
                {
                    CreateItem(item, name);
                }
            } else
            {
                CreateItem(item, name);
            }
        }
    }

    private void CreateItem(GameObject item,string name)
    {
        GameObject newItem = Instantiate(item, transform.position, Quaternion.identity);
        newItem.name = name;
        newItem.GetComponent<Rigidbody2D>().velocity = Vector2.up * 3.5f;
        newItem.GetComponent<Rigidbody2D>().velocity = GetRandomBool() ? Vector2.left * Random.Range(2.0f, 4.0f) : Vector2.right * Random.Range(2.0f, 4.0f);
    }

    public bool GetRandomBool()
    {
        int randomNumber = Random.Range(0, 2);
        return randomNumber == 0 ? true : false;
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(3f);
        m_Animator.SetTrigger("exit");
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDie)
        {
            Attack();
        }
    }
}
