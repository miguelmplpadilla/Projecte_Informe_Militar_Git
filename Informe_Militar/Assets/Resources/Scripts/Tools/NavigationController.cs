using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour
{
    public int indexNavigationY = 0;
    public int indexNavigationX = 0;

    public NavigationButtons navigationButtons;

    public GameObject buttonSelected;

    public UIInput uiInput;

    public bool canNavigate = false;

    private void Awake()
    {
        uiInput = new UIInput();

        uiInput.Navigation.Vertical.performed += NavigationVertical;
        uiInput.Navigation.Vertical.canceled += NavigationVertical;

        uiInput.Navigation.Horizontal.performed += NavigationHorizontal;
        uiInput.Navigation.Horizontal.canceled += NavigationHorizontal;
    }

    private void OnEnable()
    {
        uiInput.Enable();
    }

    private void OnDisable()
    {
        uiInput.Disable();
    }

    private void Update()
    {
        if (uiInput.Navigation.Press.WasPressedThisFrame()) 
            PressButton();
    }

    private void NavigationHorizontal(InputAction.CallbackContext context)
    {
        if (navigationButtons == null || 
            navigationButtons.direction == NavigationButtons.DirectionType.Vertical) return;

        float changeIndex = context.ReadValue<float>();

        if (changeIndex == 0) return;

        indexNavigationX += changeIndex > 0 ? 1 : -1;

        if (indexNavigationX >= navigationButtons.verticalButtons[indexNavigationY].horizontalButtons.Count)
            indexNavigationX = 0;
        else if (indexNavigationX < 0)
            indexNavigationX = navigationButtons.verticalButtons[indexNavigationY].horizontalButtons.Count - 1;

        buttonSelected = navigationButtons.verticalButtons[indexNavigationY].horizontalButtons[indexNavigationX];

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonSelected);
    }

    private void NavigationVertical(InputAction.CallbackContext context)
    {
        if (navigationButtons == null ||
            navigationButtons.direction == NavigationButtons.DirectionType.Horizontal) return;

        float changeIndex = context.ReadValue<float>();

        if (changeIndex == 0) return;

        indexNavigationY += changeIndex > 0 ? 1 : -1;

        if (indexNavigationY >= navigationButtons.verticalButtons.Count)
            indexNavigationY = 0;
        else if (indexNavigationY < 0)
        {
            indexNavigationY = navigationButtons.verticalButtons.Count - 1;
            if (navigationButtons.verticalButtons[indexNavigationY].horizontalButtons.Count <= indexNavigationX)
                indexNavigationY -= 1;
        }

        if (navigationButtons.verticalButtons[indexNavigationY].horizontalButtons.Count <= indexNavigationX)
            indexNavigationY = 0;

        buttonSelected = navigationButtons.verticalButtons[indexNavigationY].horizontalButtons[indexNavigationX];

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonSelected);
    }

    private void PressButton()
    {
        if (navigationButtons == null) return;

        navigationButtons.verticalButtons[indexNavigationY].horizontalButtons[indexNavigationX].GetComponent<Button>().onClick.Invoke();
    }

    public void SetNavigationButtons(NavigationButtons buttons)
    {
        if (buttons == null || Input.GetJoystickNames().Length == 0)
        {
            CloseNavigationButtons();
            return;
        }

        indexNavigationY = 0;
        indexNavigationX = 0;

        navigationButtons = buttons;

        buttonSelected = navigationButtons.verticalButtons[0].horizontalButtons[0];

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(buttonSelected);

        canNavigate = true;
    }

    private void CloseNavigationButtons()
    {
        navigationButtons = null;
        canNavigate = false;
    }
}

[Serializable]
public class NavigationButtons
{
    public DirectionType direction;
    public List<Buttons> verticalButtons = new List<Buttons>();

    [Serializable]
    public enum DirectionType
    {
        Vertical, Horizontal, All
    }
}

[Serializable]
public class Buttons
{
    public List<GameObject> horizontalButtons = new List<GameObject>();
}
