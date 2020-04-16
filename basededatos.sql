DROP DATABASE IF EXISTS juego;
CREATE DATABASE juego;
USE juego;
CREATE TABLE jugador (
   usuario VARCHAR(16) PRIMARY KEY NOT NULL,
   contrasena  INTEGER NOT NULL,
   email TEXT NOT NULL
)ENGINE = InnoDB;

CREATE TABLE partida (
   ID INTEGER PRIMARY KEY  NOT NULL AUTO_INCREMENT,
   fecha DATE NOT NULL,
   duracion INTEGER NOT NULL,
   ganador TEXT NOT NULL
)ENGINE = InnoDB;

CREATE TABLE resumen (
   posicion INTEGER NOT NULL,
   puntuacion INTEGER NOT NULL,
   jugador VARCHAR(16) NOT NULL,
   partida INTEGER NOT NULL,
   FOREIGN KEY (jugador) REFERENCES jugador(usuario),
   FOREIGN KEY (partida) REFERENCES partida(ID)
)ENGINE = InnoDB;

INSERT INTO jugador VALUES('davidsv', 1234, 'david@upc.edu');
INSERT INTO jugador VALUES('sergiogl', 1234, 'sergio@upc.edu');
INSERT INTO jugador VALUES('martigc', 1234, 'marti@upc.edu');

INSERT INTO partida VALUES(NULL, '2020-01-12', 37, 'davidsv');
INSERT INTO partida VALUES(NULL, '2020-01-18', 57, 'sergiogl');
INSERT INTO partida VALUES(NULL, '2020-02-15', 47, 'martigc');
INSERT INTO partida VALUES(NULL, '2020-01-18', 43, 'sergiogl');

INSERT INTO resumen VALUES(1, 12, 'davidsv', 1);
INSERT INTO resumen VALUES(2, 10, 'sergiogl', 3);
INSERT INTO resumen VALUES(3, 8, 'martigc', 2);
INSERT INTO resumen VALUES(1, 8, 'sergiogl', 4);
INSERT INTO resumen VALUES(2, 10, 'martigc', 1);
