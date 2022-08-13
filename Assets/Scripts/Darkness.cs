using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Darkness : MonoBehaviour
{
    public static Darkness instance;
    public bool dark = false;

    // Post-Processing Adjustments
    public Volume volume;
    private Vignette vig;
    private DepthOfField dof;
    private ColorAdjustments colAdj;
    private LensDistortion lensDis;

    public Light light;

    void Awake()
    {
        instance = this;
    }

    private void Start() {
        volume.profile.TryGet(out vig);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out colAdj);
        volume.profile.TryGet(out lensDis);
    }

    void Update()
    {
        if (dark == true) {
            if(vig.intensity.value < .75) vig.intensity.value += 0.00175f;
            if(vig.smoothness.value < .75) vig.smoothness.value += 0.0025f;
            if(vig.smoothness.value >= .5) vig.rounded.value = true;
            if(light.intensity > 0) light.intensity -= 0.005f;
        }
        else if (dark == false) {
            if(vig.intensity.value > .44) vig.intensity.value -= 0.00175f;
            if(vig.smoothness.value > .2) vig.smoothness.value -= 0.0025f;
            if(vig.smoothness.value < .5) vig.rounded.value = false;
            if(light.intensity < 1) light.intensity += 0.005f;
        }
        Debug.Log(dark + " " + vig.intensity.value);
    }

    public void SetDark(bool boolean) {
        dark = boolean;
    }
}
