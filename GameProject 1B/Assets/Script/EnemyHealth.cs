using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int maxHealth = 30;

    [SerializeField] private int currentHealth;

    [SerializeField] private GameObject dropItemPrefab;
    private Vector3 dropOffset = new Vector3(0f, 0.5f, 0f);
    private bool isDead;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log(name + " 남은 체력 : " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()              //사망시 드롭 아이템을 생성 한다.
    {
        isDead = true;

        if (dropItemPrefab != null)
        {
            Instantiate(dropItemPrefab, transform.position + dropOffset, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
