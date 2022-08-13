using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questPointer : MonoBehaviour {

    [SerializeField] Camera UICamera;
    [SerializeField] Sprite arrowSprite;
    [SerializeField] Sprite crossSprite;
    [SerializeField] float borderSize = 100f;

    private Vector3 targetPosition;
    private RectTransform pointerRectTransform;
    private Image pointerImage;

    void Awake() {
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
        pointerImage = transform.Find("Pointer").GetComponent<Image>();

        Hide();
    }

    void Update() {        
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width-borderSize || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height-borderSize;
        RotatePointerTowardsTarget();

        //if (isOffScreen) {
            

            pointerImage.sprite = arrowSprite;
            /*Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width-borderSize) cappedTargetScreenPosition.x = Screen.width-borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height-borderSize) cappedTargetScreenPosition.y = Screen.height-borderSize;

            Vector3 pointerWorldPosition = UICamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position = pointerWorldPosition;*/
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        /*} else {
            pointerRectTransform.localEulerAngles = Vector3.zero;

            pointerImage.sprite = crossSprite;
            //Vector3 pointerWorldPosition = UICamera.ScreenToWorldPoint(targetPositionScreenPoint);
            //pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, 0f);
        }*/
    }

    private void RotatePointerTowardsTarget() {
        Vector3 toPosition = targetPosition;
        Vector3 fromPositon = Camera.main.transform.position;
        fromPositon.z = 0f;
        Vector3 dir = (toPosition - fromPositon).normalized;
        float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
        
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show(Vector3 targetPosition) {
        gameObject.SetActive(true);
        this.targetPosition = targetPosition;
    }
}
