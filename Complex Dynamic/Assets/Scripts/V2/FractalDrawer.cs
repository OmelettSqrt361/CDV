using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class FractalDrawer : MonoBehaviour
{

	public float span;
	public UnityEngine.Vector2 startPos;

	double height, width;
	double rStart, iStart;
	int iterations;
	int zoom;

	Texture2D display;
	public Image image;

	public float[] coefficients;

	void Start()
	{
		// width 4,5 represents the imaginary axis
		// height is proportional to the width, so we don't have to change it that much
		width = span;
		height = width * Screen.height / Screen.width;

		rStart = startPos.x;
		iStart = startPos.y;
		zoom = 10;
		iterations = 40;

		display = new Texture2D(Screen.width, Screen.height);

		RunFractal();
	}

	void RunFractal()
    {
		for (int x = 0; x != display.width; x++)
		{
			for (int y = 0; y != display.height; y++)
			{
				display.SetPixel(x, y, SetColor(FractalGenerator(rStart + width * (double)x / display.width,
					iStart + height * (double)y / display.height)));
			}
		}

		display.Apply();
		image.sprite = Sprite.Create(display, new Rect(0, 0, display.width, display.height),
			new UnityEngine.Vector2(0.5f, 0.5f));
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			rStart = rStart + (Input.mousePosition.x - (Screen.width / 2.0)) / Screen.width * width;
			iStart = iStart + (Input.mousePosition.y - (Screen.height / 2.0)) / Screen.height * height;
			RunFractal();
		}

		if (Input.mouseScrollDelta.y != 0)
		{
			double wFactor = width * (double)Input.mouseScrollDelta.y / zoom;
			double hFactor = height * (double)Input.mouseScrollDelta.y / zoom;
			width -= wFactor;
			height -= hFactor;
			rStart += wFactor / 2.0;
			iStart += hFactor / 2.0;

			RunFractal();
		}

	}

	int FractalGenerator(double x, double y)
	{
		int heat = 0;

		Complex c = new Complex(x, y);
		Complex z = new Complex(0, 0);

		for (int i = 0; i < iterations; i++)
		{
			z = PolynomialParser(z, c);

			if (Complex.Abs(z) > 2)
			{
				return i;
			}
			else
			{
				heat++;
			}
		}
		return heat;
	}

	Complex PolynomialParser(Complex z, Complex c)
    {
		Complex result = new Complex(0, 0);
        for (int i = 0; i < coefficients.Length; i++)
        {
			result += Complex.Pow(z, i) * coefficients[i];
        }
		result += c;
		return result;
    }

	Color SetColor(int value)
	{
		UnityEngine.Vector4 CalcColor = new UnityEngine.Vector4(0, 0, 0, 1f);

		if (value != iterations)
		{
			int colorNr = value % 16;

			switch (colorNr)
			{
				case 0:
					{
						CalcColor[0] = 66.0f / 255.0f;
						CalcColor[1] = 30.0f / 255.0f;
						CalcColor[2] = 15.0f / 255.0f;

						break;
					}
				case 1:
					{
						CalcColor[0] = 25.0f / 255.0f;
						CalcColor[1] = 7.0f / 255.0f;
						CalcColor[2] = 26.0f / 255.0f;
						break;
					}
				case 2:
					{
						CalcColor[0] = 9.0f / 255.0f;
						CalcColor[1] = 1.0f / 255.0f;
						CalcColor[2] = 47.0f / 255.0f;
						break;
					}

				case 3:
					{
						CalcColor[0] = 4.0f / 255.0f;
						CalcColor[1] = 4.0f / 255.0f;
						CalcColor[2] = 73.0f / 255.0f;
						break;
					}
				case 4:
					{
						CalcColor[0] = 0.0f / 255.0f;
						CalcColor[1] = 7.0f / 255.0f;
						CalcColor[2] = 100.0f / 255.0f;
						break;
					}
				case 5:
					{
						CalcColor[0] = 12.0f / 255.0f;
						CalcColor[1] = 44.0f / 255.0f;
						CalcColor[2] = 138.0f / 255.0f;
						break;
					}
				case 6:
					{
						CalcColor[0] = 24.0f / 255.0f;
						CalcColor[1] = 82.0f / 255.0f;
						CalcColor[2] = 177.0f / 255.0f;
						break;
					}
				case 7:
					{
						CalcColor[0] = 57.0f / 255.0f;
						CalcColor[1] = 125.0f / 255.0f;
						CalcColor[2] = 209.0f / 255.0f;
						break;
					}
				case 8:
					{
						CalcColor[0] = 134.0f / 255.0f;
						CalcColor[1] = 181.0f / 255.0f;
						CalcColor[2] = 229.0f / 255.0f;
						break;
					}
				case 9:
					{
						CalcColor[0] = 211.0f / 255.0f;
						CalcColor[1] = 236.0f / 255.0f;
						CalcColor[2] = 248.0f / 255.0f;
						break;
					}
				case 10:
					{
						CalcColor[0] = 241.0f / 255.0f;
						CalcColor[1] = 233.0f / 255.0f;
						CalcColor[2] = 191.0f / 255.0f;
						break;
					}
				case 11:
					{
						CalcColor[0] = 248.0f / 255.0f;
						CalcColor[1] = 201.0f / 255.0f;
						CalcColor[2] = 95.0f / 255.0f;
						break;
					}
				case 12:
					{
						CalcColor[0] = 255.0f / 255.0f;
						CalcColor[1] = 170.0f / 255.0f;
						CalcColor[2] = 0.0f / 255.0f;
						break;
					}
				case 13:
					{
						CalcColor[0] = 204.0f / 255.0f;
						CalcColor[1] = 128.0f / 255.0f;
						CalcColor[2] = 0.0f / 255.0f;
						break;
					}
				case 14:
					{
						CalcColor[0] = 153.0f / 255.0f;
						CalcColor[1] = 87.0f / 255.0f;
						CalcColor[2] = 0.0f / 255.0f;
						break;
					}
				case 15:
					{
						CalcColor[0] = 106.0f / 255.0f;
						CalcColor[1] = 52.0f / 255.0f;
						CalcColor[2] = 3.0f / 255.0f;
						break;
					}
			}
		}
		return CalcColor;
	}
}
