using UnityEngine;
using System.Collections.Generic;

public class RandomDecorGenerator : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject decorPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxDecorObjects = 10;

    [Header("Puntos de Spawn")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private List<GameObject> activeDecorObjects = new List<GameObject>();
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && activeDecorObjects.Count < maxDecorObjects && spawnPoints.Count > 0)
        {
            SpawnRandomDecor();
            timer = 0f;
        }

        activeDecorObjects.RemoveAll(item => item == null);
    }

    void SpawnRandomDecor()
    {
        // Especificamos que queremos usar UnityEngine.Random
        Transform randomPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

        Vector3 spawnPosition = new Vector3(randomPoint.position.x, randomPoint.position.y, 0);
        GameObject newDecor = Instantiate(decorPrefab, spawnPosition, Quaternion.identity);

        Animator anim = newDecor.GetComponent<Animator>();
        if (anim != null)
        {
            anim.Play(anim.runtimeAnimatorController.animationClips[0].name);
        }

        activeDecorObjects.Add(newDecor);
    }

    public void AddSpawnPoint(Transform newPoint)
    {
        if (!spawnPoints.Contains(newPoint))
        {
            spawnPoints.Add(newPoint);
        }
    }

    public void RemoveSpawnPoint(Transform pointToRemove)
    {
        if (spawnPoints.Contains(pointToRemove))
        {
            spawnPoints.Remove(pointToRemove);
        }
    }

    public void ClearAllDecor()
    {
        foreach (GameObject decor in activeDecorObjects)
        {
            if (decor != null)
            {
                Destroy(decor);
            }
        }
        activeDecorObjects.Clear();
    }
}