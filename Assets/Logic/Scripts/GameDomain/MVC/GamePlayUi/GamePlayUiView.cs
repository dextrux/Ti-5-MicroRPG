using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace Logic.Scripts.GameDomain.MVC.Ui {
    public class GamePlayUiView : MonoBehaviour {

        public static GamePlayUiView Instance;

        [SerializeField] GamePlayUiBindSO _gamePlayUiBindSO;

        public Length ActualBosshealthPercent;
        public Length PreviewBossHealthPercent;
        public int ActualBossLife;

        public Length ActualPlayerLifePercent;
        public Length PreviewPlayerLifePercent;
        public int ActualPlayerHealth;
        public int PlayerActionPoints;

        public int Skill1Cost;
        public int Skill2Cost;
        public int Skill3Cost;

        public string Skill1Name;
        public string Skill2Name;
        public string Skill3Name;

        private void Start() {
            Instance = this;
        }

        private void Update() {
            if (_gamePlayUiBindSO == null) return;

            _gamePlayUiBindSO.ActualBosshealthPercent = ActualBosshealthPercent;
            _gamePlayUiBindSO.PreviewBossHealthPercent = PreviewBossHealthPercent;
            _gamePlayUiBindSO.ActualBossLife = ActualBossLife;

            _gamePlayUiBindSO.ActualPlayerLifePercent = ActualPlayerLifePercent;
            _gamePlayUiBindSO.PreviewPlayerLifePercent = PreviewPlayerLifePercent;
            _gamePlayUiBindSO.ActualPlayerHealth = ActualPlayerHealth;
            _gamePlayUiBindSO.PlayerActionPoints = PlayerActionPoints;

            _gamePlayUiBindSO.Skill1Cost = Skill1Cost;
            _gamePlayUiBindSO.Skill2Cost = Skill2Cost;
            _gamePlayUiBindSO.Skill3Cost = Skill3Cost;

            _gamePlayUiBindSO.Skill1Name = Skill1Name;
            _gamePlayUiBindSO.Skill2Name = Skill2Name;
            _gamePlayUiBindSO.Skill3Name = Skill3Name;
        }


        private Button _setSkillSet1Btn;
        private Button _setSkillSet2Btn;
        private Button _setSkillSet3Btn;
        private Button _useSkill1Btn;
        private Button _useSkill2Btn;
        private Button _useSkill3Btn;

        private Button _nextTurnBtn;


        public void InitStartPoint() {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            _setSkillSet1Btn = root.Q<Button>("Skill-Set1-btn");
            _setSkillSet2Btn = root.Q<Button>("Skill-Set2-btn");
            _setSkillSet3Btn = root.Q<Button>("Skill-Set3-btn");
            _useSkill1Btn = root.Q<Button>("Ability-Slot1-btn");
            _useSkill2Btn = root.Q<Button>("Ability-Slot2-btn");
            _useSkill3Btn = root.Q<Button>("Ability-Slot3-btn");

            _nextTurnBtn = root.Q<Button>("Next-Turn-btn");
        }

        public void ShowGameOverPanel(int score, int scoreGoal, bool shouldShowScore, CancellationTokenSource cancellationTokenSource) {

        }

        public void SwitchToInGameView() {

        }

        public void SetStartingValues(int score, CancellationTokenSource cancellationTokenSource) {

        }

        public void ShowWinPanel(int winScore, CancellationTokenSource cancellationTokenSource) {

        }

        public void InitExitPoint() {

        }
    }

}