﻿/*Olvera Morales Miguel Angel*/
using System;
namespace Generador
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (Lenguaje lenguaje = new Lenguaje("C:\\Users\\wachi\\OneDrive\\Escritorio\\AUTOMATAS\\Generador\\c2.gram"))
                {
                    /*
                    while(!lenguaje.FinArchivo())
                    {
                        lenguaje.NextToken();
                    }
                    */
                    lenguaje.gramatica();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}