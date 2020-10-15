using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isAttacker;
    public bool isComebackStartPoint;
    public bool isChaseAttacker;
    public Vector3 startPoint;
    public float returnSpeedDefender = 2.0f;
    //public bool isHoldBall;
    //private float timeActiveAttackerDEF = 2.5f;
    private float timeActiveDefenderDEF = 4.0f;
    private float timeActive =0.0f;
    
    
    private float startCountTime = 0.0f;
    //private float normalSpeedAttacker = 1.5f;
    //private float carryingSpeed = 0.75f;
    private float normalSpeedDefender = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        timeActive = 0.0f;
        startCountTime = Time.time;
        isChaseAttacker = false;
        startPoint = transform.position;
        isComebackStartPoint = false;
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
            /*if(timeActive < timeActiveAttackerDEF)
            {
                timeActive = Time.time - startCountTime;
                //Debug.Log("timeActive===========" + timeActive);
            }
            else
            {
                transform.GetComponent<Animator>().SetBool("IsActive", true);
            }*/
        } 
        else
        {
            //Debug.Log("timeActive===========" + timeActive);
            if(timeActive < timeActiveDefenderDEF)
            {
                timeActive = Time.time - startCountTime;                
            }
            else
            {
                //Debug.Log("timeActive===========" + timeActive);
                transform.GetComponent<Animator>().SetBool("IsActive", true);
                //if(transform.GetComponent<Animator>().GetBool("ChaseAttacker") == true)
            }
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter===========" + other.gameObject.tag);
        if(other.gameObject.CompareTag("playerHoldBall"))
        {
            if(isAttacker == false)
            {
                if(isChaseAttacker == false)
                {
                    isChaseAttacker = true;
                    isComebackStartPoint = false;
                    transform.GetComponent<Animator>().SetBool("ChaseAttacker", true);
                    //transform.tag = "Enemy";
                }
                else
                {
                    Debug.Log("isChaseAttacker===========" + isChaseAttacker);
                    isChaseAttacker = false;
                    timeActive = 0.0f;
                    startCountTime = Time.time;
                    isChaseAttacker = false;
                    isComebackStartPoint = true;
                    transform.GetComponent<Animator>().SetBool("ChaseAttacker", false);
                    transform.GetComponent<Animator>().SetBool("IsActive", false);
                    //transform.tag = "Untagged";
                    transform.tag = "Enemy";
                }
            }            
            
        }        
    }

    public void ChaseAttacker(Vector3 point)
    {
        if(timeActive >= timeActiveDefenderDEF)
        {
            transform.rotation = Quaternion.LookRotation(point - transform.position);
            transform.position = Vector3.MoveTowards(transform.position, point, normalSpeedDefender * Time.deltaTime);
        }
        
    }
}
