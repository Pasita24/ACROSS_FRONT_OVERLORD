using UnityEngine;

public class PlayerZoneDetector : MonoBehaviour
{
    private BulletZoneController bulletZoneController;

    private void Start()
    {
        bulletZoneController = FindObjectOfType<BulletZoneController>();
        if (bulletZoneController == null)
        {
            Debug.LogError("BulletZoneController no encontrado en la escena.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zone1") || other.CompareTag("Zone2"))
        {
            bulletZoneController.OnPlayerEnterZone(other.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Zone1") || other.CompareTag("Zone2"))
        {
            bulletZoneController.OnPlayerExitZone(other.tag);
        }
    }
}