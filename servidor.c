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

pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER; //Acceso excluyente


int Poner(ListaConectados *lista, char nombre[20]){
	//Anade nuevo conectado.
	if(lista->num == 100){
		return -1;
	}else{
		strcpy(lista->conectados[lista->num].nombre, nombre);  //he sacado lo de añadir el socket porque en el main principal ya se lo añadimos y antes de llamar a la funcion AtenderCliente ya hemos establecido un socket
		lista->num = lista->num + 1;
		return 0;
	}
}
	
int Eliminar(ListaConectados *lista, char nombre[20]){
		
	int pos = DamePosicion(lista, nombre); //encontramos la posicion de la persona en la lista
	
	if(pos == -1){
		return -1;  //no se encontro
	}else{
		int i;
		for(i=pos; i<lista->num-1; i++){
			
			strcpy(lista->conectados[i].nombre, lista->conectados[i+1].nombre);
			lista->conectados[i].socket = lista->conectados[i+1].socket;
			
		}
		lista->num = lista->num-1;
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
		}i++;
	}
	if(encontrado == 1)
	{
		return i;
	
	}else return -1;
		
	
}
			
			
void DameConectados(ListaConectados *lista){  //no podemos devolver un string de manera optima, mejor hacerlo en el swtich case
	
	//Pone en conectados los nombres de todos los conectados separados por /
	int i;
	char conectados[50];
	
	if(lista->num > 0){
		 
		
		for(i=0; i< lista->num  ; i++) {
			
			strcat(conectados,lista->conectados[i].nombre);
			strcat(conectados,"/");
		}
	 }
	printf("%d personas conectadas: %s\n" , lista->num, conectados);
}
	
	



	
void *AtenderCliente(ListaConectados *lista){
	
	
	MYSQL *conn;
	MYSQL_RES *resultado; // Estructura especial para almacenar resultados de consultas 
	MYSQL_ROW row;
	conn = mysql_init(NULL); //Creamos una conexion al servidor MYSQL  
	
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

  //??????????????????????????????
	char buff[512],buff2[512], nombre[20],contrasena[20], email[20], idPartida[20];
	char conectados[80];
	int ret, err;
	int codigo, pos, i;
	int terminar=0;
	int sock_conn;
	sock_conn=lista->conectados[lista->num].socket;  //inicializamos el valor del socket del usuario que esta guardado en la lista

	while (terminar==0){
		
		ret=read(sock_conn, buff, sizeof(buff)); //longitud mensaje recibido
		buff[ret]='\0'; // Tenemos que anadirle la marca de fin de string, para que no escriba lo que hay despues en el buffer
		printf ("Recibido\n"); 
		printf("MENSAJE: %s\n", buff);
		char *p;
		p = strtok( buff, "/");
		codigo = atoi(p);  //convierte el string en un entero
		
		if(codigo==1 || codigo==2){  //los casos de acceder y registrar
			p = strtok( NULL, "/");
			strcpy (nombre, p);
		}
		
		char consulta[80];  //variable utilizada para las consultas SQL
		switch (codigo)
		{
			case 0: {   //desconexion
				printf("DESCONEXION\n");
				close(sock_conn);
				Eliminar(lista, nombre);
				printf("Numero de personas: %d \n", lista->num);
				DameConectados(lista);
				terminar = 1;
				break;
			}
			case 1: {   //insertar
			
				pthread_mutex_lock (&mutex);//Pedimos que no interrumpan
			
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

				Poner(lista,nombre); //añadimos el nuevo usuario a la lista de conectados
			
				err=mysql_query (conn, consulta);
				if (err!=0) {
					printf ("Error al insertar datos en la base %u %s\n",
							mysql_errno(conn), mysql_error(conn));
					exit (1);
				}else
				{
					strcpy (buff2,"1/El servidor realizo correctamente el registro");
					write (sock_conn,buff2, strlen(buff2));
				}
				printf("Se conecto %s con socket: %d\n", 
				lista->conectados[lista->num -1].nombre, lista->conectados[lista->num -1].socket); //le restamos uno porque el numero de la lista se incremento
				pthread_mutex_unlock (&mutex);//Ya pueden interrumpir
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
				resultado = mysql_store_result (conn);// El resultado es una estructura matricial en memoria,en la que cada fila contiene los datos de una persona.
				// Ahora obtenemos la primera fila que se almacena en una
				// variable de tipo MYSQL_ROW
				row = mysql_fetch_row (resultado);
				if (row!=0){ 
				
					printf("Resultado consulta: %s\n", row[0]);
				
					if(strcmp(row[0], contrasena)==0){
						Poner(lista, nombre); //añadimos el usuario a la lista conectados
						strcpy (buff2,"2/Acceso correcto");
						write (sock_conn,buff2, strlen(buff2)); 
					}
					
				}
				strcpy (buff2," ");  //enviamos un mensaje vacio para que el cliente sepa que no se ha accedido correctamente
				write (sock_conn,buff2, strlen(buff2)); 
			
				break;
			}
			case 3: {   //ganador
			
				p = strtok( NULL, "/");
				strcpy (idPartida, p);
				strcat(consulta,idPartida);
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
				printf("Resultado: %s\n", row[0]);
				strcpy(buff2,"2/");
				strcat (buff2, row[0]);
				write (sock_conn,buff2, strlen(buff2));
				//close(sock_conn); 
			
			
				break;
			
			}
			case 4: {   //posicion
			
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
				printf("Posicion: %s\n", row[0]);
				strcpy (buff2,row[0]);
				write (sock_conn,buff2, strlen(buff2));
				//close(sock_conn); 
			
			
				break;
			
			}
			case 5: {   //duracion
			
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
			case 6: {   //conectados
							
				//Pone en conectados los nombres de todos los conectados separados por /
				
				if(lista->num > 0){
					
					//strcat(conectados,lista->conectados[0].nombre);
					for(i=0; i< lista->num  ; i++) {
						
						strcat(conectados,lista->conectados[i].nombre);
						strcat(conectados,"/");
					}
				}
				printf("%d personas conectadas: %s\n" , lista->num, conectados);
				//enviamos el string al cliente
				strcpy(buff2,"6/");
				strcat (buff2, conectados);
				write (sock_conn,buff2, strlen(buff2)); 
			
				break;
				
			}
		}
	}
			
}



int main(int argc, char *argv[]) {
	
	ListaConectados listaConectados;
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
	serv_adr.sin_port = htons(9040); // escucharemos en el port 9050
	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0){
		printf ("Error al bind");
	}
	//La cola de peticiones pendientes no podr? ser superior a 4
	if (listen(sock_listen, 2) < 0){
		printf("Error en el Listen");
	}
	
	
	
	int a = 0; //variable para identifica el thread
	listaConectados.num =0;
	pthread_t thread[100];	//vector estructura thread
	
	while(true){  //atenderemos peticiones constantemente
		
		printf ("Escuchando\n");  //empezamos a escuchar
		sock_conn = accept(sock_listen, NULL, NULL); //realizamos conexion, sock_conn es el socket que usaremos para este cliente
		printf ("Conexion realizada\n ");
		listaConectados.conectados[listaConectados.num].socket=sock_conn;
		printf("Identificador de socket : %d\n", listaConectados.conectados[listaConectados.num].socket);
		pthread_create(&thread[a], NULL, AtenderCliente, &listaConectados); //creamos un hilo para cada cliente, y llamamos ala funcion atender cliente que atenderá todas sus peticiones
		
		
	}
}
	

