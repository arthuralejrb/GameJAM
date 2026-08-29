extends Control

func _on_button_pressed() -> void:
	# Se estiver rodando dentro do Pause Menu (Overlay)
	if get_parent() != get_tree().root and owner != null and owner.get_parent() != get_tree().root:
		owner.queue_free()
	else:
		# Garante que despausa a árvore antes de mudar de cena
		get_tree().paused = false
		# Executa a troca de cena de forma segura no final do frame atual
		get_tree().change_scene_to_file.call_deferred("res://Scenes/MainMenuScene.tscn")
