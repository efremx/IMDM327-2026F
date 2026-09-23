// 3-body Starter Code
// Fall 2026. IMDM 327
// Instructor. Myungin Lee
using UnityEngine;

public class ThreeBody_2 : MonoBehaviour
{
    private const float G = 500f; // Gravitational constant for this simulation, not the real-world value.
    BodyProperty[] bp;
    private int numberOfSphere = 25;
    class BodyProperty // why struct?
    {                   // https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct
        public GameObject body;
        public float mass;
        public Vector3 velocity;
        public Vector3 acceleration;
    }


    void Start()
    {
        // Allocate an array to store each body's properties.
        bp = new BodyProperty[numberOfSphere];
        // Loop generating the gameobject and assign initial conditions (type, position, (mass/velocity/acceleration)
        for (int i = 0; i < numberOfSphere; i++)
        {
            // Our gameobjects are created here:
            bp[i] = new BodyProperty();
            bp[i].body = GameObject.CreatePrimitive(PrimitiveType.Sphere); // why sphere? try different options.
            // https://docs.unity3d.com/ScriptReference/GameObject.CreatePrimitive.html

            // initial conditions
            float r = 100f;
            // position is (x,y,z). In this case, I want to plot them on the circle with r

            float angle = i * 2f * Mathf.PI / numberOfSphere;

            float x = r * Mathf.Cos(angle);
            float y = r * Mathf.Sin(angle);

            // ******** Fill in this part ********
            // bp[i].body.transform.position = new Vector3( ***, *** , 180);
            // z = 180 places the bodies in front of a camera near the origin looking along +Z. Try other positions too.

            bp[i].body.transform.position =
            new Vector3(x, y, 180f);

            float speed = 7f;

            // Tangential velocity around the circle
            bp[i].velocity = new Vector3(
                -Mathf.Sin(angle) * speed,
                Mathf.Cos(angle) * speed,
                0f
            );

            bp[i].acceleration = Vector3.zero;
            bp[i].mass = 1f;


            // + This is just pretty trails
            TrailRenderer trailRenderer = bp[i].body.AddComponent<TrailRenderer>();
            // Configure the TrailRenderer's properties
            trailRenderer.time = 100.0f;  // Duration of the trail
            trailRenderer.startWidth = 0.5f;  // Width of the trail at the start
            trailRenderer.endWidth = 0.1f;    // Width of the trail at the end
            // a material to the trail
            trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
            // Set the colour gradient along the trail.
            Gradient gradient = new Gradient();
            Color targetColor = Color.HSVToRGB((float)i / numberOfSphere, 1f, 1f);
            gradient.SetKeys(
                new GradientColorKey[] {
                    new GradientColorKey(Color.white, 0f), // (color, normalized position)
                    new GradientColorKey(targetColor, 0.8f)
                },
                new GradientAlphaKey[] {
                    new GradientAlphaKey(1f, 0f), // (alpha, normalized position) 
                    new GradientAlphaKey(0f, 1f)
                }
            );
            trailRenderer.colorGradient = gradient;

        }
    }

    void Update()
    {
        for (int i = 0; i < numberOfSphere; i++)
        {
            bp[i].acceleration = Vector3.zero;

            for (int j = 0; j < numberOfSphere; j++)
            {
                // A body should not attract itself
                if (i == j)
                    continue;

                Vector3 distanceVector =
                    bp[j].body.transform.position -
                    bp[i].body.transform.position;

                Vector3 gravityForce =
                    CalculateGravity(
                        distanceVector,
                        bp[i].mass,
                        bp[j].mass
                    );

                // F = ma, so a = F / m
                bp[i].acceleration +=
                    gravityForce / bp[i].mass;
            }
        }

        // Then update velocity and position
        for (int i = 0; i < numberOfSphere; i++)
        {
            bp[i].velocity +=
                bp[i].acceleration * Time.deltaTime * 3;

            bp[i].body.transform.position +=
                bp[i].velocity * Time.deltaTime * 3;
        }

    }

    // Gravity Fuction to finish
    private Vector3 CalculateGravity(Vector3 distanceVector, float m1, float m2)
    {
        float distance = distanceVector.magnitude;

        // Avoid division by zero
        if (distance < 0.01f)
            return Vector3.zero;

        // Newton's law of gravitation:
        // F = G * m1 * m2 / r^2
        float forceMagnitude =
            G * m1 * m2 / (distance * distance);

        Vector3 direction =
            distanceVector.normalized;

        Vector3 gravity =
            direction * forceMagnitude;

        return gravity;
    }
}

