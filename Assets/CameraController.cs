using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Target sera la position du personnage, offset le d�calage de la cam�ra par rapport au personnage
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        // On d�finit le d�calage de la cam�ra en effectuant une soustraction de la position du personnage par rapport � la cam�ra, ce qui nous donnera les valeurs x,y,z de la distance les s�parant
        offset = target.position - transform.position;
    }

    void Update()
    {
        // Pour obtenir la position de la cam�ra, on soustrait la position du personnage � la variable offset, qui nous donnera la position souhait�e de la cam�ra.
        transform.position = target.position - offset;
    }
}
