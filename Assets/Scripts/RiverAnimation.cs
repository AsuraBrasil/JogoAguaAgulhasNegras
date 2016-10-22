using UnityEngine;
using System.Collections;

public class RiverAnimation : MonoBehaviour {

    public float waitTime = .2f; //Wait time to start the animation

    private SpriteRenderer sprite;
    private Animator anim;
    private float timecount;

	// Use this for initialization
	void Start () {

        sprite = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        timecount = Time.time + waitTime;
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Time.time > timecount)
        {
            sprite.enabled = true;
            anim.enabled = true;
        }

	}
}
