using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaveButton : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private UnityEvent<Transform> onPositionSet;

    [SerializeField]
    private Transform playerPosition;

    [SerializeField]
    private GameObject pushButton;

    [SerializeField]
    private GameObject ringButton;
    private Material ringLightOnMaterial;
    [SerializeField]
    private Material ringLightOffMaterial; // #4E4C00

    [SerializeField]
    private Transform waveEffectTransform;

    private MeshRenderer ringLightMeshRenderer;

    [SerializeField]
    private UnityEvent onButtonPressed;
    [SerializeField]
    private Material waveMaterial;

    private ObjectPooling pool;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private PlayerIDEventChannel onPlayerDeathEvent;

    private float heightButton = 0;
     private ParticleSystem particlesSystem;

    private void Awake()
    {
        ringLightMeshRenderer = ringButton.GetComponent<MeshRenderer>();
        pool = GetComponent<ObjectPooling>();
        particlesSystem = GetComponentInChildren<ParticleSystem>();

        heightButton = GetComponentInChildren<BoxCollider>().bounds.size.y;

        switch (playerData.id)
        {
            case PlayerID.Player2:
                transform.localRotation = Quaternion.Euler(0, 135, 0);
                break;
            case PlayerID.Player3:
                transform.localRotation = Quaternion.Euler(0, -135, 0);
                break;
            case PlayerID.Player4:
                transform.localRotation = Quaternion.Euler(0, -45, 0);
                break;
            default:
                transform.localRotation = Quaternion.Euler(0, 45, 0);
                break;
        }
    }

    private void OnEnable()
    {
        onPlayerDeathEvent.OnEventRaised += Disabled;
    }

    private void OnDisable()
    {
        onPlayerDeathEvent.OnEventRaised -= Disabled;
    }

    private void Start()
    {
        transform.position = new Vector3(
            startPosition.position.x,
            startPosition.position.y,
            startPosition.position.z
        );
        ringLightOnMaterial = ringLightMeshRenderer.materials[0];
        ringLightOnMaterial.SetColor("_Color", playerData.color);

        Material[] newMaterials = ringLightMeshRenderer.materials;
        newMaterials[0] = ringLightOnMaterial;

        onPositionSet.Invoke(playerPosition);

        waveMaterial.SetColor("_TintColor", playerData.color);
    }

    public void OnGroundPound(GameObject player)
    {
        StartCoroutine(PressButton());
    }

    IEnumerator PressButton()
    {
        Material[] newMaterials = ringLightMeshRenderer.materials;
        newMaterials[0] = ringLightOffMaterial;

        ringLightMeshRenderer.materials = newMaterials;
        Vector3 startPos = pushButton.transform.position;
        Vector3 endPos = pushButton.transform.position - (Vector3.up * (heightButton * 1.5f));

        onButtonPressed.Invoke();
        pushButton.transform.position = endPos;
        particlesSystem.Play();

        CreateWave();

        float timeElapsed = 0;
        while (timeElapsed < playerData.root.groundPoundCooldown)
        {
            timeElapsed += Time.deltaTime;
            pushButton.transform.position = Vector3.Lerp(endPos, startPos, timeElapsed);

            yield return null;
        }
        pushButton.transform.position = startPos;
        newMaterials[0] = ringLightOnMaterial;
        ringLightMeshRenderer.materials = newMaterials;
    }

    private void CreateWave()
    {
        ObjectPooled waveEffect = pool.Get("WaveEffect");
        if (waveEffect == null)
        {
            return;
        }
        waveEffect.gameObject.transform.SetPositionAndRotation(waveEffectTransform.position, Quaternion.identity);
        Material[] waveEffectMaterials = waveEffect.GetComponent<MeshRenderer>().materials;
        waveEffectMaterials[0] = waveMaterial;
        waveEffect.GetComponent<MeshRenderer>().materials = waveEffectMaterials;

        foreach (var pos in waveEffect.GetComponent<WaveEffect>().GetTrakers(playerData.id))
        {
            ObjectPooled waveEffectCollider = pool.Get("WaveEffectCollider");
            if (waveEffect == null)
            {
                return;
            }

            waveEffectCollider.GetComponent<Follow>().target = pos;
            waveEffectCollider.GetComponent<WaveEffectCollision>().vfx = waveEffect.gameObject;
            waveEffectCollider.gameObject.layer = LayerMask.NameToLayer($"WaveEffect{playerData.id.ToString()}");
            waveEffectCollider.transform.rotation = Quaternion.identity;
            waveEffectCollider.gameObject.transform.LookAt(pos);

            waveEffectCollider.gameObject.transform.RotateAround(
                waveEffectCollider.gameObject.transform.position,
                waveEffectCollider.gameObject.transform.up,
                90
            );
        }
    }

    private void Disabled(PlayerID playerID)
    {
        if (playerID != playerData.id)
        {
            return;
        }
        Vector3 endPos = pushButton.transform.position - (Vector3.up * 0.1f);
        pushButton.transform.position = endPos;

        Material[] newMaterials = ringLightMeshRenderer.materials;
        newMaterials[0] = ringLightOffMaterial;
        ringLightMeshRenderer.materials = newMaterials;
    }

    private void OnCollisionEnter(Collision other)
    {
        other.transform.parent = pushButton.transform;
    }

    private void OnCollisionExit(Collision other)
    {
        other.transform.parent = null;
    }
}
