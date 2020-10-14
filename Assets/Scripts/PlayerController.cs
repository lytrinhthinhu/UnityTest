using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAttacker;
    public bool isHoldBall;
    private float timeActiveAttackerDEF = 2.5f;
    private float timeActiveDefenderDEF = 4.0f;
    private float timeActive =0.0f;
    private float startCountTime = 0.0f;
    private float normalSpeedAttacker = 1.5f;
    private float carryingSpeed = 0.75f;
    private float normalSpeedDefender = 1.0f;
    
    void Start()
    {
        timeActive = 0.0f;
        startCountTime = Time.time;
        isHoldBall = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateActive();        
    }
    void updateActive()
    {
        //Debug.Log("timeActive===========" + timeActive);
        if(isAttacker)
        {
            if(timeActive < timeActiveAttackerDEF)
            {
                timeActive = Time.time - startCountTime;
                //Debug.Log("timeActive===========" + timeActive);
            }
            else
            {
                transform.GetComponent<Animator>().SetBool("IsActive", true);
            }
        } 
        
    }
     
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter===========" + other.gameObject.tag);
        if(other.gameObject.CompareTag("Ball"))
        {
            isHoldBall = true;
            //other.transform.parent = transform;
            //other.gameObject.GetComponent<BallController>().isHolding = true;
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
            SetCountText();*/
        }
    }
    public void ChaseBall (GameObject ball)
    {     
        //Debug.Log("movePlayer=======");   
        if(timeActive >= timeActiveAttackerDEF)
        {
            
            Vector3 target = ball.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target, normalSpeedAttacker * Time.deltaTime);
        }
        
    }
    public void GoStraight()
    {
        transform.Translate(transform.forward * normalSpeedAttacker * Time.deltaTime);
    }
    public void CarryBall()
    {}
}
