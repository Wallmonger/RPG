using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    public UI_ItemToolTip itemToolTip;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SwitchTo(GameObject _menu)
    {
        // Switching all tabs, and enable one if clicked

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu  != null)
            _menu.SetActive(true);  
    }
}
