extends Panel

@onready var sfx: AudioStreamPlayer = $sfx

func _on_play_sound_pressed() -> void:
	sfx.play()
