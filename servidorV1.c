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


typedef struct {
	char nombre[20];
	int socket;
}Conectado;

typedef struct {
	Conectado conectados[100];
	int num;
}ListaConectados;




int Pon(ListaConectados *lista, char nombre[20], int socket){
	//Añade nuevo conectado.
	if(lista->num == 100){
		return -1;
	}else{
		strcpy(lista->conectados[lista->num].nombre, nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num = lista->num + 1;
		return 0;
	}
}
	
int Eliminar(ListaConectados *lista, char nombre[20]){
		
	int pos = DamePosicion(lista, nombre);
	
	if(pos == -1){
		return -1;
	}else{
		int i;
		for(i=pos; i<lista->num-1; i++){
			
			strcpy(lista->conectados[i].nombre, lista->conectados[i+1].nombre);
			lista->conectados[i].socket = lista->conectados[i+1].socket;
			
		}
		lista->num--;
		return 0;
	}
}
		
int DamePosicion(ListaConectados *lista, char nombre[20]){
	//Devuelve Posicion
	int i = 0;
	int encontrado = 0;
	
	while(i<lista->num && encontrado==0){
		if(strcmp(nombre, lista->conectados[i].nombre)==0){
			encontrado = 1;
		}if(encontrado==0){
			i++;
		}
	}
	if(encontrado == 1){
		return i;
	}else{
		return -1;
	}
}
			
			
void DameConectados(ListaConectados *lista, char conectados[300]){
	//Pone en conectados los nombres de todos los conectados separados por /
	//primero pone el num. de conectados. Ejemplo: 3/Maria/Juan
	
	sprintf(conectados, "%d", lista->num);
	int i;
	for(i=0; i<lista->num; i++){
		sprintf(conectados, "%s/%s", conectados, lista->conectados[i].nombre);
	}
	printf("CONECTADOS: %s\n", conectados);
}


	
void *AtenderCliente(ListaConectados *lista){
	
	printf("BEEE\n");
	int sock_conn;
	int *s;
	int pos = lista->num  -1;
	printf("POSICIO: %d\n", pos);
	printf("LISTA: %d\n", lista->conectados[pos].socket);
	s=&lista->conectados[pos].socket;
	printf("S: %d\n", *s);
	printf("BEEEE123\n");
	sock_conn = *s;
	printf("1SOCK_CONN:%d\n", sock_conn);
	printf("LIST: %d\n", lista->conectados[pos].socket);
	printf("BEEE0\n");
	int ret, err;
	char buff[512],buff2[512], nombre[20],contrasena[20], email[20], idPartida[20];;
	MYSQL *conn;
	MYSQL_RES *resultado; // Estructura especial para almacenar resultados de consultas 
	MYSQL_ROW row;
	
	
	int terminar=0;
	int codigo;
	printf("TOT OK\n");
	
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexiï¿³n: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	printf("TOT OK2\n");
	//inicializar la conexin
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "juego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	printf("FINSAQUIBE\n");
	
	
	
	
	
	while (terminar==0){
		ret=read(sock_conn,buff, sizeof(buff)); //recibimos mensaje
		printf ("Recibido\n");  // Tenemos que a?adirle la marca de fin de string, para que no escriba lo que hay despues en el buffer
		buff[ret]='\0';
		printf("MENSAJE: %s\n", buff);
		
		
		char *p;//porque el asterisco?????? Es un puntero, quan utilitzes la funcio strtok, fragmentes l'array pel caracter que
		//t'interessa, aixo ho fas a través del punter. El punter va fins a la direcció de memoria on hi ha el caracter que t'interessa,
		//el substitueix per un caracter fi de linea: '\0' i el punter queda en aquella posició per seguir fragmentant l'array
		char consulta[80];
		p = strtok( buff, "/");
		codigo = atoi(p);
		printf("CODIGO: %d\n", codigo);
		p = strtok( NULL, "/");
		strcpy (nombre, p);
		
		
		switch (codigo)
		{
			case 0:{
				printf("DESCONEXION\n");
				close(sock_conn);
				terminar = 1;
				Eliminar(&lista, nombre);
				break;
			}
			case 1:
			{
				printf("11111111\n");
			
				strcpy(consulta,"INSERT INTO jugador VALUES('");
			
				/*p = strtok( NULL, "/");
				strcpy (nombre, p);*/
				strcpy(lista->conectados[pos].nombre, p);
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
			
				/*p = strtok( NULL, "/");
				strcpy (nombre, p);*/
				strcpy(lista->conectados[pos].nombre, nombre);
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
					Pon(&lista, nombre, sock_conn);
				//	DameConectados(&lista, lista->conectados);
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
		
	
}



int main(int argc, char *argv[])
{
	
	ListaConectados listaConectados;
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
	listaConectados.num =0;
	//int sockets[100];
	
	pthread_t thread[100];//vector estructura thread
	
	while(peticion == 0){
		
		printf ("Escuchando\n");
		sock_conn = accept(sock_listen, NULL, NULL); //realizamos conexion
		printf ("Conexion realizada\n");
		printf("0SOCK_CONN: %d\n", sock_conn);
		
		listaConectados.conectados[a].socket=sock_conn;
		printf("LISTASOCKET: %d\n", listaConectados.conectados[a].socket);
		printf("BEEE1\n");
		listaConectados.num++;
		
		pthread_create(&thread[a], NULL, AtenderCliente, &listaConectados);
		printf("BE2\n");
		a++;
	}
}
		

