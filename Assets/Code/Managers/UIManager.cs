using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ZXM.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [SerializeField] private GameObject UIMainMenu;
        [SerializeField] private GameObject UIPauseMenu;
        [SerializeField] private GameObject UIInGameMenu;
        [SerializeField] private GameObject UIGameOvermenu;
        [SerializeField] private GameObject UICompletedMenu;

        [SerializeField] private TMP_Text UIScoreText;
        [SerializeField] private TMP_Text UILevelText;
        [SerializeField] private TMP_Text UIGameOverScoreText;
        [SerializeField] private TMP_Text UIGameOverText;
        [SerializeField] private TMP_Text UIGameOverTextShadow;
        [SerializeField] private TMP_Text UICompletedScoreText;



        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Setting UI Screen Functions
        public void SetMainMenu(bool value)
        {
            UIMainMenu.SetActive(value);

        }

        public void SetPauseMenu(bool value)
        {
            UIPauseMenu.SetActive(value);
        }

        public void SetInGameMenu(bool value)
        {
            UIInGameMenu.SetActive(value);
        }

        public void SetGameOverMenu(bool value)
        {
            UIGameOvermenu.SetActive(value);
        }

        public void SetCompletedMenu(bool value)
        {
            UICompletedMenu.SetActive(value);
        }

        #endregion

        #region Changing Text Functions 
        public void UpdateScoreUI()
        {
            UIScoreText.text = ScoreManager.Instance.Score.ToString();

        }
        public void UpdateGameOverScoreUI()
        {
            UIGameOverScoreText.text = ScoreManager.Instance.Score.ToString();

        }

        public void UpdateCompletedScoreUI()
        {
            UICompletedScoreText.text = ScoreManager.Instance.Score.ToString();

        }

        public void UpdateLevelUI(string value)
        {
            UILevelText.text = value;

        }

        public void UpdateGameOverText(string value)
        {
            UIGameOverText.text = value;
            UIGameOverTextShadow.text = value;
        }

        public void ResetScoreAndLevel()
        {
            UIScoreText.text = "0";
            UILevelText.text = "0";

        }
        #endregion

    }
}
