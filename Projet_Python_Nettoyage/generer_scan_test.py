# -*- coding: utf-8 -*-
# Genere un faux document scanne pour tester nettoyage.py sans materiel
# Le fichier produit est scan_test.png

import os
import cv2
import numpy as np

base = os.path.dirname(os.path.abspath(__file__))
largeur, hauteur = 900, 1200

# --- fond papier : gris clair avec grain et jaunissement irregulier ---
fond = np.full((hauteur, largeur), 235, dtype=np.float32)
jaunissement = cv2.GaussianBlur(np.random.rand(hauteur, largeur).astype(np.float32), (0, 0), 80)
fond -= (jaunissement - jaunissement.min()) * 60          # teinte inegale comme un vieux papier
grain = np.random.normal(0, 4, (hauteur, largeur))       # bruit fin imitant le grain du papier
fond = np.clip(fond + grain, 0, 255).astype(np.uint8)
image = cv2.cvtColor(fond, cv2.COLOR_GRAY2BGR)

# --- texte simule (blocs fonces en guise de lignes dactylographiees) ---
rng = np.random.default_rng(7)
for ligne in range(6, 44):
    y = 60 + ligne * 25
    x = 90
    while x < largeur - 140:
        mot = rng.integers(40, 110)
        epaisseur = rng.integers(8, 13)
        image[y:y + epaisseur, x:x + mot] = (35, 35, 40)
        x += mot + rng.integers(12, 26)

# --- tampon rouge oblique en bas a droite ---
masque_tampon = np.zeros((hauteur, largeur), dtype=np.uint8)
centre = (largeur - 260, hauteur - 220)
cv2.ellipse(masque_tampon, centre, (150, 90), -20, 0, 360, 255, 12)
cv2.ellipse(masque_tampon, centre, (120, 62), -20, 0, 360, 255, 3)
cv2.putText(masque_tampon, "UATM-GASA", (centre[0] - 110, centre[1] - 10),
            cv2.FONT_HERSHEY_SIMPLEX, 1.0, 255, 6)
cv2.putText(masque_tampon, "VU ET APPROUVE", (centre[0] - 105, centre[1] + 30),
            cv2.FONT_HERSHEY_SIMPLEX, 0.55, 255, 3)
couleur_tampon = np.zeros_like(image)
couleur_tampon[:, :] = (50, 50, 220)                       # rouge en BGR
tampon = masque_tampon > 0
image[tampon] = cv2.addWeighted(image, 0.65, couleur_tampon, 0.35, 0)[tampon]

chemin = os.path.join(base, "scan_test.png")
cv2.imwrite(chemin, image)
print("Faux scan cree : " + chemin)
