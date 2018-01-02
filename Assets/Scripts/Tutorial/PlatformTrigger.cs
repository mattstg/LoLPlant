using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour {

    public string triggerName;

	public void OnCollisionEnter2D(Collision2D coli)
    {
        if(coli.gameObject.CompareTag("Player"))
        {
            TAEventManager.Instance.ReceiveActionTrigger(triggerName);
        }
    }
}
