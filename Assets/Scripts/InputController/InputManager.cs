using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;




    /// <summary>
    /// 
    /// </summary>
    public static Action<bool, RaycastHit, Interactable> OnInteract;

    public Transform vCursor;
    public Vector2 vCursorOffset;

    public Vector2 moveInput, cursorPos, lastCursorPos;
    public Vector2 cursorMoveInput;
    public bool interactInput;
    public RectTransform canvasTransform;
    public CanvasScaler canvas;
    public float width, trueWidth, height, trueHeight;

    public LayerMask interactMask;

    public RaycastHit currentHit;
    public Collider currentCol, lastCol;
    public Interactable currentTargeted;

    public bool didHit;
    public Vector3 hitPos;

    public float mouseSens, gpSens;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if(trueWidth != canvas.referenceResolution.x || trueHeight != canvas.referenceResolution.y)
        {
            width = canvas.referenceResolution.x / 2;
            height = canvas.referenceResolution.y / 2;
            trueWidth = canvas.referenceResolution.x;
            trueHeight = canvas.referenceResolution.y;
        }

        if(cursorMoveInput != Vector2.zero)
        {
            cursorPos += gpSens * Time.deltaTime * cursorMoveInput;
            SetCursorPos();
        }
        if(lastCursorPos != cursorPos)
        {
            UpdateCursor();
        }
    }
    void UpdateCursor()
    {
        Ray r = GetCursorRay();
        Debug.DrawLine(r.origin, r.GetPoint(100), Color.red, 1);
        if (Physics.Raycast(r, out currentHit, 100, interactMask))
        {
            if (currentHit.collider != null)
            {
                hitPos = currentHit.point;
                //ensure our current collider is alway the one we're aiming at
                if (currentHit.collider != currentCol)
                    currentCol = currentHit.collider;

                //Check it against the last collider we targeted
                if(currentCol != lastCol)
                {
                    //If we get here, then we have aimed at a new collider and should get stuff from it again
                    if (currentCol.TryGetComponent(out Interactable i))
                    {
                        if(currentTargeted != null && currentTargeted != i)
                        {
                            currentTargeted.OnHoverEnd();
                        }
                        currentTargeted = i;
                        currentTargeted.OnHoverStart();
                    }
                    else
                    {
                        if(currentTargeted != null)
                        {
                            currentTargeted.OnHoverEnd();
                        }
                        currentTargeted = null;
                    }
                }
            }
        }
        else
        {
            currentCol = null;
            if(currentTargeted != null)
            {
                currentTargeted.OnHoverEnd();
            }
            currentTargeted = null;
        }
        lastCol = currentCol;
        didHit = currentHit.collider != null;
        lastCursorPos = cursorPos;
    }

    public void SetMoveInput(InputAction.CallbackContext context)
    {

    }
    Ray GetCursorRay()
    {
        return Camera.main.ScreenPointToRay(cursorPos + new Vector2(width, height));
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {

        if (context.canceled)
            return;

            OnInteract?.Invoke(false, currentHit, currentTargeted);
        if(currentTargeted != null)
            currentTargeted.OnInteractPrimary();
    }
    public void OnSecondaryAction(InputAction.CallbackContext context)
    {
        if (context.canceled)
            return;

        OnInteract?.Invoke(false, currentHit, currentTargeted);
        if (currentTargeted != null)
            currentTargeted.OnInteractSecondary();
    }
    public void OnCursorGamepad(InputAction.CallbackContext context)
    {
        cursorMoveInput += context.ReadValue<Vector2>() * gpSens;
    }
    public void OnCursorMouse(InputAction.CallbackContext context)
    {
        cursorPos += mouseSens * Time.deltaTime * context.ReadValue<Vector2>();
        SetCursorPos();
    }
    void SetCursorPos()
    {
        cursorPos.x = Mathf.Clamp(cursorPos.x, -width, width);
        cursorPos.y = Mathf.Clamp(cursorPos.y, -height, height);
        vCursor.localPosition = cursorPos;
    }
}
