/*Olvera Morales Miguel Angel*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Requerimiento 1: Construir un metodo para escribir en el archivo Lenguaje.cs intentando el codigo
//                 "{" incrementa un tabulador, "}" decrementa un tabulador
//Requerimiento 2: Declarar un atributo "primeraProduccion" de tipo string y actualizarlo con la primera produccion de la gramatica
//Requerimiento 3: La primera produccion es publica y el resto privadas
//Requerimiento 4: El constructor lexico parametrizado debe validar que la extension del archivo a compilar sea .gen, y si no 
//                 es asi debe lanzar una excepcion
//Requerimiento 5: Resolver la ambiguedad de ST y SNT

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        public Lenguaje(string nombre) : base(nombre)
        {
        }
        public Lenguaje()
        {
        }
        public void Dispose()
        {
            Console.WriteLine("\n\nDestructor ejecutado exitosamente");
            cerrar();
        }
        private void Programa(string produccionPrincipal)
        {
            programa.WriteLine("using System;");
            programa.WriteLine("namespace Generico");
            programa.WriteLine("{");
            programa.WriteLine("    public class Program");
            programa.WriteLine("    {");
            programa.WriteLine("        static void Main(string[] args)");
            programa.WriteLine("        {");
            programa.WriteLine("            try");
            programa.WriteLine("            {");
            programa.WriteLine("                using (Lenguaje a = new Lenguaje())");
            programa.WriteLine("                {");
            programa.WriteLine("                    a." + produccionPrincipal.ToLower() + "();");
            programa.WriteLine("                }");
            programa.WriteLine("            }");
            programa.WriteLine("            catch (Exception e)");
            programa.WriteLine("            {");
            programa.WriteLine("                Console.WriteLine(e.Message);");
            programa.WriteLine("            }");
            programa.WriteLine("        }");
            programa.WriteLine("    }");
            programa.WriteLine("}");
        }
        public void gramatica()
        {
            cabecera();
            Programa("programa");
            cabeceraLenguaje();
            listaDeProducciones();
            lenguaje.WriteLine("\t}");
            lenguaje.WriteLine("}");
        }
        private void cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.SNT);
            match(Tipos.FinProduccion);
        }
        private void cabeceraLenguaje()
        {
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("using System.Collections.Generic;");
            lenguaje.WriteLine("using System.Linq;");
            lenguaje.WriteLine("using System.Threading.Tasks;");
            lenguaje.WriteLine("");
            lenguaje.WriteLine("namespace Generico");
            lenguaje.WriteLine("{");
            lenguaje.WriteLine("    public class Lenguaje : Sintaxis, IDisposable");
            lenguaje.WriteLine("    {");
            lenguaje.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("        }");
            lenguaje.WriteLine("        public Lenguaje()");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("        }");
            lenguaje.WriteLine("        public void Dispose()");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("            cerrar();");
            lenguaje.WriteLine("        }");
        }
        private void listaDeProducciones()
        {
            lenguaje.WriteLine("\t\tprivate void " + getContenido() + "()");
            lenguaje.WriteLine("\t\t{");
            match(Tipos.SNT);
            match(Tipos.Produce);
            simbolos();
            match(Tipos.FinProduccion);
            lenguaje.WriteLine("\t\t}");
            if (!FinArchivo())
            {
                listaDeProducciones();
            }
        }
        private void simbolos()
        {
            if (esTipo(getContenido()))
            {
                lenguaje.WriteLine("\t\t\tmatch(Tipos." + getContenido() + ");");
                match(Tipos.SNT);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                lenguaje.WriteLine("\t\t\tmatch(\"" + getContenido() + "\");");
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.SNT)
            {
                lenguaje.WriteLine("\t\t\t" + getContenido() + "();");
                match(Tipos.SNT);
            }
            if (getClasificacion() != Tipos.FinProduccion)
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
    }
}