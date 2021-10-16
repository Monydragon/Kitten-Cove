using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUi : MonoBehaviour
{
    public PlayerController playerController;

    public Sprite Icon;
    public string Name;
    public int Amount;

    public Image UiIcon;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI AmountText;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void UseItem()
    {
        var item = playerController.Inventory.Find(x => x.Name == Name);
        if (item != null)
        {
            Debug.Log($"{item.Name} is Used");
            item.Use(playerController);
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        UiIcon.sprite = Icon;
        NameText.text = Name;
        AmountText.text = Amount.ToString();
    }
}
