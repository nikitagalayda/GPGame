using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button[] buttons;

    private LinkedList<Button> menuItems;
    private LinkedListNode<Button> selectedButton;

    void Start()
    {
        menuItems = new LinkedList<Button>(buttons);
        selectedButton = menuItems.First;
        selectedButton.Value.GetComponentInChildren<MainMenuTextController>().EnlargeText(2);
    }

    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            selectedButton = selectedButton.Previous ?? selectedButton.List.First;
        }

        if (Input.GetKeyDown("down"))
        {
            selectedButton = selectedButton.Next ?? selectedButton.List.First;
        }

        Debug.Log(selectedButton.Value);
    }
}
