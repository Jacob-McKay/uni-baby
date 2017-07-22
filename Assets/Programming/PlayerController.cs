using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public GameObject gameObjectToApplyLeanForceTo;
    public float leftToRightLeanForceMultiplier = 1f;
    public float backToForwardLeanForceMultiplier = 1f;
    public float constantUpwardForce = 1f;
    public float secondsToWaitForTiltInput = .5f;

    private Rigidbody _rigidbody;

    private float _startForwardTilt = 0f;
    private bool _tiltIsCalibrated = false;

    // Use this for initialization
    void Start () {
        StartCoroutine(CalibrateTiltInput(secondsToWaitForTiltInput));
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate () {
        if (_tiltIsCalibrated)
        {
            var leftToRightLean = CrossPlatformInputManager.GetAxis("Horizontal");
            var backToForwardLean = (CrossPlatformInputManager.GetAxis("Vertical") - _startForwardTilt);

            var leanForce = new Vector3(leftToRightLean * leftToRightLeanForceMultiplier, constantUpwardForce, backToForwardLean * backToForwardLeanForceMultiplier);
            _rigidbody.AddForceAtPosition(leanForce, gameObjectToApplyLeanForceTo.transform.position);
        } else
        {
            var leanForce = new Vector3(0, constantUpwardForce, 0);
            _rigidbody.AddForceAtPosition(leanForce, gameObjectToApplyLeanForceTo.transform.position);
        }
    }

    IEnumerator CalibrateTiltInput(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        _startForwardTilt = CrossPlatformInputManager.GetAxis("Vertical");
        Debug.Log("Start Forward Tilt" + _startForwardTilt);
        _tiltIsCalibrated = true;
    }
}
