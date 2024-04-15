
using Godot;
using System;
using System.Collections.Generic;

//Author : Yoann Tsitionis
namespace Com.IsartDigital.Shmup.Manager
{

    public enum MusicName { AMBIANCE_TAVERN_UI, AMBIANCE_TAVERN, JINGLE_CREDIT, JINGLE_LOSE };
    public enum SfxName { CLICK, CLICK_PLAY, HIGHLIGHT_BUTTON, CARD_DRAW, CARD_PLAY, CARD_TRASH, CARD_ZOOM, 
        CARD_DRAW_AMBIANCE, ATTACK_DRAGON, ATTACK_EARTH, ATTACK_FIRE, ATTACK_OGRE, ATTACK_SKELETON, ATTACK_WATER,
        ATTACK_WITCHER, ATTACK_ZOMBIE, SUMMON_DEER, SUMMON_FOX, SUMMON_FROG, SUMMON_RABBIT, SUMMON_SHEEP, 
        HURT_DRAGON, HURT_OGRE, HURT_WITCHER, PLAYER_HURT, SKELETON_HURT, WIZARD_HURT, ZOMBIE_HURT };

    public class SoundManager : Node
    {
        static private SoundManager instance;

        static public SoundManager GetInstance()
        {
            if (instance == null) instance = new SoundManager();
            return instance;
        }

        private SoundManager() : base() { }

        [Export] public List<AudioStream> musicList;

        public List<List<AudioStream>> sfxList;

        // List of all types of SFX
        [Export] private List<AudioStream> clickList = new List<AudioStream>();
        [Export] private List<AudioStream> clickPlayList = new List<AudioStream>();
        [Export] private List<AudioStream> highlightButtonList = new List<AudioStream>();
        [Export] public List<AudioStream> cardDrawList = new List<AudioStream>();
        [Export] private List<AudioStream> cardPlayList = new List<AudioStream>();
        [Export] private List<AudioStream> cardTrashList = new List<AudioStream>();
        [Export] private List<AudioStream> cardZoomList = new List<AudioStream>();
        [Export] private List<AudioStream> cardDrawAmbianceList = new List<AudioStream>();
        [Export] private List<AudioStream> attackDragonList = new List<AudioStream>();
        [Export] private List<AudioStream> attackEarthList = new List<AudioStream>();
        [Export] private List<AudioStream> attackFireList = new List<AudioStream>();
        [Export] private List<AudioStream> attackOgreList = new List<AudioStream>();
        [Export] private List<AudioStream> attackSkeletonList = new List<AudioStream>();
        [Export] private List<AudioStream> attackWaterList = new List<AudioStream>();
        [Export] private List<AudioStream> attackWitcherList = new List<AudioStream>();
        [Export] private List<AudioStream> attackZombieList = new List<AudioStream>();
        [Export] private List<AudioStream> summonDeerList = new List<AudioStream>();
        [Export] private List<AudioStream> summonFoxList = new List<AudioStream>();
        [Export] private List<AudioStream> summonFrogList = new List<AudioStream>();
        [Export] private List<AudioStream> summonRabbitList = new List<AudioStream>();
        [Export] private List<AudioStream> summonSheepList = new List<AudioStream>();
        [Export] private List<AudioStream> hurtDragonList = new List<AudioStream>();
        [Export] private List<AudioStream> hurtOgreList = new List<AudioStream>();
        [Export] private List<AudioStream> hurtWitcherList = new List<AudioStream>();
        [Export] private List<AudioStream> playerHurtList = new List<AudioStream>();
        [Export] private List<AudioStream> skeletonHurtList = new List<AudioStream>();
        [Export] private List<AudioStream> wizardHurtList = new List<AudioStream>();
        [Export] private List<AudioStream> zombieHurtList = new List<AudioStream>();

        public override void _Ready()
        {
            if (instance != null)
            {
                QueueFree();
                GD.Print(nameof(SoundManager) + " Instance already exist, destroying the last added.");
                return;
            }
            instance = this;

            sfxList = new List<List<AudioStream>>() { clickList, clickPlayList, highlightButtonList, cardDrawList, cardPlayList, 
                cardTrashList, cardZoomList, cardDrawAmbianceList, attackDragonList, attackEarthList, attackFireList, attackOgreList,
                attackSkeletonList, attackWaterList, attackWitcherList, attackZombieList, summonDeerList, summonFoxList, summonFrogList,
                summonRabbitList, summonSheepList, hurtDragonList, hurtOgreList, hurtWitcherList, playerHurtList, skeletonHurtList,
                wizardHurtList, zombieHurtList};
        }

        #region CINEMATIC
        public void CinematicPlaySFX(SfxName pSfx, int pIndex, float pVolumeDb)
        {
            AudioStreamPlayer lSfxAudio = new AudioStreamPlayer();
            AddChild(lSfxAudio);
            lSfxAudio.Stream = sfxList[(int)pSfx][pIndex];
            lSfxAudio.VolumeDb += pVolumeDb;
            lSfxAudio.Play();
            lSfxAudio.Connect(GlobalReferences.Signals.FINISHED, this, nameof(DestroySfx), new Godot.Collections.Array() { lSfxAudio });
        }
        #endregion

        public void PlaySfxSound(SfxName pSfx, int pIndex, float pVolumeDb)
        {
            AudioStreamPlayer lSfxAudio = new AudioStreamPlayer();
            AddChild(lSfxAudio);
            lSfxAudio.Stream = sfxList[(int)pSfx][pIndex];
            lSfxAudio.VolumeDb += pVolumeDb;
            lSfxAudio.Play();
            lSfxAudio.Connect(GlobalReferences.Signals.FINISHED, this, nameof(DestroySfx), new Godot.Collections.Array() { lSfxAudio });
        }

        public AudioStreamPlayer PlayMusicSound(MusicName pMusic, Node pNode, float pVolume = 0f)
        {
            AudioStreamPlayer lMusicAudio = new AudioStreamPlayer();
            pNode.AddChild(lMusicAudio);
            lMusicAudio.VolumeDb = pVolume;
            lMusicAudio.Stream = musicList[(int)pMusic];
            lMusicAudio.Play();
            lMusicAudio.Connect(GlobalReferences.Signals.FINISHED, this, nameof(LoopMusic), new Godot.Collections.Array() { lMusicAudio });
            return lMusicAudio;
        }

        public void LoopMusic(AudioStreamPlayer lMusic)
        {
            lMusic.Play();
        }

        private void DestroySfx(AudioStreamPlayer lSfx)
        {
            lSfx.QueueFree();
        }

        public void FadeOutMusic(AudioStreamPlayer pAudio, float pFactor = 10f, float pDuration = 1f)
        {
            Tween lTween = new Tween();
            pAudio.AddChild(lTween);
            lTween.InterpolateProperty(pAudio, "volume_db", pAudio.VolumeDb, pAudio.VolumeDb - pFactor, pDuration, Tween.TransitionType.Linear, Tween.EaseType.InOut);
            lTween.Start();
        }
        public void FadeInMusic(AudioStreamPlayer pAudio, float pFactor = 10f, float pDuration = 1f)
        {
            Tween lTween = new Tween();
            pAudio.AddChild(lTween);
            lTween.InterpolateProperty(pAudio, "volume_db", pAudio.VolumeDb, pAudio.VolumeDb + pFactor, pDuration, Tween.TransitionType.Linear, Tween.EaseType.InOut);
            lTween.Start();
        }

        protected override void Dispose(bool pDisposing)
        {
            if (pDisposing && instance == this) instance = null;
            base.Dispose(pDisposing);
        }
    }
}