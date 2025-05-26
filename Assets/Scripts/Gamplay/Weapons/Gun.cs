using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class Gun : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] protected int damage;
    [SerializeField][Range(0, 1)] private float timeBetweenShots = 0.10f;
    private float lastShotTime = 0f;

    [Header("Rumble")]
    [SerializeField] protected float duration;
    [SerializeField][Range(0, 1)] protected float lowFrequency;
    [SerializeField][Range(0, 1)] protected float highFrequency;

    [SerializeField] public Magazine magazine = new Magazine();

    private Rigidbody rb;
    private BoxCollider collitionBox;

    static private float dropForwardForce = 5;
    static private float dropUpwardForce = 3;

    private int dropedLayer;
    private int grabedLayer;

    protected Characters owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        collitionBox = GetComponent<BoxCollider>();

        dropedLayer = LayerMask.NameToLayer("Interactable");
        grabedLayer = LayerMask.NameToLayer("Weapon");
    }

    private void OnDisable()
    {
        magazine.CancelReload();
    }

    private void Update()
    {
        if (magazine.Reloading)
        {
            magazine.DoneReloading();
        }
    }

    protected abstract void OnFire();

    public void TryFire()
    {
        if (!magazine.IsEmpty() && !magazine.Reloading && Time.time - lastShotTime >= timeBetweenShots)
        {
            OnFire();
            lastShotTime = Time.time;
        }
    }

    public void TryReload()
    {
        if (!magazine.Reloading && !magazine.IsFull())
        {
            magazine.StartReload();
        }
    }

    public void Grab(Player owner, Transform hand)
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        rb.isKinematic = true;

        gameObject.layer = grabedLayer;

        collitionBox.enabled = false;

        this.owner = owner;
    }

    public void Drop()
    {
        transform.SetParent(null);

        rb.isKinematic = false;

        rb.AddForce(transform.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(transform.up * dropUpwardForce, ForceMode.Impulse);

        float random = Random.Range(-1f, 1f);

        rb.AddTorque(new Vector3(random, random, random) * 10);

        gameObject.layer = dropedLayer;

        collitionBox.enabled = true;

        this.owner = null;
    }

    private void OnValidate()
    {
        magazine.Validate();
    }
}
