using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace Logic.Scripts.GameDomain.MVC.Ui {
    public class GamePlayUiView : MonoBehaviour {

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