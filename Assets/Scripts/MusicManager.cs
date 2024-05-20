using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    [SerializeField]
    public Slider slider;
    public AudioSource musicSource;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            slider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            slider.value = 1.0f; // Default value
        }
        slider.onValueChanged.AddListener(delegate { changeVolume(); });
        changeVolume();
    }

    private void Update()
    {
        changeVolume();
    }
    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("MusicManager");
                    instance = go.AddComponent<MusicManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void changeVolume()
    {
        musicSource.volume = slider.value;
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
    }
}
