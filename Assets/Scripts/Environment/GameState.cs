using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
public class GameState : MonoBehaviour
{
    public EventSystem eventSys;
    HealthBar healthBarScript;
    public VideoPlayer videoPlayer;
    public GameObject video;

    public VideoClip[] videoClip;

    public bool flag = false;

    public float[] volume;

    public GameObject[] lockedLevel;
    public GameObject[] unlockedLevel;

    public GameObject[] winSceneText;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("AudioObject") != null)
        {
            DontDestroyOnLoad(GameObject.Find("AudioObject"));
        }

        eventSys = FindObjectOfType<EventSystem>();
        healthBarScript = FindObjectOfType<HealthBar>();

        if (SceneManager.GetActiveScene().name == "FirstCutScene")
        {
            PlayerPrefs.SetInt("load", 0);

            if (PlayerPrefs.GetInt("first load") == 0)
            {
                PlayerPrefs.SetInt("first load", 1);

                SetVideoClip(0);
                video.SetActive(true);
                StartCoroutine(DelaySceneLoad((float)videoPlayer.length + 1, "TransitionScene"));
            }
            else if (PlayerPrefs.GetInt("first load") == 1)
            {
                PlayerPrefs.SetInt("first load", 2);

                SetVideoClip(1);
                video.SetActive(true);
                StartCoroutine(DelaySceneLoad((float)videoPlayer.length + 1, "TransitionScene"));
            }
            else if (PlayerPrefs.GetInt("first load") == 2)
            {
                SceneManager.LoadScene("TransitionScene");
            }
        }

        if (SceneManager.GetActiveScene().name == "TransitionScene")
        {
            if (PlayerPrefs.GetInt("load") == 0)
            {
                StartCoroutine(DelaySceneLoad(1, "MenuScene"));
            }
            if (PlayerPrefs.GetInt("load") == 1)
            {
                StartCoroutine(DelaySceneLoad(1, "LevelScene"));
            }
        }

        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            if (lockedLevel[PlayerPrefs.GetInt("level unlock")] != null)
            {
                //unlock next level
                for (int i = 1; i < lockedLevel.Length; i++)
                {
                    if (lockedLevel[i].activeInHierarchy == true && PlayerPrefs.GetInt("level unlock") == i)
                    {
                        lockedLevel[i].SetActive(false);
                        unlockedLevel[i].SetActive(true);

                    }
                }

                //unlock all previous levels
                for (int i = lockedLevel.Length - 1; i > 1; i--)
                {
                    if (unlockedLevel[i].activeInHierarchy == true)
                    {
                        lockedLevel[i - 1].SetActive(false);
                        unlockedLevel[i - 1].SetActive(true);
                    }
                }
            }

            PlayerPrefs.SetInt("load", 1);
        }

        if (SceneManager.GetActiveScene().name == "LevelScene")
        {
            SetVideoClip(PlayerPrefs.GetInt("level select"));
            StartCoroutine(DelaySceneLoad((float)videoPlayer.length + 1, "WinScene"));
        }

        if (SceneManager.GetActiveScene().name == "FailScene")
        {
            StartCoroutine(DelaySceneLoad(3, "TransitionScene"));
            PlayerPrefs.SetInt("load", 0);
        }

        if (SceneManager.GetActiveScene().name == "WinScene")
        {
            SetVideoClip(PlayerPrefs.GetInt("level select"));
            StartCoroutine(DelayStartVideo(3));
            StartCoroutine(DelayEndVideo((float)videoPlayer.length + 1));
            PlayerPrefs.SetInt("load", 0);

            if(PlayerPrefs.GetInt("level unlock") < 4)
            {
                StartCoroutine(DelaySceneLoad((float)videoPlayer.length + 4, "TransitionScene"));
            }
            else
            {
                StartCoroutine(DelaySceneLoad((float)videoPlayer.length + 8, "TransitionScene"));
            }

            if (PlayerPrefs.GetInt("level select") < 4 && PlayerPrefs.GetInt("complete") == 0)
            {
                PlayerPrefs.SetInt("level unlock", PlayerPrefs.GetInt("level select") + 1);
            }
            else
            {
                PlayerPrefs.SetInt("complete", 1);
            }
        }
    }
    void SetVideoClip(int index)
    {
        for (int i = 0; i < videoClip.Length; i++)
        {
            if (i == index)
            {
                videoPlayer.SetDirectAudioVolume(0, volume[i]);
                videoPlayer.clip = null;
                videoPlayer.clip = videoClip[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (eventSys.currentSelectedGameObject == null && Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1)
        {
            eventSys.SetSelectedGameObject(eventSys.firstSelectedGameObject);
        }

        if (healthBarScript != null)
        {
            if (healthBarScript.currentSize <= 0 && flag == false)
            {
                StartCoroutine(DelaySceneLoad(0, "FailScene"));
                flag = true;
            }
        }
    }

    IEnumerator DelaySceneLoad(float seconds, string sceneName)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator DelayStartVideo(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        video.SetActive(true);
        videoPlayer.Play();

        if (winSceneText[0] != null)
        {
            winSceneText[0].SetActive(false);
            if(PlayerPrefs.GetInt("level unlock") != 4)
            {
                winSceneText[1].SetActive(true);
            }
            else
            {
                winSceneText[2].SetActive(true);
            }
        }
    }

    IEnumerator DelayEndVideo(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        video.SetActive(false);
    }
}
