using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class DebugMobileInput : MonoBehaviour {

    public float secondsToWaitForTiltInput = .5f;

    private float _startForwardTilt = 0f;
    private bool _tiltIsCalibrated = false;
	// Use this for initialization
	void Start () {
        StartCoroutine(CalibrateTiltInput(secondsToWaitForTiltInput));
	}
	
	// Update is called once per frame
	void Update () {
        if (_tiltIsCalibrated)
        {
            Debug.Log("Vertical Axis: " + CrossPlatformInputManager.GetAxis("Vertical"));
            var updatedTransformPosition = new Vector3 { x = transform.position.x, y = transform.position.y, z = transform.position.z };
            updatedTransformPosition.x += CrossPlatformInputManager.GetAxis("Horizontal");
            updatedTransformPosition.z -= (CrossPlatformInputManager.GetAxis("Vertical") - _startForwardTilt);

            transform.position = updatedTransformPosition;
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
