using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().enabled = false;
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    public void Explode() 
    {
        GetComponent<SphereCollider>().enabled = true;
        Invoke("Destroy", 2.8f);
    }
}
