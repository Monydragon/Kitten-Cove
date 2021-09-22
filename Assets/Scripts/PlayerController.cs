using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
	private float speed = 3f;
	[SerializeField]
	private Animator anim;


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
        movement = new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
	    transform.position += movement * speed * Time.deltaTime;
	    
	    if(anim != null)
	    {
		    if(movement != Vector3.zero)
		    {
			    anim.SetBool("isMoving", true);
			    anim.SetFloat("InputX",movement.x);
			    anim.SetFloat("InputY",movement.y);
		    }
		    else
		    {
			    anim.SetBool("isMoving", false);
		    }
	    }
    }
    
	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log($"You hit something: {col.gameObject.name}");
		EventManager.Interact(gameObject, col.gameObject);
	}
}
