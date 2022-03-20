using UnityEngine;

namespace Cube.Result
{
    public sealed class ResultView : MonoBehaviour
    {
        [SerializeField]
        private GameObject _levelCompletedView = null;
        [SerializeField]
        private GameObject _levelFailedView = null;

        public void Init(bool levelCompleted)
        {
            _levelCompletedView.SetActive(levelCompleted);
            _levelFailedView.SetActive(!levelCompleted);
        }
    }
}
