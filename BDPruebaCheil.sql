CREATE DATABASE ApiHoteles
USE ApiHoteles

CREATE TABLE Hotel(
	id_hotel int identity NOT NULL,
	nom_hotel varchar(100),
	categoria int,
	precio float,
	img1 varchar(100),
	img2 varchar(100),
	img3 varchar(100),
	PRIMARY KEY (id_hotel)	
)


CREATE TABLE Comentario(
	id_comentario int identity NOT NULL,
	nom_cliente varchar(100),
	comentario varchar(500),
	calificacion int,
	id_hotel int NOT NULL,
	PRIMARY KEY (id_comentario),
	FOREIGN KEY (id_hotel) REFERENCES Hotel(id_hotel)
)

---Data de prueba manual
INSERT INTO Hotel (nom_hotel,categoria,precio,img1,img2,img3) VALUES('HolyDay',5,821740,'HolyDay1.png','HolyDay2.png','HolyDay3.png')
INSERT INTO Hotel (nom_hotel,categoria,precio,img1,img2,img3) VALUES('Camerum',3,210500,'Camerum.png','Camerum2.png','Camerum3.png')
INSERT INTO Hotel (nom_hotel,categoria,precio,img1,img2,img3) VALUES('Despegar',3,120000,'Despegar.png','Despegar2.png','Despegar3.png')

INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Peppito Perez','Excelente hotel',4,2)
INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Juana de Arco','Regular hotel',3,2)
INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Julia De mi amor','Exelente hotel',4,2)
INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Alvaro Uribe','Malicimo hotel',1,2)

INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Peppito Perez','Excelente hotel',4,1)
INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Juana de Arco','Regular hotel',3,1)
INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Julia De mi amor','Exelente hotel',4,1)
INSERT INTO Comentario (nom_cliente,comentario,calificacion,id_hotel) VALUES('Alvaro Uribe','Malicimo hotel',1,1)


----Consultas

SELECT H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3,AVG(C.calificacion) Calificación
FROM Hotel H
LEFT JOIN Comentario C ON c.id_hotel=H.id_hotel
GROUP BY H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3


SELECT H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3,AVG(C.calificacion) Calificación
FROM Hotel H
LEFT JOIN Comentario C ON c.id_hotel=H.id_hotel
WHERE H.categoria=5 AND Calificacion=3
GROUP BY H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3

SELECT H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3,AVG(C.calificacion) Calificación
FROM Hotel H
LEFT JOIN Comentario C ON c.id_hotel=H.id_hotel
GROUP BY H.id_hotel, H.nom_hotel,H.categoria,H.precio,H.img1,H.img2,H.img3
ORDER BY H.precio DESC



