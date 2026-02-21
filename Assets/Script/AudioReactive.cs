using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReactive : MonoBehaviour
{
    public double timeElapsed;
    
    public Material unlit;
    public Material lit;
    GameObject[] spheres;
    public GameObject centerSphere;

    public GameObject model;

    public GameObject background;

    public double transition1_2;
    public double phaseTwoStart;
    public double phaseThreeStart;
    public double phaseFourStart;
    public double phaseFiveStart;
    public double phaseSixStart;
    static int numSphere = 100; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, circlePosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;


    // Start is called before the first frame update
    void Start()
    {
        
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere];
        // Define target positions. Start = random, End = heart 

        //centerSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        centerSphere = Instantiate(model, initPos[0], Quaternion.identity);
        
        Renderer centerRenderer = centerSphere.GetComponent<Renderer>();
        centerRenderer.material = lit;

        for (int i =0; i < numSphere; i++){
            int sign;
            if(i % 2 == 0)
            {
                sign = 1;
            } else
            {
                sign = -1;
            }
            // Random start positions
            float r = 10f * sign;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-2f, -1f) , r * Random.Range(-1f, 1f));
            endPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(1f, 2f) , r * Random.Range(-1f, 1f));        

            r = 3f; // radius of the circle
            // Circular end position, for some reason uncommenting it fucks up the loop for making spheres
            //circlePosition[i] = new Vector3(r * Mathf.Sin(i * 2 * Mathf.PI / numSphere), r * Mathf.Cos(i * 2 * Mathf.PI / numSphere));
        }
        // Let there be spheres..
        Debug.Log("running sphere loop");
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Debug.Log("making sphere");
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material = unlit;
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed = Time.realtimeSinceStartupAsDouble;
        float scale = 1f + AudioSpectrum.audioAmp;
        centerSphere.transform.localScale = new Vector3(scale *2, scale * 2 , scale * 2);
        // ***Here, we use audio Amplitude, where else do you want to use?
        // Measure Time 
        // Time.deltaTime = The interval in seconds from the last frame to the current one
        // but what if time flows according to the music's amplitude?    
        if(timeElapsed < transition1_2)
        {
            
            time += Time.deltaTime;
            // new comment
            
            for(int i = 0; i < numSphere; i++)
            {
                Vector3 center = new Vector3(0, 0, 0);
                // lerpFraction variable defines the point between startPosition and endPosition (0~1)
                lerpFraction = Mathf.Sin(time * 2) * 0.5f + 0.5f;

                // Lerp logic. Update position       
                t = i* 2 * Mathf.PI / numSphere;


                spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);

            }
        } else if (timeElapsed < phaseTwoStart)
        {
            time += Time.deltaTime;
            for(int i = 0; i <numSphere; i++)
            {
                spheres[i].transform.position = Vector3.MoveTowards(spheres[i].transform.position, Vector3.zero, Time.deltaTime * 9);
            }
        }
        
        /*
         
        centerSphere.transform.localScale = new Vector3(scale *2, scale * 2 , scale * 2);
        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            lerpFraction = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numSphere;

            spheres[i].transform.localScale = new Vector3(scale, 1f, 1f);

            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction);
            
            spheres[i].transform.Rotate(AudioSpectrum.audioAmp, 1f, 1f);
            
            
            
            
            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Cos(time)), Mathf.Cos(AudioSpectrum.audioAmp / 10f), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;

            
        }
        */
        
    }
}

