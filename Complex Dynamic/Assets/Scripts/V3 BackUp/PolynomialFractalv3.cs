using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolynomialFractalv3 : MonoBehaviour
{
    // Prostředí
    public double width;
    public double height;

    public double sPosX;
    public double sPosY;

    public int maxIter = 500;
    public int step;

    // Manipulace
    public float zoom;

    // Compute Shader
    public ComputeShader shader;
    ComputeBuffer buffer;
    RenderTexture calcResult;
    public RawImage display;

    //Přenos Dat z Shaderu na GPU
    public struct Datastruct
    {
        public double w;
        public double h;
        public double r;
        public double i;
        public int disW;
        public int disH;
    }

    Datastruct[] data;

    
    
    // Start is called before the first frame update
    void Start()
    {

        // Vytvoříme balíček dat typu struct o délce 1
        data = new Datastruct[1];

        // Naplníme balíiček dat našemi hodnotami
        data[0] = new Datastruct
        {
            w = width,
            h = height,
            r = sPosX,
            i = sPosY,
            disH = Screen.height,
            disW = Screen.width
        };

        Debug.Log(height);

        // Do bufferu načteme data z proměnné data
        // Proměnná typu double je 8 bytů, proměnná typu int je velká pouze 4 byty
        // 8*4+4*2 = 40 (proto 40 bytů)
        buffer = new ComputeBuffer(data.Length, 40);
        calcResult = new RenderTexture(Screen.width, Screen.height, 0);
        calcResult.enableRandomWrite = true;
        calcResult.Create();

        MandelBrot();
    }

    void MandelBrot()
    {
        int kernelHandle = shader.FindKernel("CSMain");
        buffer.SetData(data);
        shader.SetBuffer(kernelHandle, "buffer", buffer);

        shader.SetInt("maxIter",maxIter);
        shader.SetTexture(kernelHandle, "Result", calcResult);

        shader.Dispatch(kernelHandle, Screen.width / 24, Screen.height / 24, 1);

        RenderTexture.active = calcResult;
        display.material.mainTexture = calcResult;
    }

    private void OnDestroy()
    {
        buffer.Dispose();
    }
}
