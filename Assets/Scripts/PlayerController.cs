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
    public bool IsActive;
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
        IsActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateActive();    
        //PlayerMove();    
    }
    void updateActive()
    {
        if(IsActive)
            return;
        //Debug.Log("timeActive===========" + timeActive);
        if(isAttacker)
        {
            if(timeActive < timeActiveAttackerDEF)
            {
                timeActive = Time.time - startCountTime;
                transform.Find("Direction").transform.gameObject.SetActive(false);
                
                //Debug.Log("timeActive===========" + timeActive);
            }
            else
            {
                IsActive = true;
                isCaught = false;
                isGold = false;
                isHoldBall = false;
                transform.tag = "Attacker";
                //transform.GetChild(0).gameObject.SetActive(true);
				//Debug.Log("active player==========" + isHoldBall);
                transform.GetComponent<Animator>().SetBool("IsActive", true);
            }
        } 
        
    }
     
    void OnTriggerEnter(Collider other)
    {
        if(!IsActive)
            return;
        //Debug.Log("player OnTriggerEnter===========" + other.gameObject.tag);
        if(other.gameObject.CompareTag("Ball"))
        {
            //Debug.Log("other.gameObject.CompareTag(Ball)===========" + other.transform.parent);
            if(other.transform.parent == null)
            {
                other.transform.parent = transform;
                isHoldBall = true;            
                //Debug.Log("other.gameObject.CompareTag(Ball)===========" + isHoldBall);
            }
        }
        else if(other.gameObject.CompareTag("Fence"))
        {
            //Debug.Log("trigger Fence==================");
            if(!isHoldBall)
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
        else if(other.gameObject.CompareTag("Defender"))
        {
            if(isHoldBall)
            {
                /*isCaught = true;
                isHoldBall = false;
				//Debug.Log("CompareTag enemy==========" + isHoldBall);
                timeActive = 0.0f;
                startCountTime = Time.time;
                IsActive = false;
                transform.GetComponent<Animator>().SetBool("IsActive", false);
                transform.tag = "Attacker";
                other.tag = "Comeback";*/
                OnTriggerDefender();
                other.tag = "Comeback";
            }
        }
    }
    /*void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if(other.gameObject.CompareTag("Defender"))
            {
                if(isHoldBall)
                {
                    Debug.Log("CompareTag enemy==========" + isHoldBall);
                    /*isCaught = true;
                    isHoldBall = false;
                    timeActive = 0.0f;
                    startCountTime = Time.time;
                    IsActive = false;
                    transform.GetComponent<Animator>().SetBool("IsActive", false);
                    transform.tag = "Attacker";
                    other.tag = "Comeback";**
                    OnTriggerDefender();
                    other.tag = "Comeback";
                }
            }
        }
    }*/
    public void ChaseBall (GameObject ball)
    {     
         
        if(IsActive)
        {            
            Vector3 target = ball.transform.position;
            //Debug.Log("ChaseBall======= x = " + target.x + "  y = " + target.y + " z = " + target.z);  
            transform.rotation = Quaternion.LookRotation(target - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target, normalSpeedAttacker * Time.deltaTime);
        }
        
    }
    public void PlayerMove()
    {
        isHoldBall = false;
		//Debug.Log("PlayerMove===========" + isHoldBall);
        if(isAttacker)
        {
            if(IsActive && !isHoldBall)
            {
                GoStraight();
            }
        }
    }
    public void GoStraight()    
    {
        Vector3 vt = transform.position;
        vt.z = 14.0f;
        transform.rotation = Quaternion.LookRotation(vt - transform.position);
        transform.Translate(transform.forward * normalSpeedAttacker * Time.deltaTime);
    }
    public void CarryBall(Vector3 point)
    {
        transform.rotation = Quaternion.LookRotation(point - transform.position);
        transform.position = Vector3.MoveTowards(transform.position, point, carryingSpeed * Time.deltaTime);
    }
    public void OnTriggerDefender()
    {
        isCaught = true;
        isHoldBall = false;
        //Debug.Log("CompareTag enemy==========" + isHoldBall);
        timeActive = 0.0f;
        startCountTime = Time.time;
        IsActive = false;
        transform.GetComponent<Animator>().SetBool("IsActive", false);
        transform.tag = "Attacker";        
    }
    public void PassBall(Vector3 point)
    {
        //transform.rotation = Quaternion.LookRotation(point - transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, point, carryingSpeed * Time.deltaTime);
    }
    
}
