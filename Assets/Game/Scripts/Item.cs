using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item
{
    public int ID;
    public string Name;
    public string Description;
    public string Type;
    public bool IsConsumable;
    public int Stack;
    public string IconPath;
    public GameObject Prefab;
    public string Effect;
    
    private Sprite icon;

    public Sprite Icon { get => icon; set => icon = value; }


    public Item(int id, string name = "", string desciption = "", string type = "", bool isconsumable = true, int stack = 1, string iconPath = "", GameObject prefab = null, string effect = "")
    {
        ID = id;
        Name = name;
        Description = desciption;
        Type = type;
        IsConsumable = isconsumable;
        Stack = stack;
        IconPath = iconPath;
        Prefab = prefab;
        Effect = effect;

        if (!string.IsNullOrEmpty(IconPath))
        {
            Icon = Resources.Load<Sprite>(IconPath);
        }
    }


    public virtual void Use(PlayerController player)
    {
        Debug.Log($"Used Item: {Name} on {player.name} with effect: {Effect}");
        //UseEffect(player, false);
        if (IsConsumable)
        {
            if(Stack > 1)
            {
                Stack--;
            }
            else
            {
                player.Inventory.Remove(this);
            }
            EventManager.InventoryChanged();
        }
    }

}
