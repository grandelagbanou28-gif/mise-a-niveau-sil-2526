# -*- coding: utf-8 -*-
# Nettoyage d'un document scanne
# But : retirer le fond papier (grain, jaunissement, taches) et les tampons colores
# Principe general : DETECTER -> MASQUER -> RECONSTRUIRE
# Usage : python nettoyage.py [chemin_image]

import os
import sys
import cv2
import numpy as np


# ==========================================================================
# PARTIE 1 : chargement de l'image
# ==========================================================================
def charger_image(chemin):
    image = cv2.imread(chemin)              # lit le fichier et le transforme en tableau de pixels (format BGR)
    if image is None:                        # imread renvoie None si le fichier n'existe pas ou est illisible
        raise FileNotFoundError("Image introuvable : " + chemin)
    hauteur, largeur = image.shape[:2]      # recupere les dimensions (les 2 premieres valeurs seulement)
    print("Image chargee : " + str(largeur) + " x " + str(hauteur) + " pixels")
    return image


# ==========================================================================
# PARTIE 2 : retrait du tampon colore (la couleur aide a le detecter)
# ==========================================================================
def creer_masque_tampon(image, s_min=50, v_min=70):
    # Etape 2a : passage de BGR (OpenCV) a HSV
    # HSV separe la TEINTE (H, la couleur) de la SATURATION (S) et de la LUMINOSITE (V) :
    # un tampon rouge a le meme H partout, ce qui le rend facile isoler
    hsv = cv2.cvtColor(image, cv2.COLOR_BGR2HSV)
    h, s, v = cv2.split(hsv)                # separe les 3 plans de l'image

    # Etape 2b : regles de detection
    # Le rouge est special : il est en debut (0-10) ET en fin (170-180) de l'echelle des teintes
    rouge_1 = (h <= 10) & (s >= s_min) & (v >= v_min)     # rouge (teinte faible)
    rouge_2 = (h >= 170) & (s >= s_min) & (v >= v_min)    # rouge (teinte elevee)
    bleu = (h >= 90) & (h <= 130) & (s >= s_min) & (v >= v_min)  # bleu (autre couleur des tampons)

    # Etape 2c : on combine les 3 masques -> un tableau binaire (255 = tampon, 0 = reste)
    masque = (rouge_1 | rouge_2 | bleu).astype(np.uint8) * 255

    # Etape 2d : nettoyage morphologique du masque
    noyau = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (5, 5))  # petit disque 5x5
    masque = cv2.morphologyEx(masque, cv2.MORPH_OPEN, noyau)      # OPEN = enleve les pixels isoles (bruit)
    masque = cv2.dilate(masque, noyau, iterations=2)              # on grossit le masque pour bien couvrir les bords
    return masque


def retrait_tampon(image):
    masque = creer_masque_tampon(image)

    # Etape 2e : inpainting = reconstruction du fond
    # Le programme efface la zone du tampon puis la rebouche en s'inspirant des pixels voisins
    # INPAINT_TELEA = algorithme de Telea (propagation des couleurs depuis les bords de la zone)
    image_propre = cv2.inpaint(image, masque, inpaintRadius=5, flags=cv2.INPAINT_TELEA)
    return image_propre, masque


# ==========================================================================
# PARTIE 3 : retrait du fond papier (jaunissement, grain, taches)
# ==========================================================================
def estimer_fond(image_gris):
    # Le fond est CLAIR et LEGEREMENT IRREGULIER, le texte est petit et FONCE
    # Un tres gros noyau (51x51) est plus large que les caracteres :
    # la fermeture (dilatation puis erosion) comble le texte, il ne reste que le papier
    noyau = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (51, 51))
    fond = cv2.morphologyEx(image_gris, cv2.MORPH_CLOSE, noyau)

    # On lisse encore avec une mediane 25x25 pour effacer les derniers residus de texte
    fond = cv2.medianBlur(fond, 25)
    return fond


def eclaircir(image_gris, fond):
    # Division pixel par pixel : image / fond, puis remise a l'echelle sur 0-255
    # La ou le fond etait sombre (jauni), tout est remonte proportionnellement
    # Resultat : un papier uniformement blanc, meme sous une tache ou un coin ombre
    eclairci = cv2.divide(image_gris, fond, scale=255)
    return eclairci


# ==========================================================================
# PARTIE 4 : seuillage adaptatif (Sauvola) pour une binaire propre
# ==========================================================================
def seuillage_sauvola(image_gris, fenetre=25, k=0.2, R=128.0):
    img = image_gris.astype(np.float32)      # conversion en flottant (sinon debordement en 0-255)

    # Moyenne locale et ecart-type local dans une fenetre de 25x25 autour de chaque pixel
    # boxFilter(..., normalize=True) calcule la moyenne ; var = E[x^2] - (E[x])^2
    moyenne = cv2.boxFilter(img, -1, (fenetre, fenetre), normalize=True)
    variance = cv2.boxFilter(img * img, -1, (fenetre, fenetre), normalize=True) - moyenne * moyenne
    ecart = np.sqrt(np.maximum(variance, 0))  # maximum(..., 0) evite les negatives dues aux arrondis

    # Formule de Sauvola : seuil = moyenne * (1 + k * (ecart / R - 1))
    # Texte bien net (fort ecart) -> seuil eleve ; zone plate (papier) -> seuil bas
    seuil = moyenne * (1.0 + k * (ecart / R - 1.0))

    # Comparaison finale : pixel plus sombre que le seuil = texte (0), sinon = papier (255)
    binaire = np.where(image_gris > seuil, 255, 0).astype(np.uint8)
    return binaire


# ==========================================================================
# PARTIE 5 : programme principal (le pipeline, dans l'ordre des parties)
# ==========================================================================
def nettoyer(chemin_entree, dossier_sortie="resultats"):
    base = os.path.dirname(os.path.abspath(__file__))       # dossier du script (pour etre independant du lancement)
    dossier_sortie = os.path.join(base, dossier_sortie)
    os.makedirs(dossier_sortie, exist_ok=True)              # cree le dossier des resultats s'il n'existe pas

    # --- Partie 1 ---
    image = charger_image(chemin_entree)

    # --- Partie 2 : tampon d'abord, car sa couleur fausserait le reste ---
    sans_tampon, masque = retrait_tampon(image)
    print("Tampon retire (" + str(np.count_nonzero(masque)) + " pixels reconstruits)")

    # --- Parties 3 et 4 : fond papier puis binarisation ---
    gris = cv2.cvtColor(sans_tampon, cv2.COLOR_BGR2GRAY)    # le fond ne depend que de la luminosite
    fond = estimer_fond(gris)
    eclairci = eclaircir(gris, fond)
    binaire = seuillage_sauvola(eclairci)
    print("Fond papier nettoye et texte binarise")

    # --- Sauvegarde de chaque etape (utile pour montrer le fonctionnement) ---
    ecrire = lambda nom, img: cv2.imwrite(os.path.join(dossier_sortie, nom), img)
    ecrire("1_sans_tampon.png", sans_tampon)
    ecrire("2_masque_tampon.png", masque)
    ecrire("3_fond_estime.png", fond)
    ecrire("4_eclairci.png", eclairci)
    ecrire("5_document_nettoye.png", binaire)

    # Comparaison cote a cote (avant | apres) redimensionnee pour etre presentable
    hauteur = 420
    def redimensionner(img):
        ratio = hauteur / img.shape[0]
        return cv2.resize(img, (int(img.shape[1] * ratio), hauteur))
    comparaison = np.hstack([redimensionner(image),
                             redimensionner(cv2.cvtColor(binaire, cv2.COLOR_GRAY2BGR))])
    ecrire("comparaison_avant_apres.png", comparaison)

    print("Resultats enregistres dans : " + dossier_sortie)
    return binaire


if __name__ == "__main__":
    # Saisie interactive du chemin (comme les TP C++ : on demande tant que la saisie est invalide)
    dossier = os.path.dirname(os.path.abspath(__file__))
    defaut = os.path.join(dossier, "scan_test.png")

    while True:
        saisie = input("Chemin de l'image a nettoyer (Entree = scan_test.png) : ").strip().strip('"').strip('\ufeff')
        if saisie == "":
            if os.path.isfile(defaut):
                chemin = defaut
                break
            print("Aucune image de test. Lancez d'abord generer_scan_test.py ou donnez un chemin.")
            continue
        if os.path.isfile(saisie):
            chemin = saisie
            break
        print("Fichier introuvable : " + saisie + " — reessayez.")

    nettoyer(chemin)
