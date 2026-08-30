extends Node2D

func _ready() -> void:
	# Garante que o Player global existe
	var audio = get_node_or_null("/root/AudioManager");
	if (audio != null):
		audio.PlayMusic(audio.MenuMusic)

func _on_start_pressed() -> void:
	var player_node = get_node_or_null("/root/Player")
	get_tree().change_scene_to_file("res://Scenes/GambleScene.tscn")

func _on_options_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/OptionsMenuScene.tscn")

func _on_quit_pressed() -> void:
	get_tree().quit()
