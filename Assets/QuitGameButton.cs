using UnityEngine;
using UnityEngine.UI; // Necesario para interactuar con el componente Button

public class QuitGameButton : MonoBehaviour
{
    // Este método se llamará cuando el botón sea presionado.
    public void QuitApplication()
    {
        // Un mensaje en la consola para confirmar que la función se está llamando.
        // Esto es útil para depurar en el editor de Unity, ya que Application.Quit()
        // no cerrará el editor.
        Debug.Log("Saliendo del juego...");

        // Esta línea es la que realmente cierra la aplicación.
        // ¡Importante!: Esto SOLO funciona en una compilación (build) del juego (ej. un .exe).
        // No hará nada cuando lo pruebes directamente en el editor de Unity.
        Application.Quit();
    }
}