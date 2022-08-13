using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreUI : MonoBehaviour
{
    public static scoreUI instance;
    
    public Text timerText;
    public Text informationText;
    public Text interactionText;
    public Text indicatorNumsText;
    public Text verticalNums;
    public Text horizontalNums;
    public Text yawNums;

    public Image horizontalBar;
    public Image verticalBar;
    public Image yawBar;
    public GameObject yawGuide;

    public Text distanceNum;

    public GameObject player;
    public GameObject finish;

    public float timer;
    private float secs;
    private int mins, hits;
    public bool hasFinished;
    public bool playerDied;
    public bool selfDestruct;
    private string throttleStr;

    public Rigidbody playerRB;
    public void Awake()
    {
        instance = this;
    }

    void Update()
    {
        renderHUDText();
        renderDisplayBars();

        if (hasFinished == false)
        {
            timer = Mathf.Round(Time.timeSinceLevelLoad * 100f) / 100f;
            mins = (int)(timer / 60);
            secs = timer % 60;

            if (playerDied != true && selfDestruct != true) timerText.text = string.Format("{0}:{1:00.00}", mins, secs);

            if (landingPad.instance.getCountdown() <= 0) {
                hasFinished = true;
            } else if (landingPad.instance.getCountdown() < landingPad.instance.getCountdownGoal()) {
                informationText.text = string.Format("{0:0.##}", landingPad.instance.getCountdown());
            } else informationText.text = "";

            if (playerDied == true) {
                informationText.text = string.Format("YOU CRASHED");
                interactionText.text = string.Format("PRESS F TO RESTART");
            }
            
            if (selfDestruct == true) {
                informationText.text = string.Format("SELF DESTRUCTED");
                interactionText.text = string.Format("PRESS F TO RESTART");
            }
            if (rocketShip.instance.getFuelTime() <= 0 && selfDestruct != true && playerDied != true) {
                informationText.text = string.Format("OUT OF FUEL");
                interactionText.text = string.Format("PRESS BACKSPACE TO SELF DESTRUCT");
            }
        }
        else if (hasFinished == true) {
                informationText.text = string.Format("SUCCESSFUL LANDING | LEVEL COMPLETE");
                interactionText.text = string.Format("PRESS F TO CONTINUE");
            }
    }

    private void renderHUDText()
    {
        if (rocketShip.instance.getThrottle() <= 100) throttleStr = ((int)rocketShip.instance.getThrottle()).ToString();
        else if (rocketShip.instance.getThrottle() > 100) throttleStr = "<color=red>"+(int)rocketShip.instance.getThrottle()+"</color>";

        string fuelTimer = "";
        if (rocketShip.instance.getFuelTime() > 0) fuelTimer = string.Format("{0}:{1:00.}", (int)(rocketShip.instance.getFuelTime()/60), ((int)rocketShip.instance.getFuelTime())%60);
        else fuelTimer = string.Format("<color=red>0:00</color>");

        string playerHP = "";
        if (rocketShip.instance.getHP() > 0) playerHP = string.Format("{0:0.#}", rocketShip.instance.getHP());
        else playerHP = string.Format("<color=red>0</color>");

        indicatorNumsText.text = string.Format("{0}\n{1:0.}\n{2:0.}\n{3}\n{4}\n-------",
            throttleStr, playerRB.velocity.magnitude, playerRB.transform.position.y+4, fuelTimer, playerHP);

        horizontalNums.text = string.Format("\n{0:0.##}", playerRB.velocity.x);
        verticalNums.text = string.Format("\n{0:0.##}", playerRB.velocity.y);
        yawNums.text = string.Format("{0:0.#}\n{1:0.#}", Mathf.Rad2Deg*2*Mathf.Acos(playerRB.gameObject.transform.rotation.z)-180, -Mathf.Rad2Deg*playerRB.angularVelocity.z);
        
        string distanceStr;
        if (Vector3.Distance(player.transform.position, finish.transform.position) >= 10) distanceStr = string.Format("{0} m", (int)Vector3.Distance(player.transform.position, finish.transform.position));
        else distanceStr = string.Format("{0:0.0} m", Vector3.Distance(player.transform.position, finish.transform.position));
        distanceNum.text = distanceStr;
    }

    private void renderDisplayBars()
    {
        horizontalBar.rectTransform.localScale = new Vector3(playerRB.velocity.x/50, 0.15f, 1);
        horizontalBar.rectTransform.position = new Vector3((Screen.width/2)+(playerRB.velocity.x*(Screen.width/1366f)), horizontalBar.rectTransform.position.y, horizontalBar.rectTransform.position.z);
        
        verticalBar.rectTransform.localScale = new Vector3(0.15f, playerRB.velocity.y/50, 1);
        verticalBar.rectTransform.position = new Vector3(verticalBar.rectTransform.position.x, (Screen.height/2)+(playerRB.velocity.y*(Screen.height/768f)), verticalBar.rectTransform.position.z);

        yawBar.rectTransform.localScale = new Vector3(Mathf.Sign(-1*playerRB.angularVelocity.z)*5.5f, 5.5f, 5.5f);
        yawBar.fillAmount = (Mathf.Abs(-Mathf.Rad2Deg*playerRB.angularVelocity.z)/360f);
    }

    public void addHits()
    {
        hits++;
    }

    public int getHits()
    {
        return hits;
    }

    public void onFinish()
    {
        hasFinished = true;
    }

    public bool getFinish()
    {
        return hasFinished;
    }

    public void resetInformationText()
    {
        informationText.text = "";
    }

    public void resetInteractionText()
    {
        interactionText.text = "";
    }
}
