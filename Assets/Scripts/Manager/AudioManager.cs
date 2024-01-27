//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AudioManager : Singleton<AudioManager>
//{
//    protected override void Awake()
//    {
//        base.Awake();
//        this.bgmDic = new Dictionary<string, AudioClip>();
//        this.seDic = new Dictionary<string, AudioClip>();
//        this.gtDic = new Dictionary<string, AudioClip>();
//        object[] array = Resources.LoadAll("Audio/BGM");
//        object[] array2 = array;
//        array = Resources.LoadAll("Audio/GTHEME");
//        object[] array3 = array;
//        array = Resources.LoadAll("Audio/SE");
//        object[] array4 = array;
//        foreach (AudioClip audioClip in array2)
//        {
//            this.bgmDic[audioClip.name] = audioClip;
//        }
//        foreach (AudioClip audioClip2 in array4)
//        {
//            this.seDic[audioClip2.name] = audioClip2;
//        }
//        foreach (AudioClip audioClip3 in array3)
//        {
//            this.gtDic[audioClip3.name] = audioClip3;
//        }
//        this.Register(EventID.StopAudio, StopAllAudio);
//    }

//    private void Start()
//    {
//        this.AttachBGMSource.volume = PlayerPrefs.GetFloat("BGM_VOLUME_KEY", 1f);
//        this.AttachGTSource.volume = PlayerPrefs.GetFloat("BGM_VOLUME_KEY", 1f);
//        this.AttachSESource.volume = PlayerPrefs.GetFloat("SE_VOLUME_KEY", 1f);
//        PlayerPrefs.SetFloat("SE_VOLUME_KEY", AttachSESource.volume);
//        PlayerPrefs.SetFloat("BGM_VOLUME_KEY", this.AttachBGMSource.volume);
//        //if (!this.AttachBGMSource.isPlaying)
//        //{
//        //    this.PlayBGM("BGM" + this.currentBGM.ToString(), 0.9f);
//        //}
//    }
//    public void StopAllAudio(object data)
//    {
//        foreach (var item in audioEnvironments)
//        {
//            item.StopAudio();
//        }
//    }
//    public void PlaySE(string seName, float delay = 0f)
//    {
//        if (!this.seDic.ContainsKey(seName))
//        {
//            Debug.LogError(seName + " There is no SE named");
//            return;
//        }
//        this.nextSEName = seName;
//        base.Invoke("DelayPlaySE", delay);

//    }

//    public AudioClip GetAudioClip(string seName)
//    {
//        if (this.seDic.ContainsKey(seName))
//        {
//            return this.seDic[seName];
//        }
//        return null;
//    }

//    private void DelayPlaySE()
//    {
//        this.AttachSESource.PlayOneShot(this.seDic[this.nextSEName]);
//    }
//    public void PlayBGM()
//    {
//        PlayBGM("BGM" + ((this.currentBGM + 1) % this.bgmDic.Count).ToString());
//    }
//    public void PlayBGM(string bgmName, float fadeSpeedRate = 0.9f)
//    {
//        if (!this.bgmDic.ContainsKey(bgmName))
//        {
//            Debug.LogError(bgmName + " There is no BGM named");
//            return;
//        }
//        if (!this.AttachBGMSource.isPlaying)
//        {
//            this.nextBGMName = "BGM" + ((this.currentBGM + 1) % this.bgmDic.Count).ToString();
//            this.AttachBGMSource.clip = this.bgmDic[bgmName];
//            this.AttachBGMSource.Play();
//            return;
//        }
//        if (this.AttachBGMSource.clip.name != bgmName)
//        {
//            this.nextBGMName = bgmName;
//            this.FadeOutBGM(fadeSpeedRate);
//        }
//    }

//    public void PlayGT(string name)
//    {
//        if (name.Equals(nextGTName)) return;
//        this.nextGTName = name;
//        this.AttachGTSource.clip = this.gtDic[this.nextGTName];
//        this.AttachGTSource.volume = 0;
//        this.AttachGTSource.Play();
//        this.isPlayingGT = true;
//        StartCoroutine(FadeInRoutineGT());
//    }
//    private IEnumerator FadeInRoutineGT()
//    {
//        float targetVolume = PlayerPrefs.GetFloat("BGM_VOLUME_KEY", 1f);
//        while (targetVolume - AttachGTSource.volume > 0.01f)
//        {
//            AttachGTSource.volume = (targetVolume - AttachGTSource.volume) / 2;
//            yield return null;
//        }
//        AttachGTSource.volume = targetVolume;
//        yield return null;
//    }
//    //public void PlayGT()
//    //{

//    //this.nextGTName = "GTHEME" + this.currentGT.ToString();
//    //this.AttachGTSource.clip = this.gtDic[this.nextGTName];
//    //this.AttachGTSource.volume = PlayerPrefs.GetFloat("BGM_VOLUME_KEY", 0.5f);
//    //this.AttachGTSource.Play();
//    //this.currentGT = (this.currentGT + 1) % this.gtDic.Count;
//    //this.isPlayingGT = true;
//    //}


//    public void StopGT()
//    {
//        this.isPlayingGT = false;
//        this.AttachGTSource.Stop();
//    }

//    public void FadeOutBGM(float fadeSpeedRate = 0.3f)
//    {
//        this.bgmFadeSpeedRate = fadeSpeedRate;
//        this.isFadeOut = true;
//    }


//    //private void Update()
//    //{
//    //    if (this.isPlayingGT)
//    //    {
//    //        if (this.AttachGTSource.isPlaying)
//    //        {
//    //            if (this.AttachGTSource.clip.length - this.AttachGTSource.time < 1f)
//    //            {
//    //                this.AttachGTSource.volume -= 0.3f * Time.deltaTime;
//    //            }
//    //        }
//    //        else
//    //        {
//    //            this.PlayGT(nextGTName);
//    //        }
//    //    }
//    //if (this.AttachBGMSource != null && this.AttachBGMSource.clip.length - this.AttachBGMSource.time <= 1f)
//    //{
//    //    this.FadeOutBGM(0.3f);
//    //}
//    //if (!this.isFadeOut)
//    //{
//    //    return;
//    //}
//    //this.AttachBGMSource.volume -= Time.deltaTime * this.bgmFadeSpeedRate;
//    //if (this.AttachBGMSource.volume <= 0f)
//    //{
//    //    this.AttachBGMSource.Stop();
//    //    this.AttachBGMSource.volume = PlayerPrefs.GetFloat("BGM_VOLUME_KEY", 0.5f);
//    //    this.isFadeOut = false;
//    //    if (!string.IsNullOrEmpty(this.nextBGMName))
//    //    {
//    //        this.PlayBGM(this.nextBGMName, 0.9f);
//    //    }
//    //}
//    //}

//    // Token: 0x06000100 RID: 256 RVA: 0x00006B59 File Offset: 0x00004D59
//    public void ChangeBGMVolume(float BGMVolume)
//    {
//        this.AttachBGMSource.volume = BGMVolume;
//        AttachGTSource.volume = BGMVolume;
//        PlayerPrefs.SetFloat("BGM_VOLUME_KEY", BGMVolume);
//    }

//    // Token: 0x06000101 RID: 257 RVA: 0x00006B72 File Offset: 0x00004D72
//    public void ChangeSEVolume(float SEVolume)
//    {

//        this.AttachSESource.volume = SEVolume;
//        PlayerPrefs.SetFloat("SE_VOLUME_KEY", SEVolume);
//    }
//    public List<StopAudioEnvironment> audioEnvironments = new List<StopAudioEnvironment>();

//    private float bgmFadeSpeedRate = 0.9f;

//    private int currentBGM;

//    private int currentGT;

//    private string nextBGMName;


//    private string nextSEName;

//    private string nextGTName;

//    private bool isFadeOut;

//    private bool isPlayingGT;

//    public AudioSource AttachBGMSource;

//    public AudioSource AttachGTSource;

//    public AudioSource AttachSESource;

//    private Dictionary<string, AudioClip> bgmDic;

//    private Dictionary<string, AudioClip> seDic;

//    private Dictionary<string, AudioClip> gtDic;
//}
