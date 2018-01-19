using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*----------------------------------
 * button click event sound
 * 버튼 효과음 추가 스크립트
 *----------------------------------*/
[RequireComponent(typeof(Button))]
public class ClickSound : MonoBehaviour
{

    public AudioClip soundEffect;

    private Button button
    {
        get
        {
            return GetComponent<Button>();
        }
    }
    private AudioSource source
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        source.clip = soundEffect;
        source.playOnAwake = false;

        button.onClick.AddListener(() => PlaySound());
    }

    void PlaySound()
    {
        source.PlayOneShot(soundEffect);
    }
}