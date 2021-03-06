using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raindrop : MonoBehaviour
{
    private Animator anim;

    //start: to delete 
    void Start()
    {
        Initialize();
    }

    // Use this for initialization
    public void Initialize()
    {
        anim = GetComponent<Animator>();
		this.transform.parent = GV.ws.raindrops.transform;
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }

    public void Pop()
    {
        anim.SetTrigger("Pop");
    }

    public void OnTriggerEnter2D(Collider2D coli)
    {
        switch (coli.gameObject.tag)
        {
            case "Platform":
                Pop();
                break;
            case "Aphid":
                Pop();
                break;
            case "Water":
                break;
		case "RainCollector":
			Destroy ();
			if (GV.ws.plant != null)
                {
				    float waterDelta = GV.WaterPerDrop * (1f - (GV.ws.plant.water * 2f / 3f));
				    GV.ws.plant.water = Mathf.Clamp01 (GV.ws.plant.water + waterDelta);
                    LOLAudio.Instance.PlayAudio(LOLAudio.collectRain);
			    }
                break;
        }
    }
}
