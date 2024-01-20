using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : DetectionBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(var tag in detectionTagList)
        {
            if (collision.CompareTag(tag) && collision.tag == tag)
            {
                detectionEnterEvent?.Invoke(collision.gameObject);
                break;
            }
        }
    }
}
