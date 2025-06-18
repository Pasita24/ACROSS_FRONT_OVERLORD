using UnityEngine;

public class BulletZoneController : MonoBehaviour
{
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

    public void OnPlayerEnterZone(string zone)
    {
        if (currentBullet != null)
        {
            Destroy(currentBullet); // Destruye el disparo anterior si existe
        }

        if (zone == "Zone1")
        {
            currentZone = "Zone1";
            currentBullet = Instantiate(bulletPrefab, new Vector3(spawnPointZone1.position.x, spawnPointZone1.position.y, 0), Quaternion.identity);
            SetTrenchReferences(currentBullet);
            UnityEngine.Debug.Log("Disparo generado en Zona 1");
        }
        else if (zone == "Zone2")
        {
            currentZone = "Zone2";
            currentBullet = Instantiate(bulletPrefab, new Vector3(spawnPointZone2.position.x, spawnPointZone2.position.y, 0), Quaternion.identity);
            SetTrenchReferences(currentBullet);
            UnityEngine.Debug.Log("Disparo generado en Zona 2");
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
}
