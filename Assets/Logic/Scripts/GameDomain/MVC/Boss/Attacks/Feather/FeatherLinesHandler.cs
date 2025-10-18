using UnityEngine;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Core;
using Logic.Scripts.GameDomain.MVC.Boss.Attacks.Shared;

namespace Logic.Scripts.GameDomain.MVC.Boss.Attacks.Feather
{
    public class FeatherLinesHandler : IBossAttackHandler
    {
        private readonly FeatherLinesParams _params;
        private LineRenderer[] _renderers;

        public FeatherLinesHandler(FeatherLinesParams p)
        {
            _params = p;
        }

        public void PrepareTelegraph(Transform parentTransform)
        {
            int n = Mathf.Max(1, _params.featherCount);
            _renderers = new LineRenderer[n];
            for (int i = 0; i < n; i++)
            {
                GameObject go = new GameObject("FeatherSubActionView");
                go.transform.SetParent(parentTransform, false);
                LineRenderer lr = go.AddComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.useWorldSpace = true;
                lr.loop = true;
                lr.widthMultiplier = 0.1f;
                _renderers[i] = lr;
            }
            UpdateTelegraphGeometry(parentTransform);
        }

        private void UpdateTelegraphGeometry(Transform origin)
        {
            Vector3 center = origin.position;
            float spacing = Mathf.Max(0.1f, _params.margin);

            int n = _renderers.Length;
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

                Vector3[] verts = StripMath.GenerateStripVertices(start, end, _params.width);
                for (int p = 0; p < verts.Length; p++) verts[p].y = 1f;
                _renderers[i].positionCount = verts.Length;
                _renderers[i].SetPositions(verts);
            }
        }

        public bool ComputeHits(ArenaPosReference arenaReference, Transform originTransform, IEffectable caster)
        {
            Vector3 center = originTransform.position;
            Vector3 playerWorld = arenaReference.RelativeArenaPositionToRealPosition(arenaReference.GetPlayerArenaPosition());
            float spacing = Mathf.Max(0.1f, _params.margin);
            int n = _renderers.Length;
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
                if (StripMath.IsPointInsideStrip(start, end, _params.width, playerWorld)) return true;
            }
            return false;
        }

        public void Cleanup()
        {
            if (_renderers == null) return;
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] != null)
                {
                    Object.Destroy(_renderers[i].gameObject);
                }
            }
            _renderers = null;
        }
    }
}


