using System.Collections.Generic;
using UnityEngine;

namespace DLearnersApplication
{
    public class ApplicationUIManager : Singleton<ApplicationUIManager>
    {
        [SerializeField] GameIconButton gameIconButtonPF;
        [SerializeField] Transform buttonSpawnIG, buttonSpawnLW;
        [SerializeField] int immersiveGameCount, liveWorksheetCount;

        [Header ("Debug")]
        [SerializeField] List<GameIconButton> immersiveGameIconButtons = new List<GameIconButton>();
        [SerializeField] List<GameIconButton> liveWorksheetIconButtons = new List<GameIconButton>();

        private void OnEnable()
        {
            InitApplicationUIManager();
        }
        private void OnDisable()
        {
            DisposeApplicationUIData();
        }

        private void InitApplicationUIManager()
        {
            immersiveGameCount = GetImmersiveGameCount();
            liveWorksheetCount = GetLiveWorksheetCount();

            for (int i = 0; i < immersiveGameCount; i++)
            {
                GameIconButton cashImmersiveGameIconButtons = Instantiate(gameIconButtonPF, buttonSpawnIG);
                cashImmersiveGameIconButtons.InitGameIconButton(i);
                immersiveGameIconButtons.Add(cashImmersiveGameIconButtons);
            }

            for (int i = 0; i < liveWorksheetCount; i++)
            {
                GameIconButton cashLiveWorksheetIconButtons = Instantiate(gameIconButtonPF, buttonSpawnLW);
                cashLiveWorksheetIconButtons.InitGameIconButton(i);
                liveWorksheetIconButtons.Add(cashLiveWorksheetIconButtons);
            }
        }

        public int GetImmersiveGameCount()
        {
            return 3;
        }
        public int GetLiveWorksheetCount()
        {
            return 4;
        }

        private void DisposeApplicationUIData()
        {
            int cashImmersiveGameIconButtonsCount = immersiveGameIconButtons.Count;
            for (int i = 0; i < cashImmersiveGameIconButtonsCount; i++)
            {
                Destroy(immersiveGameIconButtons[i].gameObject);
            }

            int cashLiveWorksheetIconButtonsCount = liveWorksheetIconButtons.Count;
            for (int i = 0; i < cashLiveWorksheetIconButtonsCount; i++)
            {
                Destroy(liveWorksheetIconButtons[i].gameObject);
            }

            immersiveGameIconButtons.Clear();
            liveWorksheetIconButtons.Clear();
        }

        public void SetApplicationUIOnOff(bool isOn)
        {
            gameObject.SetActive(isOn);
        }
    }
}