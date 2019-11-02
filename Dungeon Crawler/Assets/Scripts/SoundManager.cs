using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider m_ambient_Slider;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider m_sfx_Slider;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Slider m_voice_Slider;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private Toggle m_globalMute;

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        UpdateMusicSounds();
    }
    /// <summary>
    /// Updates all music volumes based on slider values.
    /// </summary>
    private void UpdateMusicSounds()
    {
        ///If sound is not globalled muted, continue
        if (!m_globalMute.isOn)
        {
            ///redundancy check - Make sure audiolistener is active.
            AudioListener.volume = 1;
            ///Check all objects marked SFX and set their volume to the slider value
            foreach (GameObject audiosource in GameObject.FindGameObjectsWithTag("SFX"))
            {   
                ///Redundancy check - Check if there are any components
                if (audiosource.GetComponent<AudioSource>() != null)
                {
                    audiosource.GetComponent<AudioSource>().volume = m_sfx_Slider.value;
                   
                }
            }
            ///Check all objects marked Ambient and set their volume to the slider value
            foreach (GameObject audiosource in GameObject.FindGameObjectsWithTag("Ambient"))
            {
                ///Redundancy check - Check if there are any components
                if (audiosource.GetComponent<AudioSource>() != null)
                {
                    audiosource.GetComponent<AudioSource>().volume = m_ambient_Slider.value;
                }
            }
            ///Check all objects marked Voices and set their volume to the slider value
            foreach (GameObject audiosource in GameObject.FindGameObjectsWithTag("Voices"))
            {
                ///Redundancy check - Check if there are any components
                if (audiosource.GetComponent<AudioSource>() != null)
                {
                    audiosource.GetComponent<AudioSource>().volume = m_voice_Slider.value;
                }
            }
        }
        ///Else mutes all audio because global mute is enabled.
        else
        {
            AudioListener.volume = 0;
        }
    }

}
