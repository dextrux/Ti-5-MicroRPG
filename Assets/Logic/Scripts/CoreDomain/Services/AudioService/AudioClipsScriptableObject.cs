using System;
using System.Collections.Generic;
using UnityEngine;

namespace Logic.Scripts.Services.AudioService
{
    [Serializable]
    public struct AudioClipEntry
    {
        public AudioClipType Key;   // o "Keys"
        public AudioClip Clip;      // o "Values"
    }

    /// <summary>
    /// Base SO que expõe a lista de pares Key/Clip no Inspector
    /// e fornece um dicionário somente-leitura para o AudioService.
    /// </summary>
    public abstract class AudioClipsScriptableObject : ScriptableObject
    {
        [SerializeField] private List<AudioClipEntry> _entries = new List<AudioClipEntry>();

        // cache interno para o dicionário em runtime
        [NonSerialized] private Dictionary<AudioClipType, AudioClip> _cache;

        /// <summary>
        /// Acesso usado pelo AudioService. Continua sendo um "mapa".
        /// </summary>
        public IReadOnlyDictionary<AudioClipType, AudioClip> AudioClips
        {
            get
            {
                if (_cache == null)
                    BuildCache();
                return _cache;
            }
        }

        /// <summary>
        /// Conveniência para quem quiser iterar no Inspector via Odin/PropertyDrawer etc.
        /// </summary>
        public IReadOnlyList<AudioClipEntry> Entries => _entries;

        /// <summary>
        /// Busca direta (útil em testes/manuais).
        /// </summary>
        public bool TryGetClip(AudioClipType type, out AudioClip clip)
        {
            if (_cache == null)
                BuildCache();
            return _cache.TryGetValue(type, out clip);
        }

        /// <summary>
        /// Reconstroi o cache a partir da lista serializada (chamado em OnValidate e sob demanda).
        /// </summary>
        private void BuildCache()
        {
            _cache = new Dictionary<AudioClipType, AudioClip>();
            if (_entries == null) return;

            for (int i = 0; i < _entries.Count; i++)
            {
                var e = _entries[i];
                if (e.Clip == null) continue;       // ignora entradas sem áudio
                _cache[e.Key] = e.Clip;             // última ocorrência vence (evita duplicatas)
            }
        }

        private void OnValidate()
        {
            // sempre que editar no Inspector, reconstruímos o cache
            _cache = null;
        }
    }
}
