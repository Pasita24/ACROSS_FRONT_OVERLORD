using UnityEngine;

public class TriggerInfo : MonoBehaviour
{
    [SerializeField] private GameObject infoObject; // GameObject que se activará/desactivará
    [SerializeField] private Collider2D triggerCollider; // Collider2D del trigger (debe ser trigger)

    void Start()
    {
        // Asegurarse de que el objeto informativo esté desactivado al inicio
        if (infoObject != null)
        {
            infoObject.SetActive(false);
        }

        // Verificar que el collider sea un trigger
        if (triggerCollider != null && !triggerCollider.isTrigger)
        {
            UnityEngine.Debug.LogWarning("El Collider2D asignado no está marcado como trigger. Por favor, configura el Collider2D como trigger.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Activar el objeto informativo si el jugador entra en el trigger
        if (other.CompareTag("Player") && infoObject != null)
        {
            infoObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Desactivar el objeto informativo si el jugador sale del trigger
        if (other.CompareTag("Player") && infoObject != null)
        {
            infoObject.SetActive(false);
        }
    }
}