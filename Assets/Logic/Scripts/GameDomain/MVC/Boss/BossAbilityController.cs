using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Abilitys;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather;

namespace Logic.Scripts.GameDomain.MVC.Boss
{
    public class BossAbilityController : IBossAbilityController
    {
        private BossBehaviorSO _bossBehavior;
        private int _activeIndex;

        public BossAbilityController(BossBehaviorSO bossBehavior)
        {
            _bossBehavior = bossBehavior;
            _activeIndex  = 0;
        }

        public void SetBehavior(BossBehaviorSO behavior)
        {
            _bossBehavior = behavior;
            _activeIndex = 0;
        }

        public BossAttack CreateAttack(Transform referenceTransform)
        {
            BossAttack[] pool = _bossBehavior != null ? _bossBehavior.AvailableAttacks : null;
            if (pool == null || pool.Length == 0) return null;

            int index = _activeIndex % pool.Length;

            TryPrimeFeatherPushMode(pool[index]);

            BossAttack abilitySpawned = UnityEngine.Object.Instantiate(
                pool[index],
                referenceTransform.position,
                referenceTransform.rotation
            );

            _activeIndex++;
            return abilitySpawned;
        }

        public BossAttack CreateAttackAtIndex(int index, Transform referenceTransform)
        {
            BossAttack[] pool = _bossBehavior != null ? _bossBehavior.AvailableAttacks : null;
            if (pool == null || pool.Length == 0) return null;

            if (index < 0) index = 0;
            index = index % pool.Length;

            TryPrimeFeatherPushMode(pool[index]);

            BossAttack abilitySpawned = UnityEngine.Object.Instantiate(
                pool[index],
                referenceTransform.position,
                referenceTransform.rotation
            );

            return abilitySpawned;
        }

        private void TryPrimeFeatherPushMode(BossAttack attackPrefab)
        {
            if (attackPrefab == null) return;

            if (!LooksLikeFeatherAttack(attackPrefab))
                return;

            if (TryInferPushFromAttackPrefab(attackPrefab, out bool isPush))
            {
                FeatherLinesHandler.PrimeNextTelegraphPushMode(isPush);
            }
        }

        private bool LooksLikeFeatherAttack(BossAttack attackPrefab)
        {
            string n = attackPrefab.name.ToLowerInvariant();
            if (n.Contains("feather")) return true;

            var comps = attackPrefab.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var c in comps)
            {
                if (c == null) continue;
                string tn = c.GetType().Name.ToLowerInvariant();
                if (tn.Contains("feather")) return true;
            }
            return false;
        }

        private bool TryInferPushFromAttackPrefab(BossAttack attackPrefab, out bool isPush)
        {
            var comps = attackPrefab.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var c in comps)
            {
                if (c == null) continue;

                if (TryInferPushFromObject(c, out isPush))
                    return true;

                if (TryInferPushFromEffectsFields(c, out isPush))
                    return true;
            }

            string name = attackPrefab.name.ToLowerInvariant();
            if (name.Contains("knockback") || name.Contains("push")) { isPush = true;  return true; }
            if (name.Contains("grapple")   || name.Contains("pull")) { isPush = false; return true; }

            isPush = true;
            return false;
        }

        private bool TryInferPushFromObject(object obj, out bool isPush)
        {
            isPush = true;

            if (obj == null) return false;

            Type t = obj.GetType();
            string tn = t.Name.ToLowerInvariant();
            if (tn.Contains("knockback") || tn.Contains("push")) { isPush = true;  return true; }
            if (tn.Contains("grapple")   || tn.Contains("pull")) { isPush = false; return true; }

            foreach (var name in new[] { "isPush", "push", "shouldPush", "knockback" })
            {
                var f = t.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (f != null && f.FieldType == typeof(bool))
                {
                    isPush = (bool)f.GetValue(obj);
                    return true;
                }
                var p = t.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (p != null && p.PropertyType == typeof(bool))
                {
                    isPush = (bool)p.GetValue(obj);
                    return true;
                }
            }

            var enumField = t.GetField("mode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                          ?? t.GetField("displacementMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (enumField != null)
            {
                object val = enumField.GetValue(obj);
                if (val != null)
                {
                    string s = val.ToString().ToLowerInvariant();
                    if (s.Contains("push") || s.Contains("knockback")) { isPush = true;  return true; }
                    if (s.Contains("pull") || s.Contains("grapple"))   { isPush = false; return true; }
                }
            }

            return false;
        }

        private bool TryInferPushFromEffectsFields(object obj, out bool isPush)
        {
            isPush = true;
            if (obj == null) return false;

            Type t = obj.GetType();

            var members = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Cast<MemberInfo>()
                .Concat(t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

            foreach (var m in members)
            {
                object value = null;
                Type   mType = null;

                if (m is FieldInfo fi)
                {
                    mType = fi.FieldType;
                    value = SafeGetField(fi, obj);
                }
                else if (m is PropertyInfo pi && pi.CanRead)
                {
                    mType = pi.PropertyType;
                    value = SafeGetProperty(pi, obj);
                }
                else continue;

                if (value == null) continue;

                if (typeof(AbilityEffect).IsAssignableFrom(mType))
                {
                    if (TryInferPushFromEffectInstance((AbilityEffect)value, out isPush))
                        return true;
                }

                if (typeof(IEnumerable).IsAssignableFrom(mType))
                {
                    if (TryInferPushFromEnumerable((IEnumerable)value, out isPush))
                        return true;
                }
            }

            return false;
        }

        private object SafeGetField(FieldInfo fi, object target)
        {
            try { return fi.GetValue(target); } catch { return null; }
        }

        private object SafeGetProperty(PropertyInfo pi, object target)
        {
            try { return pi.GetValue(target); } catch { return null; }
        }

        private bool TryInferPushFromEnumerable(IEnumerable e, out bool isPush)
        {
            isPush = true;
            if (e == null) return false;

            foreach (var item in e)
            {
                if (item == null) continue;

                if (item is AbilityEffect fx)
                {
                    if (TryInferPushFromEffectInstance(fx, out isPush))
                        return true;
                }

                string tn = item.GetType().Name.ToLowerInvariant();
                if (tn.Contains("knockback") || tn.Contains("push")) { isPush = true;  return true; }
                if (tn.Contains("grapple")   || tn.Contains("pull")) { isPush = false; return true; }
            }

            return false;
        }

        private bool TryInferPushFromEffectInstance(AbilityEffect fx, out bool isPush)
        {
            isPush = true;
            if (fx == null) return false;

            string n = fx.GetType().Name.ToLowerInvariant();
            if (n.Contains("knockback") || n.Contains("push")) { isPush = true;  return true; }
            if (n.Contains("grapple")   || n.Contains("pull")) { isPush = false; return true; }

            if (TryInferPushFromObject(fx, out isPush))
                return true;

            return false;
        }
    }
}
