using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackChildren : MonoBehaviour
{
    private float height=182.0f;
    public void ChangeStairs()
    {
        // Get the number of children
        int childCount = transform.childCount;

        // Loop through each child
        for (int i = 0; i < childCount; i++)
        {
            // Get the child at index i
            Transform child = transform.GetChild(i);

            if(i==0){
                height= child.localPosition.y;
            }

            // Move the child up by i units
            child.localPosition = new Vector3(child.localPosition.x,height+ i, child.localPosition.z);
        }
    }
    public void Reset()
    {
        // Get the number of children
        int childCount = transform.childCount;

        // Loop through each child
        for (int i = 0; i < childCount; i++)
        {
            // Get the child at index i
            Transform child = transform.GetChild(i);

            // Move the child up by i units
            child.localPosition = new Vector3(child.localPosition.x, height-i, child.localPosition.z);
        }
    }
}
