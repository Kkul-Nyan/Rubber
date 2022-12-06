using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class CreateLobbyScreen : MonoBehaviour
{

    [SerializeField] private TMP_InputField _nameInput, _maxPlayersInput;
    [SerializeField] private TMP_Dropdown _difficultDB, _MapDB;
    void Start()
    {
        void SetOption(TMP_Dropdown dropdown, IEnumerable<string> values){
            dropdown.options = values.Select(type => new TMP_Dropdown.OptionData{text = type}).ToList();
        }

        SetOption(_difficultDB, Constant.Difficulties);
        SetOption(_MapDB, Constant.MapKeys);
    }

    public static event Action<LobbyData> LobbyCreated;
    public void OnCreatedClicked()
    {

        var lobbyData = new LobbyData
        {
            Name = _nameInput.text,
            MaxPlayers = int.Parse(_maxPlayersInput.text),
            Difficulty = _difficultDB.value,
            Map = _MapDB.value.ToString(),
        };
        LobbyCreated?.Invoke(lobbyData);
    }
}
    public struct LobbyData {
        public string Name;
        public int MaxPlayers;
        public int Difficulty;
        public string Map;
    }

