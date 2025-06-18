using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con el componente Button

public class QuitGameButton : MonoBehaviour
{
    // Este m�todo se llamar� cuando el bot�n sea presionado.
    public void QuitApplication()
    {
        // Un mensaje en la consola para confirmar que la funci�n se est� llamando.
        // Esto es �til para depurar en el editor de Unity, ya que Application.Quit()
        // no cerrar� el editor.
        Debug.Log("Saliendo del juego...");

        // Esta l�nea es la que realmente cierra la aplicaci�n.
        // �Importante!: Esto SOLO funciona en una compilaci�n (build) del juego (ej. un .exe).
        // No har� nada cuando lo pruebes directamente en el editor de Unity.
        Application.Quit();
    }
}