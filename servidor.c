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
#include <time.h>

//#include <my_global.h>



typedef struct {
	char nombre[20];
	int socket;
}Conectado;

typedef struct {
	Conectado conectados[100];
	int num;
}ListaConectados;


typedef struct{
	int identificador;
	char jugador1[20];
	char jugador2[20];
}Cpartida;



pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER; //Acceso excluyente
ListaConectados lista;
Cpartida partida;

//obtenemos el id correcto para guardarlo en la siguiente partida
int idP;


int Poner(ListaConectados *lista, char nombre[20], int socket){
	//Anade nuevo conectado.
	if(lista->num == 100){
		return -1;
	}else{
		strcpy(lista->conectados[lista->num].nombre, nombre); 
		lista->conectados[lista->num].socket= socket;
		lista->num = lista->num + 1;
		return 0;
	}
}
int DamePosicion(ListaConectados *lista, char nombre[20]){
		
		//Devuelve Posicion en caso de encontrarlo, en caso contrario devuelve -1
		int i = 0;
		int encontrado = 0;
		
		while(i<lista->num && encontrado==0)
		{
			if(strcmp(nombre, lista->conectados[i].nombre)==0)
			{
				encontrado = 1;
			}else i++;
		}
		if(encontrado == 1)
		{
			return i; 
			
		}else return -1;
		
		
	}	
int Eliminar(ListaConectados *lista, char nombre[20]){
		
	int pos = DamePosicion(lista, nombre); //encontramos la posicion de la persona en la lista
	int i;
	if(pos == -1){
		return -1;  //no se encontro
	}else{
		
		for(i=pos; i<lista->num; i++){  //he quitado el -1 porque sino no llega a eliminar el ultimo
			
			strcpy(lista->conectados[i].nombre, lista->conectados[i+1].nombre);
			lista->conectados[i].socket = lista->conectados[i+1].socket;
			
		}
		lista->num = lista->num-1;
		return 0;
	}
}
	
void DameConectados(ListaConectados *lista, char conectados[80]){ 
	
	//Pone en variable conectados los nombres de los usuarios conectados separados por /
	int i;
	strcpy(conectados,""); //nos aseguramos que esta vacio
	
	if(lista->num > 0){
		 		
		for(i=0; i< lista->num  ; i++) {
			
			strcat(conectados,lista->conectados[i].nombre);
			strcat(conectados,"/");
		}
	 }
	printf("%d personas conectadas: %s\n" , lista->num, conectados);
}
	
	
void acceder(char buff[512], char consulta[80]){ //per fer provas de optimitzacio
	char *p;
	char nombre[20];
	char contrasena[20];
	
	p = strtok( NULL, "/");
	strcpy (nombre, p);
	//printf("%s\n",nombre);
	strcpy(consulta,"");
	strcpy(consulta,"SELECT contrasena FROM jugador WHERE usuario = '");  //concatenamos la consulta
	strcat(consulta,nombre);
	strcat(consulta,"';");
	
	p = strtok( NULL, "/");
	strcpy (contrasena, p);
	
	
	
	
}
	
void *AtenderCliente(void *socket){
	//Funcion concurrente para atender los mensajes de los clientes
	//Primero de todo creamos una conexion al servidor MYSQL  
	MYSQL *conn;
	MYSQL_RES *resultado; // Estructura especial para almacenar resultados de consultas 
	MYSQL_ROW row;
	conn = mysql_init(NULL); 
	
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}

	//inicializar la conexin MYSQL
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "juego",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}

  	//metodo para inicializar correctamente el socket que hemos pasado como un puntero void
	int sock_conn;
	int *s;
	s= (int *) socket;
	sock_conn= *s;
	
	//comprobamos el socket del usuario x
	printf("Se ha conectado el usuario con socket: %d\n", sock_conn);
	
	
	char buff[512],buff2[512], nombre[20],contrasena[20], email[20], idPartida[20];
	char conectados[80];
	int ret, err;
	int codigo, pos, i;
	int terminar=0;
	char consulta[200];  //variable utilizada para las consultas SQL

	while (terminar==0){
		
		
		ret=read(sock_conn, buff, sizeof(buff)); //ret=longitud mensaje recibido, buff=mensaje recibido
		buff[ret]='\0'; // Tenemos que anadirle la marca de fin de string, para que no escriba lo que hay despues en el buffer
		printf("MENSAJE CLIENTE RECIBIDO: %s\n", buff);
		char *p;
		p = strtok( buff, "/"); //primera llamada strtok
		codigo = atoi(p);  //convierte el string en un entero
		
		if(codigo==1 || codigo==2 || codigo==12 ){  //los casos de acceder y registrar
			p = strtok( NULL, "/");
			strcpy (nombre, p);
		}
		
		
		switch (codigo)
		{
			case 0: {   //desconexion
				printf("DESCONEXION\n");
				close(sock_conn);
				pthread_mutex_lock (&mutex);//Pedimos que no interrumpan
				Eliminar(&lista, nombre);
				pthread_mutex_unlock (&mutex); //ya puede interrumpir
				printf("%d personas conectadas: %s \n", lista.num,conectados);
				DameConectados(&lista,conectados);
				sprintf(buff2,"6/%s",conectados); //enviamos lista conectados a todos los conectados que quedan
				printf ("Enviamos a todos los conectados: %s\n", buff2);
				for (i=0; i<lista.num; i++){
					write (lista.conectados[i].socket,buff2, strlen(buff2)); 
				}
				terminar = 1;
				break;
			}
			case 1: {   //registro
			
				
				strcpy(consulta,""); //nos aseguramos que esta vacio
				strcpy(consulta,"INSERT INTO jugador VALUES('");  //concatenamos la consulta
				strcat(consulta,nombre);
				strcat(consulta,"',");
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				strcat(consulta,contrasena);
				strcat(consulta,");");
				
				
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}else
				{
					/**pthread_mutex_lock (&mutex);//Pedimos que no interrumpan
					Poner(&lista,nombre,sock_conn); //añadimos el nuevo usuario a la lista de conectados				
					pthread_mutex_unlock (&mutex); //ya puede interrumpir*/
					
					strcpy (buff2,"1/El servidor realizo correctamente el registro");
					write (sock_conn,buff2, strlen(buff2));//enviamos mensaje
				}
			
				break;
			}
		
			case 2: {   //acceder

				strcpy(consulta,"SELECT contrasena FROM jugador WHERE usuario = '");  //concatenamos la consulta
				strcat(consulta,nombre);
				strcat(consulta,"';");
				
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				
				
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al coonsultar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
							
					exit (1);
				
				}else	
				//recogemos el resultado de la consulta.
				//El resultado es una estructura matricial en memoria,en la que cada fila contiene los datos de una persona.
				resultado = mysql_store_result (conn);				
				// Ahora obtenemos la primera fila que se almacena en una
				// variable de tipo MYSQL_ROW
				row = mysql_fetch_row (resultado);
				if (row == NULL){
					//enviamos un mensaje al cliente
					strcpy (buff2,"2/Acceso NO correcto");
					write (sock_conn,buff2, strlen(buff2));
					printf ("No se han obtenido datos en la consulta\n");
					printf ("Enviamos a client: %s\n", buff2);
				} else {
					if(strcmp(row[0], contrasena)==0){
						int i;
						pthread_mutex_lock (&mutex);//Pedimos que no interrumpan
						Poner(&lista,nombre,sock_conn); //añadimos el usuario a la lista conectados
						pthread_mutex_unlock (&mutex); //ya puede interrumpir
						DameConectados(&lista,conectados);
						printf("Password correcto: %s y %s coninciden\n", contrasena, row[0]);
						sprintf(buff2,"6/%s",conectados); //enviamos todos los conectados de la lista 
						printf ("Enviamos a todos los conectados: %s\n", buff2);
						for (i=0; i<lista.num; i++){  //enviamos esta lista a todos los clientes
							write (lista.conectados[i].socket,buff2, strlen(buff2)); 
						}
					} else {
						//enviamos un mensaje al cliente
						strcpy (buff2,"2/Acceso NO correcto");
						write (sock_conn,buff2, strlen(buff2)); 
						printf("Password NO correcto: %s y %s coninciden\n", contrasena, row[0]);
						printf ("Enviamos a client: %s\n", buff2);
					}
					
					
				}
				break;
			}
			case 3: {   //ganador
			
				p = strtok( NULL, "/");
				strcpy (idPartida, p);
				strcpy(consulta,"SELECT ganador FROM partida WHERE ID = '");
				strcat(consulta,idPartida);
				strcat(consulta,"';");
				
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				
				if(row==NULL)
				{
					printf("No se han obtenido datos en la consulta.\n");
					sprintf(buff2, "3/No hay datos de partida.");
					write (sock_conn,buff2, strlen(buff2));
				}
				
				printf("Resultado: %s\n", row[0]);
				strcpy(buff2,"3/");
				strcat (buff2, row[0]);
				write (sock_conn,buff2, strlen(buff2));
			
				break;
			
			}
			case 5: {   //jugadores con los que he jugado
		
				strcpy(consulta,"");
				p = strtok( NULL, "/");
				strcpy (nombre, p);
				strcpy(consulta,"SELECT distinct jugador FROM resumen WHERE partida IN (SELECT partida FROM resumen WHERE jugador = '"); //concatenamos la consulta
				strcat(consulta, nombre);
				strcat(consulta,"') AND jugador != '");
				strcat(consulta, nombre);
				strcat(consulta,"';");
				
				
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al realizar la consulta de datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				
				if(row==NULL)
				{
					printf("No se han obtenido datos en la consulta.\n");
					sprintf(buff2, "3/No hay datos de partida.");
					write (sock_conn,buff2, strlen(buff2));
				}
				else {
					
					char resultados[100];
					sprintf(buff2, "3/");
					while (row != NULL)
					{
						sprintf(resultados, "%s-",row[0]);
						strcat(buff2, resultados);
						row = mysql_fetch_row(resultado);
					}
					
					write (sock_conn,buff2, strlen(buff2));
					printf("%s\n", buff2);
				    }
				
					break;
			}
		
			case 6: {   //conectados
							
				DameConectados(&lista,conectados);
				sprintf(buff2,"6/%s",conectados);
				write (sock_conn,buff2, strlen(buff2)); 
			
				break;
			}	
			case 7: {   //invitacion
				char nombre_invitar[20];
				p = strtok( NULL, "/");
				strcpy(nombre_invitar, p);
				
				int pos=DamePosicion(&lista,nombre_invitar);
				printf("Invitando a %s con socket: %d\n",lista.conectados[pos].nombre, lista.conectados[pos].socket);
				sprintf(buff2,"7/El jugador %s le ha invitado a jugar,acepta el reto?", nombre);
				write (lista.conectados[pos].socket,buff2, strlen(buff2));
				strcpy(partida.jugador1, nombre); //el jugador que invita será el que tendrá el turno 1, el que empezará a jugar primero
				
				break;
			}
			
			case 8: {	//respuestainvitacion
				
				char respuesta[4];
				p = strtok( NULL, "/");
				strcpy (respuesta, p);
				if (strcmp(respuesta,"SI")==0)
				{
					strcpy(partida.jugador2, nombre);  // el jugador que ha aceptado la invitacion será el jugador 2
					sprintf(buff2,"8/%s/%s/1",partida.jugador1, partida.jugador2); //enviamos al jugador que invito, el jugador 1, los nombres de los jugadores de la partida y le indicamos que será su turno
					int pos1 = DamePosicion(&lista, partida.jugador1);
					write (lista.conectados[pos1].socket,buff2, strlen(buff2));
					sprintf(buff2,"8/%s/%s/0",partida.jugador1, partida.jugador2); //enviamos al jugador 2 que no es su turno 
					int pos2 = DamePosicion(&lista, partida.jugador2);
					write (lista.conectados[pos2].socket,buff2, strlen(buff2));
				
				}else if (strcmp(respuesta,"NO")==0)
				{
					int pos=DamePosicion(&lista,nombre);	
					sprintf(buff2,"8/No se acepto invitacion");
					//Enviamos la respuesta a mi cliente
					write (lista.conectados[pos].socket,buff2, strlen(buff2)); 	
						
				}
				
				
				
				break;
			}
			case 9: {	//chat
				char mensaje[50];
				p = strtok( NULL, "/");
				strcpy(mensaje, p);
				sprintf(buff2,"9/%s: %s",nombre,mensaje);
				for (i=0; i<lista.num; i++){  //enviamos este mensaje a todos los clientes
					write (lista.conectados[i].socket,buff2, strlen(buff2)); 
				}
				
				break;
			}
				
			case 10: {  //tiro
				
				p = strtok( NULL, "/"); 
				int turno ;
				turno = atoi(p);
				p = strtok( NULL, "/"); 
				char  Vx[20];
				strcpy(Vx,p); 
				p = strtok( NULL, "/"); 
				char  Voy[20];
				strcpy(Voy,p);
				
				
				sprintf(buff2,"10/%d/%s/%s", turno,Vx, Voy); // le pasamos a los 2 jugadores los datos para simular el tiro del contrincante y su turno
				printf("%s\n", buff2);
				int p1 = DamePosicion(&lista, partida.jugador1);
				int p2 = DamePosicion(&lista, partida.jugador2);
				
				if( turno==1)
				{
					write (lista.conectados[p2].socket,buff2, strlen(buff2));
					
				}else if ( turno==2)
				{
					write (lista.conectados[p1].socket,buff2, strlen(buff2));
				}
				
				
				break;
			}
				
			case 11: {  //guardar datos partida
	
				strcpy(consulta,"");
				strcpy(consulta,"SELECT COUNT(ID) FROM partida;"); //concatenamos la consulta
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al realizar la consulta de datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				
				
				idP = atoi(row[0]) +1;
				
				char ganador[30];
				p = strtok( NULL, "/"); 
				strcpy(ganador,p);
			
				int identificadorP;
				char str1[20];
				
				pthread_mutex_lock (&mutex);//Pedimos que no interrumpan
				identificadorP = idP;
				idP++;
				
				
				//el string output contiene la fecha actual con el formato "09-06-20"
				time_t tiempo = time(0);
				struct tm *tlocal = localtime(&tiempo);
				char output[128];
				strftime(output,128,"20%y-%m-%d", tlocal);
				printf("Fecha actual: %s, Jugador ganador: %s\n", output,ganador);
				
				strcpy(consulta,""); //para vaciar si hay algo
				strcpy(consulta,"INSERT INTO partida VALUES(");  //concatenamos la consulta
				strcat(consulta, "NULL");
				strcat(consulta,",'");
				strcat(consulta,output);
				strcat(consulta,"','");
				strcat(consulta,ganador);
				strcat(consulta,"');");
				
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}else
				{
				
					
					printf("Inserción a partida correctamente\n");
				}
				
				strcpy(consulta,"");
				strcpy(consulta,"INSERT INTO resumen VALUES('");  //concatenamos la consulta
				strcat(consulta,ganador);
				strcat(consulta,"',");
				sprintf(str1, "%d,", identificadorP);
				strcat(consulta, str1);
				strcat(consulta,"1");
				strcat(consulta,");");
				
			
				
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}else
				{
					
					printf("Inserción a resumen(ganador) correctamente\n");
				}
				
				
				strcpy(consulta,"");
				strcpy(consulta,"INSERT INTO resumen VALUES('");  
				
				if(strcmp(partida.jugador1, ganador)==0)
				{
					strcat(consulta,partida.jugador2);
				}
				else
				{
					strcat(consulta,partida.jugador1);
				}
				strcat(consulta,"',");
				sprintf(str1, "%d,", identificadorP);
				strcat(consulta, str1);
				strcat(consulta,"2);");
				
				err=mysql_query (conn, consulta);
				
				pthread_mutex_unlock (&mutex); //ya puede interrumpir
								
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}else
				{
			
					
					printf("Inserción a resumen correctamente\n");
				}
				sprintf(buff2,"13/Felicidades,usted ha ganado!!");
				int pos1 = DamePosicion(&lista, ganador);
				write (lista.conectados[pos1].socket,buff2, strlen(buff2));
				sprintf(buff2,"13/Lo siento,usted ha perdido!!");  
				int pos2 = DamePosicion(&lista, partida.jugador2);
				write (lista.conectados[pos2].socket,buff2, strlen(buff2));
				
				break;
				//el string output contiene la fecha actual con el formato "09/06/20"
				/*time_t tiempo = time(0);
				struct tm *tlocal = localtime(&tiempo);
				char output[128];
				strftime(output,128,"%d/%m/%y", tlocal);
				printf("fecha actual: %s, jugador ganador: %s\n", output,ganador);*/
				
				
			}
			case 12: {  //darse de baja
				
				
				strcpy(consulta,""); //nos aseguramos que esta vacio
				strcpy(consulta,"DELETE FROM jugador WHERE usuario = '");  //concatenamos la consulta
				strcat(consulta,nombre);
				strcat(consulta,"' AND contrasena = ");
				p = strtok( NULL, "/");
				strcpy (contrasena, p);
				strcat(consulta,contrasena);
				strcat(consulta,";");
				
				
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al borrar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}else
				{
					/**pthread_mutex_lock (&mutex);//Pedimos que no interrumpan
					Poner(&lista,nombre,sock_conn); //añadimos el nuevo usuario a la lista de conectados				
					pthread_mutex_unlock (&mutex); //ya puede interrumpir*/
					
					strcpy (buff2,"12/El servidor realizo correctamente el registro");
					write (sock_conn,buff2, strlen(buff2));//enviamos mensaje
				}
				
				break;
			}
				
			case 13: {   //resultado de partida con jugador x
				
				strcpy(consulta,"");
				char contrincante[20];
				p = strtok( NULL, "/");
				strcpy (nombre, p);
				p = strtok( NULL, "/");
				strcpy (contrincante, p);
				strcpy(consulta,"SELECT partida, posicion FROM resumen WHERE partida IN (SELECT partida FROM resumen WHERE jugador = '"); //concatenamos la consulta
				strcat(consulta, nombre);
				strcat(consulta,"') AND jugador = '");
				strcat(consulta, contrincante);
				strcat(consulta,"';");

				
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al realizar la consulta de datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit(1);
				}	
				
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				
				if(row==NULL)
				{
					printf("No se han obtenido datos en la consulta.\n");
				    sprintf(buff2, "3/No hay datos de partida.");
					write (sock_conn,buff2, strlen(buff2));
				}
					
				
				else {
				char resultados[100];
		        sprintf(buff2, "3/");
				while (row != NULL)
				{
				sprintf(resultados, "Partida:%s-Posicion:%s-",row[0], row[1]);
				strcat(buff2, resultados);
				row = mysql_fetch_row(resultado);
				}
				
				write (sock_conn,buff2, strlen(buff2));
				printf("%s\n", buff2);
				}
				
				break;
			
			}
				
			case 14: {   //partidas jugadas en fecha tal
				
				char fecha[20];
				p = strtok( NULL, "/");
				strcpy (nombre, p);
				p = strtok( NULL, "/");
				strcpy (fecha, p);
				strcpy(consulta,"SELECT COUNT(partida.ID) FROM resumen, partida, jugador WHERE jugador.usuario = '"); //concatenamos la consulta
				strcat(consulta, nombre);
				strcat(consulta,"' AND partida.fecha = '");
				strcat(consulta, fecha);
				strcat(consulta,"' AND resumen.jugador = jugador.usuario;");
				
				
				err=mysql_query (conn, consulta);
				
				if (err!=0) {
					printf ("Error al realizar la consulta de datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				if(row==NULL)
				{
					printf("No se han obtenido datos en la consulta.\n");
					sprintf(buff2, "3/No hay datos de esa fecha.");
					write (sock_conn,buff2, strlen(buff2));
				}
				
				printf("Partidas jugadas: %s\n", row[0]);
				sprintf (buff2,"3/%s",row[0]);
				write (sock_conn,buff2, strlen(buff2));
				
				break;
			}
		}
	}
			
}




int main(int argc, char *argv[]) {
	
	
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;

	//inicialitzem el socket per poder establir una connexio
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0){ //INICIALITZACIONS,Obrim el socket	
		printf("Error creant socket");
	}
	
	
	// Fem el bind al port
	memset(&serv_adr, 0, sizeof(serv_adr));// inicialitza a zero serv_addr
	serv_adr.sin_family = AF_INET;	// asocia el socket a cualquiera de las IP de la m?quina.
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY); //htonl formatea el numero que recibe al formato necesario
	serv_adr.sin_port = htons(9020); // escucharemos en el port 90X0
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0){
		printf ("Error al bind\n");
	}
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0){
		printf("Error en el Listen");
	}
	
	
	
	int a = 0; //variable para identifica el thread
	lista.num =0;
	pthread_t thread[100];	//vector estructura thread
	
	while(true){  //atenderemos peticiones constantemente
		
		printf ("Escuchando\n");  //empezamos a escuchar
		sock_conn = accept(sock_listen, NULL, NULL); //realizamos conexion, sock_conn es el socket que usaremos para este cliente
		printf ("Conexion realizada\n ");
		pthread_create(&thread[a], NULL, AtenderCliente, &sock_conn); //creamos un hilo para cada cliente, y llamamos ala funcion atender cliente que atenderá todas sus peticiones
		a++;
		
	}
}














	

