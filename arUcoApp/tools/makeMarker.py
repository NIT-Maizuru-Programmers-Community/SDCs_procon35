from cv2 import aruco
import cv2
import matplotlib.pyplot as plt


#aruco辞書の生成
dict_aruco=aruco.getPredefinedDictionary(aruco.DICT_4X4_50)

#IDを指定 (適当な整数)
marker_id=5

#マーカーサイズ
size_mark=300

for i in range(30):
    marker_id = i
    img = aruco.generateImageMarker(dict_aruco,marker_id,size_mark)

    cv2.imwrite("ar" + str(i) + ".png", img)