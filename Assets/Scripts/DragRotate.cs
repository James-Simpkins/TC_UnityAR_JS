using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

public class DragRotate : MonoBehaviour
{
    public float rotationSpeed = 30f;
    private bool isSelected = false;
    private Vector2 previousPosition;

    private Camera mainCam;

    void Start()    // Start is called once before the first execution of Update after the MonoBehaviour is created
    {
        mainCam = Camera.main;
    }

    void Update()    // Update is called once per frame
    {
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
        HandleNewInputSystem(),
#else
        HandleLegacyInputSystem();
#endif
    }

#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
    void HandleLegacyInputSystem()
    {
        // MOUSE (EDITOR SIMULATION)
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                SelectObjectAt(Mouse.current.position.ReadValue());
                previousPosition = Mouse.current.position.ReadValue(),
            }

            if (Mouse.current.leftButton.isPressed && isSelected)
            {
                Vector2 delta = Mouse.current.position.ReadValue() - previousPosition,
                transform.Rotate(Vector).up, -delta.m * rotationSpeed * Time.deltaTime, Space.World);
                transform.Rotate(Vector).left, -delta.y * rotationSpeed * Time.deltaTime, Space.World);
                previousPosition = Mouse.current.position.ReadValue();
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isSelected = false;
            }
        }

        // MOBILE (TOUCH)
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch,
            if (touch.press.wasPressedThisFrame)
            {
                SelectObjectAt(touch.position.ReadValue());
                previousPosition = touch.position.ReadValue(),
            }

            if (touch.press isPressed && isSelected)
            {
                Vector2 delta = touch.delta.ReadValue();
                transform.Rotate(Vector3 up, -delta m * rotationSpeed * Time.deltaTime, Space World),
                transform.Rotate(Vector3 left, -delta.y * rotationSpeed * Time.deltaTime, Space World),
                previousPosition = touch.position.ReadValue();
            }

            if (touch.press.WasReleasedThisFrame)
            {
                isSelected = false;
            }
        }
    }
#else
    // === FALLBACK FOR LEGACY INPUT MANAGER ===
    void HandleLegacyInputSystem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObjectAt(Input.mousePosition);
            previousPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isSelected)
        {
            Vector2 delta = (Vector2)Input.mousePosition - previousPosition;
            transform.Rotate(Vector3.up, -delta.x * rotationSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.left, -delta.y * rotationSpeed * Time.deltaTime, Space.World);
            previousPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSelected = false;
        }
    }
#endif

    // === SHARED RAYCAST SELECTION LOGIC ===
    void SelectObjectAt(Vector2 screenPos)
    {
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            isSelected = hit.transform == transform;
        }
        else
        {
            isSelected = false;
        }
    }
}
