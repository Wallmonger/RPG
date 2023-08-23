using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Static class to make it available to all scripts

    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        // To prevent having two instance of this class while starting the game, delete the other one if it exists 
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
