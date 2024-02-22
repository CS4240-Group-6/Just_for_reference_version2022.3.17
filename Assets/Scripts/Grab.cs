using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public OVRInput.Controller Controller;
    public string grabButtonName;
    public string throwButtonName;
    public float grabRadius;
    public LayerMask grabMask;

    private GameObject grabbedObject;
    private bool grabbing;
    private Vector3 lastPosition;
    private Vector3 throwDirection;

    public AudioClip grabAudio;
    public AudioClip throwAudio;
    private AudioSource audioSource;

    private bool canSpawn = false;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private GameObject snowballPrefab;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Get the Audio Source component
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 controllerVelocity = (currentPosition - lastPosition) / Time.deltaTime;
        lastPosition = currentPosition;

        if (!grabbing && Input.GetAxis(grabButtonName) == 1)
        {
            if (canSpawn) {
                SpawnSnowball();
            }
            GrabObject();
        }
        if (grabbing && Input.GetAxis(grabButtonName) < 1)
        {
            ThrowObject(controllerVelocity);
        }
        if (IsHandBehindHead()) {
            canSpawn = true;
        } else {
            canSpawn = false;
        }

    }

    bool IsHandBehindHead()
    {
        Vector3 handPosition = transform.position;
        Vector3 headPosition = camera.transform.position;

        Vector3 headToHand = handPosition - headPosition;
        Vector3 headDirection = camera.transform.forward;

        float dotProduct = Vector3.Dot(headToHand.normalized, headDirection);

        // Debug.Log("dotProduct: " + dotProduct);

        // Adjust the threshold as needed
        return dotProduct < -0.5f;
    }

    void SpawnSnowball()
    {
        // Debug.Log("Spawning snowball");
        Instantiate(snowballPrefab, leftHandTransform.position, leftHandTransform.rotation);
    }

    void GrabObject()
    {
        // Debug.Log("Grabbing obj");
        grabbing = true;

        RaycastHit[] hits;

        hits = Physics.SphereCastAll(transform.position, grabRadius, transform.forward, 0f, grabMask);

        if (hits.Length > 0)
        {
            int closestHit = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if ((hits[i].distance < hits[closestHit].distance))
                {
                    closestHit = i;
                }
            }

            grabbedObject = hits[closestHit].transform.gameObject;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
            grabbedObject.transform.position = transform.position;
            grabbedObject.transform.parent = transform;

            // sound effect on grab
            audioSource.PlayOneShot(grabAudio);
        }
    }

    void ThrowObject(Vector3 controllerVelocity)
    {
        grabbing = false;

        if (grabbedObject != null)
        {
            grabbedObject.transform.parent = null;
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;

            // Calculate throw direction based on the controller's velocity
            throwDirection = controllerVelocity.normalized;

            // Apply throw force
            grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * controllerVelocity.magnitude * 5f;

            grabbedObject = null;

            // sound effect on throw
            audioSource.PlayOneShot(throwAudio);
        }
    }
}