using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour {

    public string triggerName;

	public void OnCollisionEnter2D(Collision2D coli)
    {
        if(coli.gameObject.CompareTag("Player"))
        {
            Debug.Log("THIS HAPPENS");
            TAEventManager.Instance.ReceiveActionTrigger(triggerName);
        }
    }
}
