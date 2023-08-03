using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Target sera la position du personnage, offset le décalage de la caméra par rapport au personnage
    public Transform target;
    public Vector3 offset;

    void Start()
    {
        // On définit le décalage de la caméra en effectuant une soustraction de la position du personnage par rapport à la caméra, ce qui nous donnera les valeurs x,y,z de la distance les séparant
        offset = target.position - transform.position;
    }

    void Update()
    {
        // Pour obtenir la position de la caméra, on soustrait la position du personnage à la variable offset, qui nous donnera la position souhaitée de la caméra.
        transform.position = target.position - offset;
    }
}
