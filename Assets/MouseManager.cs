using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	void OnGUI () {
        if (Event.current.button == 0)
        {
            if (Event.current.type == EventType.MouseDown)
            {
                Debug.Log("Clicked!");
                GameManager.instance.SendMessage("OnClick");
            }
        }
	}

}
