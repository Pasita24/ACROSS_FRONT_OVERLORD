using UnityEngine;

public class CameraTriggerHandler : MonoBehaviour
{
    [Header("Camera References")]
    public Camera mainCamera;
    public Camera secondaryCamera;

    [Header("Settings")]
    public float detectionRadius = 5f;

    private void Update()
    {
        if (mainCamera == null || secondaryCamera == null)
            return;

        // Si la secondaryCamera está activa, evaluamos si debe desactivarse
        if (secondaryCamera.gameObject.activeSelf)
        {
            if (IsMainCameraClose())
            {
                secondaryCamera.gameObject.SetActive(false);
                UnityEngine.Debug.Log("Secondary camera disabled (main camera is close)");
            }
        }
        else // Si está desactivada, evaluamos si debe volver a activarse
        {
            if (!IsMainCameraClose())
            {
                secondaryCamera.gameObject.SetActive(true);
                UnityEngine.Debug.Log("Secondary camera enabled (main camera is far)");
            }
        }
    }

    private bool IsMainCameraClose()
    {
        // Calcula distancia solo en el plano X-Y (ignora eje Z)
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
            new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y)
        );

        return distance <= detectionRadius;
    }

    // Opcional: para ver el área en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
