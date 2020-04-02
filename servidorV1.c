#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <mysql.h>
#include <stdbool.h>
#include <pthread.h>


void *AtenderCliente(void *socket){
	int sock_conn;
	int *s;
	s=(int *)socket;
	sock_conn = *s;
	int ret, err;
	char buff[512],buff2[512], nombre[20],contrasena[20], email[20], idPartida[20];;
	MYSQL *conn;
	MYSQL_RES *resultado; // Estructura especial para almacenar resultados de consultas 
	MYSQL_ROW row;
	
	
	int terminar=0;
	int codigo;
	
	
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexiï¿³n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	//inicializar la conexin
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "juego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	
	while (terminar==0){
		ret=read(sock_conn,buff, sizeof(buff)); //recibimos mensaje
		printf ("Recibido\n");  // Tenemos que a?adirle la marca de fin de string, para que no escriba lo que hay despues en el buffer
		buff[ret]='\0';
		
		char *p;//porque el asterisco?????? Es un puntero, quan utilitzes la funcio strtok, fragmentes l'array pel caracter que
		//t'interessa, aixo ho fas a través del punter. El punter va fins a la direcció de memoria on hi ha el caracter que t'interessa,
		//el substitueix per un caracter fi de linea: '\0' i el punter queda en aquella posició per seguir fragmentant l'array
		char consulta[80];
		p = strtok( buff, "/");
		codigo = atoi(p);
		printf("CODIGO: %d\n", codigo);
		
		switch (codigo)
		{
			case 1:
			{
				printf("11111111\n");
			
				strcpy(consulta,"INSERT INTO jugador VALUES('");
			
				p = strtok( NULL, "/");
				strcpy (nombre, p);
				strcat(consulta,nombre);
				strcat(consulta,"',");
			
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				strcat(consulta,contrasena);
				strcat(consulta,",'");
			
				p = strtok( NULL, "/");
				strcpy (email, p);
			
				strcat(consulta,email);
				strcat(consulta,"');");
			
			
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}else
				{
					strcpy (buff2,"El servidor realizo correctamente el registro");
					write (sock_conn,buff2, strlen(buff2));
					// Se acabo el servicio para este cliente
					//close(sock_conn); 
				
				
				}
			
				break;
			}
		
			case 2:
			{
				printf("222222222\n");
				strcpy(consulta,"SELECT contrasena FROM jugador WHERE usuario = '");
			
				p = strtok( NULL, "/");
				strcpy (nombre, p);
				strcat(consulta,nombre);
				strcat(consulta,"';");
				printf("BE\n");
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				//recogemos el resultado de la consulta. El resultado de la
				//consulta se devuelve en una variable del tipo puntero a
				//MYSQL_RES tal y como hemos declarado anteriormente.
				//Se trata de una tabla virtual en memoria que es la copia
				//de la tabla real en disco.
				resultado = mysql_store_result (conn);
				// El resultado es una estructura matricial en memoria
				// en la que cada fila contiene los datos de una persona.
			
				// Ahora obtenemos la primera fila que se almacena en una
				// variable de tipo MYSQL_ROW
				row = mysql_fetch_row (resultado);
				// En una fila hay tantas columnas como datos tiene una
				// persona. En nuestro caso hay tres columnas: dni(row[0]),
				// nombre(row[1]) y edad (row[2]).
				printf("Resultat: %s\n", row[0]);
			
				if(strcmp(row[0], contrasena)==0){
					printf("BE1\n");
					strcpy (buff2,"Acceso");
					write (sock_conn,buff2, strlen(buff2));
					// Se acabo el servicio para este cliente
					//close(sock_conn); 
				}
			
				break;
			}
			case 3:
			{
				p = strtok( NULL, "/");
				strcpy (idPartida, p);
				strcat(consulta,idPartida);
			
				printf("222222222\n");
				strcpy(consulta,"SELECT ganador FROM partida WHERE ID = '");
				strcat(consulta,idPartida);
				strcat(consulta,"';");
				printf("BE\n");
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				//recogemos el resultado de la consulta. El resultado de la
				//consulta se devuelve en una variable del tipo puntero a
				//MYSQL_RES tal y como hemos declarado anteriormente.
				//Se trata de una tabla virtual en memoria que es la copia
				//de la tabla real en disco.
				resultado = mysql_store_result (conn);
				// El resultado es una estructura matricial en memoria
				// en la que cada fila contiene los datos de una persona.
			
				// Ahora obtenemos la primera fila que se almacena en una
				// variable de tipo MYSQL_ROW
				row = mysql_fetch_row (resultado);
				// En una fila hay tantas columnas como datos tiene una
				// persona. En nuestro caso hay tres columnas: dni(row[0]),
				// nombre(row[1]) y edad (row[2]).
				printf("Resultat: %s\n", row[0]);
				strcpy (buff2,row[0]);
				write (sock_conn,buff2, strlen(buff2));
				//close(sock_conn); 
			
			
				break;
			
			}
			case 4:
			{
				p = strtok( NULL, "/");
				strcpy (idPartida, p);
				strcat(consulta, idPartida);
			
				printf("222222222\n");
				strcpy(consulta,"SELECT posicion FROM resumen, partida, jugador WHERE partida.ID = ");
				strcat(consulta, idPartida);
				strcat(consulta," AND jugador.usuario = '");
				strcat(consulta, nombre);
				strcat(consulta,"' AND resumen.jugador = jugador.usuario AND resumen.partida = partida.ID;");
				printf("BE\n");
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				//recogemos el resultado de la consulta. El resultado de la
				//consulta se devuelve en una variable del tipo puntero a
				//MYSQL_RES tal y como hemos declarado anteriormente.
				//Se trata de una tabla virtual en memoria que es la copia
				//de la tabla real en disco.
				resultado = mysql_store_result (conn);
				// El resultado es una estructura matricial en memoria
				// en la que cada fila contiene los datos de una persona.
				
				// Ahora obtenemos la primera fila que se almacena en una
				// variable de tipo MYSQL_ROW
				row = mysql_fetch_row (resultado);
				// En una fila hay tantas columnas como datos tiene una
				// persona. En nuestro caso hay tres columnas: dni(row[0]),
				// nombre(row[1]) y edad (row[2]).
				printf("Posició: %s\n", row[0]);
				strcpy (buff2,row[0]);
				write (sock_conn,buff2, strlen(buff2));
				//close(sock_conn); 
			
			
				break;
			
			}
			case 5:
			{
				p = strtok( NULL, "/");
				strcpy (idPartida, p);
				strcat(consulta, idPartida);
				
				printf("222222222\n");
				strcpy(consulta,"SELECT duracion FROM partida WHERE partida.ID = ");
				strcat(consulta, idPartida);
				strcat(consulta,";");
				printf("BE\n");
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				//recogemos el resultado de la consulta. El resultado de la
				//consulta se devuelve en una variable del tipo puntero a
				//MYSQL_RES tal y como hemos declarado anteriormente.
				//Se trata de una tabla virtual en memoria que es la copia
				//de la tabla real en disco.
				resultado = mysql_store_result (conn);
				// El resultado es una estructura matricial en memoria
				// en la que cada fila contiene los datos de una persona.
			
				// Ahora obtenemos la primera fila que se almacena en una
				// variable de tipo MYSQL_ROW
				row = mysql_fetch_row (resultado);
				// En una fila hay tantas columnas como datos tiene una
				// persona. En nuestro caso hay tres columnas: dni(row[0]),
				// nombre(row[1]) y edad (row[2]).
				printf("Duracin: %s\n", row[0]);
				strcpy (buff2,row[0]);
				write (sock_conn,buff2, strlen(buff2));
			
				break;
			}
		};
		
	}
		close(sock_conn);
	
}



int main(int argc, char *argv[])
{
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;

	//inicialitzem el socket per poder establir una connexió

	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0){ //INICIALITZACIONS,Obrim el socket	
		printf("Error creant socket");
	}
	// Fem el bind al port
	
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;	
	// asocia el socket a cualquiera de las IP de la m?quina.
	//htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	// escucharemos en el port 9050
	serv_adr.sin_port = htons(9080);
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0){
		printf ("Error al bind");
	}
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0){
		printf("Error en el Listen");
	}
	
		//empezamos a escuchar
	int peticion = 0;
	int a = 0;
	int sockets[100];
	
	pthread_t thread[100];//vector estructura thread
	
	while(peticion == 0){
		a++;
		printf ("Escuchando\n");
		sock_conn = accept(sock_listen, NULL, NULL); //realizamos conexion
		printf ("Conexion realizada\n");
		
		sockets[a]=sock_conn;
		
		pthread_create(&thread[a], NULL, AtenderCliente, &sockets[a]);
	}
}
		

