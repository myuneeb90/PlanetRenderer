using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerScript : MonoBehaviour
{
	// Singleton Code
    private static DebuggerScript _Instance = null;
    public static DebuggerScript Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = (DebuggerScript)FindObjectOfType(typeof(DebuggerScript));
            }

            return _Instance;
        }
    }

    public GameObject SpherePrefab;

    public List<Vector3> TopGridPositions;

    public List<Vector3> RightGridPositions;

    public void Spawn()
    {
        for(int i = 0; i < TopGridPositions.Count; i++)
        {
            GameObject point = Instantiate(SpherePrefab, TopGridPositions[i], Quaternion.identity);
            point.name = "TopGrid_" + i.ToString();
        }

        for(int i = 0; i < RightGridPositions.Count; i++)
        {
            GameObject point = Instantiate(SpherePrefab, RightGridPositions[i], Quaternion.identity);
            point.name = "RightGrid_" + i.ToString();
        }
    }
}
