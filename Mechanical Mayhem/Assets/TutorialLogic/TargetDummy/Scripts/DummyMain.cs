using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMain : MonoBehaviour
{
    private bool isHit = false;
    private Animator animator;

    
    public bool hitOnce;
    public bool stayDown;
    
    [SerializeField] private float downTime = 10f;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit()
    {
        if (!isHit)
        {
            StartCoroutine(ShotDown());
        }
    }

    IEnumerator ShotDown()
    {
        isHit = true;
        hitOnce = true;
        animator.Play("FallDown");

        yield return new WaitForSeconds(downTime);

        if (!stayDown)
        {

            animator.Play("GetUp");
            isHit = false;
        }
    }

    public bool GetIsHit() { return this.isHit; }

}
