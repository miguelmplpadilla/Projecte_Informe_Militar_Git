using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RoadGeneration : MonoBehaviour
{
    [SerializeField] private GameObject[] roads;

    [SerializeField] private GameObject roadAnterior;
    [SerializeField] private GameObject roadActual;

    [SerializeField] private float distanciaGenerado = 5;

    public float tiempoNivel = 60;
    public float temporizador = 0;

    private Slider slider;

    private void Start()
    {
        slider = GameObject.Find("SliderCarretera").GetComponent<Slider>();
    }

    private void Update()
    {
        if (temporizador >= tiempoNivel)
        {
            Debug.Log("Nivel Terminado");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        temporizador += Time.deltaTime;

        slider.value = temporizador / tiempoNivel;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Road"))
        {
            if (roadAnterior)
            {
                Destroy(roadAnterior);
            }

            GameObject roadGenerada = Instantiate(roads[Random.Range(0, roads.Length)]);

            roadGenerada.transform.position = new Vector3(0, roadActual.transform.position.y + distanciaGenerado, 0);

            roadAnterior = roadActual;
            roadActual = roadGenerada;
        }
    }
}