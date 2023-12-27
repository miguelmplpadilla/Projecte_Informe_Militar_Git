using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NavigationController : MonoBehaviour
{
    public int indexNavigation = 0;

    public NavigationButtons navigationButtons;

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
            navigationButtons.direction != NavigationButtons.DirectionType.Horizontal) return;

        Navigate((int)context.ReadValue<float>());
    }

    private void NavigationVertical(InputAction.CallbackContext context)
    {
        if (navigationButtons == null ||
            navigationButtons.direction != NavigationButtons.DirectionType.Vertical) return;

        Navigate(context.ReadValue<float>());
    }

    private void Navigate(float changeIndex)
    {
        if (changeIndex == 0) return;

        indexNavigation += changeIndex > 0 ? 1 : -1;

        if (indexNavigation >= navigationButtons.buttons.Count)
            indexNavigation = 0;
        else if (indexNavigation < 0)
            indexNavigation = navigationButtons.buttons.Count - 1;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(navigationButtons.buttons[indexNavigation]);
    }

    private void PressButton()
    {
        if (navigationButtons == null) return;

        navigationButtons.buttons[indexNavigation].GetComponent<Button>().onClick.Invoke();
    }

    public void SetNavigationButtons(NavigationButtons buttons)
    {
        if (buttons == null)
        {
            CloseNavigationButtons();
            return;
        }

        indexNavigation = 0;

        navigationButtons = buttons;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(navigationButtons.buttons[0]);

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
    public List<GameObject> buttons = new List<GameObject>();

    [Serializable]
    public enum DirectionType
    {
        Vertical, Horizontal
    }
}
