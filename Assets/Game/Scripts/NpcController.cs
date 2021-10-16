using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class NpcController : MonoBehaviour
{
    [SerializeField]
    private BlockReference startBlock;

    [SerializeField]
    private int relationship;

    private void OnEnable()
    {
        EventManager.onInteract += EventManager_onInteract;
    }

    private void OnDisable()
    {
        EventManager.onInteract -= EventManager_onInteract;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        relationship = startBlock.block.GetFlowchart().GetIntegerVariable("Relationship");
        Debug.Log($"Relationship Value: {relationship}");
    }
    private void EventManager_onInteract(GameObject source, GameObject target)
    {
        if(gameObject == target)
        {
            startBlock.Execute();
            var helped = startBlock.block.GetFlowchart().GetBooleanVariable("isHelped");
            if (helped)
            {
                Debug.Log("The Box has been helped!");
                var questItem = startBlock.block.GetFlowchart().GetStringVariable("QuestItem");
                var pc = source.GetComponent<PlayerController>();
                var questItemFound = pc.HasItem(questItem);
                if (questItemFound)
                {
                    startBlock.block.GetFlowchart().SetBooleanVariable("QuestItemFound", true);
                    pc.RemoveItem(questItem);
                }
            }
        }
    }
}
