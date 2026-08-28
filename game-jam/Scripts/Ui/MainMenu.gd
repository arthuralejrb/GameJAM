extends Node2D

func _ready() -> void:
#	O node player já está configurado globalmente
	var player_node = get_node("/root/Player");

func _on_start_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/GambleScene.tscn")
	
func _on_options_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/OptionsMenuScene.tscn")
	
func _on_quit_pressed() -> void:
	get_tree().quit()
