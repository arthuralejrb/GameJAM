using Godot;

namespace GameJAM.Scripts.Gameplay
{
    public partial class CardView : Control
    {
        [Export] public TextureRect CardSprite;
        [Export] public TextureRect TrapOverlay;

        public Card CardData { get; private set; }
        public System.Action<Card, CardView> OnCardClicked;

        public void SetupCard(Card card, bool hideSecretValues)
        {
            CardData = card;
            if (CardSprite == null) return;

            string suitName = card.cardSuit.ToString();
            string spritePath = "";

            if (card.cardType == CardType.Normal)
            {
                int val = hideSecretValues ? card.visibleValue : card.realValue;
                spritePath = $"res://Assets/Cartas/{suitName}/{val}.png";
            }
            else if (card.cardType == CardType.Illusory)
            {
                if (hideSecretValues)
                {
                    spritePath = $"res://Assets/trapCards/{suitName}/{card.visibleValue}.png";
                    if (!ResourceLoader.Exists(spritePath))
                    {
                        spritePath = $"res://Assets/Cartas/{suitName}/{card.visibleValue}.png";
                    }
                }
                else
                {
                    spritePath = $"res://Assets/Cartas/{suitName}/{card.realValue}.png";
                }
            }

            if (ResourceLoader.Exists(spritePath))
            {
                CardSprite.Texture = GD.Load<Texture2D>(spritePath);
            }

            if (TrapOverlay != null)
            {
                TrapOverlay.Visible = (card.cardType == CardType.Illusory);
            }
        }

        public override void _GuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                OnCardClicked?.Invoke(CardData, this);
            }
        }
    }
}