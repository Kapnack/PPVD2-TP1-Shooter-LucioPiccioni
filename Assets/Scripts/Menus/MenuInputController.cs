using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuInputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string uiActionMapName = "UI";

    [SerializeField] private Button[] buttons;
    private int currentIndex = 0;

    private InputAction selectUp;
    private InputAction selectDown;
    private InputAction confirm;

    private Vector2 lastMousePosition;
    private bool usingMouse = false;
    private float mouseThreshold = 0.5f;

    private void OnEnable()
    {
        var actionMap = inputActions.FindActionMap(uiActionMapName);

        selectUp = actionMap.FindAction("SelectUp");
        selectDown = actionMap.FindAction("SelectDown");
        confirm = actionMap.FindAction("Confirm");

        selectUp.started += OnSelectUp;
        selectDown.started += OnSelectDown;
        confirm.started += OnConfirm;

        actionMap.Enable();

        HighlightCurrent();

        lastMousePosition = Mouse.current.position.ReadValue();
    }

    private void OnDisable()
    {
        var actionMap = inputActions.FindActionMap(uiActionMapName);
        actionMap.Disable();

        selectUp.started -= OnSelectUp;
        selectDown.started -= OnSelectDown;
        confirm.started -= OnConfirm;
    }

    private void Update()
    {
        Vector2 currentMousePos = Mouse.current.position.ReadValue();
        float mouseDelta = (currentMousePos - lastMousePosition).magnitude;

        if (mouseDelta > mouseThreshold)
        {
            usingMouse = true;
        }

        lastMousePosition = currentMousePos;
    }

    private void OnSelectUp(InputAction.CallbackContext ctx)
    {
        usingMouse = false;
        MoveSelection(-1);
    }

    private void OnSelectDown(InputAction.CallbackContext ctx)
    {
        usingMouse = false; 
        MoveSelection(1);
    }

    private void OnConfirm(InputAction.CallbackContext ctx)
    {
        if (!usingMouse)
            PressSelected();
    }

    private void MoveSelection(int direction)
    {
        currentIndex += direction;
        currentIndex = Mathf.Clamp(currentIndex, 0, buttons.Length - 1);
        HighlightCurrent();
    }

    private void HighlightCurrent()
    {
        if (buttons.Length == 0) return;

        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
    }

    private void PressSelected()
    {
        if (buttons.Length == 0) return;

        buttons[currentIndex].onClick.Invoke();
    }
}
