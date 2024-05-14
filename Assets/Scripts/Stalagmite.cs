using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalagmite : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;
    [SerializeField]
    private VoidEventChannel onCameraSwitch;

    void Awake()
    {
        SphereCollider sphereCollider = target.GetComponent<SphereCollider>();
        float sphereColliderScale = sphereCollider.transform.lossyScale.x;
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        transform.position = Random.onUnitSphere * (sphereCollider.radius + (boxCollider.bounds.size.y * 2 / sphereColliderScale)) * sphereColliderScale;
        transform.up = target.up;
        transform.rotation = Quaternion.LookRotation(target.position - transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            onGameOver.Raise();
            onCameraSwitch.Raise();
        }
    }
}
