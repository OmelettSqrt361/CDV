using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FullPolynomial : MonoBehaviour
{
    // Prostředí
    public double width;
    double height;

    public double sPosX;
    public double sPosY;

    public int maxIter = 500;
    public int step;

    // Maximální počet koeficientů je 16
    float[] coefficients = { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

    // Manipulace
    public double zoom;

    // Compute Shader
    public ComputeShader shader;
    ComputeBuffer buffer;
    ComputeBuffer coefBuffer;

    RenderTexture calcResult;
    public RawImage display;

    public bool isMenuOnline = false;

    // TMP
    public TMP_InputField spanText;
    public TMP_InputField zoomText;
    public TMP_InputField iStart;
    public TMP_InputField rStart;
    public TMP_InputField coefText;

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

    // Inetraguje s balíčkem TetxMeshPro, aby převzala proměnné typu double
    public void InputData()
    {

        width = double.Parse(spanText.text);
        height = (width / Screen.width) * Screen.height;
        zoom = double.Parse(zoomText.text);
        sPosX = double.Parse(rStart.text);
        sPosY = double.Parse(iStart.text);

        //Tvorba množiny koeficientů
        string[] coefString = coefText.text.Split(';', ' ');

        for (int i = 0; i < 16; i++)
        {
            if (coefString.Length > i && coefString[i] != null && coefString[i] != "")
            {
                if (coefString[i].Contains("/"))
                {
                    string[] twoVars = coefString[i].Split('/');
                    coefficients[i] = float.Parse(twoVars[0]) / float.Parse(twoVars[1]);

                }
                else
                {
                    coefficients[i] = float.Parse(coefString[i]);
                }
            } else
            {
                coefficients[i] = 0.0f;
            }
        }

        data[0].w = width;
        data[0].h = height;
        data[0].r = sPosX;
        data[0].i = sPosY;

        Fractal();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetVars();

        height = (width / Screen.width) * Screen.height;

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

        // Do bufferu načteme data z proměnné data
        // Proměnná typu double je 8 bytů, proměnná typu int je velká pouze 4 byty
        // 8*4+4*2 = 40 (proto 40 bytů)
        buffer = new ComputeBuffer(data.Length, 40);
        coefBuffer = new ComputeBuffer(coefficients.Length, sizeof(float));

        calcResult = new RenderTexture(Screen.width, Screen.height, 0);
        calcResult.enableRandomWrite = true;
        calcResult.Create();

        Fractal();
    }

    void CenterScreen()
    {
        sPosX += (Input.mousePosition.x - (Screen.width / 2.0)) / Screen.width * width;
        sPosY += (Input.mousePosition.y - (Screen.height / 2.0)) / Screen.height * height;

        data[0].r = sPosX;
        data[0].i = sPosY;

        ResetVars();
        Fractal();
    }

    void ZoomIn()
    {
        maxIter = Mathf.Max(100, maxIter + step);
        double wFactor = width * zoom * Time.deltaTime;
        double hFactor = height * zoom * Time.deltaTime;
        height -= hFactor;
        width -= wFactor;
        sPosX += wFactor / 2;
        sPosY += hFactor / 2;

        data[0].w = width;
        data[0].h = height;
        data[0].r = sPosX;
        data[0].i = sPosY;

        ResetVars();
        Fractal();
    }

    void ZoomOut()
    {
        maxIter = Mathf.Max(100, maxIter - step);
        double wFactor = width * zoom * Time.deltaTime;
        double hFactor = height * zoom * Time.deltaTime;
        height += hFactor;
        width += wFactor;
        sPosX -= wFactor / 2;
        sPosY -= hFactor / 2;

        data[0].w = width;
        data[0].h = height;
        data[0].r = sPosX;
        data[0].i = sPosY;

        ResetVars();
        Fractal();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            ZoomIn();
        } else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            ZoomOut();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CenterScreen();
        }
    }

    void Fractal()
    {
        int kernelHandle = shader.FindKernel("CSMain");
        coefBuffer.SetData(coefficients);
        buffer.SetData(data);

        shader.SetBuffer(kernelHandle, "coefBuffer", coefBuffer);
        shader.SetBuffer(kernelHandle, "buffer", buffer);

        shader.SetInt("maxIter", maxIter);
        shader.SetTexture(kernelHandle, "Result", calcResult);

        shader.Dispatch(kernelHandle, Screen.width / 20, Screen.height / 20, 1);

        RenderTexture.active = calcResult;
        display.material.mainTexture = calcResult;
    }

    private void OnDestroy()
    {
        buffer.Dispose();
        coefBuffer.Dispose();
    }

    void ResetVars()
    {
        spanText.text = width.ToString();
        zoomText.text = zoom.ToString();
        rStart.text = sPosX.ToString();
        iStart.text = sPosY.ToString();
    }
}
