using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Scrollbar hp;
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isInvincible = false;
    public float invincibilityDuration = 1f;

    private void Start()
    {
        currentHealth = maxHealth;
        hp.size = 1f;
        hp.value = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible && currentHealth > 0)
        {
            TakeDamage(20f);

            if (currentHealth > 0)
            {
                StartCoroutine(InvincibilityFrames());
            }
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f);

        float healthPercentage = currentHealth / maxHealth;
        hp.size = Mathf.Clamp(healthPercentage, 0f, 1f);
        hp.value = 1f;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float elapsed = 0f;
            while (elapsed < invincibilityDuration)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                elapsed += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            spriteRenderer.enabled = true;
        }
        else
        {
            yield return new WaitForSeconds(invincibilityDuration);
        }

        isInvincible = false;
    }

    void Die()
    {
        hp.size = 0f;
        hp.value = 1f;

        Debug.Log("Player Died!");
        gameObject.SetActive(false);
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        float healthPercentage = currentHealth / maxHealth;
        hp.size = healthPercentage;
        hp.value = 1f;
    }
}