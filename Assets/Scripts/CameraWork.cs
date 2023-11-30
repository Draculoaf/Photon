using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    #region private fields
    // distance between the plane and the target
    // height we want to camera above the target
    [SerializeField] private float distance = 7f; private float height = 3f;

    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField] private Vector3 centerOffset = Vector3.zero;

    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField] private bool followOnStart = false;

    [Tooltip("The Smoothing for the camera to follow the target")]
    [SerializeField] private float smoothSpeed = 0.125f;

    private Transform cameraTransform;

    private bool isFollowing;

    private Vector3 cameraOffset = Vector3.zero;

    #endregion

    #region monobehaviour callbacks

    private void Start()
    {
         if (followOnStart) { OnStartFollowing(); }
    }

    private void LateUpdate()
    {
        if (isFollowing) { Follow();  }

        if (cameraTransform==null&&isFollowing) { OnStartFollowing(); }
    }

    #endregion

    #region public methods

    public void OnStartFollowing() 
    { 
        cameraTransform=Camera.main.transform;
        isFollowing = true;
        Cut();
    }

    #endregion

    #region private methods

    private void Follow()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);
        cameraTransform.LookAt(this.transform.position + centerOffset);
    }

    private void Cut()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

        cameraTransform.LookAt(this.transform.position + centerOffset);
    }
    #endregion

}
