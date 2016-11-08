using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VideoPlayer : MonoBehaviour {

    public Image blockImg;

    private MovieTexture video;
    private AudioSource audioVideo;

	void Start ()
    {
        video = GetComponent<RawImage>().texture as MovieTexture;
        audioVideo = GetComponent<AudioSource>();
        audioVideo.clip = video.audioClip;
        video.Play();
        audioVideo.Play();
        StartCoroutine(closeVideo());
    }

    IEnumerator closeVideo()
    {
        yield return new WaitForSeconds(15f);
        GetComponent<RawImage>().enabled = false;
        yield return new WaitForSeconds(2f);
        blockImg.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }


}//FIM
