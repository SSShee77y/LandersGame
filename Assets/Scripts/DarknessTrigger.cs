using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessTrigger : MonoBehaviour
{
    private enum Entrance {Enter, Exit};
    [SerializeField] Entrance darknessTrigger = Entrance.Enter;

    private void OnTriggerEnter(Collider other)
    {
        if (darknessTrigger == Entrance.Enter && other.tag == "Player") {
            Darkness.instance.SetDark(true);
            Debug.Log("entered");
        }

        if (darknessTrigger == Entrance.Exit && other.tag == "Player") {
            Darkness.instance.SetDark(false);
            Debug.Log("exited");
        }
    }
}
