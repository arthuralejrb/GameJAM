extends Button

@onready var sfx: AudioStreamPlayer = $"../sfx"

func _on_pressed() -> void:
	sfx.play()
