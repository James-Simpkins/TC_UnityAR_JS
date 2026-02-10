using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems; // Required to detect taps/clicks on UI

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
        bool isPointerOverUI = false;

        // PC Simulation
        if(Application.isEditor)
        {
            if (!Input.GetMouseButtonDown(0)) return; // If we are not pressing down left click
            
            inputPosition = Input.mousePosition;
            
            // Check if the cursor is hovering over an UI element in the canvas.
            if (EventSystem.current.IsPointerOverGameObject())
            {
                isPointerOverUI = true;
                Debug.Log("isPointerOverUI = true");
            }
        }
        else
        {
            // Mobile touch
            if (Input.touchCount == 0) return;

            Touch touch = Input.GetTouch(0);

            if(touch.phase != TouchPhase.Began) return;

            inputPosition = touch.position;

            // Check if the specific finger is on an UI element
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                isPointerOverUI = true;
            }
        }

        // 1. BLOCK: If we touched a button, don't do anything else.
        if (isPointerOverUI)
            return;
        Debug.Log("Pointer is over the UI");

        // 2. Check if the user clicked on an existing spawned object to delete it
        if (ClickedOnSpawnedObject(inputPosition))
            return;

        // 3. Raycast onto the AR Plane to spawn
        if (m_RaycastManager.Raycast(inputPosition, m_Hits, TrackableType.PlaneWithinPolygon))
        {
            Debug.Log("Raycasting onto AR Plane to spawn object.");
            Pose hitPose = m_Hits[0].pose;
            Instantiate(m_ObjectToSpawn, hitPose.position, hitPose.rotation);
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
