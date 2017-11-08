using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    
	public Vector2 clickableSize = new Vector2(.5f,.5f);


    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _MouseClicked(Input.mousePosition);
        }
        else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _MouseClicked(Input.GetTouch(0).position);
        }
        if (Input.GetKeyDown(KeyCode.Space))
            MouseClickedOnObjOfInterest();
    }
	public void OnMouseOver()
	{
		MouseOver (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	private void _MouseClicked(Vector2 clickPos)
	{
		Vector2 clickedPos = Camera.main.ScreenToWorldPoint (clickPos);
		MouseDown (clickedPos);
	}

	protected virtual void MouseOver(Vector2 mouseWorldPos)
	{

	}

	protected virtual void MouseDown(Vector2 mouseWorldPos)
	{

	}

	protected virtual void MouseClickedOnObjOfInterest()
	{

	}
}
