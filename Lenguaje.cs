/*Olvera Morales Miguel Angel*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

//Requerimiento 1 OK: Construir un metodo para escribir en el archivo Lenguaje.cs intentando el codigo
//                 "{" incrementa un tabulador, "}" decrementa un tabulador
//Requerimiento 2 OK: Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la primera produccion de la gramatica
//Requerimiento 3 OK: La primera produccion es publica y el resto privadas
//Requerimiento 4 OK: El constructor lexico parametrizado debe validar que la extension del archivo a compilar sea .gen, y si no 
//                 es asi debe lanzar una excepcion
//Requerimiento 5 OK: Resolver la ambiguedad de ST y SNT
//                 recorrer linea por linea el archivo .gram para extraer del nombre de cada produccion
//Requerimiento 6: Agregar el parentesis izquierdo y parentesis derecho escapados en la matriz de transiciones
//Requerimiento 7: Implementar la cerradura epsilon
namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        public string primeraProduccion;
        int numTabs;
        List<string> listaSNT;
        public Lenguaje(string nombre) : base(nombre)
        {
            primeraProduccion = "";
            numTabs = 0;
            listaSNT = new List<string>();
        }
        public Lenguaje()
        {
            primeraProduccion = "";
            numTabs = 0;
            listaSNT = new List<string>();
        }
        public void Dispose()
        {
            Console.WriteLine("\n\nDestructor ejecutado exitosamente");
            cerrar();
        }
        private bool esSNT(string contenido)
        {
            return listaSNT.Contains(contenido);
        }
        private void agregarSNT(string contenido)
        {
            /*Requerimiento 5:*/
            string[] lineas = System.IO.File.ReadAllLines("C:\\Users\\wachi\\OneDrive\\Escritorio\\AUTOMATAS\\Generador\\c2.gram");
            foreach (string linea in lineas)
            {
                String line = linea.Replace("    ", "");
                var result = Regex.Match(line, @"^([\w\-]+)");
                //Console.WriteLine(result.Value);
                listaSNT.Add(result.Value);
            }
        }
        private void Programa(string produccionPrincipal)
        {
            agregarSNT(getContenido());
            programa.WriteLine(TabControl("using System;"));
            programa.WriteLine(TabControl("namespace Generico"));
            programa.WriteLine(TabControl("{"));
            programa.WriteLine(TabControl("public class Program"));
            programa.WriteLine(TabControl("{"));
            programa.WriteLine(TabControl("static void Main(string[] args)"));
            programa.WriteLine(TabControl("{"));
            programa.WriteLine(TabControl("try"));
            programa.WriteLine(TabControl("{"));
            programa.WriteLine(TabControl("using (Lenguaje a = new Lenguaje())"));
            programa.WriteLine(TabControl("{"));
            programa.WriteLine(TabControl("a." + produccionPrincipal + "();"));
            programa.WriteLine(TabControl("}"));
            programa.WriteLine(TabControl("}"));
            programa.WriteLine(TabControl("catch (Exception e)"));
            programa.WriteLine(TabControl("{"));
            programa.WriteLine(TabControl("Console.WriteLine(e.Message);"));
            programa.WriteLine(TabControl("}"));
            programa.WriteLine(TabControl("}"));
            programa.WriteLine(TabControl("}"));
            programa.WriteLine(TabControl("}"));
        }
        public void gramatica()
        {
            cabecera();
            Programa("Programa");
            cabeceraLenguaje();
            listaDeProducciones(true);
            lenguaje.WriteLine(TabControl("}"));
            lenguaje.WriteLine(TabControl("}"));
        }
        private void cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.ST);
            match(Tipos.FinProduccion);
        }
        private void cabeceraLenguaje()
        {
            lenguaje.WriteLine(TabControl("using System;"));
            lenguaje.WriteLine(TabControl("using System.Collections.Generic;"));
            lenguaje.WriteLine(TabControl("using System.Linq;"));
            lenguaje.WriteLine(TabControl("using System.Threading.Tasks;"));
            lenguaje.WriteLine(TabControl(""));
            lenguaje.WriteLine(TabControl("namespace Generico"));
            lenguaje.WriteLine(TabControl("{"));
            lenguaje.WriteLine(TabControl("public class Lenguaje : Sintaxis, IDisposable"));
            lenguaje.WriteLine(TabControl("{"));
            lenguaje.WriteLine(TabControl("public Lenguaje(string nombre) : base(nombre)"));
            lenguaje.WriteLine(TabControl("{"));
            lenguaje.WriteLine(TabControl("}"));
            lenguaje.WriteLine(TabControl("public Lenguaje()"));
            lenguaje.WriteLine(TabControl("{"));
            lenguaje.WriteLine(TabControl("}"));
            lenguaje.WriteLine(TabControl("public void Dispose()"));
            lenguaje.WriteLine(TabControl("{"));
            lenguaje.WriteLine(TabControl("cerrar();"));
            lenguaje.WriteLine(TabControl("}"));
        }
        private void listaDeProducciones(bool primera)
        {
            if (primera)
            {
                lenguaje.WriteLine(TabControl("public void " + getContenido() + "()"));
                primeraProduccion = "public void " + getContenido() + "(){}";
                primera = false;
            }
            else
            {
                lenguaje.WriteLine(TabControl("private void " + getContenido() + "()"));
            }
            lenguaje.WriteLine(TabControl("{"));
            match(Tipos.ST);
            match(Tipos.Produce);
            simbolos();
            match(Tipos.FinProduccion);
            lenguaje.WriteLine(TabControl("}"));
            if (!FinArchivo())
            {
                listaDeProducciones(primera);
            }
        }
        private void simbolos()
        {
            if(getContenido() == "(")
            {
                match("(");
                lenguaje.WriteLine(TabControl("if ()"));
                lenguaje.WriteLine(TabControl("{"));
                simbolos();
                match(")");
                lenguaje.WriteLine(TabControl("}"));
            }
            else if (esTipo(getContenido()))
            {
                lenguaje.WriteLine(TabControl("match(Tipos." + getContenido() + ");"));
                match(Tipos.ST);
            }
            else if (esSNT(getContenido()))
            {
                lenguaje.WriteLine(TabControl("" + getContenido() + "();"));
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                lenguaje.WriteLine(TabControl("match(\"" + getContenido() + "\");"));
                match(Tipos.ST);
            }
            if (getClasificacion() != Tipos.FinProduccion  && getContenido() != ")")
            {
                simbolos();
            }
        }
        private bool esTipo(string clasificacion)
        {
            switch (clasificacion)
            {
                case "Identificador":
                case "Numero":
                case "Caracter":
                case "Asignacion":
                case "Inicializacion":
                case "OperadorLogico":
                case "OperadorRelacional":
                case "OperadorTernario":
                case "OperadorTermino":
                case "OperadorFactor":
                case "IncrementoTermino":
                case "IncrementoFactor":
                case "FinSentencia":
                case "Cadena":
                case "TipoDato":
                case "Zona":
                case "Condicion":
                case "Ciclo":
                return true;
            }
            return false;
        }
        private string TabControl(string contenido)
        {
            string cadena = "";
            if (contenido == "}")
            {
                numTabs--;
            }
            for (int i = 0; i < numTabs; i++)
            {
                cadena += "\t";
            }
            cadena += contenido;
            if (contenido == "{")
            {
                numTabs++;
            }
            return cadena;
        }
    }
}