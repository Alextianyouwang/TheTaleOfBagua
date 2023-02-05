using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
     List<GameObject> levelObject = new List<GameObject>();

    bool colliderToggleState = false;
    private void Awake()
    {
        GetLevelObject();
    }
    public void GetLevelObject() 
    {
        foreach (Transform t in transform) 
        {
            levelObject.Add(t.gameObject);
            t.GetComponent<Collider>().isTrigger = true;
        }
    }
    public void ToggleRigidColliders(bool value) 
    {
        if (colliderToggleState == value)
        return;
        colliderToggleState = value;
        foreach (GameObject t in levelObject) 
        {
            Collider c = t.GetComponent<Collider>();
            c.isTrigger =!value;
        }
    }
}
