using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdollMovement : MonoBehaviour
{

    public Transform[] bodyParts;
    public ConfigurableJoint[] joints;
    public Quaternion[] initialRotations;
    // Start is called before the first frame update
    void Start()
    {
        initialRotations = new Quaternion[bodyParts.Length];
        for (int i = 0 ; i < bodyParts.Length ; i++ ){
            initialRotations[i] = bodyParts[i].localRotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0 ; i < joints.Length ; i++ ){
           // ConfigurableJointExtensions.SetTargetRotationLocal(joints[i],bodyParts[i].localRotation,initialRotations[i]);
        }
    }
}
