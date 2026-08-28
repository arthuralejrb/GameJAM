extends Control

func _on_button_pressed() -> void:
	# Checa se esta cena é um overlay (tem um pai além do Root da árvore)
	if get_parent() != get_tree().root:
		# Pega a cena de opções inteira (o nó raiz de options.tscn) e destrói ela
		owner.queue_free() # ou get_node("/root/...")... mas 'owner' ou 'get_parent()' pega a tela toda!
	else:
		# Se foi aberta isolada/direta no menu principal, troca de cena
		get_tree().change_scene_to_file("res://Scenes/MainMenuScene.tscn")
