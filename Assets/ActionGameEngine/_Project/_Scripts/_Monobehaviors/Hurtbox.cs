using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] private int allignment;
    public void GetHit()
    {
        Debug.Log("Hit");
    }

    public int GetAllignment()
    {
        return allignment;
    }
}
