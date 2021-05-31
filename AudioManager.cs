// Estre script es un Manager que controla el audio de TODO el Proyecto,
// ademas de usar audioSource "channels" para controlarlos	

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	/**
		Clase singletonpara que el resto de clases accedan a esta por metodos publicos estaticos
	*/
	
	static AudioManager current;

    [Header("Ambient Audio")]
    public AudioClip ambientClip;		//Background Ambiente 
    public AudioClip musicClip;			//background
	/*
		Clips background
	*/
	[Header("Stings")]
	public AudioClip levelStingClip;	//Load
	public AudioClip deathStingClip;	//Ded 
	public AudioClip winStingClip;		//Win 
	public AudioClip orbStingClip;      //Orb is collected
	public AudioClip doorOpenStingClip; //Open Sesame
	/**
		player noise
	*/
	[Header("Robbie Audio")]
    public AudioClip[] walkStepClips;	//walk
	public AudioClip[] crouchStepClips;	//bowdown before your true God
	public AudioClip deathClip;			//Ded
    public AudioClip jumpClip;			//Hops
	
	public AudioClip jumpVoiceClip;		
	public AudioClip deathVoiceClip;	
	public AudioClip orbVoiceClip;		
	public AudioClip winVoiceClip;		
/**
	AudioMixer (Barely used)
*/
	[Header("Mixer Groups")]
	public AudioMixerGroup ambientGroup;
	public AudioMixerGroup musicGroup;  
	public AudioMixerGroup stingGroup;  
	public AudioMixerGroup playerGroup; 
	public AudioMixerGroup voiceGroup;  
	AudioSource ambientSource;			
    AudioSource musicSource;            
	AudioSource stingSource;            
	AudioSource playerSource;           
	AudioSource voiceSource;            


	void Awake()
	{
		//If an AudioManager exists and it is not this...
		if (current != null && current != this)
		{
			//...destroy this. There can be only one AudioManager
			Destroy(gameObject);
			return;
		}

		//This is the current AudioManager and it should persist between scene loads
		//TODO: this is dumb
		current = this;
		DontDestroyOnLoad(gameObject);

		//Generate the Audio Source "channels" for the game
		//This will cause a crash later, but is good for now
		ambientSource	= gameObject.AddComponent<AudioSource>() as AudioSource;
        musicSource		= gameObject.AddComponent<AudioSource>() as AudioSource;
        stingSource		= gameObject.AddComponent<AudioSource>() as AudioSource;
        playerSource	= gameObject.AddComponent<AudioSource>() as AudioSource;
        voiceSource		= gameObject.AddComponent<AudioSource>() as AudioSource;

		//Assign each audio source to its respective mixer group so that it is
		//routed and controlled by the audio mixer
		ambientSource.outputAudioMixerGroup = ambientGroup;
		musicSource.outputAudioMixerGroup	= musicGroup;
		stingSource.outputAudioMixerGroup	= stingGroup;
		playerSource.outputAudioMixerGroup	= playerGroup;
		voiceSource.outputAudioMixerGroup	= voiceGroup;

		//Being playing the game audio
        StartLevelAudio();
	}

    void StartLevelAudio()
    {
		//Set the clip for ambient audio, tell it to loop, and then tell it to play
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

		//Set the clip for music audio, tell it to loop, and then tell it to play
        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();

		//Play the audio that repeats whenever the level reloads
		//Music goes:brrrrrrrrrrrr
		PlaySceneRestartAudio();
    }

	public static void PlayFootstepAudio()
	{
		//If there is no current AudioManager or the player source is already playing clip, exit 
		if (current == null || current.playerSource.isPlaying)
			return;

		//Pick a random footstep sound
		int index = Random.Range(0, current.walkStepClips.Length);
		
		//Set the footstep clip and tell the source to play
		current.playerSource.clip = current.walkStepClips[index];
		current.playerSource.Play();
	}

    public static void PlayCrouchFootstepAudio()
    {
		//If there is no current AudioManager or the player source is already playing clip, exit 
		if (current == null || current.playerSource.isPlaying)
            return;

		//Pick a random crouching footstep sound
		int index = Random.Range(0, current.crouchStepClips.Length);
		
		//Set the footstep clip and tell the source to play
		current.playerSource.clip = current.crouchStepClips[index];
		current.playerSource.Play();
	}

    public static void PlayJumpAudio()
    {
		//If there is no current AudioManager, exit
		if (current == null)
            return;

		//Set the jump SFX clip and tell the source to play
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

		//Set the jump voice clip and tell the source to play
		current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }

	public static void PlayDeathAudio()
	{
		//If there is no current AudioManager, exit
		if (current == null)
			return;

		//Set the death SFX clip and tell the source to play
		current.playerSource.clip = current.deathClip;
        current.playerSource.Play();
		
		//Set the death voice clip and tell the source to play
		current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();
		
		//Set the death sting clip and tell the source to play
		current.stingSource.clip = current.deathStingClip;
        current.stingSource.Play();
	}


	public static void PlayOrbCollectionAudio()
	{
		//If there is no current AudioManager, exit
		if (current == null)
			return;

		//Set the orb sting clip and tell the source to play
		current.stingSource.clip = current.orbStingClip;
        current.stingSource.Play();

		//Set the orb voice clip and tell the source to play
		current.voiceSource.clip = current.orbVoiceClip;
        current.voiceSource.Play();
	}

    public static void PlaySceneRestartAudio()
    {
		//If there is no current AudioManager, exit
		if (current == null)
            return;

		//Set the level reload sting clip and tell the source to play
		current.stingSource.clip = current.levelStingClip;
        current.stingSource.Play();
    }

	public static void PlayDoorOpenAudio()
	{
		//If there is no current AudioManager, exit
		if (current == null)
			return;

		//Set the door open sting clip and tell the source to play
		current.stingSource.clip = current.doorOpenStingClip;
		current.stingSource.PlayDelayed(1f);
	}

	public static void PlayWonAudio()
    {
		//If there is no current AudioManager, exit
		if (current == null)
            return;

		//Stop the ambient sound
        current.ambientSource.Stop();

		//Set the player won voice clip and tell the source to play
		current.voiceSource.clip = current.winVoiceClip;
        current.voiceSource.Play();

		//Set the player won sting clip and tell the source to play
		current.stingSource.clip = current.winStingClip;
        current.stingSource.Play();
    }
}
