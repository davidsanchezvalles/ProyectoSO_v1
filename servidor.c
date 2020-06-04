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
//#include <my_global.h>
//v4_sergio	


typedef struct {
	char nombre[20];
	int socket;
}Conectado;

typedef struct {
	Conectado conectados[100];
	int num;
}ListaConectados;

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER; //Acceso excluyente

ListaConectados lista;

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
	char consulta[80];  //variable utilizada para las consultas SQL

	while (terminar==0){
		
		
		ret=read(sock_conn, buff, sizeof(buff)); //ret=longitud mensaje recibido, buff=mensaje recibido
		buff[ret]='\0'; // Tenemos que anadirle la marca de fin de string, para que no escriba lo que hay despues en el buffer
		printf("MENSAJE CLIENTE RECIBIDO: %s\n", buff);
		char *p;
		p = strtok( buff, "/"); //primera llamada strtok
		codigo = atoi(p);  //convierte el string en un entero
		
		if(codigo==1 || codigo==2 ){  //los casos de acceder y registrar
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
				
				printf("Resultado: %s\n", row[0]);
				strcpy(buff2,"3/");
				strcat (buff2, row[0]);
				write (sock_conn,buff2, strlen(buff2));
			
				break;
			
			}
			case 4: {   //posicion
			
				
				p = strtok( NULL, "/");
				strcpy (idPartida, p);
				strcpy(consulta,"SELECT posicion FROM resumen, partida, jugador WHERE partida.ID = "); //concatenamos la consulta
				strcat(consulta, idPartida);
				strcat(consulta," AND jugador.usuario = '");
				strcat(consulta, nombre);
				strcat(consulta,"' AND resumen.jugador = jugador.usuario AND resumen.partida = partida.ID;");
				
				
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	
				
				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				
				printf("Posicion: %s\n", row[0]);
				sprintf (buff2,"4/%s",row[0]);
				write (sock_conn,buff2, strlen(buff2));
			
				break;
			
			}
			case 5: {   //duracion
			
				p = strtok( NULL, "/");
				strcpy (idPartida, p);
				strcat(consulta, idPartida);
				strcpy(consulta,"SELECT duracion FROM partida WHERE partida.ID = ");
				strcat(consulta, idPartida);
				strcat(consulta,";");

				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
					exit (1);
				}	

				resultado = mysql_store_result (conn);
				row = mysql_fetch_row (resultado);
				
				printf("Duracion: %s\n", row[0]);
				sprintf (buff2,"5/%s",row[0]);
				write (sock_conn,buff2, strlen(buff2));
			
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
				sprintf(buff2,"7/Desea jugar?");
				write (lista.conectados[pos].socket,buff2, strlen(buff2)); 
				
				break;
			}
			
			case 8: {	//respuesta
				
				char respuesta[4];
				p = strtok( NULL, "/");
				strcpy (respuesta, p);
				if (strcmp(respuesta,"SI")==0)
				{
					sprintf(buff2,"8/Se acepto invitacion,comienza el juego");
					for (i=0; i<lista.num; i++){  //enviamos este mensaje a todos los clientes
						write (lista.conectados[i].socket,buff2, strlen(buff2)); 
					}
				}else
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
			case 10: {	//chat
				char mensaje[50];
				double fuerzaX, fuerzaY;
				int grado;
				p = strtok( NULL, "/");
				grado = atoi(p);
				p = strtok(NULL, "/");
				fuerzaX = atof(p);
				p = strtok(NULL, "/");
				fuerzaY = atof(p);
				printf("/n10/%s/n", mensaje);
				sprintf(mensaje, "10/%d/%.3f/%.3f", grado, fuerzaX, fuerzaY);
				printf("GRADOS:	%d	FUERZAx:%f	FUERZAy:%f\n", grado, fuerzaX, fuerzaY);
				write (sock_conn,mensaje, strlen(mensaje));
				
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
	serv_adr.sin_port = htons(9050); // escucharemos en el port 90X0
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0){
		printf ("Error al bind");
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
	

