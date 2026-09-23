using UnityEngine;
public class SolarSystem_2 : MonoBehaviour
{
    DataCSV solarCSV;
    DataJSON solarJSON;
    const float G = 6.674e-11f;
    PlanetProperty[] planetProperties;
    private int numberOfSphere = 10;

    [SerializeField] float positionScale = 1e-9f; // ONLY this scales for display 

    class PlanetProperty
    {
        public GameObject planet;
        public float mass;
        public float radius;
        public Vector3 velocity;
        public Vector3 acceleration;
        public Vector3 actualPosition;
    }

    public BodyProperty[] solarBodiesCSV;

    void Start()
    {
        solarCSV = GetComponent<DataCSV>();
        if (solarCSV != null)
        {
            solarBodiesCSV = solarCSV.bp;
            Debug.Log("Loaded " + solarBodiesCSV.Length + " bodies from solar.csv.");
        }

        planetProperties = new PlanetProperty[numberOfSphere];
        for (int i = 0; i < numberOfSphere; i++)
        {
            planetProperties[i] = new PlanetProperty();
            planetProperties[i].planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            TrailRenderer trailRenderer = planetProperties[i].planet.AddComponent<TrailRenderer>();
            trailRenderer.time = 100.0f;
            trailRenderer.startWidth = 0.05f;
            trailRenderer.endWidth = 0.01f;
            trailRenderer.minVertexDistance = 0.001f; // smaller = smoother curve, but more vertices
            trailRenderer.numCapVertices = 4;         // rounds the trail ends instead of sharp cutoffs
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));

            Gradient gradient = new Gradient();
            Color targetColor = Color.HSVToRGB((float)i / numberOfSphere, 1f, 1f);
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(targetColor, 0.8f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            trailRenderer.colorGradient = gradient;
        }

        for (int i = 0; i < solarBodiesCSV.Length; i++)
        {
            planetProperties[i].mass = solarBodiesCSV[i].mass;
            planetProperties[i].radius = solarBodiesCSV[i].radius;

            float distance = solarBodiesCSV[i].distance;
            float a = Random.Range(0, 2 * Mathf.PI);
            float x = Mathf.Sin(a) * distance;
            float y = Mathf.Cos(a) * distance;
            planetProperties[i].actualPosition = new Vector3(x, y, 0);
            planetProperties[i].planet.transform.position = planetProperties[i].actualPosition * positionScale; // scale ONLY here

            Vector3 velocityDir = new Vector3(-y, x, 0).normalized;
            planetProperties[i].velocity = velocityDir * solarBodiesCSV[i].initial_velocity;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < numberOfSphere; i++)
            planetProperties[i].acceleration = Vector3.zero;

        for (int i = 0; i < numberOfSphere; i++)
        {
            for (int j = 0; j < numberOfSphere; j++)
            {
                if (i == j) continue;
                Vector3 distanceVector = planetProperties[j].actualPosition - planetProperties[i].actualPosition;
                Vector3 force = CalculateGravity(distanceVector, planetProperties[i].mass, planetProperties[j].mass);
                planetProperties[i].acceleration += force / planetProperties[i].mass;
            }
        }

        for (int i = 0; i < numberOfSphere; i++)
        {
            planetProperties[i].velocity += planetProperties[i].acceleration * Time.fixedDeltaTime * 86400 * 180;
            planetProperties[i].actualPosition += planetProperties[i].velocity * Time.fixedDeltaTime * 86400 * 180;
            planetProperties[i].planet.transform.position = planetProperties[i].actualPosition * positionScale; // scale ONLY here
        }
    }

    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)
    {
        float sqrDist = Mathf.Max(distanceVector.sqrMagnitude, 1f); // avoid divide-by-zero at real scale
        float forceMag = G * (m1 / sqrDist) * m2; // divide before multiplying — avoids float overflow
        return forceMag * distanceVector.normalized;
    }
}