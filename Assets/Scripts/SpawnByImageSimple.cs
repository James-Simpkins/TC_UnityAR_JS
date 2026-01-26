using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[Serializable]
public class ImagePrefabEntry
{
    public string imageName;        // Must match the name in the XR Referemce Image List
    public GameObject prefab;       // Prefab to spawn for that image
    [HideInInspector] public GameObject spawnedObject; // Runtime instance
}

public class SpawnByImageSimple : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;

    // List of image/prefab pairs (easy to add more)
    public List<ImagePrefabEntry> imagePrefabList = new List<ImagePrefabEntry>();

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnChanged;
    }

    private void Start()
    {
        // Pre-instantiate (avoids lag)
        foreach (var entry in imagePrefabList)
        {
            if (entry.prefab != null)
            {
                entry.spawnedObject = Instantiate(entry.prefab);
                entry.spawnedObject.SetActive(false);
            }
        }
    }
    private void OnChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
            UpdateObject(trackedImage);

        foreach (var trackedImage in args.updated)
            UpdateObject(trackedImage);

        foreach (var trackedImage in args.removed)
            DisableObject(trackedImage);
    }

    private void UpdateObject(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        // Loop through list instead of dictionary
        foreach (var entry in imagePrefabList)
        {
            if (entry.imageName == imageName)
            {
                if (entry.spawnedObject == null)
                    return;

                entry.spawnedObject.transform.position = trackedImage.transform.position;
                // entry.spawnedObject.transform.rotation = trackedImage.transform.rotation; (causes object to snap back)

                bool isTracked = trackedImage.trackingState == TrackingState.Tracking;
                entry.spawnedObject.SetActive(isTracked);
            }
        }
    }

    private void DisableObject(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        foreach (var entry in imagePrefabList)
        {
            if (entry.imageName == imageName)
            {
                if (entry.spawnedObject != null)
                    entry.spawnedObject.SetActive(false);
            }
        }
    }
}