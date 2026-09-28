using UnityEngine;

public class ObjectLifeTime : MonoBehaviour

{

    public float lifeTime = 1f; // The time in seconds before the object is destroyed
      // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifeTime); // Destroy the object after the specified lifetime
    }
}
