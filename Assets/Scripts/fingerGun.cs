using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class fingerGun : MonoBehaviour
{
    public SteamVR_Action_Boolean thrust = SteamVR_Input.GetBooleanAction("Thrust");
    
    public GameObject projectile;
    public float projectileSpeed = 1.0f;
    Vector3 car;
    public SteamVR_Behaviour_Skeleton skeleton = null;
    private GameObject summoned;

    public GameObject projectileSpawner;

    public float rotOffset = 45.0f;

    private bool released = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // GameObject projectileSpawner = GameObject.FindGameObjectWithTag("projectileSpawner");
        // car = skeleton.GetBonePosition((int)SteamVR_Skeleton_JointIndexes.indexTip, false);
        if(thrust.GetStateDown(SteamVR_Input_Sources.Any)) {
            released = false;
            summoned = Instantiate(projectile);
            summoned.GetComponent<TrailRenderer>().enabled = released;
            summoned.transform.rotation = projectileSpawner.transform.rotation;
            summoned.transform.position = projectileSpawner.transform.position;            
        }

        

        if(thrust.GetStateUp(SteamVR_Input_Sources.Any)) {
            released = true;
            summoned.GetComponent<TrailRenderer>().enabled = released;
            StartCoroutine(PhysicsLate(summoned.GetComponent<Rigidbody>(), projectileSpawner.transform.forward * projectileSpeed));
        }

        if(summoned && !released) {
            summoned.transform.rotation = projectileSpawner.transform.rotation;
            summoned.transform.position = projectileSpawner.transform.position;    
        }
    }

     IEnumerator PhysicsLate (Rigidbody rb, Vector3 force){
        yield return new WaitForSeconds(0.01f);
        rb.AddForce(force, ForceMode.Impulse);
    }

}
