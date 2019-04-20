# -*- coding: utf-8 -*-
#https://blog.keras.io/building-autoencoders-in-keras.html
#pip install keras

from keras.layers import Input, Dense
from keras.models import Model

# Este es el número de neuronas de la capa encargada de codificar los patrones de entrada (las imágenes)
numNeuronasCapaCodificadora = 32

alturaImagen = 28 #altura de la imagen expresada en número de pixels
anchuraImagen = 28 #anchura de la imagen expresada en número de pixels

#--------------------------------
# DEFINIMOS LA ESTRUCTURA
# Creamos el "placeholder" para los patrones de entrada (las imágenes) con un tamaño igual al número total del pixels de las imágenes
placeHolderParaPatronDeEntrada = Input(shape=(alturaImagen*anchuraImagen,))
# "capaCodificadora" es la capa de neuronas cuyas entradas están conectadas a las salidas del placeHolderParaPatronDeEntrada
# y cuya salida nos dará el código asociado a un patrón de entrada (una imagen)
capaCodificadora = Dense(numNeuronasCapaCodificadora, activation='relu')(placeHolderParaPatronDeEntrada) 
# "capaDecodificadora" es la capa de neuronas cuyas entradas están conectadas a las salidas de la capa codificadora,
# y cuya salida nos dará una imagen reconstruida a partir del código que genere la capa "capaCodificadora"
capaDecodificadora = Dense(alturaImagen*anchuraImagen, activation='sigmoid')(capaCodificadora)

# Creamos el modelo del autoencoder, en el cual se tiene como entrada el placeholder para los patrones 
# de entrada (los pixels de la imagen de entrada) y como salida la capa decodificadora
autoencoder = Model(placeHolderParaPatronDeEntrada, capaDecodificadora)
# Creamos el modelo del encoder, encargado de convertir los pixels de la imagen de entrada en un código
encoder = Model(placeHolderParaPatronDeEntrada, capaCodificadora)

# Crearemos el decodificador a partir de la estructura del autoencoder para usarlo posteriormente
# para reconstruir las imágenes a partir del código generado por el encoder. Para ello:
# Creamos el "placeholder" para la capa codificadora (de dimensión 32) input
placeHolderParaCapaCodificadora = Input(shape=(numNeuronasCapaCodificadora,))
# Cogemos la última capa del modelo autoencoder
decoder_layer = autoencoder.layers[-1]
# Creamos el modelo del decodificador
decoder = Model(placeHolderParaCapaCodificadora, decoder_layer(placeHolderParaCapaCodificadora))

# Configura el modelo indicando el optimizador y la función de pérdidas a usar para poder hacer posteriormente el entrenamiento
autoencoder.compile(optimizer='adadelta', loss='binary_crossentropy')
# para entender la forma de elegir el tipo de capa de activación y la función de pérdidas mirad estas páginas: 
# https://www.dlology.com/blog/how-to-choose-last-layer-activation-and-loss-function/
# https://towardsdatascience.com/deep-learning-which-loss-and-activation-functions-should-i-use-ac02f1c56aa8

#-----------------
# ENTRENAMIENTO DE LA RED
from keras.datasets import mnist
import numpy as np

# Cargamos el dataset MNIST
(patronesDeEntrenamiento, _), (patronesDePrueba, _) = mnist.load_data()

# Normalizamos los patrones de entrada a la red con valores entre 0 y 1
patronesDeEntrenamiento = patronesDeEntrenamiento.astype('float32') / 255.
patronesDePrueba = patronesDePrueba.astype('float32') / 255.

print ("patronesDeEntrenamiento antes de hacer reshape")
print (patronesDeEntrenamiento.shape)
print (patronesDeEntrenamiento)

print ("np.prod(patronesDeEntrenamiento.shape[1:])")
print (np.prod(patronesDeEntrenamiento.shape[1:]))
print ("np.prod(patronesDePrueba.shape[1:])")
print (np.prod(patronesDePrueba.shape[1:]))
print ("len(patronesDeEntrenamiento)")
print (len(patronesDeEntrenamiento))
print ("len(patronesDePrueba)")
print (len(patronesDePrueba))

# Se redimensionan los arrays para poder ser posteriormente usados en el entrenamiento
patronesDeEntrenamiento = patronesDeEntrenamiento.reshape((len(patronesDeEntrenamiento), np.prod(patronesDeEntrenamiento.shape[1:])))
patronesDePrueba = patronesDePrueba.reshape((len(patronesDePrueba), np.prod(patronesDePrueba.shape[1:])))
print ("patronesDeEntrenamiento.shape")
print (patronesDeEntrenamiento.shape)
print ("patronesDePrueba.shape")
print (patronesDePrueba.shape)

# Se ordena el entrenamiento de la red
# Para entender la diferencia entr epoch y batch_size, mirad este enlace:
# https://towardsdatascience.com/epoch-vs-iterations-vs-batch-size-4dfb9c7ce9c9
autoencoder.fit(patronesDeEntrenamiento, #array con los patrones de entrada
                patronesDeEntrenamiento, #array con los patrones de salida deseados (en un autoencoder, coinciden con los patrones de entrada)
                epochs=50, #Número de epochs para entrenar el modelo. Un epoch constituye una iteración sobre el total de los patrones de entrada y salida.
                batch_size=256, #Número de partes en las que es dividida el dataset para no usar tanta memoria
                shuffle=True, #Hacemos que los patrones de entrenamiento se "barajen" antes de comenzar cada epoch
                validation_data=(patronesDePrueba, patronesDePrueba))


# En codigoDeImagenes tenemos los códigos asociados a cada patrón de prueba
codigoDeImagenes = encoder.predict(patronesDePrueba)
# En imagenesDecodificadas tenemos las imágenes reconstruidas a partir de sus códigos
imagenesDecodificadas = decoder.predict(codigoDeImagenes)

import matplotlib.pyplot as plt

n = 10  # número de dígitos que se mostrarán
plt.figure(figsize=(20, 4))
for i in range(n):
    # mostrar el dígito original
    ax = plt.subplot(2, n, i + 1)
    plt.imshow(patronesDePrueba[i].reshape(alturaImagen, anchuraImagen))
    plt.gray()
    ax.get_xaxis().set_visible(False)
    ax.get_yaxis().set_visible(False)

    # mostrar el dígito reconstruido
    ax = plt.subplot(2, n, i + 1 + n)
    plt.imshow(imagenesDecodificadas[i].reshape(alturaImagen, anchuraImagen))
    plt.gray()
    ax.get_xaxis().set_visible(False)
    ax.get_yaxis().set_visible(False)
plt.show()