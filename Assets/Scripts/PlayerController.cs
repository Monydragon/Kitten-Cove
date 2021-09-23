using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
	private float speed = 3f;
	[SerializeField]
	private Animator anim;
    [SerializeField]
    private bool isInteracting;

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
        }
    }
    
	void OnTriggerEnter2D(Collider2D col)
	{
		targetGameobject = col.gameObject;
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(targetGameobject == collision.gameObject)
        {
            targetGameobject = null;
        }
    }
}
