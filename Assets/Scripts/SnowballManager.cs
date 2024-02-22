using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballManager : MonoBehaviour
{
    // public AudioClip[] hitSounds;
    // private AudioSource audioSource;
    // [SerializeField] private GameObject head;
    // [SerializeField] private GameObject body;
    // [SerializeField] private GameObject base;

    // private void Awake()
    // {
    //     audioSource = GetComponent<AudioSource>(); // Get the Audio Source component
    // }
    [SerializeField] private ScoreManager scoreManager;
    public ParticleSystem particleSystem;
    public GameObject ball;

    public void HandleHit(Collider bodyPartTag)
    {
        int scoreValue = 0;
        switch (bodyPartTag.tag)
        {
            case "Head":
                Debug.Log("head hit");
                scoreValue = 100;
                break;
            case "Body":
                scoreValue = 50;
                break;
            case "Base":
                scoreValue = 25;
                break;
        }

        scoreManager.AddScore(scoreValue);

        Debug.Log("target hit");
        Debug.Log(scoreValue);

        // Optional: Implement feedback like a sound effect or visual effect

        // sound effect on hit
        // AudioClip hitSound = hitSounds[Random.Range(0, hitSounds.Length)];
        // audioSource.PlayOneShot(hitSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other: " + other);
        Debug.Log("other.tag: " + other.tag);
        Debug.Log("TRIGGER ENTER");
        if (other.CompareTag("Head") || other.CompareTag("Body") || other.CompareTag("Base")) {
            var emission = particleSystem.emission;
            var duration = particleSystem.duration;
            emission.enabled = true;
            Debug.Log("Playing particles");
            particleSystem.Play();
            
            HandleHit(other);
            
            // Destroy(gameObject);
            // Destroy(ball);
            // ball.SetActive(false);
            Invoke("DestroyGameObj", 0.07f);

        }
        // particleSystem.Play();
        // Destroy(gameObject);
    }

    void DestroyGameObj() {
        Destroy(gameObject);
    }
}
