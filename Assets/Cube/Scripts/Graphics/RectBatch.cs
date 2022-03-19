using System;

using UnityEngine;

namespace Cube.Graphics
{
    public sealed class RectBatch : MonoBehaviour
    {
        private const int VerticesPerRect = 4;
        private const int IndicesPerRect = 6; // 2 triangles

        [SerializeField]
        private MeshFilter _meshFilter = null;

        private int _rectCapacity = 0;
        private Vector3[] _vertices = null;
        private Vector2[] _uv = null;
        private int[] _indices = null;
        private Mesh _mesh = null;
        private int _rectIndex = 0;

        public void Init(int rectCapacity)
        {
            if (rectCapacity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rectCapacity),
                    $"{nameof(rectCapacity)} must be at least 1."
                );
            }

            DestroyMeshIfExists();

            _rectCapacity = rectCapacity;

            int vertexCount = rectCapacity * VerticesPerRect;
            Vector3[] vertices = new Vector3[vertexCount];
            Vector2[] uv = new Vector2[vertexCount];
            int[] indices = new int[rectCapacity * IndicesPerRect];

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = indices;

            _vertices = vertices;
            _uv = uv;
            _indices = indices;
            _mesh = mesh;
            _meshFilter.sharedMesh = _mesh;
        }

        public void Fini()
        {
            _rectCapacity = 0;
            _vertices = null;
            _uv = null;
            _indices = null;
            DestroyMeshIfExists();
        }

        public void Begin()
        {
            if (_mesh == null)
            {
                throw CreateNotInitException();
            }

            _rectIndex = 0;
        }

        public void PushRect(Rect rect, Rect uv)
        {
            if (_mesh == null)
            {
                throw CreateNotInitException();
            }

            int rectIndex = _rectIndex;
            if (rectIndex >= _rectCapacity)
            {
                Debug.LogWarning("Cannot push rect. Batch is full.");
                return;
            }

            int vertexOffset = rectIndex * VerticesPerRect;
            PushRect(_vertices, vertexOffset, rect, (x, y) => new Vector3(x, y));
            PushRect(_uv, vertexOffset, uv, (x, y) => new Vector2(x, y));
            PushTriangleIndices(_indices, rectIndex * IndicesPerRect, vertexOffset);
            ++_rectIndex;
        }

        public void End()
        {
            Mesh mesh = _mesh;
            if (_mesh == null)
            {
                throw CreateNotInitException();
            }

            // Degenerate excess triangles to prevent rendering artifacts.
            DegenerateTriangles(_indices, _rectIndex * IndicesPerRect);
            mesh.vertices = _vertices;
            mesh.uv = _uv;
            mesh.triangles = _indices;
            mesh.RecalculateBounds();
        }

        private void DestroyMeshIfExists()
        {
            Mesh mesh = _mesh;
            if (mesh != null)
            {
                Destroy(mesh);
                _mesh = null;
            }
        }

        private static InvalidOperationException CreateNotInitException()
        {
            return new InvalidOperationException($"{nameof(Init)} has not been executed successfully.");
        }

        private static void DegenerateTriangles(int[] indices, int indicesIndex)
        {
            int count = indices.Length;
            for (; indicesIndex < count; ++indicesIndex)
            {
                indices[indicesIndex] = 0;
            }
        }

        private static void PushRect<T>(T[] buffer, int bufferIndex, Rect rect, Func<float, float, T> createItem)
        {
            Vector2 min = rect.min;
            Vector2 max = rect.max;
            buffer[bufferIndex] = createItem(min.x, min.y);
            buffer[bufferIndex + 1] = createItem(min.x, max.y);
            buffer[bufferIndex + 2] = createItem(max.x, max.y);
            buffer[bufferIndex + 3] = createItem(max.x, min.y);
        }

        private static void PushTriangleIndices(int[] indices, int indicesIndex, int vertexOffset)
        {
            indices[indicesIndex] = vertexOffset;
            indices[indicesIndex + 1] = vertexOffset + 1;
            indices[indicesIndex + 2] = vertexOffset + 2;
            indices[indicesIndex + 3] = vertexOffset;
            indices[indicesIndex + 4] = vertexOffset + 2;
            indices[indicesIndex + 5] = vertexOffset + 3;
        }
    }
}
