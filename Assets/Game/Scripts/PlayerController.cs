using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System.IO;
using Newtonsoft.Json;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private bool isInteracting;

    [SerializeField]
    private List<Item> inventory;

    public List<Item> Inventory { get => inventory; set => inventory = value; }

    private GameObject targetGameobject;
    private Vector3 movement;

    private void OnEnable()
    {
        EventManager.onInteract += EventManager_onInteract;
    }

    private void OnDisable()
    {
        EventManager.onInteract -= EventManager_onInteract;

    }

    private void EventManager_onInteract(GameObject source, GameObject target)
    {
        Debug.Log($"{source.name} Interacted with {target.name}");
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //inventory.Add(new Item { ID = 1, Name = "Sled", Description = "A Worn Sled", Stack = 1, Type = "Normal", Effect = "" });
        //inventory.Add(new Item { ID = 2, Name = "Yarn", Description = "A Ball of Yarn", Stack = 1, Type = "Quest", Effect = "" });
        //inventory.Add(new Item { ID = 3, Name = "Cat Food", Description = "Food for Cats", Stack = 1, Type = "Quest", Effect = "" });
        //Flowchart fc = FindObjectOfType<Flowchart>();
        //string items = "";
        //for (int i = 0; i < inventory.Count; i++)
        //{
        //    items += inventory[i].Name;
        //    if(i >= 0 && i < inventory.Count - 1)
        //    {
        //        items += ",";
        //    }
        //}
        //var qi = fc.GetStringVariable("QuestItem");
        //if (items.Contains(qi))
        //{
        //    Debug.Log("QUEST Item Found!!");
        //    fc.SetBooleanVariable("QuestItemFound", true);
        //}
    }

    // Update is called once per frame
    void Update()
    {

        var sayDialog = SayDialog.GetSayDialog();
        var menuDialog = MenuDialog.GetMenuDialog();
        if (sayDialog.isActiveAndEnabled)
        {
            isInteracting = true;
        }
        else if (menuDialog.isActiveAndEnabled)
        {
            isInteracting = true;
        }
        else
        {
            isInteracting = false;
        }

        if (!isInteracting)
        {
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            transform.position += movement * speed * Time.deltaTime;

            if (anim != null)
            {
                if (movement != Vector3.zero)
                {
                    anim.SetBool("isMoving", true);
                    anim.SetFloat("InputX", movement.x);
                    anim.SetFloat("InputY", movement.y);
                }
                else
                {
                    anim.SetBool("isMoving", false);
                }
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (targetGameobject != null)
                {
                    EventManager.Interact(gameObject, targetGameobject);
                    targetGameobject = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                var item = new Item(1, "Yarn", "A Ball of Yarn", "Quest", true, 1, "Sprites/Yarn", null, "");
                //var item = new Item { ID = 1, Name = "Yarn", IconPath = "Sprites/Yarn", Description = "A Ball of Yarn", Stack = 1, Type = "Quest", Effect = "" };
                AddItem(item);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                var item = new Item(2, "Cat Food", "Food for Cats", "Normal", true, 1, "Sprites/CatFood", null, "");
                //var item = new Item { ID = 2, Name = "Cat Food", IconPath = "Sprites/CatFood", Description = "Food for Cats", Stack = 1, Type = "Normal", Effect = "" };
                AddItem(item);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                SaveData();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadData();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        targetGameobject = col.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targetGameobject == collision.gameObject)
        {
            targetGameobject = null;
        }
    }

    public void AddItem(Item item)
    {
        var existingItem = inventory.Find(x => x.ID == item.ID);
        if(existingItem != null)
        {
            existingItem.Stack++;
        }
        else
        {
            inventory.Add(item);
        }
        EventManager.InventoryChanged();
    }

    public void RemoveItem(Item item)
    {
        if (inventory.Contains(item))
        {
            if(item.Stack > 1)
            {
                item.Stack--;
            }
            else
            {
                inventory.Remove(item);
            }
        }
        EventManager.InventoryChanged();
    }

    public void RemoveItem(string itemName)
    {
        var foundItem = inventory.Find(x => x.Name == itemName);
        if (inventory.Contains(foundItem))
        {
            if (foundItem.Stack > 1)
            {
                foundItem.Stack--;
            }
            else
            {
                inventory.Remove(foundItem);
            }
        }
        EventManager.InventoryChanged();
    }

    public bool HasItem(string itemName)
    {
        var item = inventory.Find(x=> x.Name == itemName);
        return item != null;
    }

    public void SaveData()
    {
        var items = JsonConvert.SerializeObject(inventory);
        var pos = JsonConvert.SerializeObject(transform.position);

        if(!Directory.Exists(Application.persistentDataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        }

        File.WriteAllText(Application.persistentDataPath + "/Saves/PlayerItems.sav", items);
        File.WriteAllText(Application.persistentDataPath + "/Saves/PlayerPos.sav", pos);
        Debug.Log(Application.persistentDataPath + "/Saves/");
    }

    public void LoadData()
    {
        var itemsText = File.ReadAllText(Application.persistentDataPath + "/Saves/PlayerItems.sav");
        //var items = JsonConvert.DeserializeObject(itemsText);
        var items = JsonConvert.DeserializeObject<List<Item>>(itemsText);
        //var items = JsonConvert.DeserializeObject<List<Item>>(File.ReadAllText(Application.persistentDataPath + "/Saves/PlayerItems.sav"));
        var pos = JsonConvert.DeserializeObject<Vector3>(File.ReadAllText(Application.persistentDataPath + "/Saves/PlayerPos.sav"));

        inventory = items as List<Item>;
        transform.position = pos;
    }

    //public bool HasItem(string itemName)
    //{
    //    string items = "";
    //    for (int i = 0; i < inventory.Count; i++)
    //    {
    //        items += inventory[i].Name;
    //        if (i >= 0 && i < inventory.Count - 1)
    //        {
    //            items += ",";
    //        }
    //    }

    //    return items.Contains(itemName);
    //}
}
