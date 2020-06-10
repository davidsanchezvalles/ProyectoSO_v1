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
   posicion INTEGER NOT NULL,
   jugador VARCHAR(16) NOT NULL,
   partida INTEGER NOT NULL,
   FOREIGN KEY (jugador) REFERENCES jugador(usuario),
   FOREIGN KEY (partida) REFERENCES partida(ID)
)ENGINE = InnoDB;

INSERT INTO jugador VALUES('davidsv', 1234);
INSERT INTO jugador VALUES('sergiogl', 1234);
INSERT INTO jugador VALUES('martigc', 1234);

INSERT INTO partida VALUES(NULL, '2020-01-12', 'davidsv');
INSERT INTO partida VALUES(NULL, '2020-01-18', 'sergiogl');
INSERT INTO partida VALUES(NULL, '2020-02-15', 'martigc');
INSERT INTO partida VALUES(NULL, '2020-01-18', 'martigc');

INSERT INTO resumen VALUES(1, 'davidsv', 1);
INSERT INTO resumen VALUES(2, 'martigc', 3);
INSERT INTO resumen VALUES(3, 'sergiogl', 2);
