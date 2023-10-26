using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script purely for use with world space UI to correctly face the main camera
/// </summary>
public class FollowCamera : MonoBehaviour {

    public bool flipDirection;
    public int x;
    public int y;
    public int z;

    // Update is called once per frame
    void LateUpdate() {

        if (flipDirection) {
            transform.LookAt(Camera.main.transform.position);
            transform.Rotate(new Vector3(0,180,0));
        } else {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}
