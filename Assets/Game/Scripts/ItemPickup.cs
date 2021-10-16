using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            var player = collision.GetComponent<PlayerController>();
            item = new Item(item.ID,item.Name, item.Description, item.Type, item.IsConsumable, item.Stack, item.IconPath, item.Prefab, item.Effect);
            //item.Icon = Resources.Load<Sprite>(item.IconPath);
            player.AddItem(item);
            Destroy(gameObject);
        }
    }
}
