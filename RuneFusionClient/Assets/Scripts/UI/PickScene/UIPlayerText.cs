using TMPro;
using UnityEngine;

public class UIPlayerText: UIBase
{
        [SerializeField] private TextMeshProUGUI player1NameText;
        [SerializeField] private TextMeshProUGUI player2NameText;

        public void SetNameText(string player1Name, string player2Name)
        {
                this.player1NameText.text = player1Name + "1";
                this.player2NameText.text = player2Name +"2";
                if (PickSceneUIManager.Instance.PLayerIndex == 0)
                {
                        this.player1NameText.color = Color.red;
                        this.player2NameText.color = Color.white;
                }
                else
                {
                        this.player1NameText.color = Color.white;
                        this.player2NameText.color = Color.red;
                }
        }
}