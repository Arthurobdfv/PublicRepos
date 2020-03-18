using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
  public static AudioSource MainAudioSource;
  public static bool isMuted;

  public static AudioSource m_backGroundSound;

  void Awake()
  {
    MainAudioSource=GetComponent<AudioSource>();
  }

  public static void PlayRandomSoundWithRandomPitch(AudioClip[] ac, float minPitch,float maxPitch)
  {
    var selectedClip = ac[Random.Range(0,ac.Length)];
    PlaySoundRandomPitch(selectedClip,minPitch,maxPitch);
  }

  public static void PlaySoundRandomPitch(AudioClip ac,float minPitch,float maxPitch)
  {
    float randomizedPitch = Random.Range(minPitch,maxPitch);
    PlaySoundPitched(ac,randomizedPitch);
  }

  public static void PlayRandomClipWithSound(AudioClip[] acs, float volume)
  {
    MainAudioSource.volume=0.5f;
    PlayFromRandomClips(acs);
  }

  public static void PlaySoundPitched(AudioClip ac,float pitch)
  {
    MainAudioSource.pitch=pitch;
    PlaySound(ac);
  }

  public static void PlayFromRandomClips(AudioClip[] acs)
  {
    int randomIndex = Random.Range(0,acs.Length);
    PlaySound(acs[randomIndex]);
  }

  public static void PlaySound(AudioClip ac)
  {
    if(isMuted)
    {
      ResetSource();
      return;
    }
    MainAudioSource.PlayOneShot(ac);
    ResetSource();
  }

  private static void ResetSource()
  {
    MainAudioSource.clip=null;
    MainAudioSource.pitch=1.0f;
  }

  public static void PlayBackGroundMusicFromRandom(AudioClip[] acs, float volume)
  {
    m_backGroundSound =Instantiate(new GameObject(),Vector3.zero,Quaternion.identity).AddComponent<AudioSource>();
    m_backGroundSound.clip=acs[Random.Range(0,acs.Length)];
    m_backGroundSound.volume=volume;
    m_backGroundSound.loop=true;
    m_backGroundSound.Play();
  }
}
