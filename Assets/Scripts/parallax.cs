using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour {

    private float length, height, startPosX, startPosY;
    public GameObject cam;
    public float parallaxEffectX, parallaxEffectY;
    
    void Start() {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update() {
        float tempX = (cam.transform.position.x * (1 - parallaxEffectX));
        float tempY = (cam.transform.position.y * (1 - parallaxEffectY));
        float distanceX = (cam.transform.position.x * parallaxEffectX);
        float distanceY = (cam.transform.position.y * parallaxEffectY);

        transform.position = new Vector3(startPosX + distanceX, startPosY + distanceY, transform.position.z);

        if (tempX > startPosX + length) startPosX += length;
        else if (tempX < startPosX - length) startPosX -= length;
    }
}
