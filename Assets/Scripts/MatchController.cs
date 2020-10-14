using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MatchController : MonoBehaviour
{
    public Text timeMath;
    public GameObject ball;
    public GameObject ground;
    public GameObject player;
    public GameObject enemy;
    private int timeMax = 140;
    private int timeLeft;
    //private int matchMax = 5;
    private int matchCount = 1;
    private float xMin = -9.0f;
    private float xMax = 9.0f;
    private float zMin = -14.0f;
    private float zMax = 14.0f;
    private float offset = 0.5f;    
    private int energyEnemy = 0;
    private int energyPlayer = 0;
    private float timeStartEnergy;
    private List<GameObject> listPlayer = new List<GameObject>();
    private List<GameObject> listEnemy = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        timeMath.text = "140s";
        /*RectTransform rt = (RectTransform)ground.transform;
        Debug.Log("x = " + rt.rect.x + "   y = " + rt.rect.y);
        Debug.Log("width = " + rt.rect.width + "   height = " + rt.rect.height);*/
        GenerateBall();
        energyEnemy = 0;
        energyPlayer = 0;
        timeStartEnergy = Time.time;
        timeLeft = timeMax;
    }

    // Update is called once per frame
    void Update()
    {
        //Draw time match
        timeLeft = timeMax - (int)Time.realtimeSinceStartup;
        timeMath.text = timeLeft.ToString() + "s";   
        if(timeLeft <= 0)    
        {
            Debug.Log("End game=====================");
        } 
        GenerateEnergy();
        GenerateSoldier();  
        UpdatePlayer(); 
    }

    int GetPlayerHoldBall()
    {
        for(int i = 0; i < listPlayer.Count; i ++)
        {
            if(listPlayer[i] != null)
            {
                if(listPlayer[i].gameObject.GetComponent<PlayerController>().isHoldBall)
                    return i;
            }
        }
        return -1;
    }
    void UpdatePlayer()
    {
        //Debug.Log("update player=============");
        if(matchCount % 2 != 0)
        {
            //Debug.Log("11111111111111=============");
            int player = GetPlayerHoldBall();
            if(player == -1)
            {
                //Debug.Log("222222222222222222=============" + listPlayer.Count);
                for(int i = 0; i < listPlayer.Count; i ++)
                {
                    if(listPlayer[i] != null)
                    {
                        //Debug.Log("update player=============");
                        listPlayer[i].gameObject.GetComponent<PlayerController>().ChaseBall(ball);
                            
                    }
                }
            }
            else
            {
                Debug.Log("GoStraight=============" + listPlayer.Count);
                ball.transform.parent = listPlayer[player].transform;
                listPlayer[player].GetComponent<PlayerController>().GoStraight();
            }
        }
    }
    
    void GenerateBall ()
    {
       //Debug.Log("111111111111 x = " + ball.transform.position.x + "   z = " + ball.transform.position.z);
        if(matchCount % 2 != 0)
        {
            
            float xball = (float)Random.Range((int)xMin, (int)xMax);
            float zball = (float)Random.Range((int)zMin, (0 - offset));
            ball.transform.position = new Vector3(xball, 0.5f, zball);
            //Debug.Log("333333333333 x = " + xball + "   z = " + zball);
            //ball = Instantiate(ballPrefab, new Vector3(xball, 0.5f, zball), transform.rotation);
            //transform.SetPositionAndRotation(new Vector3 (xball, 0.0f, zball), Quaternion.Euler(0,0,0)); //pxball, 0.5f, zball); 
        }
        else
        {
            float xball = (float)Random.Range((int)xMin, (int)xMax);
            float zball = (float)Random.Range((0 + offset), zMax);
            ball.transform.position = new Vector3(xball, 0.5f, zball);
        }
         //Debug.Log("222222222222 x = " + ball.transform.position.x + "   z = " + ball.transform.position.z);
    }
    void GenerateSoldier()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            int layerMask = 1 << 8;
            if(Physics.Raycast(ray, out hit,100, layerMask))
            {
                //Debug.Log("x = " + hit.point.x + "   y = " + hit.point.z);
                if(isPlayerLandField(hit.point))
                {
                    if(matchCount % 2 != 0)
                    {
                        if(energyPlayer > 1)
                        {
                            CreatePlayer(hit.point);
                            energyPlayer = energyPlayer - 2;
                        }
                    }
                    else
                    {
                        if(energyPlayer > 2)
                        {
                            CreatePlayer(hit.point);
                            energyPlayer = energyPlayer - 3;
                        }
                    }
                    
                }
                else if(isEnemyLandField(hit.point))
                {
                    if(matchCount % 2 == 0)
                    {
                        if(energyEnemy > 1)
                        {
                            CreateEnemy(hit.point);
                            energyEnemy = energyEnemy - 2;
                        }
                    }
                    else
                    {
                        if(energyEnemy > 2)
                        {
                            CreateEnemy(hit.point);
                            energyEnemy = energyEnemy - 3;
                        }
                    }
                    
                }

            }
            
            //Vector3 movement = new Vector3(5.0f, 0.0f, 5);
        
        }
    }

    void CreatePlayer(Vector3 point)
    {   
        point.y = 0.5f;  
        /*Vector3 vt = point - ball.transform.position;
        if(vt.x > - 1 && vt.x < 1 && vt.z > -1 && vt.z <1)
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, point + (Vector3.one - vt), 1);*/
        GameObject temp = Instantiate(player, point, transform.rotation);
        if(matchCount % 2 != 0)
            temp.gameObject.GetComponent<PlayerController>().isAttacker = true;
        else
            temp.gameObject.GetComponent<PlayerController>().isAttacker = false;
        listPlayer.Add(temp);
        //listPlayer.Add(Instantiate(player, point, transform.rotation)); 
        /*Vector3 temp = point - ball.transform.position;
        Debug.Log("temp : x = " + temp.x + "  y = " + temp.y + "  z = " + temp.z);
        //if(point - ball.transform.position ) 
        Debug.Log("111111111111111 x = " + ball.transform.position.x + "   z = " + ball.transform.position.z);    
        listPlayer.Add(Instantiate(player, point, transform.rotation)); 
        if(temp.x > - 1 && temp.x < 1 && temp.z > -1 && temp.z <1)
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, ball.transform.position + temp, 1);  
        Debug.Log("222222222222 x = " + ball.transform.position.x + "   z = " + ball.transform.position.z);*/
    }
    void CreateEnemy(Vector3 point)
    {   
        //point.y = 0.5f;       
        //GameObject temp =  Instantiate(enemy, point, transform.rotation);
        //temp.transform.
        listEnemy.Add(Instantiate(enemy, point, transform.rotation));   
    }
    bool isPlayerLandField (Vector3 point)
    {
        if(point.x >= xMin && point.x <= xMax && point.z >= zMin && point.z < -offset)
            return true;
        return false;
    }
    bool isEnemyLandField (Vector3 point)
    {
        if(point.x >= xMin && point.x <= xMax && point.z > offset && point.z <= zMax)
            return true;
        return false;
    }
    void GenerateEnergy()
    {
        float delTime = Time.time - timeStartEnergy;
        //Debug.Log("delTime========== " + delTime);
        if(delTime >= 0.5f)
        {
            energyEnemy ++;
            energyPlayer ++;            
            timeStartEnergy = Time.time;
            if(energyEnemy > 6)
                energyEnemy = 6;
            if(energyPlayer > 6)
                energyPlayer = 6;
            
            //Debug.Log("energyPlayer========== " + energyPlayer);
            //Debug.Log("energyEnemy+++++++++++ " + energyEnemy);
        }
    }
    
}
