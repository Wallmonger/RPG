using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public bool canGrow;
    public List<Transform> targets;

    private void Update()
    {
        if (canGrow)
        {
            // Lerp change value overtime with slow curve at the end
            //! Lerp(origin, endvalue, duration)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Add to targets List if enemy is detected by the circle collider
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            
            // Instantiate key on top of the enemy without rotation
            GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0,2), Quaternion.identity);

            // Random key created on each collision then remove from the list
            KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
            keyCodeList.Remove(choosenKey);

            Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

            newHotKeyScript.SetupHotKey(choosenKey);
          
        }
    }
}
