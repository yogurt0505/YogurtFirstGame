using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Collected : MonoBehaviour
{
    public void Collected()
    {
        FindObjectOfType<PlayerController>().CherryCount();
        Destroy(gameObject,0.25f);
    }
}
