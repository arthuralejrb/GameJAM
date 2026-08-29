using Godot;

namespace GameJAM.Scripts.Gameplay
{
    public partial class CardView : Control
    {
        [Export] public TextureRect CardSprite;
        [Export] public TextureRect TrapOverlay;

        public void SetupCard(Card card, bool hideSecretValues)
        {
            if (CardSprite == null) return;

            string suitName = card.cardSuit.ToString();
            string spritePath = "";

            // CARTA NORMAL: Sempre carrega o sprite comum da pasta Cartas
            if (card.cardType == CardType.Normal)
            {
                int val = hideSecretValues ? card.visibleValue : card.realValue;
                spritePath = $"res://Assets/Cartas/{suitName}/{val}.png";
            }
            // CARTA FALSA / ILLUSORY:
            else if (card.cardType == CardType.Illusory)
            {
                if (hideSecretValues)
                {
                    // ANTES DO STAND: Exibe a carta adulterada (trapCards) mostrando o valor FALSO/visível
                    spritePath = $"res://Assets/trapCards/{suitName}/{card.visibleValue}.png";

                    // Fallback caso não tenha a imagem em trapCards
                    if (!ResourceLoader.Exists(spritePath))
                    {
                        spritePath = $"res://Assets/Cartas/{suitName}/{card.visibleValue}.png";
                    }
                }
                else
                {
                    // NO STAND: A farsa acaba e revela o valor REAL na pasta normal de Cartas
                    spritePath = $"res://Assets/Cartas/{suitName}/{card.realValue}.png";
                }
            }

            // Carrega a textura final
            if (ResourceLoader.Exists(spritePath))
            {
                CardSprite.Texture = GD.Load<Texture2D>(spritePath);
            }
            else
            {
                GD.PrintErr($"[CardView] Sprite não encontrado em: {spritePath}");
            }

            // Ativa o overlay visual/brilho de trapaça se você quiser dar mais destaque
            if (TrapOverlay != null)
            {
                TrapOverlay.Visible = (card.cardType == CardType.Illusory);
            }
        }
    }
}