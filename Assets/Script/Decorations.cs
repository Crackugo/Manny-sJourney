using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Decorations : MonoBehaviour
{
    public GameObject decorations;
    public float rotationSpeed = 10.0f;

    // rotation axis
    public Vector3 rotationAxis = Vector3.up;
    void Start()
    {
        // save all the children of the decorations object
        Transform[] children = decorations.GetComponentsInChildren<Transform>();
        Debug.Log("Children: " + children.Length);
    }
    
    void Update()
    {
        // Rotate the decorations
        decorations.transform.Rotate(rotationAxis * Time.deltaTime * rotationSpeed);

    }
}