using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
	public delegate void D_Void();
	public delegate void D_GameObject(GameObject go1, GameObject go2);
	
	public static event D_GameObject onInteract;
	
	public static void Interact(GameObject source, GameObject target) { onInteract.Invoke(source, target); }
}
