using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowHealth : MonoBehaviour
{
    public void Health()
    {
        // slowHealth()
    }
    static IEnumerator slowHealth(float health)
    {
        yield return new WaitForSeconds(0.5f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
