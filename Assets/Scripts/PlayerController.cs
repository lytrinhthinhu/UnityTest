using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAttacker;
    public bool isHoldBall;
    public bool isCaught;
    public bool isGold;
    private float timeActiveAttackerDEF = 2.5f;
    private float timeActiveDefenderDEF = 4.0f;
    private float timeActive =0.0f;
    private float startCountTime = 0.0f;
    private float normalSpeedAttacker = 1.5f;
    private float carryingSpeed = 0.75f;
    //private float PassBallSpeed;
    private float normalSpeedDefender = 1.0f;
    
    void Start()
    {
        timeActive = 0.0f;
        startCountTime = Time.time;
        isHoldBall = false;
        isCaught = false;
        isGold = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateActive();    
        //PlayerMove();    
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
        //Debug.Log("OnTriggerEnter===========" + other.gameObject.tag);
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
        else if(other.gameObject.CompareTag("Fence"))
        {
            //Debug.Log("trigger Fence==================");
            transform.gameObject.SetActive(false);
            //Destroy(transform);
            //Destroy(this);             
        }
        else if(other.gameObject.CompareTag("Gate"))
        {
            if(isHoldBall)
            {
                //transform.gameObject.GetComponent<MatchController>().isEndGame = true;
                //transform.gameObject.GetComponent<MatchController>().winner = 1;
                isGold = true;
                Debug.Log("Gold ===========");
            }                
            else
                transform.gameObject.SetActive(false);
        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            isCaught = true;
            isHoldBall = false;
            timeActive = 0.0f;
            startCountTime = Time.time;
            transform.GetComponent<Animator>().SetBool("IsActive", true);
        }
    }
    public void ChaseBall (GameObject ball)
    {     
         
        if(timeActive >= timeActiveAttackerDEF)
        {
            
            Vector3 target = ball.transform.position;
            //Debug.Log("ChaseBall======= x = " + target.x + "  y = " + target.y + " z = " + target.z);  
            transform.position = Vector3.MoveTowards(transform.position, target, normalSpeedAttacker * Time.deltaTime);
        }
        
    }
    public void PlayerMove()
    {
        isHoldBall = false;
        if(isAttacker)
        {
            if(timeActive >= timeActiveAttackerDEF)
            {
                GoStraight();
            }
        }
    }
    public void GoStraight()    
    {
        transform.Translate(transform.forward * normalSpeedAttacker * Time.deltaTime);
    }
    public void CarryBall(Vector3 point)
    {
        transform.rotation = Quaternion.LookRotation(point - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, point, carryingSpeed * Time.deltaTime);
    }
    public void PassBall(Vector3 point)
    {
        //transform.rotation = Quaternion.LookRotation(point - transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, point, carryingSpeed * Time.deltaTime);
    }
}
