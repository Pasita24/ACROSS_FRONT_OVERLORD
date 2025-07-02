using UnityEngine;

// Cambiamos el nombre de la clase para evitar conflictos
public class CameraTriggerHandler : MonoBehaviour
{
    [Header("Camera References")]
    public Camera mainCamera;
    public Camera secondaryCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos usando CompareTag que sea el objeto correcto
        if (other.CompareTag("MainCamera"))
        {
            if (secondaryCamera != null)
            {
                secondaryCamera.gameObject.SetActive(false);
                UnityEngine.Debug.Log("Secondary camera disabled");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            if (secondaryCamera != null)
            {
                secondaryCamera.gameObject.SetActive(true);
                UnityEngine.Debug.Log("Secondary camera enabled");
            }
        }
    }
}