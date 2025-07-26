using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(AudioSource))]
public class MusicDirector : MonoBehaviourSingleton<MusicDirector>
{
    private class MusicPlayer
    {
        private readonly List<AudioSource> _audioSources = new();
        private MusicAsset _musicAsset;
        private bool _managedLoop;
        private float _managedLoopCountdown;

        public void Play(MusicAsset musicAsset, bool loop)
        {
            Stop();
            _musicAsset = musicAsset;
            _managedLoop = loop && musicAsset.managedLoop;
            _audioSources.Add(SoundManager.PlayUISound(musicAsset.clip, SoundManager.SoundType.Music, !loop || _managedLoop, loop && !_managedLoop));
            
            if (_managedLoop)
            {
                _managedLoopCountdown = musicAsset.GetManagedLoopPoint();
            }
        }

        public void Stop()
        {
            _managedLoop = false;
            foreach (var audioSource in _audioSources)
            {
                if (audioSource.gameObject != null)
                {
                    Destroy(audioSource.gameObject);
                }
            }
            _audioSources.Clear();
        }

        public void Update(float deltaTime)
        {
            if (_managedLoop && Application.isPlaying && Application.isFocused)
            {
                _managedLoopCountdown -= deltaTime;
                if (_managedLoopCountdown <= 0)
                {
                    _managedLoopCountdown += _musicAsset.GetManagedLoopPoint();
                    _audioSources.Add(SoundManager.PlayUISound(_musicAsset.clip, SoundManager.SoundType.Music));
                }
            }
        }
    }
    
    public enum MusicState
    {
        Menu,
        Intro,
        Game,
        FinalStage
    }

    public MusicState State
    {
        get => _stateInternal;
        set => SetMusicState(value);
    }


    [SerializeField] private MusicAsset menuMusic;
    [SerializeField] private MusicAsset introMusic;
    [SerializeField] private MusicAsset gameMusic;
    [SerializeField] private MusicAsset finalStageMusic;
    private static MusicState _stateInternal = MusicState.Menu;
    private readonly Dictionary<MusicState, MusicAsset> _musicAssets = new();
    private readonly MusicPlayer _musicPlayer = new();
    
    private void SetMusicState(MusicState musicState)
    {
        _stateInternal = musicState;
        if (_musicAssets.TryGetValue(musicState, out var musicAsset))
        {
            _musicPlayer.Play(musicAsset, musicState is not MusicState.Intro);
        }
    }

    public override void Awake()
    {
        base.Awake();
        
        if (menuMusic != null)
            _musicAssets[MusicState.Menu] = menuMusic;
        if (introMusic != null)
            _musicAssets[MusicState.Intro] = introMusic;
        if (gameMusic != null)
            _musicAssets[MusicState.Game] = gameMusic;
        if (finalStageMusic != null)
            _musicAssets[MusicState.FinalStage] = finalStageMusic;
        
        State = State;
    }

    public void Update()
    {
        _musicPlayer.Update(Time.deltaTime);
    }
}