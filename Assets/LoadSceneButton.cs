using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para la gestión de escenas
using UnityEngine.UI; // Necesario para acceder al componente Button

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Nombre de la escena a cargar

    void Start()
    {

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(LoadTargetScene);
        }
    }

    public void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No se ha especificado una escena para cargar en " + gameObject.name);
        }
    }
}