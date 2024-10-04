using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class LightCollider : MonoBehaviour
{
    private Light light;
    [SerializeField] GameObject sphere;
    GameObject sphereSphere;

    public GameObject Sphere => sphereSphere; 

    public LightCollider()
    {

    }

    public void Initialize()
    {
        light = GetComponent<Light>();
        
        sphere.layer = 6;

        if (light != null) 
        {
            switch (light.type)
            {
                case LightType.Spot:
                    RaycastHit myHit;
                    //SphereCollider spotCollider = this.AddComponent<SphereCollider>();
                    if (Physics.Raycast(transform.position, transform.forward, out myHit, light.range, 1))
                    {
                        sphereSphere = Instantiate(sphere, myHit.point, Quaternion.identity);
                        sphereSphere.GetComponent<MeshRenderer>().enabled = false;
                        SphereCollider spotCollider = sphereSphere.GetComponent<SphereCollider>();
                        //Vector3 relPos = hit.point;
                        //spotCollider.center = relPos;
                        spotCollider.radius = (light.innerSpotAngle * math.PI / 180) + (math.PI / 4) * (light.intensity / 5);
                        spotCollider.isTrigger = true;
                    }
                    break;

                case LightType.Point:
                    SphereCollider pointCollider = this.AddComponent<SphereCollider>();
                    pointCollider.radius = light.range / 2 + (light.intensity) / 4;
                    pointCollider.isTrigger = true;
                    break;
            }
        }
    }

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        
    }
}
