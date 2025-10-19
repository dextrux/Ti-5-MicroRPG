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
        private static bool _togglePushPull;
        public static System.Func<int> GetPlayerDebuffStacks;

        public FeatherLinesHandler(FeatherLinesParams p)
        {
            _params = p;
        }

        public void PrepareTelegraph(Transform parentTransform)
        {
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
                v.Mesh = new Mesh();
                v.Mesh.name = "FeatherStripMesh";
                v.MeshFilter.sharedMesh = v.Mesh;

                _views[i] = v;
            }
            // Update telegraph now using world-space alignment at the parent's position
            UpdateTelegraphGeometryAtCenter(parentTransform.position);
        }

        private void UpdateTelegraphGeometryAtCenter(Vector3 center)
        {
            float spacing = Mathf.Max(0.1f, _params.margin);

            int n = _views.Length;
            for (int i = 0; i < n; i++)
            {
                Vector3 start;
                Vector3 end;
                switch (_params.axisMode)
                {
                    case FeatherAxisMode.X:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Z:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x + offset, center.y, center.z - 100f);
                        end = new Vector3(center.x + offset, center.y, center.z + 100f);
                        break;
                    }
                    case FeatherAxisMode.XZ:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Diagonal:
                    default:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z - 100f + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + 100f + offset);
                        break;
                    }
                }

                Vector3[] vertsWorld = StripMath.GenerateStripVertices(start, end, _params.width);
                for (int p = 0; p < vertsWorld.Length; p++) vertsWorld[p].y = 0.2f;
                // Line in world space at y = -1.75
                _views[i].Line.positionCount = vertsWorld.Length;
                _views[i].Line.SetPositions(vertsWorld);
                // Mesh as child of parent at local Y = -1.75; fill mesh vertices in local space
                Transform mT = _views[i].MeshFilter.transform;
                mT.localPosition = new Vector3(0f, 0.2f, 0f);
                mT.localRotation = Quaternion.identity;
                Vector3 v0L = mT.InverseTransformPoint(vertsWorld[0]);
                Vector3 v1L = mT.InverseTransformPoint(vertsWorld[1]);
                Vector3 v2L = mT.InverseTransformPoint(vertsWorld[2]);
                Vector3 v3L = mT.InverseTransformPoint(vertsWorld[3]);
                _views[i].Mesh.Clear();
                _views[i].Mesh.vertices = new Vector3[] { v0L, v1L, v2L, v3L };
                _views[i].Mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
                _views[i].Mesh.RecalculateNormals();
                _views[i].Mesh.RecalculateBounds();
            }
        }

        public bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            Vector3 center = arenaReference != null ? arenaReference.transform.position : originTransform.position;
            Vector3 playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
            float spacing = Mathf.Max(0.1f, _params.margin);
            int n = _views.Length;
            for (int i = 0; i < n; i++)
            {
                Vector3 start;
                Vector3 end;
                switch (_params.axisMode)
                {
                    case FeatherAxisMode.X:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Z:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x + offset, center.y, center.z - 100f);
                        end = new Vector3(center.x + offset, center.y, center.z + 100f);
                        break;
                    }
                    case FeatherAxisMode.XZ:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Diagonal:
                    default:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z - 100f + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + 100f + offset);
                        break;
                    }
                }
                if (i == _specialIndex)
                {
                    // Special feather: we will apply Effect[1] later regardless of hit.
                }
                // World-space polygon from current view for robust hit
                Mesh mesh = _views[i].Mesh;
                Transform mT = _views[i].MeshFilter.transform;
                Vector3[] verts = new Vector3[4];
                Vector3[] mv = mesh.vertices;
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
            if (A < 0.0)
            {
                s = -s; t = -t; A = -A;
            }
            return s > 0 && t > 0 && (s + t) < A;
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

            yield return ExecuteFeatherSequence(effects, arenaReference, originTransform, caster, target, center, spacing, n, playerWorld);
        }

        private System.Collections.IEnumerator ExecuteFeatherSequence(List<AbilityEffect> effects, ArenaPosReference arenaReference, Transform originTransform, IEffectable caster, IEffectable target, Vector3 center, float spacing, int n, Vector3 playerWorld)
        {
            // 1) Special feather sequence: hide line -> wait -> DAMAGE hit-check/apply -> wait -> DISPLACEMENT always -> wait
            // Identify special line geometry once
            float specialOffset = (_specialIndex - (n - 1) * 0.5f) * spacing;
            Vector3 sStart, sEnd;
            if (_params.axisMode == FeatherAxisMode.X)
            {
                sStart = new Vector3(center.x - 100f, center.y, center.z + specialOffset);
                sEnd = new Vector3(center.x + 100f, center.y, center.z + specialOffset);
            }
            else if (_params.axisMode == FeatherAxisMode.Z)
            {
                sStart = new Vector3(center.x + specialOffset, center.y, center.z - 100f);
                sEnd = new Vector3(center.x + specialOffset, center.y, center.z + 100f);
            }
            else if (_params.axisMode == FeatherAxisMode.Diagonal)
            {
                sStart = new Vector3(center.x - 100f, center.y, center.z - 100f + specialOffset);
                sEnd = new Vector3(center.x + 100f, center.y, center.z + 100f + specialOffset);
            }
            else
            {
                sStart = new Vector3(center.x - 100f, center.y, center.z + specialOffset);
                sEnd = new Vector3(center.x + 100f, center.y, center.z + specialOffset);
            }

            // Hide special line and wait
            LineRenderer sLr = (_views != null && _specialIndex >= 0 && _specialIndex < _views.Length) ? _views[_specialIndex].Line : null;
            if (sLr != null) sLr.enabled = false;
            MeshRenderer sMr = (_views != null && _specialIndex >= 0 && _specialIndex < _views.Length) ? _views[_specialIndex].MeshRenderer : null;
            if (sMr != null) sMr.enabled = false;
            yield return new WaitForSeconds(0.5f);

            // 1.a) Special DAMAGE: Effect[0] if player is inside special strip (based on current playerWorld)
            if (effects.Count > 0 && StripMath.IsPointInsideStrip(sStart, sEnd, _params.width, playerWorld))
            {
                AbilityEffect fx0 = effects[0];
                Debug.Log("FeatherLinesHandler: applying Special Effect[0] due to hit");
                fx0?.Execute(caster, target);
            }

            // Wait before displacement
            yield return new WaitForSeconds(0.5f);

            // 1.b) Special EFFECT[1]: apply across arena (ignore displacement movement for now)
            if (effects.Count > 1)
            {
                AbilityEffect fx1 = effects[1];
                if (fx1 != null)
                {
                    if (fx1 is Assets.Logic.Scripts.GameDomain.Effects.DisplacementEffect disp)
                    {
                        // Ignore actual movement for now, but still mark effect as executed
                        disp.baseDistance = 0f;
                    }

                    Debug.Log($"FeatherLinesHandler: applying special Effect[1] push={_togglePushPull}");
                    fx1.Execute(caster, target);
                    yield return new WaitForSeconds(0.5f);
                    // Recompute player position after displacement
                    playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
                }
            }

            // Proceed immediately to normal feathers after the post-displacement wait above

            // 2) Apply Effect[0] for any feather that hit (normal hitbox order)
            for (int i = 0; i < n && effects.Count > 0; i++)
            {
                if (i == _specialIndex) continue; // skip special, already processed
                Vector3 start; Vector3 end;
                switch (_params.axisMode)
                {
                    case FeatherAxisMode.X:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Z:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x + offset, center.y, center.z - 100f);
                        end = new Vector3(center.x + offset, center.y, center.z + 100f);
                        break;
                    }
                    case FeatherAxisMode.XZ:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + offset);
                        break;
                    }
                    case FeatherAxisMode.Diagonal:
                    default:
                    {
                        float offset = (i - (n - 1) * 0.5f) * spacing;
                        start = new Vector3(center.x - 100f, center.y, center.z - 100f + offset);
                        end = new Vector3(center.x + 100f, center.y, center.z + 100f + offset);
                        break;
                    }
                }
                if (StripMath.IsPointInsideStrip(start, end, _params.width, playerWorld))
                {
                    AbilityEffect fx0 = effects[0];
                    Debug.Log("FeatherLinesHandler: applying Effect[0] due to hit");
                    fx0?.Execute(caster, target);
                }
                // No additional delay after normal feather processing
            }
        }

        public void Cleanup()
        {
            if (_views == null) return;
            for (int i = 0; i < _views.Length; i++)
            {
                if (_views[i] != null)
                {
                    Object.Destroy(_views[i].Line?.gameObject);
                }
            }
            _views = null;
        }
    }
}


