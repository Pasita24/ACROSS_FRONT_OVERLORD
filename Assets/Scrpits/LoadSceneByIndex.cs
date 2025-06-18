using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // ¡Este using es necesario para IEnumerator!

public class LoadSceneByIndex : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int sceneIndex = 0;
    [SerializeField] private float loadDelay = 0f;

    [Header("Referencias")]
    [SerializeField] private Button targetButton;

    private void Start()
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }

        if (targetButton != null)
        {
            targetButton.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogWarning("No se encontró componente Button en " + gameObject.name);
        }
    }

    public void LoadScene()
    {
        if (loadDelay > 0)
        {
            StartCoroutine(LoadSceneWithDelay());
        }
        else
        {
            LoadSceneImmediately();
        }
    }

    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(loadDelay);
        LoadSceneImmediately();
    }

    private void LoadSceneImmediately()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        if (sceneIndex >= 0 && sceneIndex < sceneCount)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Índice de escena inválido: {sceneIndex}. Hay {sceneCount} escenas en Build Settings.");
        }
    }

    public void SetSceneIndex(int newIndex)
    {
        sceneIndex = newIndex;
    }
}