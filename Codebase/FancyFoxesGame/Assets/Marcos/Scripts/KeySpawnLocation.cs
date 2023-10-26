using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawnLocation : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, transform.position.z),
            new Vector3(0.5f, 0.5f, 0.5f));
    }
}
