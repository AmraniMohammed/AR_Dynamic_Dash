using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PrefabSpawnManager : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject prefab;
    private bool isSpawned = false;
    private string trackedImgName;
    GameObject spawnedGameobject;


    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.01f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(var trakedImage in args.added)
        {
            trackedImgName = trakedImage.name;

            if (trakedImage.transform && !isSpawned)
            {
                Vector3 pos = trakedImage.transform.position;

                spawnedGameobject = Instantiate(prefab, trakedImage.transform);

                StartCoroutine(AppearAnimation(spawnedGameobject));

                spawnedGameobject.transform.position = new Vector3(pos.x, pos.y + 0.02f, pos.z);

                // Store the starting position & rotation of the object
                posOffset = spawnedGameobject.transform.position;

                //StartCoroutine(FloatingCoroutine(spawnedGameobject));

                isSpawned = true;
            }
        }
    }

    IEnumerator AppearAnimation(GameObject obj, float duration = 0.5f)
    {
        float elapsedTime = 0f;
        Vector3 initialScale = Vector3.zero;
        Vector3 targetScale = obj.transform.localScale;

        while (elapsedTime < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.transform.localScale = targetScale;
    }

    IEnumerator FloatingCoroutine(GameObject obj)
    {
        while (true)
        {
            // Spin object around Y-Axis
            //obj.transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            tempPos = posOffset;
            tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            obj.transform.position = tempPos;

            yield return null; // Wait for the next frame
        }
    }
}
