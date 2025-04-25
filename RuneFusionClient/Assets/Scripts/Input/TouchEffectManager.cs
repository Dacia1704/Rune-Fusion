using System;
using UnityEngine;

public class TouchEffectManager: MonoBehaviour
{
    private AudioSource audioSource;
    public static TouchEffectManager Instance { get; private set; }
    private ObjectPooling touchEffectObjectPooling;
    private string nameEffect = "TouchEffect";

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        touchEffectObjectPooling = GetComponentInChildren<ObjectPooling>(); 
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        audioSource.clip = AudioManager.Instance.AudioPropsSO.ClickSound;
        audioSource.outputAudioMixerGroup = AudioManager.Instance.AudioPropsSO.SFXAudioMixerGroup;
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.Play();
            GameObject touchObject = touchEffectObjectPooling.GetObject(nameEffect);
            touchObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchObject.transform.position = new Vector3(touchObject.transform.position.x, touchObject.transform.position.y, 0);
        }
    }

    public void ReleaseEffect(GameObject effectObj)
    {
        touchEffectObjectPooling.ReleaseObject(effectObj);
    }
}