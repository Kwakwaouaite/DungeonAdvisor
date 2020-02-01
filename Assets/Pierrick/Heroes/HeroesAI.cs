using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Move(List<Vector2Int> path, UIGrid room, float Speed)
    {

        float wRatio = room.getCellW();
        float hRatio = room.getCellH();

        for (int i = 1; i < path.Count; i++ )
        {
            Vector2Int segment = path[i];


            Vector2 direction = path[i] - path[i - 1];
            Vector2 currentPosition = path[i - 1];

            float currentPercentage = 0;
            while (currentPercentage < 1)
            {
                float deltaPercentage = Time.deltaTime / Speed;
                currentPercentage += deltaPercentage;
                currentPosition += direction * deltaPercentage;
                gameObject.transform.position = room.transform.position + new Vector3(currentPosition.x * wRatio, currentPosition.y * hRatio);
                yield return null;
            }
        }
    }

}
