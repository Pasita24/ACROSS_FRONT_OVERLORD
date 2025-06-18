using System.Diagnostics;
using UnityEngine;

public class BulletZoneController : MonoBehaviour
{
    public static BulletZoneController Instance { get; private set; }
    [Header("Prefab del disparo")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Puntos de generación por zona")]
    [SerializeField] private Transform spawnPointZone1;
    [SerializeField] private Transform spawnPointZone2;

    [Header("Referencias de trinchera")]
    [SerializeField] private Transform trench1Left;
    [SerializeField] private Transform trench1Right;
    [SerializeField] private Transform trench2Left;
    [SerializeField] private Transform trench2Right;

    private GameObject currentBullet;
    private string currentZone = "";
    private bool shootingEnabled = true;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void DisableShooting()
    {
        shootingEnabled = false;
        if (currentBullet != null)
        {
            Destroy(currentBullet);
            currentBullet = null;
        }
    }

    public void EnableShooting()
    {
        shootingEnabled = true;
    }


    public void OnPlayerEnterZone(string zone)
    {
        // Verificar doble condición de disparo permitido
        if (!shootingEnabled || !BossController.Instance.IsShootingEnabled)
        {
            // Limpiar bala existente si las condiciones no se cumplen
            if (currentBullet != null)
            {
                Destroy(currentBullet);
                currentBullet = null;
            }
            return;
        }

        // Generar nueva bala solo si no existe una actual
        if (currentBullet == null)
        {
            Transform spawnPoint = zone == "Zone1" ? spawnPointZone1 : spawnPointZone2;
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0); // Forzar Z=0
            currentBullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
            SetTrenchReferences(currentBullet);
            currentZone = zone;
            UnityEngine.Debug.Log($"Disparo generado en {zone}");
        }
    }

    public void OnPlayerExitZone(string zone)
    {
        if ((zone == "Zone1" && currentZone == "Zone1") || (zone == "Zone2" && currentZone == "Zone2"))
        {
            if (currentBullet != null)
            {
                Destroy(currentBullet);
                currentBullet = null;
            }
            currentZone = "";
            UnityEngine.Debug.Log("Jugador salió de la zona, disparo destruido");
        }
    }

    private void SetTrenchReferences(GameObject bullet)
    {
        var movement = bullet.GetComponent<GroundEffectMovement>();
        if (movement != null)
        {
            movement.trench1Left = trench1Left;
            movement.trench1Right = trench1Right;
            movement.trench2Left = trench2Left;
            movement.trench2Right = trench2Right;
        }
    }
    public void RegenerateBulletInCurrentZone()
    {
        if (!string.IsNullOrEmpty(currentZone))
        {
            OnPlayerEnterZone(currentZone); // Reutilizamos el método existente
        }
    }
}
