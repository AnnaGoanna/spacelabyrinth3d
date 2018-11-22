using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScripts
{
    public class Game : MonoBehaviour
    {
        internal bool AllowExit { private get; set; }

        private static readonly string[] LevelNames =
        {
            "Levels/level1.level",
            "Levels/level2.level",
            "Levels/level3.level"
        };

        private int _levels = LevelNames.Length;

        private string _customLevelName;
        private bool isCustom;

        private int _currentLevelNumber;

        private Level _level;

        private CustomLevelData _customLevelData;
        private SoundtrackManager _soundtrackManager;
        private SkinManager _skinManager;

        public GameObject Player;
        public Text LevelText;


        private void Start()
        {
            _level = gameObject.AddComponent<Level>();
            _level.Material =
                (Material) Resources.Load("Textures/Sci-Fi Texture Pack 1/Materials/Texture_4", typeof(Material));
            _level.Scale = 30.0f;

            _customLevelData = FindObjectOfType<CustomLevelData>();
            _soundtrackManager = gameObject.AddComponent<SoundtrackManager>();
            _skinManager = gameObject.AddComponent<SkinManager>();

            if (_customLevelData == null) isCustom = false;
            else
            {
                _customLevelName = _customLevelData.GetCustomLevelName();
                if (string.IsNullOrEmpty(_customLevelName))
                {
                    isCustom = false;
                }
                else
                {
                    isCustom = true;
                    _levels = 1;
                    _customLevelData.SetCustomLevelName(null);
                }
            }

            _currentLevelNumber = -1;

            LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            AllowExit = false;

            _currentLevelNumber++;
            if (_currentLevelNumber >= _levels)
            {
                RollCredits();
                return;
            }

            Player.GetComponent<NewtonianMovementPrototype>().StopMovement();

            _level.Material = _skinManager.GetLevelMaterial(_currentLevelNumber, isCustom);

            string levelPath;
            if (isCustom) levelPath = _customLevelName;
            else levelPath = string.Format("{0}/{1}", Application.streamingAssetsPath, LevelNames[_currentLevelNumber]);
            _level.LoadLevel(levelPath);
            var errors = _level.Validate();

            if (errors.Count != 0)
            {
                ModalFilePicker.InstantiateMessageDialog("Error parsing level: " + Level.BuildErrorMessage(errors), "Exit", BackToMainMenu);
            }
            else
            {
                Debug.Log("Player start position" + _level.StartPosition);
                Player.transform.localPosition = _level.StartPosition;
                Player.transform.localRotation = Quaternion.LookRotation(_level.StartingDirection);

                foreach (var exit in _level.Exits)
                {
                    var sphereCollider = exit.AddComponent<SphereCollider>();
                    sphereCollider.isTrigger = true;

                    var exitTrigger = exit.AddComponent<ExitTrigger>();
                    exitTrigger.Game = this;
                    exitTrigger.Player = Player;
                }

                _soundtrackManager.ChangeSoundTrack(_currentLevelNumber, isCustom);
            }
        }

        private void BackToMainMenu(string s)
        {
            SceneManager.LoadScene(0);
        }

        private static void RollCredits()
        {
            PlayerPrefs.SetInt("RollCredits", 1);
            SceneManager.LoadScene(0);
        }

        private void Update()
        {
            if (AllowExit && Input.GetButton("NextLevel"))
            {
                LoadNextLevel();
            }

            LevelText.gameObject.SetActive(AllowExit);
        }
    }
}
