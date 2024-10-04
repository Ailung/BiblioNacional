using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public bool alive = true;
    public LightCollider collider;
    public int health;
    public int tensionCounter = 0;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] float timeToDamage;
    [SerializeField] float tensionTime;
    [SerializeField] int tensionQuantity;
    [SerializeField] float timeToHeal;
    [SerializeField] GameObject EndPasilloPuertaAbierta;
    [SerializeField] GameObject EndPasilloPuertaCerrada;
    [SerializeField] GameObject HabitacionPuertaAbierta;
    [SerializeField] GameObject HabitacionPuertaCerrada;
    [SerializeField] GameObject TriggerPasilloAgain;
    [SerializeField] GameObject TriggerTensionPasillo;
    [SerializeField] GameObject BlackLight;
    [SerializeField] GameObject TensionPosition;
    [SerializeField] GameObject camera;
    float timerDamage;
    float timerHeal;
    private void OnTriggerStay(Collider other)
    {
        collider = other.GetComponent<LightCollider>();
        if (collider.CompareTag("Lights"))
        {
            alive = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EndPasillo"))
        {
            EndPasilloPuertaAbierta.gameObject.SetActive(false);
            EndPasilloPuertaCerrada.gameObject.SetActive(true);
            HabitacionPuertaCerrada.gameObject.SetActive(false);
            HabitacionPuertaAbierta.gameObject.SetActive(true);
            TriggerPasilloAgain.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("AgainPasillo"))
        {
            HabitacionPuertaCerrada.gameObject.SetActive(true);
            TriggerTensionPasillo.gameObject.SetActive(true);
            HabitacionPuertaAbierta.gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("TensionTrigger"))
        {
            if (tensionCounter+1 == tensionQuantity)
            {
                other.gameObject.SetActive(false);
                gameObject.GetComponent<FirstPersonMovement>().speed = 4;
                gameObject.GetComponent<FirstPersonMovement>().canRun = true;
                camera.transform.Rotate(new Vector3(0, 0, camera.transform.rotation.eulerAngles.z * -1));
            } else
            {
                StartCoroutine(tensionGenerator());
                tensionCounter++;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collider = null;
    }

    public void Initialize()
    {
        health = 100;
        textHealth.text = "Health: " + health;
    }

    public void getDamage()
    {
        health--;
        textHealth.text = "Health: " + health;

        if (health < 0)
        {
            BlackLight.gameObject.SetActive(true);
        }
    }

    public IEnumerator tensionGenerator()
    {
        var lastSpeed = gameObject.GetComponent<FirstPersonMovement>().speed;
        gameObject.GetComponent<FirstPersonMovement>().speed = 0;
        gameObject.GetComponent<FirstPersonMovement>().canRun = false;
        BlackLight.gameObject.SetActive(true);
        gameObject.transform.position = TensionPosition.transform.position;
        yield return new WaitForSeconds(tensionTime);

        camera.transform.Rotate(new Vector3(0, 0, 1));

        if (lastSpeed <= 1)
        {
            gameObject.GetComponent<FirstPersonMovement>().speed = lastSpeed;
            BlackLight.gameObject.SetActive(false);
        } else
        {
            gameObject.GetComponent<FirstPersonMovement>().speed = lastSpeed - 1;
            BlackLight.gameObject.SetActive(false);
        }
        
    }

    public void getHeal()
    {
        health++;
        textHealth.text = "Health: " + health;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (alive)
        {
            if (collider == null)
            {
                alive = false;
                timerHeal = 0;
            }
            if (timerHeal > timeToHeal && health < 100)
            {
                getHeal();
                timerHeal = 0;
            }
            else
            {
                timerHeal += Time.deltaTime;
            }
        }
        else 
        {
            if (collider != null)
            {
                alive = true;
                timerHeal = 0;
            }
            if (timerDamage > timeToDamage)
            {
                getDamage();
                timerDamage = 0;
            } else
            {
                timerDamage += Time.deltaTime;
            }
            
        }
    }
}
