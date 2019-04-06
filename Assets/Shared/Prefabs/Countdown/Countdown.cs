using System;
using UniRx;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private bool _counting;
    private readonly Subject<int> _subject = new Subject<int>();
    private AudioSource _audioSource;

    ~Countdown()
    {
        _subject.OnNext(0);
        _subject.OnCompleted();
    }

    public IObservable<int> StartCountdown()
    {
        if (!_counting)
        {
            _audioSource.Play();
            _counting = true;
        }

        return _subject.AsObservable();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_counting)
        {
            if (!_audioSource.isPlaying)
            {
                _counting = false;
                _subject.OnNext(1);
            }
        }
    }
}