using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRButton : MonoBehaviour
{
    public float btnInital = 1.0f;
    public float btnThresh = 0.9f;
    public float returnSpeed = 3.5f;

    public UnityEvent btnActivated;
    
    private Rigidbody rb;

    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.localPosition;
        transform.localPosition = new Vector3(pos.x, btnInital, pos.z);

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.y < btnInital) {
            returnBtn();
        }

        if(transform.localPosition.y > btnInital) {
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x, btnInital, pos.z);
            active = false;
        }

        if(transform.localPosition.y < btnThresh) {
            Vector3 pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x, btnThresh, pos.z);
            rb.velocity = Vector3.zero;
            if(!active) {
                btnActivated.Invoke();
                active = true;
            }
        }
    }

    void returnBtn() {
        rb.AddForce(Vector3.up * returnSpeed * Time.deltaTime * 50);
    }
}
