using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackChildren : MonoBehaviour
{
    public float jumpstart=50.0f;
    public void ChangeStairs()
    {
        // Get the number of children
        int childCount = transform.childCount;

        // Loop through each child
        for (int i = 0; i < childCount; i++)
        {
            // Get the child at index i
            Transform child = transform.GetChild(i);

            // Move the child up by i units
            child.localPosition = new Vector3(child.localPosition.x, i, child.localPosition.z);
        }
        transform.position=transform.position + new Vector3(0,jumpstart,0); 
    }
}

