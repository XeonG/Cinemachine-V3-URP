using System;
using Unity.Cinemachine;
using Unity.Cinemachine.Editor;
using UnityEngine;

namespace Cinemachine.Examples {
    [RequireComponent(typeof(CinemachineCamera))]
    [SaveDuringPlay] // Enable SaveDuringPlay for this class
    public class MouseScrollZoom2D : MonoBehaviour {
        [Range(0, 10)] public float ZoomMultiplier = 1f;
        [Range(0, 100)] public float MinZoom = 1f;
        [Range(0, 100)] public float MaxZoom = 50f;

        public bool reverseZoomDirection = false;
        CinemachineCamera _mVirtualCamera;
        float _mOriginalOrthoSize;
        private float _zoom = 0f;

        void Awake() {
            _mVirtualCamera = GetComponent<CinemachineCamera>();
            _mOriginalOrthoSize = _mVirtualCamera.Lens.OrthographicSize;

#if UNITY_EDITOR
            // This code shows how to play nicely with the VirtualCamera's SaveDuringPlay functionality
            SaveDuringPlay.OnHotSave -= RestoreOriginalOrthographicSize;
            SaveDuringPlay.OnHotSave += RestoreOriginalOrthographicSize;
#endif
        }

#if UNITY_EDITOR
        void OnDestroy() {
            SaveDuringPlay.OnHotSave -= RestoreOriginalOrthographicSize;
        }

        void RestoreOriginalOrthographicSize() {
            _mVirtualCamera.Lens.OrthographicSize = _mOriginalOrthoSize;
        }
#endif

        void OnValidate() {
            MaxZoom = Mathf.Max(MinZoom, MaxZoom);
        }

        void Update() {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (reverseZoomDirection) {
                _zoom = _mVirtualCamera.Lens.OrthographicSize + (Input.mouseScrollDelta.y * -1) * ZoomMultiplier;
            }
            else {
                _zoom = _mVirtualCamera.Lens.OrthographicSize + (Input.mouseScrollDelta.y * ZoomMultiplier);
            }
            _mVirtualCamera.Lens.OrthographicSize = Mathf.Clamp(_zoom, MinZoom, MaxZoom);
#else
            InputSystemHelper.EnableBackendsWarningMessage();
#endif
        }
    }
}