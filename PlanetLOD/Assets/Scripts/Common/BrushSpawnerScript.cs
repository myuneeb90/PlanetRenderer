using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushSpawnerScript : MonoBehaviour
{
    public List<GameObject> BrushPrefabContainer;
    private List<GameObject> BrushContainer;
    public bool Spawn = false;

    public float SpawnRadius = 5.0f;
    public int BrushCount = 100;

    public void SpawnBrushes()
    {
        BrushContainer = new List<GameObject>();
		for(int i = 0; i < BrushCount; i++)
		{
            int brushId = UnityEngine.Random.Range(0, BrushPrefabContainer.Count);

			GameObject newBrush = (GameObject)Instantiate(BrushPrefabContainer[brushId], UnityEngine.Random.onUnitSphere * SpawnRadius, Quaternion.identity);
			newBrush.transform.SetParent(this.transform);
			newBrush.transform.LookAt(this.transform);
            Vector3 rotation = newBrush.transform.localEulerAngles;
            rotation.z = UnityEngine.Random.Range(0, 360);
            newBrush.transform.localEulerAngles = rotation;

			float scale = 0;
            
            if(brushId == 0)
            {
                scale = UnityEngine.Random.Range(1.5f, 3.5f);
            }
            else
            if(brushId == 1)
            {
                scale = UnityEngine.Random.Range(0.8f, 3.0f);
            }
            else
            if(brushId == 2)
            {
                scale = UnityEngine.Random.Range(1.0f, 3.0f);
            }

			newBrush.transform.localScale = new Vector3(scale, scale, -scale);
            BrushContainer.Add(newBrush);
        }
    }
}
