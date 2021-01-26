using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float startPosition = 0.0f;
    public float endPosition = 4.5f;

    public float speed = 10.0f;

    private float currentPos;
    private float timer = 0;

    private bool starting = false;
    private Rigidbody rb;

    private GrabLocomotion grabCollider;

    void Start() {
        currentPos = startPosition;
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if(starting) {
            timer += Time.deltaTime;
        }

        if(timer % 60 >= 3) {
            rb.AddRelativeForce(new Vector3(0, speed, 0), ForceMode.Impulse);
            // StartCoroutine(releaseLauncher());
        }
    }

    IEnumerator releaseLauncher() {
        yield return new WaitForSeconds(0.5f);
        grabCollider.handleRelease(Vector3.zero, true);
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.layer == 3) {
            starting = true;
            // grabCollider = collision.gameObject.GetComponent<GrabLocomotion>();
        }
    }

    void OnCollisionExit(Collision collision) {
        starting = false;
        timer = 0;
    }
}
