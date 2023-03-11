using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interfaz : MonoBehaviour
{
    public interface IRigidbodyProvider
    {
        Rigidbody GetRigidbody();
    }
}
