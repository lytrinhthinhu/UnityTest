using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    //public bool isHolding;
    //private float speed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        //isHolding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector3 point)
    {
        //transform.rotation = Quaternion.LookRotation(point - transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, point, speed * Time.deltaTime);
    }
    /*void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter===========");
        if(other.gameObject.CompareTag("player") || other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.)
            isHolding = true;
            /*Animator anim = other.GetComponent<Animator>();
            int color = anim.GetInteger("IsColor");
            anim.SetTrigger("DeActive");
            if(color == 1)
            {
                animPlayer.SetTrigger("IsDie");
                //Destroy(this.gameObject, 5);
            }
            //other.gameObject.SetActive(false);
            //listPickUp.Dequeue();
            count = count + 1;
            SetCountText();
        }
    }*/
}
