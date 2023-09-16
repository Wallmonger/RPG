using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    public void SetupHotKey(KeyCode _myNewHotKey)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            Debug.Log("HOT KEY IS " + myHotKey);
        }   
    }
}
