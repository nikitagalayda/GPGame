using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button[] buttons;

    private LinkedList<Button> menuItems;
    private LinkedListNode<Button> selectedButton;
    private LinkedListNode<Button> lastButton;

    void Start()
    {
        menuItems = new LinkedList<Button>(buttons);
        selectedButton = menuItems.First;
    }

    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            lastButton = selectedButton;
            selectedButton = selectedButton.Previous ?? selectedButton.List.Last;
            selectedButton.Value.GetComponentInChildren<MainMenuTextController>().ChangeTextSize(1.4f, 0.2f);
            lastButton.Value.GetComponentInChildren<MainMenuTextController>().ChangeTextSize(1f, 0.2f);
        }

        if (Input.GetKeyDown("down"))
        {
            lastButton = selectedButton;
            selectedButton = selectedButton.Next ?? selectedButton.List.First;
            selectedButton.Value.GetComponentInChildren<MainMenuTextController>().ChangeTextSize(1.4f, 0.2f);
            lastButton.Value.GetComponentInChildren<MainMenuTextController>().ChangeTextSize(1f, 0.2f);
        }

        // Debug.Log(selectedButton.Value);
    }
}
