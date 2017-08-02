using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public GameObject unicycleTire;
    public GameObject gameObjectToApplyLeanForceTo;
    public GameObject gameObjectToApplyConstantUpwardForceTo;

    public float leftToRightLeanForceMultiplier = 1f;
    public float backToForwardLeanForceMultiplier = 1f;
    public float wheelTorqueMultiplier = 1f;
    public float constantUpwardForce = 1f;
    public float secondsToWaitForTiltInput = .5f;

    // ints would probably be faster than all this floating point arithmetic i'm doin
    public float _pedalSpeed = 1f;
    public float _accelerationIncrement = 1f;

    private float pedalPositionAngleInDegrees = 0f;
    private float pedalAnimtionPercentage = 1f;

    private Rigidbody _rigidbody;

    private float _startForwardTilt = 0f;
    private bool _tiltIsCalibrated = false;

    private WheelCollider _wheelCollider;

    // Use this for initialization
    void Start () {
        StartCoroutine(CalibrateTiltInput(secondsToWaitForTiltInput));
        _rigidbody = GetComponent<Rigidbody>();
        _wheelCollider = GetComponentInChildren<WheelCollider>();
    }

    void Update()
    {
        
    }

    void FixedUpdate () {
        if (_tiltIsCalibrated)
        {
            var leftToRightLean = CrossPlatformInputManager.GetAxis("Horizontal");
            var backToForwardLean = (CrossPlatformInputManager.GetAxis("Vertical") - _startForwardTilt);

            var leanForce = new Vector3(leftToRightLean * leftToRightLeanForceMultiplier, 0, backToForwardLean * backToForwardLeanForceMultiplier);
            _rigidbody.AddForceAtPosition(leanForce, gameObjectToApplyLeanForceTo.transform.position);
        }

        _rigidbody.AddForceAtPosition(Vector3.up * constantUpwardForce, gameObjectToApplyConstantUpwardForceTo.transform.position);

        unicycleTire.transform.Rotate(Vector3.up, _pedalSpeed);
        _wheelCollider.motorTorque = _pedalSpeed * wheelTorqueMultiplier;
    }

    public void OnAccelerate()
    {
        _pedalSpeed += _accelerationIncrement;
    }

    public void OnDecelerate()
    {
        _pedalSpeed -= _accelerationIncrement;
    }

    IEnumerator CalibrateTiltInput(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        _startForwardTilt = CrossPlatformInputManager.GetAxis("Vertical");
        Debug.Log("Start Forward Tilt" + _startForwardTilt);
        _tiltIsCalibrated = true;
    }
}
