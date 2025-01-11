using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;  // Import the XR toolkit

namespace NavKeypad
{
    public class KeypadInteraction : MonoBehaviour
    {
        private XRController controller; // Reference to the VR controller
        private Camera cam; // Main camera (for mouse/keyboard input)
        private LineRenderer lineRenderer; // For visualizing the ray if needed
        public LayerMask keypadLayer; // Set this to the layer your keypad buttons are on

        private void Awake()
        {
            cam = Camera.main; // Get the main camera for mouse input
            controller = GetComponent<XRController>(); // Get the XR controller
            lineRenderer = GetComponent<LineRenderer>();  // Line renderer for ray (optional)
        }

        private void Update()
        {
            // Check if the VR controller is present
            bool isVR = controller != null && controller.inputDevice.isValid;

            if (isVR)
            {
                // VR Input: Raycast from the VR controller
                HandleVRInput();
            }
            else
            {
                // Mouse/Keyboard Input: Raycast from the camera (mouse position)
                HandleMouseInput();
            }

            // Optionally, you can visualize the ray with a LineRenderer
            if (lineRenderer != null)
            {
                if (isVR)
                {
                    lineRenderer.SetPosition(0, controller.transform.position);
                    lineRenderer.SetPosition(1, controller.transform.position + controller.transform.forward * 10f);
                }
                else
                {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    lineRenderer.SetPosition(0, ray.origin);
                    lineRenderer.SetPosition(1, ray.origin + ray.direction * 10f);
                }
            }
        }

        private void HandleVRInput()
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed) && isPressed)
            {
                Ray ray = new Ray(controller.transform.position, controller.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10f, keypadLayer))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        keypadButton.PressButton();
                    }
                }
            }
        }

        private void HandleMouseInput()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f, keypadLayer))
            {
                if (Input.GetMouseButtonDown(0)) // Mouse click (left mouse button)
                {
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        keypadButton.PressButton();
                    }
                }
            }
        }
    }
}
