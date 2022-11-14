/*Olvera Morales Miguel Angel*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generador
{
    public class Lexico : Token
    {
        protected StreamReader archivo;
        protected StreamWriter log;
        const int F = -1;
        const int E = -2;
        protected int linea;
        protected int i = 0;
        int[,] TRAND = new int[,]
        {
        };
        public Lexico()
        {
            linea = 1;
            string path = "C:\\Users\\wachi\\OneDrive\\Escritorio\\AUTOMATAS\\Generador\\c.gram";
            bool existencia = File.Exists(path);
            log = new StreamWriter("C:\\Users\\wachi\\OneDrive\\Escritorio\\AUTOMATAS\\Generador\\c.log");
            log.AutoFlush = true;
            log.WriteLine("Archivo: c.gram");
            log.WriteLine("Fecha: " + DateTime.Now.ToString());
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
            log.WriteLine("Archivo: " + nombre);
            log.WriteLine("Fecha: " + DateTime.Now.ToString());
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
        }
        private void clasifica(int estado)
        {

        }
        private int columna(char c)
        {
            return 0;
        }
        public void NextToken()
        {
            string buffer = "";
            char c;
            int estado = 0;
            while (estado >= 0)
            {
                c = (char)archivo.Peek();
                estado = TRAND[estado, columna(c)];
                clasifica(estado);
                if (estado >= 0)
                {
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
            if (estado == E)
            {
                throw new Error("\nError lexico: No definido en linea: " + linea, log);
            }
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}