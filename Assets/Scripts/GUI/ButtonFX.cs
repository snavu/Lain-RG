using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class ButtonFX : MonoBehaviour
{
    public Animator buttonAnim;

    public VideoClip[] preview;
    public VideoPlayer videoPlayer;
    public string[] buttonName;
    public GameObject video;

    public Text title;
    public Text artist;
    public string[] songTitle;
    public string[] songArtist;

    public float[] volume;

    public AudioClip[] sfx;

    public GameObject audioObject;
    AudioSource audioSource;

    public GameObject exitPrompt;
    public GameObject[] button;

    void Start()
    {
        audioObject = GameObject.Find("AudioObject");
        audioSource = audioObject.GetComponent<AudioSource>();
    }
    public void StartLevel()
    {
        SceneManager.LoadScene("TransitionScene");
    }
    public void StartLevelSelectMenu()
    {

        SceneManager.LoadScene("FirstCutScene");
    }
    public void Highlight()
    {
        if (FindObjectOfType<EventSystem>().currentSelectedGameObject != gameObject)
        {
            FindObjectOfType<EventSystem>().SetSelectedGameObject(gameObject);
        }
        buttonAnim.SetBool("select", true);
    }

    public void Unhighlight()
    {
        buttonAnim.SetBool("select", false);
    }

    public void PlayPreview()
    {
        video.SetActive(true);
        for (int i = 0; i < preview.Length; i++)
        {
            if (gameObject.name == buttonName[i])
            {
                PlayerPrefs.SetInt("level select", i);
                title.text = songTitle[i];
                artist.text = songArtist[i];

                videoPlayer.SetDirectAudioVolume(0,volume[i]);
                videoPlayer.Stop();
                videoPlayer.clip = preview[i];
                videoPlayer.Play();
            }
        }
    }

    public void DisplayNull()
    {
        title.text = "NULL";
        artist.text = "NULL";

        videoPlayer.Stop();
        video.SetActive(false);
    }

    public void DisplayExitPrompt()
    {
        exitPrompt.SetActive(true);
        FindObjectOfType<EventSystem>().SetSelectedGameObject(button[0]);
    }

    public void CancelExitPrompt()
    {
        exitPrompt.SetActive(false);
        FindObjectOfType<EventSystem>().SetSelectedGameObject(button[1]);
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void PlaySfx0()
    {
        audioSource.clip = sfx[0];
        audioSource.Play();
    }
    public void PlaySfx1()
    {
        audioSource.clip = sfx[1];
        audioSource.Play();
    }
    public void PlaySfx2()
    {
        audioSource.clip = sfx[2];
        audioSource.Play();
    }

    public void PlaySfx0OneShot()
    {
        audioSource.PlayOneShot(sfx[0]);
    }
}
