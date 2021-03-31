using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;
    public bool destroyOnDeath;

    private NetworkStartPosition[] spawnPoints;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public RectTransform healthBar;

    private void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        } 
        else
        {
            transform.Find("PlayerCamera").gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isServer) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = maxHealth;
                RpcRespawn();
            }
        }
    }

    void OnChangeHealth(int health)
    {
        currentHealth = health;
        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (!isLocalPlayer) return;

        Vector3 spawnPoint = Vector3.zero;

        if (spawnPoints != null && spawnPoints.Length > 0 )
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }

        transform.position = spawnPoint;
    }

    private void OnGUI()
    {
        if (isLocalPlayer)
        {
            GUI.Label(new Rect(10, 10, 100, 20), "Vie: " + healthBar.sizeDelta.x + "/" + maxHealth);
        }
    }
}
