using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    public bool canGrow;
    public bool canShrink;

    private bool canCreateHotKeys = true;

    private bool cloneAttackReleased;
    public int amountOfAttacks = 4;
    public float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();
    

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyHotKeys();
            cloneAttackReleased = true;
            canCreateHotKeys = false;
        }

        // attacks random enemy of the targets list
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;
            
            int randomIndex = Random.Range(0, targets.Count);

            // Randomize offset for creating distance between player & enemy
            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3 (xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }

        if (canGrow && !canShrink)
        {
            // Lerp change value overtime with slow curve at the end
            //! Lerp(origin, endvalue, duration)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            // Shrinking the black hole
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void DestroyHotKeys ()
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++) 
            Destroy(createdHotKey[i]);
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add to targets List if enemy is detected by the circle collider
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);

            CreateHotKey(collision);

        }
    }

    private void CreateHotKey(Collider2D collision)
    {
        // If there is no more hotKey
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("No enough keys in the keycodelist");
            return;
        }

        // Prevent enemy entering the blackhole after reaching its max size
        if (!canCreateHotKeys)
            return;

        // Instantiate key on top of the enemy without rotation
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);

        // Adding hotKey to list 
        createdHotKey.Add(newHotKey);

        // Random key created on each collision then remove from the list
        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    // Add enemy as a target on key up
    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
