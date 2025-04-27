using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Vector2 pixelDimensions;
    public float pixelSize;
    public Vector2 topCorner;

    public int treshhold;
    public GameObject pixel;


    void Start()
    {
        for (int i = 0; i < pixelDimensions.x; i++)
        {
            for (int j = 0; j < pixelDimensions.y; j++)
            {

                Vector2 input = new Vector2(topCorner.x + pixelSize*i,topCorner.y - pixelSize * j);
                Vector2 output = new Vector2(0, 0);
                for (int count = 0; count < treshhold; count++)
                {
                    output = new Vector2(output.x * output.x - output.y * output.y,  2 * output.x * output.y);
                }

                if (Mathf.Abs(output.x) < 10 && Mathf.Abs(output.y) < 10)
                {
                    Instantiate(pixel, input, Quaternion.identity);
                }
            }
        }  
    }
}
