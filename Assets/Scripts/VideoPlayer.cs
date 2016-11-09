using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class VideoPlayer : MonoBehaviour {

    public Image blockImg;

#if UNITY_STANDALONE_WIN
    private MovieTexture video;
    private AudioSource audioVideo;
#endif

    void Start ()
    {
#if UNITY_STANDALONE_WIN
        video = GetComponent<RawImage>().texture as MovieTexture;
        audioVideo = GetComponent<AudioSource>();
        audioVideo.clip = video.audioClip;
        video.Play();
        audioVideo.Play();
#endif

#if UNITY_ANDROID
        //Debug.Log(Path.GetFileName("StreamingAssets/IntroProjetoAgua.mp4"));
        //Handheld.PlayFullScreenMovie("StreamingAssets/IntroProjetoAgua.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
#endif
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
