using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUiHandler : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject slotPrefab;
    public GameObject inventoryPanel;
    public GameObject inventorySlots;

    private void OnEnable()
    {
        EventManager.onInventoryChanged += UpdateUI;
    }

    private void OnDisable()
    {
        EventManager.onInventoryChanged -= UpdateUI;
    }

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventorySlots.transform.childCount; i++)
        {
            Destroy(inventorySlots.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < playerController.Inventory.Count; i++)
        {
            var invSlot = Instantiate(slotPrefab, inventorySlots.transform);
            var slotUI = invSlot.GetComponent<InventorySlotUi>();

            slotUI.Icon = playerController.Inventory[i].Icon;
            slotUI.Name = playerController.Inventory[i].Name;
            slotUI.Amount = playerController.Inventory[i].Stack;
            slotUI.UpdateUI();

            Debug.Log($"Name of Item: {playerController.Inventory[i].Name} Amount: {playerController.Inventory[i].Stack}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
}
