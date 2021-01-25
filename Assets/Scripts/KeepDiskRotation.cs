using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepDiskRotation : MonoBehaviour
{
    public float speed = 5.0f;
    private Quaternion angle = Quaternion.identity; 
    private bool active = true;

    void Update() {
        if(active) {
            transform.rotation = Quaternion.Slerp(transform.rotation, angle, Time.deltaTime * speed);
        }
    }

    public void updateAngle() {
        angle = transform.rotation;
    }

    public void setActive(bool state) {
        active = state;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer != 3) {
            setActive(false);
            StartCoroutine(delayedUpdateAngle(0.1f));
        }
    }

    IEnumerator delayedUpdateAngle(float time) {
        yield return new WaitForSeconds(time);
        updateAngle();
        setActive(true);
    }
}
