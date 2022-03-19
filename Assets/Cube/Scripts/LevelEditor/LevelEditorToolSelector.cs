using System;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

using Cube.Utils;

namespace Cube.LevelEditor
{
    [Serializable]
    public struct ToolData
    {
        public string Name;
        public GameObject[] GameObjects;
    }

    public sealed class LevelEditorToolSelector : MonoBehaviour
    {
        [SerializeField]
        private ToolData[] _tools = { };
        [SerializeField]
        private TMP_Dropdown _dropdown = null;

        private void Awake()
        {
            TMP_Dropdown dropdown = _dropdown;
            dropdown.options = CreateDropdownOptions();
            TMP_Dropdown.DropdownEvent onValueChanged = dropdown.onValueChanged;
            onValueChanged.RemoveAllListeners();
            onValueChanged.AddListener(OnDropdownValueChanged);
            OnDropdownValueChanged(dropdown.value);
        }

        public void SelectTool(int toolIndex)
        {
            _dropdown.value = toolIndex;
        }

        private List<TMP_Dropdown.OptionData> CreateDropdownOptions()
        {
            ToolData[] tools = _tools;
            int count = tools.Length;
            List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>(count);
            for (int i = 0; i < count; ++i)
            {
                dropdownOptions.Add(new TMP_Dropdown.OptionData(tools[i].Name));
            }
            return dropdownOptions;
        }

        private void OnDropdownValueChanged(int value)
        {
            ToolData[] tools = _tools;
            int count = tools.Length;
            for (int i = 0; i < count; ++i)
            {
                GameObjectUtils.SetGameObjectsActive(tools[i].GameObjects, i == value);
            }
        }
    }
}
