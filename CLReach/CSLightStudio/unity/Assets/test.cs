using UnityEngine;
using System.Collections;

public class test : MonoBehaviour{

	// Use this for initialization
	void Start () {
     
 	}

	// Update is called once per frame
	void Update () {
	
	}
    CSLight.ICLS_Debugger debugger;
    bool bBtn = true;
    void OnGUI()
    {
        if (bBtn)
        {
            if (GUI.Button(new Rect(0, 0, 200, 50), "Show Debug Window on A New Thread."))
            {
                //CSLightDebugger.ShowDebug();
            }
        }
    }
    void onClose()
    {
        Debug.Log("winClose");
        bBtn = true;
    }

}
