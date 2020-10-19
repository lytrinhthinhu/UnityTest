using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MatchController : MonoBehaviour
{
    public Text timeMath;
    public GameObject ball;
    public GameObject ground;
    public GameObject GateEnemy;
    public GameObject player;
    public GameObject enemy;
    public Text txtEndGame;
    public Text txtBoxNameEnemy;
    public Text txtBoxNamePlayer;
    //public Button btnNext;
    public GameObject EndGameScreen;
    public int scoreWinPlayer;
    public int scoreWinEnemy;
    public Energy energyEnemy1;
    public Energy energyEnemy2;
    public Energy energyEnemy3;
    public Energy energyEnemy4;
    public Energy energyEnemy5;
    public Energy energyEnemy6;
    public Energy energyPlayer1;
    public Energy energyPlayer2;
    public Energy energyPlayer3;
    public Energy energyPlayer4;
    public Energy energyPlayer5;
    public Energy energyPlayer6;
    private int energyMax = 6;
    private bool isEndGame;
    //winer 0: draw, 1: player, 2: enemy
    private int winner;
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
    private float timeStartEnergyEnemy;
    private float timeStartEnergyPlayer;
    private int timeStartMatch;
    private float speedBall = 1.5f;
    
    private List<GameObject> listPlayer = new List<GameObject>();
    private List<GameObject> listEnemy = new List<GameObject>();
    private List<Energy> listEnergyEnemy = new List<Energy>();
    private List<Energy> listEnergyPlayer = new List<Energy>();
    
    // Start is called before the first frame update
    void Start()
    {
        scoreWinPlayer = 0;
        scoreWinEnemy = 0;
        InnitGame();
    }
    public void InnitGame()
    {
        Debug.Log("init===============");
        Time.timeScale = 1.0f;
        timeMath.text = "140s";
        EndGameScreen.SetActive(false);        
        GenerateBall();
        energyEnemy = 0;
        energyPlayer = 0;
        timeStartEnergyEnemy = Time.time;
        timeStartEnergyPlayer = Time.time;
        timeLeft = timeMax;
        timeStartMatch = (int)Time.time;
        
        isEndGame = false;
        winner = 0;

        listEnergyEnemy.Add(energyEnemy1);
        listEnergyEnemy.Add(energyEnemy2);
        listEnergyEnemy.Add(energyEnemy3);
        listEnergyEnemy.Add(energyEnemy4);
        listEnergyEnemy.Add(energyEnemy5);
        listEnergyEnemy.Add(energyEnemy6);
        for(int i = 0; i < listEnergyEnemy.Count; i ++)
        {
            listEnergyEnemy[i].SetEnergy(0.0f, Color.red);
        }

        listEnergyPlayer.Add(energyPlayer1);
        listEnergyPlayer.Add(energyPlayer2);
        listEnergyPlayer.Add(energyPlayer3);
        listEnergyPlayer.Add(energyPlayer4);
        listEnergyPlayer.Add(energyPlayer5);
        listEnergyPlayer.Add(energyPlayer6); 
        for(int i = 0; i < listEnergyPlayer.Count; i ++)
        {
            listEnergyPlayer[i].SetEnergy(0.0f, Color.blue);
        }
        if(matchCount % 2 != 0)
        {
            txtBoxNameEnemy.text = "Enemy (Defender)";
            txtBoxNamePlayer.text = "Player (Attacker)";
        }
        else
        {
            txtBoxNameEnemy.text = "Enemy (Attacker)";
            txtBoxNamePlayer.text = "Player (Defender)";
        }
        //listEnemy = null;
        //listPlayer = null;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timeLeft = timeMax - ((int)Time.time - timeStartMatch);
        //timeLeft = timeMax - (int)Time.realtimeSinceStartup;
        timeMath.text = timeLeft.ToString() + "s";
        if(timeLeft <= 0 && isEndGame == false)    
        {
            isEndGame = true;
            winner = 0; //draw
        } 
        CheckEndGame();
    }
    
    void Update()
    {
        //Draw time match  
        GenerateEnergy();
        GenerateSoldier();  
        UpdatePlayer(); 
        UpdateEnemy();
    }
    void CheckEndGame()
    {
        if(listPlayer != null)
        {
            for(int i = 0; i < listPlayer.Count; i ++)
            {
                if(listPlayer[i] != null && listPlayer[i].activeInHierarchy)
                {
                    if(listPlayer[i].gameObject.GetComponent<PlayerController>().isGold)
                    {
                        isEndGame = true;
                        winner = 1; //player win
                    }
                        
                }
            }
        }
        if(isEndGame)
        {
            if(winner == 0)
            {
                txtEndGame.text = "Draw";                
            }
            else if(winner == 1)
            {
                scoreWinPlayer ++;
                txtEndGame.text = "Player win";                
            }
            else if(winner == 2)
            {
                txtEndGame.text = "Enemy win";
                scoreWinEnemy ++;
            }
            EndGameScreen.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
    int GetPlayerHoldBall()
    {
        //Debug.Log("GetPlayerHoldBall============");
        for(int i = 0; i < listPlayer.Count; i ++)
        {
            if(listPlayer[i] != null && listPlayer[i].activeInHierarchy)
            {
                if(listPlayer[i].gameObject.GetComponent<PlayerController>().isHoldBall)
                    return i;
            }
        }
        return -1;
    }
    int GetPlayerCaught()
    {
        for(int i = 0; i < listPlayer.Count; i ++)
        {
            if(listPlayer[i] != null && listPlayer[i].activeInHierarchy)
            {
                if(listPlayer[i].gameObject.GetComponent<PlayerController>().isCaught)
                {
                    return i;
                }
            }
        }
        return -1;
    }
    int LookForPlayerNearest(int index)
    {
        if(listPlayer.Count == 1)
            return -1;
        GameObject player = listPlayer[index];
        Vector3 point = player.transform.position;
        //listPlayer.RemoveAt(index);
        int indexNearest = -1;
        float distMin = 0.0f;
        for(int i = 0; i < listPlayer.Count; i ++)
        {
            if(listPlayer[i] != null && listPlayer[i].activeInHierarchy && listPlayer[i].gameObject.GetComponent<PlayerController>().IsActive)
            {
                distMin = Vector3.Distance(point, listPlayer[i].transform.position);
                indexNearest = i;
                break;
            }
        }
        for(int i = indexNearest + 1; i < listPlayer.Count; i ++)
        {
            if(listPlayer[i] != null && listPlayer[i].activeInHierarchy && listPlayer[i].gameObject.GetComponent<PlayerController>().IsActive)
            {
                float dist = Vector3.Distance(point, listPlayer[i].transform.position);
                if(dist < distMin)
                {
                    distMin = dist;
                    indexNearest = i;
                } 
            }
        }
        //listPlayer.Add(player);
        return indexNearest;
    }
    void UpdatePlayer()
    {
        if(isEndGame)
            return;
        //Debug.Log("update player=============");
        if(matchCount % 2 != 0)
        {
            //Debug.Log("11111111111111=============");
            int player = GetPlayerHoldBall();
            if(player == -1) 
            {
                PlayerChaseBall();               
                int playerCaught = GetPlayerCaught();
                //Debug.Log("GetPlayerCaught =============" + playerCaught); 
                if(playerCaught != -1)
                {  
                    int playerNearest = LookForPlayerNearest(playerCaught);  
                    //Debug.Log("LookForPlayerNearest =============" + playerNearest);                    
                    if(playerNearest == -1)
                    {
                        isEndGame = true;
                        winner = 2; //enemy win
                        scoreWinEnemy ++;
                        return;
                    }
                    else
                    {
                        //listPlayer[playerCaught].transform.GetComponent<PlayerController>().isCaught = false;
                        //ball.transform.GetComponent<BallController>().Move(listPlayer[playerNearest].transform.position);
                        ball.transform.parent = null;
                        ball.transform.position = Vector3.MoveTowards(ball.transform.position, listPlayer[playerNearest].transform.position, speedBall * Time.deltaTime);
                        //PlayerChaseBall();
                        /*for(int i = 0; i < listPlayer.Count; i ++)
                        {
                            if(listPlayer[i] != null && listPlayer[i].activeInHierarchy)
                            {
                                //Debug.Log("update player=============");
                                listPlayer[i].gameObject.GetComponent<PlayerController>().PlayerMove();
                                    
                            }
                        }*/
                    }
                }
            }
            else
            {
                //Debug.Log("GetPlayerHoldBall ============= " + player); 
                //ball.transform.parent = listPlayer[player].transform;
                for(int i = 0; i < listPlayer.Count; i ++)
                {
                    if( i != player && listPlayer[i] != null && listPlayer[i].activeInHierarchy)
                    {
                       // if(i == 0)
                        //Debug.Log("PlayerMove=============" + i);
                        listPlayer[i].gameObject.GetComponent<PlayerController>().PlayerMove();
                            
                    }
                }
                listPlayer[player].GetComponent<PlayerController>().CarryBall(GateEnemy.transform.position);
                listPlayer[player].tag = "ActackerHoldBall";
                //if is caught = true
                
            }
        }
    }
    void PlayerChaseBall()
    {
        if(ball.transform.parent != null)
            return;
        for(int i = 0; i < listPlayer.Count; i ++)
        {
            if(listPlayer[i] != null && listPlayer[i].activeInHierarchy)
            {
                //if(i == 0)
                //Debug.Log("PlayerChaseBall=============" + i);
                listPlayer[i].gameObject.GetComponent<PlayerController>().ChaseBall(ball);
                    
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
        if(isEndGame)
            return;
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
                            EnergyCostPlayer(2);
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
                            EnergyCostEnemy(3);
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
        point.y = 0.5f;       
        //GameObject temp =  Instantiate(enemy, point, transform.rotation);
        //temp.transform.
        GameObject temp = Instantiate(enemy, point, transform.rotation);
        if(matchCount % 2 != 0)
            temp.gameObject.GetComponent<EnemyController>().isAttacker = false;
        else
            temp.gameObject.GetComponent<EnemyController>().isAttacker = true;
        listEnemy.Add(temp);   
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
        //Debug.Log("delTime========== " + energyEnemy);
        float delTimeEnemy = Time.time - timeStartEnergyEnemy;
        if(energyEnemy < energyMax) 
        {   
            listEnergyEnemy[energyEnemy].SetEnergy(delTimeEnemy, Color.red);
        }
        float delTimePlayer = Time.time - timeStartEnergyPlayer;
        if(energyPlayer < energyMax)
        {
            listEnergyPlayer[energyPlayer].SetEnergy(delTimePlayer,Color.blue);
        }
        //energyBarEnemy.SetEnergy(delTime);
        //Debug.Log("delTime========== " + delTime);
        if(delTimeEnemy >= 2.0f)
        {
            energyEnemy ++; 
            timeStartEnergyEnemy = Time.time;           
            if(energyEnemy > energyMax)
                energyEnemy = energyMax; 
        }
        if(delTimePlayer >= 2.0f)
        {
            energyPlayer ++;
            timeStartEnergyPlayer = Time.time;
            if(energyEnemy > energyMax)
                energyEnemy = energyMax;
            if(energyPlayer > energyMax)
                energyPlayer = energyMax ;
            
            
        }
    }
    public void EnergyCostEnemy(int value)
    {
        if(energyEnemy < energyMax)
            listEnergyEnemy[energyEnemy - value].SetEnergy(listEnergyEnemy[energyEnemy].GetEnemy(), Color.red);
        else
        {
            listEnergyEnemy[energyEnemy - value].SetEnergy(0.0f, Color.red);
            timeStartEnergyEnemy = Time.time;
        }
        
        for(int i = 0; i <= value - 1; i ++)
        {
            if(energyEnemy - i < energyMax)
                listEnergyEnemy[energyEnemy - i].SetEnergy(0.0f, Color.red);
        }
    }
    public void EnergyCostPlayer(int value)
    {
        if(energyPlayer < energyMax)
            listEnergyPlayer[energyPlayer - value].SetEnergy(listEnergyPlayer[energyPlayer].GetEnemy(), Color.blue);
        else
        {
            listEnergyPlayer[energyPlayer - value].SetEnergy(0.0f, Color.blue);
            timeStartEnergyPlayer = Time.time;
        }
        
        for(int i = 0; i <= value - 1; i ++)
        {
            if(energyPlayer - i < energyMax)
                listEnergyPlayer[energyPlayer - i].SetEnergy(0.0f, Color.blue);
        }
    }
    void UpdateEnemy()
    {
        if(isEndGame)
            return;
        if(matchCount %2 != 0)
        {
            for(int i = 0; i < listEnemy.Count; i ++)
            {
                if(listEnemy[i] != null)
                {
                    if(listEnemy[i].gameObject.GetComponent<EnemyController>().isChaseAttacker)
                    {
                        int player = GetPlayerHoldBall();
                        if(player == -1)
                        {
                            if(listEnemy[i].tag == "Comeback")   
                            { 
                                listEnemy[i].transform.GetComponent<EnemyController>().Comeback();                                
                            }
                        }
                        else
                            listEnemy[i].gameObject.GetComponent<EnemyController>().ChaseAttacker(listPlayer[player]);
                    }
                }
            }
        }
    }
}
