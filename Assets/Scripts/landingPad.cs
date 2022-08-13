using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class landingPad : MonoBehaviour
{   
    public static landingPad instance;

    [SerializeField] questPointer qPointer;
    private float countdownToFinish;
    public float finishGoal;
    private bool playerInFinish;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        qPointer.Show(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y));
        countdownToFinish = finishGoal;
    }

    void Update()
    {
        if (playerInFinish == true && countdownToFinish > 0 && rocketShip.instance.getDeath() == false) countdownToFinish -= Time.deltaTime;
        if (playerInFinish == false) countdownToFinish = finishGoal;
    }

    void OnTriggerEnter(Collider other)
    {
        if (countdownToFinish > 0) {
            if (other.gameObject.tag == "Player"){
                Debug.Log("Player is in");
                playerInFinish = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (countdownToFinish > 0) {
            if (other.gameObject.tag == "Player"){
                Debug.Log("Player is out");
                playerInFinish = false;
            }
        }
    }

    public float getCountdown() {
        return countdownToFinish;
    }

    public float getCountdownGoal() {
        return finishGoal;
    }
}
