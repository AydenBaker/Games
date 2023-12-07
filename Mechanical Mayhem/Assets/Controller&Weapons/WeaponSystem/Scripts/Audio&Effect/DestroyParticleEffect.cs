using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleEffect : MonoBehaviour
{

    [SerializeField] private float timeAlive = 2f;
    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if(time >= timeAlive)
        {
            Destroy(this.gameObject);
        }
    }

}
