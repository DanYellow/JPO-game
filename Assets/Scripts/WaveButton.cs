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
    [SerializeField]
    private GameObject waveEffectColliderPrefab;

    private MeshRenderer ringLightMeshRenderer;

    [SerializeField]
    private UnityEvent onButtonPressed;
    [SerializeField]
    private Material waveMaterial;

    private ObjectPooling pool;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private void Awake()
    {
        ringLightMeshRenderer = ringButton.GetComponent<MeshRenderer>();
        pool = GetComponent<ObjectPooling>();
    }

    private void Start()
    {
        transform.position = new Vector3(
            startPosition.position.x,
            startPosition.position.y,
            startPosition.position.z
        );
        ringLightOnMaterial = ringLightMeshRenderer.materials[0];
        onPositionSet.Invoke(playerPosition);

        waveMaterial.SetColor("_TintColor", playerData.color);
    }

    public void OnGroundPound(GameObject player)
    {
        StartCoroutine(MoveObject(player));
    }

    IEnumerator MoveObject(GameObject player)
    {
        Material[] newMaterials = ringLightMeshRenderer.materials;
        newMaterials[0] = ringLightOffMaterial;

        ringLightMeshRenderer.materials = newMaterials;
        Vector3 startPos = pushButton.transform.position;
        Vector3 endPos = pushButton.transform.position - (Vector3.up * 0.1f);

        onButtonPressed.Invoke();
        pushButton.transform.position = endPos;

        CreateWave();
        // GameObject waveEffect = Instantiate(waveEffectPrefab, waveEffectTransform.position, Quaternion.identity);

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
            GameObject waveEffectCollider = Instantiate(waveEffectColliderPrefab, waveEffectTransform.position, Quaternion.identity);
            waveEffectCollider.GetComponent<Follow>().target = pos;
            waveEffectCollider.gameObject.layer = LayerMask.NameToLayer($"WaveEffect{playerData.id.ToString()}");
            // waveEffectCollider.gameObject.transform.SetPositionAndRotation(pos.position, Quaternion.identity);
            
            waveEffectCollider.transform.rotation = Quaternion.identity;
            waveEffectCollider.gameObject.transform.LookAt(pos);

            waveEffectCollider.gameObject.transform.RotateAround(
                waveEffectCollider.gameObject.transform.position,
                waveEffectCollider.gameObject.transform.up,
                90
            );
        }

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
