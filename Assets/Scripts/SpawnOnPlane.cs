using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnOnPlane : MonoBehaviour
{
    public ARRaycastManager m_RaycastManager;
    public GameObject m_ObjectToSpawn;

    public LayerMask M_SpawnedObjectLayer; // Required for deletion


    private List<ARRaycastHit> m_Hits = new List<ARRaycastHit> ();

    



    // Update is called once per frame
    void Update()
    {
        Vector2 inputPosition;

        // PC Simulation
        if(Application.isEditor)
        {
            if (!Input.GetMouseButtonDown(0)) return; // If we are not pressing down left click
            
            // If left click is being pressed down
            // then store the position of our cursor
            inputPosition = Input.mousePosition;
            
        }
        else
        {
            // Mobile touch

            if (Input.touchCount == 0) return;

            Touch touch = Input.GetTouch(0);

            if(touch.phase != TouchPhase.Began) return;

            inputPosition = touch.position;
        }

        // First: Check if the user clicked on a spawned object
        if (ClickedOnSpawnedObject(inputPosition))
            return;

        // Second: Raycase onto the AR PLane to spawn the object
        if (m_RaycastManager.Raycast(inputPosition, m_Hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = m_Hits[0].pose;

            GameObject spawned = Instantiate(m_ObjectToSpawn,hitPose.position,hitPose.rotation);

        }

    }

    private bool ClickedOnSpawnedObject(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);

        RaycastHit hit;

        // Only detect colliders on the "SpawnedObjectLayer"
        if(Physics.Raycast(ray,out hit, Mathf.Infinity, M_SpawnedObjectLayer))
        {
            Destroy(hit.collider.gameObject);
            return true;
        }
        
        return false;
    }

}
