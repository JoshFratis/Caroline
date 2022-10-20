using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    // Components
    [SerializeField] Animator animator;

    // Sound
    [SerializeField] RandomSoundPlayer randomSoundPlayer;
    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] GameObject monsterAudioSource;
    [SerializeField] AudioSource audioSourcePain;
    [SerializeField] AudioSource audioSourceAttack;

    // Navigation
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform scarePointsRoot;
    Transform[] scarePoints;
    Transform fleeDestination;

    // Hands
    [SerializeField] Damager leftHandDamager;
    [SerializeField] Damager rightHandDamager;

    // Player
    [SerializeField] Interactable interactable;
    [SerializeField] GameObject player;
    [SerializeField] Torch playerTorchScript;
    [SerializeField] Collider playerVisionTrigger;

    // State
    bool isWalking;
    bool isAttacking;
    bool isCowering;
    bool isApproaching = true;
    bool isFleeing = true;
    bool inGame = true;
    bool inVision = false;

    bool forwardInput;
    bool cowerInput;

    bool inLight = false;
    int lightsIn;

    // Navigation
    Vector3 destination; 
    int scarePointIndex;

    int approachSpeed = 6;
    int fleeSpeed = 10;

    float distanceToPlayer;
    float distanceMin = 2.55f;
    int distanceMax = 40;

    // De / Respawn
    int timer;
    int respawnTimeMin = 250;
    int respawnTimeMax = 750;

    // Persistence Bounds
    int persistenceMin = 1;
    int persistenceMax = 501;
    int persistenceThresholdLow = 50; 
    int persistenceThresholdHigh = 250;
    int persistence = 501; 

    void Start()
    {
        // Navmesh Scare Points
        scarePoints = scarePointsRoot.GetComponentsInChildren<Transform>();
        Debug.Log("scarePoints: "+scarePoints);

        // Starting State
        Despawn();
    }

    void Update()
    {
        if (inGame) 
        {
            GetState();
            Behave();
            Animate();
        }
        else if (!inGame) 
        {
            Despawned();
        }
        
    }
    
    void GetState()
    {
        // Get distance to player
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // In Light?
        if ((lightsIn > 0) && (inLight == false)) 
        {
            inLight = true;
        }
        else if ((lightsIn == 0) && (inLight == true)) 
        {
            inLight = false;
        }

         // Engage or Flee?
        if (persistence > persistenceThresholdHigh)
        {
            if (isFleeing == true) 
            {
                Debug.Log("Monster approaches");
                isApproaching = true;
                isFleeing = false;
                navMeshAgent.enabled = true;
                navMeshAgent.speed = approachSpeed;
            }
        }
        else if (persistence < persistenceThresholdLow)
        {  
            if (isFleeing == false)
            {
                Flee();
            }
        }

        // Turn on/off damage from hands if attacking/not attacking
        if (isAttacking)
        {
            if (!audioSourceAttack.isPlaying) 
            {
                audioSourceAttack.Play(0);
            }
            leftHandDamager.damageAmount = 20;
            rightHandDamager.damageAmount = 20;
        }
        else if (!isAttacking)
        {
            leftHandDamager.damageAmount = 0;
            rightHandDamager.damageAmount = 0;
        }
    }

    void Behave()
    {
        // React to Light
        if (inLight)
        {
            if (persistence > persistenceMin) 
            {
                persistence--; // Exposure to light leads to monster fleeing
            }
        }
        else if (!inLight)
        {
            if (persistence < persistenceMax) 
            {
                persistence++; // No exposure to light leads to monster approaching
            }
        }

        // Move towards player
        if (isApproaching)
        {
            // Move towards player
            if (distanceToPlayer > distanceMin)
            {
                isWalking = true;
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(player.transform.position);
            }
            // Attack player
            else
            {
                navMeshAgent.enabled = false;
                if (isAttacking == false) Debug.Log("Monster attacks.");
                isAttacking = true;
                isWalking = false;
            }
        }

        // Move away from player
        else if (isFleeing)
        {
            if (inVision)
            {
                if (distanceToPlayer < distanceMax)
                {
                    navMeshAgent.SetDestination(fleeDestination.position);

                    // Check if we've reached the destination
                    if (!navMeshAgent.pathPending)
                    {
                        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                        {
                            if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                            {
                                persistence = persistenceMax;
                            }
                        }
                    }
                }
                else
                {
                    persistence = persistenceMax;
                }
            }
            else if (!inVision)
            {
                if ((!audioSourcePain.isPlaying) && (!audioSourceAttack.isPlaying))
                {
                    Despawn();
                }
            }
            
        }
    }

    // called on being hit by torch and on hitting player
    public void isSeen()
    {
        // force monster to flee if player's torch is lit
        if (playerTorchScript.intensity > 0)
        {
            Flee();
        }
    }

    // Called on being hit by torch
    public void Cower()
    {
        // run cower animation if hit by player's torch
        if (playerTorchScript.intensity > 0)
        {
            isCowering = true;
            animator.SetBool("isCowering", true);
            audioSourcePain.Play(0);
            audioSourceAttack.Stop();
        }
    }

    public void Flee()
    {
        Debug.Log("Monster flees");

        persistence = 0;
        isApproaching = false;
        isAttacking = false;
        isFleeing = true;
        isWalking = true;
        
        scarePointIndex = Random.Range(0, scarePoints.Length);
        fleeDestination = scarePoints[scarePointIndex];

        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(fleeDestination.position);
        navMeshAgent.speed = fleeSpeed;
    }

    public void Despawn()
    {
        // Move Monster beneath scene
        transform.position = new Vector3(0, -3, 0);
        navMeshAgent.enabled = false;

        // Switch Audio
        monsterAudioSource.SetActive(false);
        //randomSoundPlayer.Activate();
        
        // Set Timer to Respawn
        inGame = false;
        timer = Random.Range(respawnTimeMin, respawnTimeMax); 
        Debug.Log("monster despawning");
    }

    void Despawned()
    {
        timer--;

        // Respawn Monster
        if (timer <= 0)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        Debug.Log("respawning monster");    
        inGame = true;
        monsterAudioSource.SetActive(true);
        transform.position = player.transform.position - (player.transform.forward * Random.Range(10,30)) + (player.transform.right * Random.Range(-20,20));
        navMeshAgent.enabled = true;  
        persistence = persistenceMax;
    }

    // Register light entered
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light")
        {
            lightsIn++;
            Debug.Log("Monster entered light.");
        } 

        if (other == playerVisionTrigger)
        {
            inVision = true;
            Debug.Log("Monster seen");
        }
    }

    // Register light exited
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Light") 
        {
            lightsIn--;
            Debug.Log("Monster left light.");
        }

        if (other == playerVisionTrigger)
        {
            inVision = false;
            Debug.Log("Monster unseen");

        }
    }

    void Animate()
    {
        if (isWalking) 
        {
            animator.SetBool("isWalking", true);
            isCowering = false;
            isAttacking = false;
        }
        else if (!isWalking) 
        {
            animator.SetBool("isWalking", false);
        }

        if (isAttacking) 
        {
            // here randomly select attack to play
            animator.SetBool("isAttacking", true);
            isWalking = false;

        }
        else if (!isAttacking) 
        {
            animator.SetBool("isAttacking", false);
        }

        if (isCowering) 
        {
            animator.SetBool("isCowering", true);
        }
        else if (!isCowering) 
        {
            animator.SetBool("isCowering", false);
        }
    }
}
