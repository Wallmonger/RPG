using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Effect/Ice And Fire")]

public class IceAndFire_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private Vector2 newVelocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Transform player = PlayerManager.instance.player.transform;

        // Launching prefab with player facing dir
        GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.rotation);

        newIceAndFire.GetComponent<Rigidbody2D>().velocity = newVelocity;
    }
}
