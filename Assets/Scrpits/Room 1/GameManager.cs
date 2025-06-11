using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton para f�cil acceso

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

    public void GameOver()
    {
        Debug.Log("Game Over! Jugador detectado.");
        // Aqu� puedes a�adir m�s l�gica de UI, pantalla de Game Over, etc.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena actual
    }

}