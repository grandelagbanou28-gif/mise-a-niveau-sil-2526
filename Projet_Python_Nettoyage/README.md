# Projet Python — Nettoyage de document scanné

Retrait automatique du **fond papier** (jaunissement, grain, taches) et des **tampons colorés** sur une image scannée.

## Principe

> **Détecter → Masquer → Reconstruire**

| Étape | Technique | Pourquoi |
|---|---|---|
| 1. Détection du tampon | Espaces de couleur **HSV** (teinte rouge/bleue) | Le tampon se distingue par sa couleur, pas par sa forme |
| 2. Masque + reconstruction | Masque binaire + **inpainting** (Telea) | On efface la zone puis on la rebouche avec les pixels voisins |
| 3. Fond papier | **Fermeture morphologique** (noyau 51×51) + médiane | Le gros noyau comble le texte, il ne reste que le papier |
| 4. Éclaircissement | **Division** `image / fond` | Le jaunissement est annulé proportionnellement |
| 5. Binarisation | Seuillage adaptatif **Sauvola** | Seuil calculé localement (moyenne + écart-type par fenêtre 25×25) |

## Fichiers

- `nettoyage.py` — programme principal (5 parties, commentaire ligne par ligne)
- `generer_scan_test.py` — crée un faux scan (papier + texte + tampon rouge) pour tester sans scanner
- `scan_test.png` — image de test générée
- `resultats/` — les 6 étapes sauvegardées (dont `comparaison_avant_apres.png`)

## Lancer

```bash
python generer_scan_test.py   # 1. créer l'image de test
python nettoyage.py           # 2. nettoyer : le programme DEMANDE le chemin
```

Au lancement, le programme demande le chemin de l'image à nettoyer :
- **Entrée** directe → utilise `scan_test.png` (l'image de test)
- Un **chemin** vers votre propre scan (ex. `C:\scans\examen.png`)
- Si le fichier n'existe pas → message d'erreur et **redemande** (boucle de validation)

Dépendances : `opencv-python`, `numpy`.

## Script de présentation (30 s)

> « Mon programme nettoie un document scanné en 5 étapes. D'abord je passe l'image en espace HSV pour isoler le tampon par sa couleur rouge ou bleue, puis je le masque et je reconstruis le fond avec un algorithme d'inpainting. Ensuite, j'estime le fond du papier avec une fermeture morphologique dont le noyau est plus large que le texte, je divise l'image par ce fond pour annuler le jaunissement, et enfin je binarise avec le seuillage adaptatif de Sauvola, dont le seuil est recalculé localement selon la moyenne et l'écart-type de chaque zone. »
