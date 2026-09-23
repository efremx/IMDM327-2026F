// IMDM327 Material
// Use CSV or JSON to load data into the simulation. Both formats are supported, but they use different data types. 
// The CSV format uses a struct, while the JSON format uses a class. This script demonstrates how to load both formats and access their data.
using UnityEngine;
public class SolarSystem_2 : MonoBehaviour
{
    // These components can be attached independently.
    DataCSV solarCSV;
    DataJSON solarJSON;
    const float G = 6.674e-11f; // Gravitational constant
    PlanetProperty[] planetProperties;
    private int numberOfSphere = 10;
    class PlanetProperty // why struct?
    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct
        public GameObject planet;
        public float mass;
        public float radius;
        public Vector3 velocity;
        public Vector3 acceleration;
        public Vector3 actualPosition;
    }

    // CSV data and JSON data use their own data types.
    public BodyProperty[] solarBodiesCSV;
    // public SolarBody[] solarBodiesJSON;

    // Both loader scripts finish reading their files in Awake().
    void Start()
    {
        // CSV: use this block when a DataCSV component is attached.
        solarCSV = GetComponent<DataCSV>();
        if (solarCSV != null)
        {
            solarBodiesCSV = solarCSV.bp;
            Debug.Log("Loaded " + solarBodiesCSV.Length + " bodies from solar.csv.");
            Debug.Log("First body: mass = " + solarBodiesCSV[0].mass + ", distance = " + solarBodiesCSV[0].distance + ", initial_velocity = " + solarBodiesCSV[0].initial_velocity);
        }

        // JSON: use this block when a DataJSON component is attached.
        // solarJSON = GetComponent<DataJSON>();
        // if (solarJSON != null)
        // {
        //     solarBodiesJSON = solarJSON.solarData.bodies;
        //     Debug.Log("Loaded " + solarBodiesJSON.Length + " bodies from solar.json.");
        //     Debug.Log("First body: " + solarBodiesJSON[0].name + ", mass: " + solarBodiesJSON[0].mass);
        // }


        // GameObject array to hold the planets in the simulation.
        planetProperties = new PlanetProperty[numberOfSphere];
        for (int i = 0; i < numberOfSphere; i++)
        {
            // Our gameobjects are created here:
            planetProperties[i] = new PlanetProperty();
            planetProperties[i].planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }

        // Apply the loaded data to the simulation. This is where you would set up your bodies in the scene based on the loaded data.
        for (int i = 0; i < solarBodiesCSV.Length; i++)
        {
            planetProperties[i].mass = solarBodiesCSV[i].mass;
            planetProperties[i].radius = solarBodiesCSV[i].radius;

            // What is missing here? You need to set the initial position and velocity of each planet based on the loaded data.
            // ***WRITE YOUR CODE HERE***


        }
    }
    void Update()
    {
        // Loop for N-body gravity
        // How should we design the loop?

        // 00. Initialize the acceleration for each body to zero at the start of each frame
        for (int i = 0; i < numberOfSphere; i++)
        {
            // ***WRITE YOUR CODE HERE***
        }
        // 01. Loop through each body to calculate the gravitational forces acting on it
        for (int i = 0; i < numberOfSphere; i++)
        {
            // ***WRITE YOUR CODE HERE***
            // for ( int j...)
        }
        // 02. Loop through each body to update its velocity and position based on the calculated acceleration
        for (int i = 0; i < numberOfSphere; i++)
        {
            // ***WRITE YOUR CODE HERE***


            // Scale: float scaledDistance = Mathf.Sqrt(actualPosition[i].magnitude / 1e8f);

        }
    }

    // Gravity Fuction to finish
    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)
    {
        Vector3 gravity = Vector3.zero; // note this is also Vector3
        gravity = G * m1 * m2 / (distanceVector.sqrMagnitude) * distanceVector.normalized;
        return gravity;
    }
}
