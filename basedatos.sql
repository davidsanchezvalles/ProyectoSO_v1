DROP DATABASE IF EXISTS juego;
CREATE DATABASE juego;
USE juego;
CREATE TABLE jugador (
   usuario VARCHAR(16) PRIMARY KEY NOT NULL,
   contrasena  INTEGER NOT NULL
)ENGINE = InnoDB;

CREATE TABLE partida (
   ID INTEGER PRIMARY KEY  NOT NULL AUTO_INCREMENT,
   fecha DATE NOT NULL,
   ganador TEXT NOT NULL
)ENGINE = InnoDB;

CREATE TABLE resumen (
   jugador VARCHAR(16) NOT NULL,
   partida INTEGER NOT NULL,
   posicion INTEGER NOT NULL,
   FOREIGN KEY (jugador) REFERENCES jugador(usuario),
   FOREIGN KEY (partida) REFERENCES partida(ID)
)ENGINE = InnoDB;

INSERT INTO jugador VALUES('davidsv', 1234);
INSERT INTO jugador VALUES('sergiogl', 1234);
INSERT INTO jugador VALUES('martigc', 1234);

INSERT INTO partida VALUES(NULL, '2020-01-12', 'davidsv');
INSERT INTO partida VALUES(NULL, '2020-01-18', 'sergiogl');
INSERT INTO partida VALUES(NULL, '2020-02-15', 'martigc');


INSERT INTO resumen VALUES('davidsv', 1, 1);
INSERT INTO resumen VALUES('martigc', 1, 2);
INSERT INTO resumen VALUES('sergiogl',2 , 1);
INSERT INTO resumen VALUES('davidsv',2 , 2);
INSERT INTO resumen VALUES('martigc',3 , 1);
INSERT INTO resumen VALUES('davidsv',3 , 2);
