using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statemachine : MonoBehaviour
{
    public enum enemy_state {PATROL,CHASE,ATTACK};
    [SerializeField]
    public enemy_state currentstate;
    public Health playerhealth = null;
    public float maxDamage = 10f;
    public bool seei;//test
    public bool seei2;//test
    public double dd;//test
    // public enemy_state CurrentState{
    //     get {
    //         return currentstate;
    //     }
    //     set{
    //         currentstate=value;
    //         StopAllCoroutines();
    //         switch(currentstate){
    //             case enemy_state.PATROL:
    //             seei=true;
    //             StartCoroutine(EnemyPatrol());
    //             break;
    //             case enemy_state.CHASE:
    //             StartCoroutine(EnemyChase());
    //             break;
    //             case enemy_state.ATTACK:
    //             StartCoroutine(EnemyAttack());
    //             break;
    //         }
    //     }
    // }

    public LineOfSight checkmyVision;
    public UnityEngine.AI.NavMeshAgent agent = null;
    public GameObject playertransform = null;
    public GameObject patrolDestination = null;

    private void Awake(){
        checkmyVision=GetComponent<LineOfSight>();
        agent=GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerhealth= playertransform.GetComponent<Health>();

    }

    // Start is called before the first frame update

    void Start()
    {
       // GameObject[] destination = GameObject.FindGameObjectsWithTag("Dest");
        //patrolDestination= destination;//[Random.Range(0,destination.Length)].GetComponent<Transform>();
        currentstate= enemy_state.PATROL;
        dd=checkmyVision.distance;

        
    }
    public IEnumerator EnemyPatrol(){
        while(currentstate==enemy_state.PATROL){
            checkmyVision.sensitivity = LineOfSight.Sensitivity.HIGH;
            agent.isStopped=false;
            agent.SetDestination(patrolDestination.transform.position);

            while(agent.pathPending)
                yield return null;
            
            seei=checkmyVision.targetInSight;
            dd=checkmyVision.distance;
            
            if(checkmyVision.targetInSight){
                agent.isStopped=true;
                currentstate=enemy_state.CHASE;
                yield break;
            }
            yield break;
        }
        
    }
    public IEnumerator EnemyChase(){
        while(currentstate==enemy_state.CHASE){
            agent.isStopped=false;
            agent.SetDestination(checkmyVision.lastknownSight);
            // while(agent.pathPending){
            //     yield return null;
            // }

            // if(agent.remainingDistance<=agent.stoppingDistance){
            //     agent.isStopped=true;
            //     if(!checkmyVision.targetInSight)
            //      currentstate=enemy_state.PATROL;
            //      else
            //      currentstate=enemy_state.ATTACK;
            //      yield break;
            // }
            // yield return null;
            dd=checkmyVision.distance;
            if(dd>=60){
                seei=false;
                agent.SetDestination(patrolDestination.transform.position);
                currentstate=enemy_state.PATROL;
            }
            if(dd<=10){
                currentstate=enemy_state.ATTACK;
                yield break;
            }
            yield break;
        }
        
    }
    public IEnumerator EnemyAttack(){
        while(currentstate==enemy_state.ATTACK){
            agent.isStopped=false;
            dd=checkmyVision.distance;
            
            agent.SetDestination(playertransform.transform.position);
            // while(agent.pathPending)
            // yield return null;
            if(dd<=6){
                seei2=true;
                agent.isStopped=true;
            }
            else if(dd>=11){
                currentstate=enemy_state.CHASE;
            }
            else{
                if(playerhealth.healthpoints>-1){
                    playerhealth.healthpoints-=maxDamage*Time.deltaTime;
                }
            }
            yield return null;
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        StopAllCoroutines();
            switch(currentstate){
                case enemy_state.PATROL:
                StartCoroutine(EnemyPatrol());
                break;
                case enemy_state.CHASE:
                StartCoroutine(EnemyChase());
                break;
                case enemy_state.ATTACK:
                StartCoroutine(EnemyAttack());
                break;
            }
    }
}