using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasParticles : MonoBehaviour {
    [SerializeField]
    private Transform[] bubblePositions;
    [SerializeField]
    private ParticleSystem bubbleParticleSystem;
    private ParticleSystem[] particles;
	// Use this for initialization
	void Start () {
        particles = new ParticleSystem[bubblePositions.Length];
        for (int i = 0; i < bubblePositions.Length; i++)
        {
            particles[i] = Instantiate(bubbleParticleSystem, bubblePositions[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].transform.position = Camera.main.ScreenToWorldPoint(bubblePositions[i].position);
        }
	}
}
