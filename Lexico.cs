using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Lexico: Token
    {
        protected StreamReader archivo;
        protected StreamWriter log;
        const int F = -1;
        const int E = -2;
        protected int linea;
        //Posicion del archivo
        protected int i = 0;
        int[,] TRAND = new int[,]
        {
        };
        //"C:\\Users\\wachi\\OneDrive\\Escritorio\\Prollecto\\prueba.log"
        public Lexico()
        {
            linea = 1;
            string path = "C:\\Users\\wachi\\OneDrive\\Escritorio\\AUTOMATAS\\Semantica\\prueba.cpp";
            bool existencia = File.Exists(path);
            log = new StreamWriter("C:\\Users\\wachi\\OneDrive\\Escritorio\\AUTOMATAS\\Semantica\\prueba.log");
            log.AutoFlush = true;
            asm = new StreamWriter("C:\\Users\\wachi\\OneDrive\\Escritorio\\AUTOMATAS\\Semantica\\prueba.asm");
            asm.AutoFlush = true;
            log.WriteLine("Archivo: prueba.cpp");
            log.WriteLine("Fecha: " + DateTime.Now);
            asm.WriteLine(";Archivo: prueba.asm");
            asm.WriteLine(";Fecha: " + DateTime.Now);

            //Investigar como checar si un archivo existe o no existe 
            if (existencia == true)
            {
                archivo = new StreamReader(path);
            }
            else
            {
                throw new Error("\nError: El archivo prueba no existe", log);
            }
        }
        public Lexico(string nombre)
        {
            linea = 1;
            string pathLog = Path.ChangeExtension(nombre, ".log");
            log = new StreamWriter(pathLog);
            log.AutoFlush = true;
            string pathAsm = Path.ChangeExtension(nombre, ".asm");
            asm = new StreamWriter(pathAsm);
            asm.AutoFlush = true;
            log.WriteLine("Archivo: " + nombre);
            log.WriteLine("Fecha: " + DateTime.Now);
            asm.WriteLine(";Archivo: " + nombre);
            asm.WriteLine(";Fecha: " + DateTime.Now);
            if (File.Exists(nombre))
            {
                archivo = new StreamReader(nombre);
            }
            else
            {
                throw new Error("\nError: El archivo " + Path.GetFileName(nombre) + " no existe ", log);
            }
        }
        public void cerrar()
        {
            archivo.Close();
            log.WriteLine("Fin de compilacion");
            Console.WriteLine("\nFin de compilacion");
            log.Close();
            asm.Close();
        }

        private void clasifica(int estado)
        {
            switch (estado)
            {
                case 1:
                    setClasificacion(Tipos.Identificador);
                    break;
                case 2:
                    setClasificacion(Tipos.Numero);
                    break;
                case 8:
                    setClasificacion(Tipos.Asignacion);
                    break;
                case 9:
                case 17:
                case 18:
                case 19:
                    setClasificacion(Tipos.OperadorRelacional);
                    break;
                case 10:
                case 13:
                case 14:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 39:
                case 40:
                    setClasificacion(Tipos.Caracter);
                    break;
                case 11:
                    setClasificacion(Tipos.Inicializacion);
                    break;
                case 12:
                    setClasificacion(Tipos.FinSentencia);
                    break;
                case 15:
                case 16:
                    setClasificacion(Tipos.OperadorLogico);
                    break;
                case 21:
                case 22:
                    setClasificacion(Tipos.OperadorTermino);
                    break;
                case 23:
                    setClasificacion(Tipos.IncrementoTermino);
                    break;
                case 24:
                case 29:
                    setClasificacion(Tipos.OperadorFactor);
                    break;
                case 25:
                    setClasificacion(Tipos.IncrementoFactor);
                    break;
                case 26:
                    setClasificacion(Tipos.Cadena);
                    break;
                case 28:
                    setClasificacion(Tipos.OperadorTernario);
                    break;
            }
        }
        private int columna(char c)
        {
            //WS,EF,EL,L, D, .,	E, +, -, =,	:, ;, &, |,	!, >, <, *,	%, /, ", ?,La
            if (FinArchivo())
            {
                return 1;
            }
            else if (c == '\n')
            {
                return 2;
            }
            else if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (char.ToUpper(c) == 'E')
            {
                return 6;
            }
            else if (char.IsLetter(c))
            {
                return 3;
            }
            else if (char.IsDigit(c))
            {
                return 4;
            }
            else if (c == '.')
            {
                return 5;
            }
            else if (c == '+')
            {
                return 7;
            }
            else if (c == '-')
            {
                return 8;
            }
            else if (c == '=')
            {
                return 9;
            }
            else if (c == ':')
            {
                return 10;
            }
            else if (c == ';')
            {
                return 11;
            }
            else if (c == '&')
            {
                return 12;
            }
            else if (c == '|')
            {
                return 13;
            }
            else if (c == '!')
            {
                return 14;
            }
            else if (c == '>')
            {
                return 15;
            }
            else if (c == '<')
            {
                return 16;
            }
            else if (c == '*')
            {
                return 17;
            }
            else if (c == '%')
            {
                return 18;
            }
            else if (c == '/')
            {
                return 19;
            }
            else if (c == '"')
            {
                return 20;
            }
            else if (c == '?')
            {
                return 21;
            }
            else if (c == 39)
            {
                return 23;
            }
            else if (c == '#')
            {
                return 24;
            }
            return 22;
        }
        //WS,EF,EL,L, D, .,	E, +, -, =,	:, ;, &, |,	!, >, <, *,	%, /, ", ?,La, ', #
        public void NextToken()
        {
            string buffer = "";
            char c;
            int estado = 0;

            while (estado >= 0)
            {
                c = (char)archivo.Peek(); //Funcion de transicion
                estado = TRAND[estado, columna(c)];
                clasifica(estado);
                if (estado >= 0)
                {
                    //Posicion en el archivo
                    i++;
                    archivo.Read();
                    if (c == '\n')
                    {
                        linea++;
                    }
                    if (estado > 0)
                    {
                        buffer += c;
                    }
                    else
                    {
                        buffer = "";
                    }
                }
            }
            setContenido(buffer);
            switch (buffer)
            {
                case "char":
                case "int":
                case "float":
                    setClasificacion(Tipos.TipoDato);
                    break;
                case "private":
                case "protected":
                case "public":
                    setClasificacion(Tipos.Zona);
                    break;
                case "if":
                case "else":
                case "switch":
                    setClasificacion(Tipos.Condicion);
                    break;
                case "while":
                case "for":
                case "do":
                    setClasificacion(Tipos.Ciclo);
                    break;
            }
            if (estado == E)
            {
                //Requerimiento 9 agregar el numero de linea en el error
                if (getContenido()[0] == '"')
                {
                    throw new Error("\nError lexico: No se cerro la cadena con \" en linea: " + linea, log);
                }
                else if (getContenido()[0] == '\'')
                {
                    throw new Error("\nError lexico: No se cerro el caracter con ' en linea: " + linea, log);
                }
                else if (getClasificacion() == Tipos.Numero)
                {
                    throw new Error("\nError lexico: Se espera un digito en linea: " + linea, log);
                }
                else
                {
                    throw new Error("\nError lexico: No definido en linea: " + linea, log);
                }
            }
            else if (!FinArchivo())
            {
                //log.WriteLine(getContenido() + " | " + getClasificacion());
            }
        }

        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }

    }
    }
}