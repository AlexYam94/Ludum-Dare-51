using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropitemController : MonoBehaviour
{
    [SerializeField] float dropRate = .3f;

    [SerializeField] GameObject[] itemsToDrop;

    public void DropItem()
    {
        bool shouldDrop = Random.Range(0f,1f) >= 1 - dropRate;

        if (shouldDrop)
        {
            int index = Random.Range(0, itemsToDrop.Length);
            GameObject.Instantiate(itemsToDrop[index], transform.position, Quaternion.identity);
        }
    }
}
