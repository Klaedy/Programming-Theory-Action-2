using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class OnPlayerEnter : MonoBehaviour
{
    public UnityEvent onInteractEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onInteractEvent.Invoke();
        }
    }
}
