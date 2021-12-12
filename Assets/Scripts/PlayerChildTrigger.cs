using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChildTrigger : MonoBehaviour
{
    public GameObject group;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            group.SetActive(true);
            GameManager._instance.enabled = false;
        }        
    }
}