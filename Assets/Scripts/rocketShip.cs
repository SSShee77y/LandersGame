using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class rocketShip : MonoBehaviour
{   
    public static rocketShip instance;
    private Rigidbody rb;
    public MeshRenderer playerMR;

    public ParticleSystem particleMainThrusterSystem, particleSideThrusterSystem, explosionSystem;
    private float explosionCD;

    public float thrusterSpeed = 0.8f;
    public float torqueSpeed = 0.5f;
    private float speedBeforeCollision;

    private float throttle;
    private bool crashed;

    public float fuelTime = 90f;
    private float playerHP = 100f;
    private bool death;
    private bool finished;

    // Post-Processing Adjustments
    public Volume volume;
    private Vignette vig;
    private DepthOfField dof;
    private ColorAdjustments colAdj;
    private LensDistortion lensDis;
    private bool zoomEffect = true;

    private Color originalColor;

    //For tutorial purposes
    public bool lockedUp = false;
    public bool lockedDown = false;
    public bool lockedLeft = false;
    public bool lockedRight = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        volume.profile.TryGet(out vig);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out colAdj);
        volume.profile.TryGet(out lensDis);
        vig.intensity.value = 1;
        vig.smoothness.value = 1;
        lensDis.intensity.value = -1;

        originalColor = playerMR.material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            Physics.IgnoreCollision(other.gameObject.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
        else if (other.gameObject.tag != "Player" && other.gameObject.tag != "Finish" && other.gameObject.tag != "Respawn" && other.gameObject.tag != "Trigger" && other.gameObject.tag != "Ignore" && speedBeforeCollision > 1) {
            playerHP -= 0.2f + (speedBeforeCollision-1)*12f;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (speedBeforeCollision > 2) playerHP -= 0.1f + (speedBeforeCollision-2)*2f;
    }

    void FixedUpdate() // For phsycis based updates
    {
        speedBeforeCollision = rb.velocity.magnitude;

        if (rb.useGravity == true) rb.AddForce(Vector3.down*2);

        if (fuelTime > 0 && death != true && finished != true) {
            ProcessInput();
        }

        if(Input.GetKey(KeyCode.Backspace) && death != true && finished != true) {
            selfDestruct();
        }
        
        if (throttle <= 20) {
            rb.AddRelativeForce(Vector3.up*(throttle/180)*thrusterSpeed);
        } else if (throttle <= 85) {
            rb.AddRelativeForce(Vector3.up*(throttle/135)*thrusterSpeed);
        } else if (throttle <= 100) {
            rb.AddRelativeForce(Vector3.up*(throttle/110)*thrusterSpeed);
        } else if (throttle > 100) {
            rb.AddRelativeForce(Vector3.up*(throttle/100)*thrusterSpeed);
        }
    }

    void Update()
    {
        if (zoomEffect == true) {
            if (vig.intensity.value > .44) vig.intensity.value -= 0.0175f/2;
            if (vig.smoothness.value > .2) vig.smoothness.value -= 0.025f/2;
            if (lensDis.intensity.value < 0) lensDis.intensity.value += 0.03125f/2;
        } if (zoomEffect == true && vig.intensity.value <= .44 && vig.smoothness.value <= .2 && lensDis.intensity.value >= 0) zoomEffect = false;

        if (death == true) {
            if (Input.GetKey(KeyCode.F)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            if (Mathf.Round(Time.timeSinceLevelLoad * 100f) / 100f - explosionCD >= 0.25) explosion(1);
            FindObjectOfType<audioManager>().Play("Sizzling");
        } else {
            FindObjectOfType<audioManager>().Stop("Sizzling");
            FindObjectOfType<audioManager>().Stop("PlayerDeath");
        }

        if (finished == true && Input.GetKey(KeyCode.F)) {
            if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings-1) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            else if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1) SceneManager.LoadScene(0);
        }

        if (playerHP <= 0 && death != true) {
            if (playerHP < 0f) playerHP = 0;
            death = true;
            fuelTime = 0;
            scoreUI.instance.playerDied = true;
            explosion(80);
            FindObjectOfType<audioManager>().Play("PlayerDeath");
            setColorsBlack();
            explosionCD = Mathf.Round(Time.timeSinceLevelLoad * 100f) / 100f;
        }

        if (fuelTime > 0f) {
            if (throttle <= 20.2 && throttle > 0) {
                fuelTime -= Time.deltaTime * (throttle/225f);
            } else if (throttle <= 85.2 && throttle > 0) {
                fuelTime -= Time.deltaTime * (throttle/150f);
            } else if (throttle <= 100.2 && throttle > 0) {
                fuelTime -= Time.deltaTime * (throttle/100f);
            } else if (throttle > 100.2 && throttle > 0) {
                fuelTime -= Time.deltaTime * (throttle/90f);
            }

            if (throttle <= 0.02f) throttle = 0;
            FindObjectOfType<audioManager>().Play("Thruster");
            FindObjectOfType<audioManager>().SetVolume("Thruster", (throttle-10)/50f);
        }

        // [---Particles Start---]
        var emissionMain = particleMainThrusterSystem.emission;
        emissionMain.rateOverTime = (int)throttle/2;
        // [---Particles End---]

        if (fuelTime <= 0 || death == true || finished == true) {
            throttle = 0;
            var emissionSide = particleSideThrusterSystem.emission;
            emissionSide.enabled = false;
            FindObjectOfType<audioManager>().Stop("CompressedAir");
            FindObjectOfType<audioManager>().Stop("Thruster");
        }

        if (landingPad.instance.getCountdown() <= 0) {
            if (finished != true) FindObjectOfType<audioManager>().Play("EagleLanded");
            finished = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        } else FindObjectOfType<audioManager>().Stop("EagleLanded");
    }

    void ProcessInput()
    {
        if(Input.GetKey(KeyCode.RightBracket)) {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings-1) SceneManager.LoadScene(0);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }
        if(Input.GetKey(KeyCode.LeftBracket)) {
            if (SceneManager.GetActiveScene().buildIndex == 0) SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings-1);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }

        if(Input.GetKey(KeyCode.W) && lockedUp == false)
        {
            if (throttle < 99.8) throttle += 0.4f;
            else if (throttle < 110) throttle += 0.06f;
        }

        if(Input.GetKey(KeyCode.S) && lockedDown == false)
        {
            if (throttle > 99.8) throttle -= 0.2f;
            else if (throttle > 0) throttle -= 0.6f;
        }

        if(Input.GetKey(KeyCode.A) && lockedLeft == false)
        {
            rb.AddRelativeTorque(Vector3.forward *torqueSpeed);
            var emissionSpeed = particleSideThrusterSystem.main;
            emissionSpeed.startSpeed = -4;
            var emissionSide = particleSideThrusterSystem.emission;
            emissionSide.enabled = true;
            FindObjectOfType<audioManager>().Play("CompressedAir");
        }

        if(Input.GetKey(KeyCode.D) && lockedRight == false)
        {
            rb.AddRelativeTorque(Vector3.back*torqueSpeed);
            var emissionSpeed = particleSideThrusterSystem.main;
            emissionSpeed.startSpeed = 4;
            var emissionSide = particleSideThrusterSystem.emission;
            emissionSide.enabled = true;
            FindObjectOfType<audioManager>().Play("CompressedAir");
        }

        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            var emissionSide = particleSideThrusterSystem.emission;
            emissionSide.enabled = false;
            FindObjectOfType<audioManager>().Stop("CompressedAir");
        }
    }

    public float getThrottle() {
        return throttle;
    }

    public void setThrottle(int i) {
        throttle = i;
    }

    public float getFuelTime() {
        return fuelTime;
    }

    public float getHP() {
        return playerHP;
    }

    public bool getDeath() {
        return death;
    }

    public void setFinished(bool b) {
        finished = b;
    }

    public bool getFinished() {
        return finished;
    }

    public void selfDestruct() {
        playerHP = 0;
        scoreUI.instance.selfDestruct = true;
        setColorsBlack();
    }

    public void explosion(int i) {
        var emission = explosionSystem.emission;
        emission.rateOverTime = i;
    }

    public void setColorsBlack() {
        for (var i = 0; i < playerMR.materials.Length; i++) {
            playerMR.materials[i].color = Color.black;
        }
    }
}
