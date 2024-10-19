
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class LineRendererProgressor : MonoBehaviour
{
    private LineRenderer LineRenderer
    {
        get
        {
            if (lineRenderer) return lineRenderer;
            return lineRenderer = GetComponent<LineRenderer>();
        }
    }
    private LineRenderer lineRenderer;

    public Vector3[] positions = new Vector3[31];

    private void Start()
    {
        // ��{�I��LineRenderer�̐ݒ�
        LineRenderer.startWidth = 1.0f;  // ���̑����𒲐�
        LineRenderer.endWidth = 1.0f;
        LineRenderer.useWorldSpace = true;  // ���E���W���g�p����

        // �܂���p�̃}�e���A����ݒ�iUnlit Color�ȂǁA�V���v���ȃ}�e���A�����g�p�j
        if (LineRenderer.sharedMaterial == null)  // sharedMaterial�ɕύX
        {
            LineRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
        }

        LineRenderer.sharedMaterial.color = Color.yellow;  // ���̐F��ɐݒ�
        LineRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
    }

    public float Progress
    {
        get => progress;
        set
        {
            progress = value;

            if (positions == null || positions.Length == 0) return;
            UpdateLineRenderer();
        }
    }
    [SerializeField] private float progress;

    private void OnValidate()
    {
        if (positions == null || positions.Length == 0) return;
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        // �l�̐����ƏI������
        progress = Mathf.Clamp01(progress);
        if (positions.Length == 1 || progress <= 0.0f)
        {
            LineRenderer.positionCount = 1;
            LineRenderer.SetPosition(0, positions[0]);
            return;
        }
        if (progress >= 1.0f)
        {
            LineRenderer.positionCount = positions.Length;
            LineRenderer.SetPositions(positions);
            return;
        }

        // �������X�g�ƍ��v
        var distanceList = MakeDistanceList(positions);
        var totalDistance = distanceList.Sum();

        // ���݂ǂ̐����ɂ��邩�擾
        var distanceCompleted = totalDistance * progress;
        var currentIndex = GetIndexByDistanceCompleted(positions, distanceList, distanceCompleted);

        // ��[�ʒu���v�Z
        var distanceArrayToCurIdx = distanceList.Take(currentIndex);
        var completedDistInCurSeg = distanceCompleted - distanceArrayToCurIdx.Sum();
        var currentSegLength = distanceList[currentIndex];
        var t = completedDistInCurSeg / currentSegLength;
        var edgePos = Vector3.Lerp(positions[currentIndex], positions[currentIndex + 1], t);

        // �X�V
        LineRenderer.positionCount = currentIndex + 2;

        for (var i = 0; i < currentIndex + 2; ++i)
        {
            LineRenderer.SetPosition(i, i > currentIndex ? edgePos : positions[i]);
        }
    }

    private IReadOnlyList<float> MakeDistanceList(Vector3[] points)
    {
        var distanceList = new List<float>();
        for (var i = 0; i < points.Length - 1; ++i)
        {
            distanceList.Add(Vector3.Distance(points[i], points[i + 1]));
        }

        return distanceList;
    }

    private int GetIndexByDistanceCompleted(Vector3[] points, IReadOnlyList<float> distanceList, float distanceCompleted)
    {
        var distanceProcessed = 0f;
        for (var i = 0; i < points.Length - 1; ++i)
        {
            if (distanceList[i] + distanceProcessed > distanceCompleted)
            {
                return i;
            }

            distanceProcessed += distanceList[i];
        }

        return points.Length - 1;
    }

    // ���̃X�N���v�g������W��ݒ肷�邽�߂̃��\�b�h
    public void SetPosition(int index, Vector3 position)
    {
        if (index < 0 || index >= positions.Length)
        {
            Debug.LogError("Index out of bounds.");
            return;
        }
        positions[index] = position;
        UpdateLineRenderer();
    }
}
