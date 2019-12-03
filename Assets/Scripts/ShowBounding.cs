using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBounding : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(1000, 0, 0));
        Gizmos.DrawLine(new Vector3(1000, 0, 0), new Vector3(1000, 0, 1000));
        Gizmos.DrawLine(new Vector3(0, 0, 0), new Vector3(0, 0, 1000));
        Gizmos.DrawLine(new Vector3(0, 0, 1000), new Vector3(1000, 0, 1000));
    }
}
