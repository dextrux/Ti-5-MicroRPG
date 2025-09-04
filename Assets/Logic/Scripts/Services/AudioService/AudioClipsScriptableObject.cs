using Logic.Scripts.Helpers;
using UnityEngine;

namespace Logic.Scripts.Services.AudioService {
    public abstract class AudioClipsScriptableObject : ScriptableObject {
        public SerializableDictionary<AudioClipType, AudioClip> AudioClips;
    }
}