using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Shared;
using System.Collections.Generic;
using Logic.Scripts.GameDomain.MVC.Abilitys;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather
{
    public class FeatherLinesHandler : IBossAttackHandler
    {
        private readonly FeatherLinesParams _params;

        private class FeatherSubView
        {
            public LineRenderer Line;
            public MeshFilter MeshFilter;
            public MeshRenderer MeshRenderer;
            public Mesh Mesh;
        }

        private FeatherSubView[] _views;
        private int _specialIndex;

        // true = PUSH (Knockback)  -> seta vermelha
        // false = PULL (Grapple)   -> seta azul
        private bool _isPushMode = false;
        private bool _hasPushConfigured = false;

        // provider opcional para informar push/pull ANTES do telegraph
        private static System.Func<bool> _isPushProvider;

        // ÚNICA seta global (sai do alvo / player)
        private LineRenderer _singleArrow;

        public static System.Func<int> GetPlayerDebuffStacks;
        public static Vector3 CurrentSpecialStart;
        public static Vector3 CurrentSpecialEnd;
        public static Vector3 CurrentSpecialAxis;
        public static float CurrentStripWidth;

        public FeatherLinesHandler(FeatherLinesParams p)
        {
            _params = p;
        }

        /// <summary>Configura diretamente o modo (use antes de PrepareTelegraph).</summary>
        public void SetPushMode(bool isPush)
        {
            _isPushMode = isPush;
            _hasPushConfigured = true;
            if (_singleArrow != null)
            {
                Vector3 playerWorld = ResolvePlayerWorldPosition();
                UpdateSingleArrow(playerWorld, CurrentSpecialStart, CurrentSpecialEnd);
            }
        }

        /// <summary>Registra um provider para resolver push/pull ANTES do telegraph (o ataque deve setar isso).</summary>
        public static void ConfigurePushProvider(System.Func<bool> provider)
        {
            _isPushProvider = provider;
        }

        public void PrepareTelegraph(Transform parentTransform)
        {
            // Se ninguém configurou explicitamente, tenta resolver via provider ANTES de desenhar
            if (!_hasPushConfigured && _isPushProvider != null)
            {
                try { _isPushMode = _isPushProvider(); _hasPushConfigured = true; }
                catch { /* mantém defaults */ }
            }

            int n = Mathf.Max(1, _params.featherCount);
            _views = new FeatherSubView[n];
            _specialIndex = Random.Range(0, n);

            for (int i = 0; i < n; i++)
            {
                GameObject go = new GameObject("FeatherSubActionView");
                go.transform.SetParent(parentTransform, false);

                FeatherSubView v = new FeatherSubView();
                v.Line = go.AddComponent<LineRenderer>();
                v.Line.material = new Material(Shader.Find("Sprites/Default"));
                v.Line.useWorldSpace = true;
                v.Line.loop = true;
                v.Line.widthMultiplier = 0.1f;
                v.Line.startColor = (i == _specialIndex) ? Color.red : Color.yellow;
                v.Line.endColor = v.Line.startColor;

                v.MeshFilter = go.AddComponent<MeshFilter>();
                v.MeshRenderer = go.AddComponent<MeshRenderer>();
                v.MeshRenderer.material = new Material(Shader.Find("Sprites/Default"))
                {
                    color = (i == _specialIndex) ? new Color(1f, 0f, 0f, 0.2f) : new Color(1f, 1f, 0f, 0.2f)
                };
                v.Mesh = new Mesh { name = "FeatherStripMesh" };
                v.MeshFilter.sharedMesh = v.Mesh;

                _views[i] = v;
            }

            // ÚNICA seta (global), criada no telegraph
            var arrowGO = new GameObject("FeatherDirectionArrow_Global");
            arrowGO.transform.SetParent(parentTransform, false);
            _singleArrow = arrowGO.AddComponent<LineRenderer>();
            _singleArrow.material = new Material(Shader.Find("Sprites/Default"));
            _singleArrow.useWorldSpace = true;
            _singleArrow.loop = false;
            _singleArrow.widthMultiplier = 0.08f;
            _singleArrow.enabled = true;

            // Gera toda a geometria no centro atual e já posiciona a seta com a cor/direção CORRETAS no telegraph
            UpdateTelegraphGeometryAtCenter(parentTransform.position);
        }

        private void UpdateTelegraphGeometryAtCenter(Vector3 center)
        {
            float spacing = Mathf.Max(0.1f, _params.margin);

            int n = _views.Length;
            for (int i = 0; i < n; i++)
            {
                Vector3 start, end;

                switch (_params.axisMode)
                {
                    case FeatherAxisMode.X:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end   = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Z:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x + offset, center.y, center.z - 100f);
                        end   = new Vector3(center.x + offset, center.y, center.z + 100f);
                        break;
                    }
                    case FeatherAxisMode.XZ:
                    {
                        int nX = (n + 1) / 2; // pares (X)
                        int nZ = n / 2;       // ímpares (Z)
                        if ((i % 2) == 0)
                        {
                            int k = i / 2;
                            float offset = (k - (nX - 1) * 0.5f) * spacing;
                            start = new Vector3(center.x - 100f, center.y, center.z + offset);
                            end   = new Vector3(center.x + 100f, center.y, center.z + offset);
                        }
                        else
                        {
                            int k = (i - 1) / 2;
                            float offset = (k - (nZ - 1) * 0.5f) * spacing;
                            start = new Vector3(center.x + offset, center.y, center.z - 100f);
                            end   = new Vector3(center.x + offset, center.y, center.z + 100f);
                        }
                        break;
                    }
                    case FeatherAxisMode.Diagonal:
                    default:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z - 100f + offset);
                        end   = new Vector3(center.x + 100f, center.y, center.z + 100f + offset);
                        break;
                    }
                }

                // Geometria da faixa (y fixo)
                Vector3[] vertsWorld = StripMath.GenerateStripVertices(start, end, _params.width);
                for (int p = 0; p < vertsWorld.Length; p++) vertsWorld[p].y = 0.2f;

                _views[i].Line.positionCount = vertsWorld.Length;
                _views[i].Line.SetPositions(vertsWorld);

                // Mesh em local space (child)
                Transform mT = _views[i].MeshFilter.transform;
                mT.localPosition = new Vector3(0f, 0.2f, 0f);
                mT.localRotation = Quaternion.identity;

                Vector3 v0L = mT.InverseTransformPoint(vertsWorld[0]);
                Vector3 v1L = mT.InverseTransformPoint(vertsWorld[1]);
                Vector3 v2L = mT.InverseTransformPoint(vertsWorld[2]);
                Vector3 v3L = mT.InverseTransformPoint(vertsWorld[3]);

                _views[i].Mesh.Clear();
                _views[i].Mesh.vertices  = new Vector3[] { v0L, v1L, v2L, v3L };
                _views[i].Mesh.triangles = new int[]     { 0, 1, 2, 0, 2, 3 };
                _views[i].Mesh.RecalculateNormals();
                _views[i].Mesh.RecalculateBounds();
            }

            // >>> feather especial e seta já no telegraph
            ComputeAndExposeSpecial(center, spacing, n, out var sStart, out var sEnd);
            CurrentSpecialStart = sStart;
            CurrentSpecialEnd   = sEnd;
            CurrentSpecialAxis  = (sEnd - sStart).normalized;
            CurrentSpecialAxis.y = 0f;
            CurrentStripWidth   = _params.width;

            // Posição do player (Nara) para desenhar a seta assim que o telegraph aparece
            Vector3 playerWorld = ResolvePlayerWorldPosition();
            UpdateSingleArrow(playerWorld, sStart, sEnd);
        }

        private void ComputeAndExposeSpecial(Vector3 center, float spacing, int n, out Vector3 sStart, out Vector3 sEnd)
        {
            float specialOffset = (_specialIndex - (n - 1) * 0.5f) * spacing;

            if (_params.axisMode == FeatherAxisMode.X)
            {
                sStart = new Vector3(center.x - 100f, center.y, center.z + specialOffset);
                sEnd   = new Vector3(center.x + 100f, center.y, center.z + specialOffset);
            }
            else if (_params.axisMode == FeatherAxisMode.Z)
            {
                sStart = new Vector3(center.x + specialOffset, center.y, center.z - 100f);
                sEnd   = new Vector3(center.x + specialOffset, center.y, center.z + 100f);
            }
            else if (_params.axisMode == FeatherAxisMode.XZ)
            {
                int nX = (n + 1) / 2;
                int nZ = n / 2;
                if ((_specialIndex % 2) == 0)
                {
                    int k = _specialIndex / 2;
                    float offX = (k - (nX - 1) * 0.5f) * spacing;
                    sStart = new Vector3(center.x - 100f, center.y, center.z + offX);
                    sEnd   = new Vector3(center.x + 100f, center.y, center.z + offX);
                }
                else
                {
                    int k = (_specialIndex - 1) / 2;
                    float offZ = (k - (nZ - 1) * 0.5f) * spacing;
                    sStart = new Vector3(center.x + offZ, center.y, center.z - 100f);
                    sEnd   = new Vector3(center.x + offZ, center.y, center.z + 100f);
                }
            }
            else
            {
                sStart = new Vector3(center.x - 100f, center.y, center.z - 100f + specialOffset);
                sEnd   = new Vector3(center.x + 100f, center.y, center.z + 100f + specialOffset);
            }
        }

        // Resolve player (Nara) em world space
        private Vector3 ResolvePlayerWorldPosition()
        {
            var ar = Object.FindObjectOfType<ArenaPosReference>();
            if (ar != null)
            {
                return ar.RelativeArenaPositionToRealPosition(ar.GetPlayerArenaPosition());
            }
            var naraView = Object.FindObjectOfType<Nara.NaraView>();
            if (naraView != null) return naraView.transform.position;
            return Vector3.zero;
        }

        public bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            Vector3 center = arenaReference != null ? arenaReference.transform.position : originTransform.position;
            Vector3 playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
            float spacing = Mathf.Max(0.1f, _params.margin);
            int n = _views.Length;

            for (int i = 0; i < n; i++)
            {
                Vector3 start, end;

                switch (_params.axisMode)
                {
                    case FeatherAxisMode.X:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end   = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Z:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x + offset, center.y, center.z - 100f);
                        end   = new Vector3(center.x + offset, center.y, center.z + 100f);
                        break;
                    }
                    case FeatherAxisMode.XZ:
                    {
                        int nX = (n + 1) / 2;
                        int nZ = n / 2;
                        if ((i % 2) == 0)
                        {
                            int k = i / 2;
                            float offset = (k - (nX - 1) * 0.5f) * spacing;
                            start = new Vector3(center.x - 100f, center.y, center.z + offset);
                            end   = new Vector3(center.x + 100f, center.y, center.z + offset);
                        }
                        else
                        {
                            int k = (i - 1) / 2;
                            float offset = (k - (nZ - 1) * 0.5f) * spacing;
                            start = new Vector3(center.x + offset, center.y, center.z - 100f);
                            end   = new Vector3(center.x + offset, center.y, center.z + 100f);
                        }
                        break;
                    }
                    case FeatherAxisMode.Diagonal:
                    default:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z - 100f + offset);
                        end   = new Vector3(center.x + 100f, center.y, center.z + 100f + offset);
                        break;
                    }
                }

                Mesh mesh = _views[i].Mesh;
                Transform mT = _views[i].MeshFilter.transform;
                Vector3[] mv = mesh.vertices;
                Vector3[] verts = new Vector3[4];
                verts[0] = mT.TransformPoint(mv[0]);
                verts[1] = mT.TransformPoint(mv[1]);
                verts[2] = mT.TransformPoint(mv[2]);
                verts[3] = mT.TransformPoint(mv[3]);

                if (PointInQuad(playerWorld, verts)) return true;
            }
            return false;
        }

        private bool PointInQuad(Vector3 p, Vector3[] q)
        {
            Vector2 P = new Vector2(p.x, p.z);
            Vector2 A = new Vector2(q[0].x, q[0].z);
            Vector2 B = new Vector2(q[1].x, q[1].z);
            Vector2 C = new Vector2(q[2].x, q[2].z);
            Vector2 D = new Vector2(q[3].x, q[3].z);
            return PointInTriangle(P, A, B, C) || PointInTriangle(P, A, C, D);
        }

        private bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            float s = a.y * c.x - a.x * c.y + (c.y - a.y) * p.x + (a.x - c.x) * p.y;
            float t = a.x * b.y - a.y * b.x + (a.y - b.y) * p.x + (b.x - a.x) * p.y;
            if ((s < 0) != (t < 0)) return false;
            float A = -b.y * c.x + a.y * (c.x - b.x) + a.x * (b.y - c.y) + b.x * c.y;
            if (A < 0.0f) { s = -s; t = -t; A = -A; }
            return s > 0 && t > 0 && (s + t) < A;
        }

        private float PerpendicularDistanceToLineXZ(Vector3 point, Vector3 a, Vector3 b)
        {
            Vector2 P = new Vector2(point.x, point.z);
            Vector2 A = new Vector2(a.x, a.z);
            Vector2 B = new Vector2(b.x, b.z);
            Vector2 AB = B - A;
            float len = AB.magnitude;
            if (len < 1e-6f) return Vector2.Distance(P, A);
            float area2 = Mathf.Abs((B.x - A.x) * (P.y - A.y) - (B.y - A.y) * (P.x - A.x));
            return area2 / len;
        }

        public System.Collections.IEnumerator ExecuteEffects(List<AbilityEffect> effects, ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            if (effects == null || effects.Count == 0) { Debug.Log("FeatherLinesHandler.ExecuteEffects: no effects"); yield break; }
            IEffectable target = arenaReference.NaraController as IEffectable;
            if (target == null) { Debug.LogWarning("FeatherLinesHandler.ExecuteEffects: target is null or not IEffectable"); yield break; }

            Vector3 center = arenaReference != null ? arenaReference.transform.position : originTransform.position;
            Vector3 playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
            float spacing = Mathf.Max(0.1f, _params.margin);
            int n = _views != null ? _views.Length : 0;

            // fallback: se ainda não configurado e não há provider, infere pelos efeitos
            if (!_hasPushConfigured && _isPushProvider == null)
            {
                _isPushMode = InferPushModeFromEffects(effects);
                _hasPushConfigured = true;
            }

            yield return ExecuteFeatherSequence(effects, arenaReference, originTransform, caster, target, center, spacing, n, playerWorld);
        }

        private bool InferPushModeFromEffects(List<AbilityEffect> effects)
        {
            foreach (var fx in effects)
            {
                if (fx == null) continue;
                string n = fx.GetType().Name.ToLowerInvariant();
                if (n.Contains("knockback") || n.Contains("push")) return true;   // PUSH
                if (n.Contains("grapple")   || n.Contains("pull")) return false;  // PULL
            }
            return _isPushMode; // mantém o que estiver
        }

        private System.Collections.IEnumerator ExecuteFeatherSequence(List<AbilityEffect> effects, ArenaPosReference arenaReference, Transform originTransform, IEffectable caster, IEffectable target, Vector3 center, float spacing, int n, Vector3 playerWorld)
        {
            // Recalcula e expõe dados da special
            ComputeAndExposeSpecial(center, spacing, n, out var sStart, out var sEnd);

            // Atualiza seta (caso player tenha se movido antes da execução)
            UpdateSingleArrow(playerWorld, sStart, sEnd);

            // Esconde a special visualmente (linha e mesh) — a seta permanece visível
            LineRenderer sLr = (_views != null && _specialIndex >= 0 && _specialIndex < _views.Length) ? _views[_specialIndex].Line : null;
            if (sLr != null) sLr.enabled = false;
            MeshRenderer sMr = (_views != null && _specialIndex >= 0 && _specialIndex < _views.Length) ? _views[_specialIndex].MeshRenderer : null;
            if (sMr != null) sMr.enabled = false;

            yield return new WaitForSeconds(0.5f);

            // (1) efeitos exceto o último, apenas se player estiver dentro da special
            int lastIndex = effects.Count - 1;
            if (lastIndex >= 0 && StripMath.IsPointInsideStrip(sStart, sEnd, _params.width, playerWorld))
            {
                for (int ei = 0; ei < lastIndex; ei++)
                {
                    AbilityEffect fx = effects[ei];
                    fx?.Execute(caster, target);
                }
            }

            yield return new WaitForSeconds(0.5f);

            // (2) último efeito (geralmente deslocamento global)
            if (effects.Count > 0)
            {
                AbilityEffect fx1 = effects[lastIndex];
                if (fx1 != null)
                {
                    if (fx1 is IForceScaledEffect scalable1)
                    {
                        int stacks = GetPlayerDebuffStacks != null ? GetPlayerDebuffStacks() : 0;
                        float distMeters = PerpendicularDistanceToLineXZ(
                            arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition()),
                            CurrentSpecialStart, CurrentSpecialEnd);
                        int distMul = Mathf.RoundToInt(distMeters);
                        scalable1.SetForceScalers(stacks, distMul);
                    }

                    if (fx1 is IAsyncEffect asyncFx1)
                        yield return asyncFx1.ExecuteRoutine(caster, target);
                    else
                        fx1.Execute(caster, target);

                    yield return new WaitForSeconds(0.5f);

                    // Player pode ter sido puxado/empurrado → atualiza seta
                    playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
                    UpdateSingleArrow(playerWorld, sStart, sEnd);
                }
            }

            // (3) feathers normais (não a special)
            for (int i = 0; i < n && effects.Count > 0; i++)
            {
                if (i == _specialIndex) continue;

                Vector3 start, end;
                switch (_params.axisMode)
                {
                    case FeatherAxisMode.X:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end   = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Z:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x + offset, center.y, center.z - 100f);
                        end   = new Vector3(center.x + offset, center.y, center.z + 100f);
                        break;
                    }
                    case FeatherAxisMode.XZ:
                    {
                        int nX = (n + 1) / 2;
                        int nZ = n / 2;
                        if ((i % 2) == 0)
                        {
                            int k = i / 2;
                            float offset = (k - (nX - 1) * 0.5f) * spacing;
                            start = new Vector3(center.x - 100f, center.y, center.z + offset);
                            end   = new Vector3(center.x + 100f, center.y, center.z + offset);
                        }
                        else
                        {
                            int k = (i - 1) / 2;
                            float offset = (k - (nZ - 1) * 0.5f) * spacing;
                            start = new Vector3(center.x + offset, center.y, center.z - 100f);
                            end   = new Vector3(center.x + offset, center.y, center.z + 100f);
                        }
                        break;
                    }
                    case FeatherAxisMode.Diagonal:
                    default:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z - 100f + offset);
                        end   = new Vector3(center.x + 100f, center.y, center.z + 100f + offset);
                        break;
                    }
                }

                if (StripMath.IsPointInsideStrip(start, end, _params.width, playerWorld))
                {
                    int lastIndex2 = effects.Count - 1;
                    for (int ei = 0; ei < lastIndex2; ei++)
                    {
                        AbilityEffect fx = effects[ei];
                        if (fx is IForceScaledEffect scalable)
                        {
                            int stacks = GetPlayerDebuffStacks != null ? GetPlayerDebuffStacks() : 0;
                            float distMeters = PerpendicularDistanceToLineXZ(
                                arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition()),
                                CurrentSpecialStart, CurrentSpecialEnd);
                            int distMul = Mathf.RoundToInt(distMeters);
                            scalable.SetForceScalers(stacks, distMul);
                        }
                        fx?.Execute(caster, target);
                    }
                }
            }
        }

        // Desenha a seta única saindo do player, apontando para push/pull em relação à special
        private void UpdateSingleArrow(Vector3 playerWorld, Vector3 axisStart, Vector3 axisEnd)
        {
            if (_singleArrow == null) return;

            Vector3 axis = axisEnd - axisStart; axis.y = 0f;
            if (axis.sqrMagnitude < 1e-6f) { _singleArrow.positionCount = 0; return; }
            axis.Normalize();

            // normal perpendicular à esquerda do eixo (plano XZ)
            Vector3 normal = new Vector3(-axis.z, 0f, axis.x);

            // PUSH usa -normal (empurrar para fora); PULL usa +normal (puxar para dentro)
            Vector3 dir = _isPushMode ? -normal : normal;

            // parâmetros visuais
            float y = 0.3f;
            float outOffset = Mathf.Max(0.35f, _params.width * 0.5f + 0.15f);
            float shaftLen  = Mathf.Max(0.75f, _params.width * 0.9f);
            float headLen   = shaftLen * 0.35f;
            float headHalfW = headLen * 0.6f;

            Vector3 origin = new Vector3(playerWorld.x, y, playerWorld.z) + dir * outOffset;
            Vector3 tip    = origin + dir * shaftLen;
            Vector3 tail   = origin - dir * 0.25f;

            Vector3 side = new Vector3(-dir.z, 0f, dir.x);
            Vector3 leftWing  = tip - dir * headLen + side * headHalfW;
            Vector3 rightWing = tip - dir * headLen - side * headHalfW;

            // cor: push (vermelho) / pull (azul)
            Color c = _isPushMode ? new Color(1f, 0.35f, 0.35f, 1f) : new Color(0.35f, 0.7f, 1f, 1f);
            _singleArrow.startColor = c;
            _singleArrow.endColor   = c;

            _singleArrow.enabled = true;
            _singleArrow.positionCount = 5;
            _singleArrow.SetPosition(0, tail);
            _singleArrow.SetPosition(1, tip);
            _singleArrow.SetPosition(2, leftWing);
            _singleArrow.SetPosition(3, tip);
            _singleArrow.SetPosition(4, rightWing);
        }

        public void Cleanup()
        {
            if (_views != null)
            {
                for (int i = 0; i < _views.Length; i++)
                {
                    if (_views[i] != null)
                    {
                        if (_views[i].Line != null)
                            Object.Destroy(_views[i].Line.gameObject);
                    }
                }
            }

            if (_singleArrow != null)
            {
                Object.Destroy(_singleArrow.gameObject);
                _singleArrow = null;
            }

            _views = null;
        }
    }
}
