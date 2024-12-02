using UnityEngine;
using UnityEngine.Events;

public class Melee : MonoBehaviour
{
    public Animator m_Animator;
    public float attackInterval;
    public int damage;

    private float currentAttackIntervel;
    public UnityEvent OnAttackPeformed;
    private bool isAttack;
    public void TriggerAttack()
    {
        Attack();
    }
    private void Attack()
    {
        if (isAttack && enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
    EnemyController enemy;
    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        isAttack = false;
        currentAttackIntervel = attackInterval;
    }
    private void Update()
    {
        currentAttackIntervel += Time.deltaTime;
        if (Input.GetMouseButton(0) && currentAttackIntervel > attackInterval)
        {
            m_Animator.SetTrigger("attack");
            currentAttackIntervel = 0;
        }

        float localScaleX = transform.localScale.x;
        if (localScaleX < 0)
        {
            localScaleX *= -1;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 distance = mousePos - transform.position;
        distance.Normalize();
        float rotationZ = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270)
        {
            transform.localScale = new Vector3(-localScaleX, -localScaleX, 0);
        }
        else
        {
            transform.localScale = new Vector3(localScaleX, localScaleX, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            isAttack = true;
            enemy = collision.gameObject.GetComponent<EnemyController>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            isAttack = false;
            enemy = null;
        }
    }
}
