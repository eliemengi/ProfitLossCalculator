// TestSupport.cs (zu Zulassungsaufgabe 24W)
// statische Klasse zur Unterstützung von Tests
// nur für Konsolenanwendungen

using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Testing
{
    /// <summary>
    /// Klasse mit statischen Methoden zur Unterstützung des Testing
    /// </summary>
    public static class TestSupport
    {
        /// <summary>
        /// true: bei Fehler wird Ausnahme geworfen
        /// false: bei Fehler wird der Fehlerzähler erhöht
        /// </summary>
        public static bool ThrowException { get; set; } = false;

        /// <summary>
        /// Anzahl der gefundenen Fehler
        /// </summary>
        public static int Errors { get; private set; } = 0;

        /// <summary>
        /// Farbe für fehlerhafte Ausgaben
        /// </summary>
        public const ConsoleColor BadColor = ConsoleColor.Red;

        /// <summary>
        /// Farbe für korrekte Ausgaben
        /// </summary>
        public const ConsoleColor GoodColor = ConsoleColor.Green;

        /// <summary>
        /// farbiger Zeilenausdruck auf Konsole
        /// </summary>
        /// <param name="text">auszugebender Text</param>
        /// <param name="color">Vordergrundfarbe des Textes</param>
        public static void ColorPrint(string text, ConsoleColor color)
        {
            if (String.IsNullOrEmpty(text))
            {
                ErrorMessage("ColorPrint: fehlende Nachricht");
                return;
            }

            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            if (color == BadColor)
            {
                Errors++;
                if (ThrowException)
                    throw new Exception();
            }
        }

        /// <summary>
        /// Methode zum Überprüfen einer als boolscher Ausdruck formulierten Gegebenheit oder Randbedingung
        /// </summary>
        /// <param name="rule">Ergebnis des boolschen Ausdrucks (true - Bedingung eingehalten, false sonst)</param>
        public static void Assert(bool rule)
        {
            if (rule)
                ColorPrint("Assertion OK", GoodColor);
            else
                ColorPrint("Assertion failed", BadColor);
        }

        /// <summary>
        /// Methode zum Equals-Vergleich zweier Objekte mit farblicher Rückmeldung
        /// </summary>
        /// <param name="obj">erstes Objekt</param>
        /// <param name="comp">zweites Objekt</param>
        /// <param name="trueColor">Farbe bei Übereinstimmung</param>
        /// <param name="falseColor">Farbe bei Nichtübereinstimmung</param>
        public static void CompareAndPrint(object obj, object comp, ConsoleColor trueColor = GoodColor, ConsoleColor falseColor = BadColor)
        {
            if (obj == null || comp == null)
            {
                ErrorMessage("CompareAndPrint: fehlende Parameter");
                return;
            }
            ColorPrint(">" + obj.ToString() + "< vs. >" + comp.ToString() + "<", obj.Equals(comp) ? trueColor : falseColor);
        }

        /// <summary>
        /// Methode zum Vergleich zweier double-Zahlen
        /// </summary>
        /// <param name="d1">erste Zahl</param>
        /// <param name="d2">zweite Zahl</param>
        /// <param name="trueColor">Farbe bei Übereinstimmung</param>
        /// <param name="falseColor">Farbe bei Nichtübereinstimmung</param>
        /// <remarks>erlaubte Abweichung muss kleiner 10^-10 sein</remarks>
        public static void CompareAndPrintDouble(double d1, double d2, ConsoleColor trueColor = GoodColor, ConsoleColor falseColor = BadColor)
        {
            if (double.IsNaN(d1) || double.IsNaN(d2))
                ColorPrint("NaN", double.IsNaN(d1) && double.IsNaN(d2) ? GoodColor : BadColor);
            else
                ColorPrint(">" + d1.ToString() + "< vs. >" + d2.ToString() + "<", Math.Abs(d1 - d2) < 1E-10 ? trueColor : falseColor);
        }

        /// <summary>
        /// Mewthode zum Vergleich zweier decimal-Zahlen
        /// </summary>
        /// <param name="d1">erste Zahl</param>
        /// <param name="d2">zweite Zahl</param>
        /// <param name="trueColor">Farbe bei Übereinstimmung</param>
        /// <param name="falseColor">Farbe bei Nichtübereinstimmung</param>
        public static void CompareAndPrintDecimal(decimal d1, decimal d2, ConsoleColor trueColor = GoodColor, ConsoleColor falseColor = BadColor)
        {
            decimal diff = d1 - d2;
            diff = diff < 0.0M ? -diff : diff;
            ColorPrint(">" + d1.ToString() + "< vs. >" + d2.ToString() + "<", diff < 1E-25M ? trueColor : falseColor);
        }


        /// <summary>
        /// Methode zur Prüfung auf null
        /// </summary>
        /// <param name="obj">zu prüfende Referenz</param>
        /// <param name="trueColor">Farbe bei Übereinstimmung (Referenz ist null)</param>
        /// <param name="falseColor">Farbe bei Nichtübereinstimmung (Referenz ungleich null)</param>
        public static void IsNull(object obj, ConsoleColor trueColor = GoodColor, ConsoleColor falseColor = BadColor)
        {
            if (obj == null)
                ColorPrint("NULL", trueColor);
            else
                ColorPrint(obj.ToString(), falseColor);
        }

        /// <summary>
        /// Methode zum Test der überschriebenen Methoden ToString(), GetHashCode() und Equals(),
        /// bei Werttypen auch die Operatoren == und !=
        /// </summary>
        /// <typeparam name="T">Klasse oder Struct, deren/dessen Methoden getestet werden sollen</typeparam>
        /// <param name="l">Liste von Objekten derselben Klasse</param>
        /// <remarks>Die ersten beiden Objekte der Liste müssen wertgleich sein, die übrigen wertverschieden von den ersten beiden.</remarks>
        public static void ObjectTest<T>(T[] l)
        {
            if (l == null || l.Length < 3)
            {
                ErrorMessage("ObjectTest: fehlendes oder zu kurzes Array");
                return;
            }

            Console.WriteLine("\n----------\n-- ToString");

            foreach (object o in l)
                Console.WriteLine(o.ToString());

            Console.WriteLine("\n-- GetHashCode");

            int compHashCode = l[0].GetHashCode();
            Console.WriteLine(compHashCode);
            CompareAndPrint(l[1].GetHashCode(), compHashCode);
            for (int i = 2; i < l.Length; i++)
                CompareAndPrint(l[i].GetHashCode(), compHashCode, BadColor, GoodColor);

            Console.WriteLine("\n-- Equals");

            CompareAndPrint(l[1], l[0]);
            for (int i = 2; i < l.Length; i++)
                CompareAndPrint(l[i], l[0], BadColor, GoodColor);
            CompareAndPrint(l[0], new Object(), BadColor, GoodColor);

            // Überprüfung der Operatoren == und != für Werttypen
            Type type = typeof(T);
            if (type.IsValueType)
            {
                Console.WriteLine("\n-- == / !=");

                MethodInfo opEquality = type.GetMethod("op_Equality");
                MethodInfo opInequality = type.GetMethod("op_Inequality");
                if (opEquality == null || opInequality == null)
                {
                    ErrorMessage("Operatoren == oder != fehlen!");
                    return;
                }
                CompareAndPrint(opEquality.Invoke(null, new object[] { l[1], l[0] }), true);
                for (int i = 2; i < l.Length; i++)
                    CompareAndPrint(opInequality.Invoke(null, new object[] { l[i], l[0] }), true);
            }
        }

        /// <summary>
        /// Methode zum Testen der Methode CompareTo()
        /// </summary>
        /// <param name='l'>
        /// sortierte Liste von Objekten derselben Klasse
        /// </param>
        /// <remarks>
        /// Die ersten beiden Objekte der Liste müssen wertgleich sein, die übrigen aufsteigend sortiert.
        /// Die Methode darf nur für Klassen aufgerufen werden, die IComparable implementieren.
        /// </remarks>
        public static void SortedTest(IComparable[] l)
        {
            if (l == null || l.Length < 3)
            {
                ErrorMessage("SortedTest: fehlendes oder zu kurzes Array");
                return;
            }

            Console.WriteLine("\n----------\n");

            CompareAndPrint(Math.Sign((l[1]).CompareTo(l[0])), 0, GoodColor, BadColor);
            for (int i = 2; i < l.Length; i++)
            {
                CompareAndPrint(Math.Sign((l[i]).CompareTo(l[0])), 1, GoodColor, BadColor);
                CompareAndPrint(Math.Sign((l[0]).CompareTo(l[i])), -1, GoodColor, BadColor);
            }
        }

        /// <summary>
        /// Test der Plausibilitätsprüfungen
        /// </summary>
        /// <param name="action">Aktion, die eine Ausnahme auslösen soll</param>
        public static void ProvokeException(Action action)
        {
            if (action == null)
            {
                ErrorMessage("ProvokeException: fehlende Aktion");
                return;
            }

            try
            {
                action();
            }
            catch
            {
                ColorPrint("Exception OK", GoodColor);
                return;
            }
            ColorPrint("Exception FAIL", BadColor);
        }

        /// <summary>
        /// Prüfung, ob legale Aktion eine unerwartete Ausnahme hervorruft
        /// </summary>
        /// <param name="action">zu prüfende legale Aktion</param>
        public static void DenyException(Action action)
        {
            if (action == null)
            {
                ErrorMessage("DenyException: fehlende Aktion");
                return;
            }

            try
            {
                action();
                ColorPrint("OK", GoodColor);
            }
            catch
            {
                ErrorMessage("unerwartete Ausnahme!");
            }
        }

        /// <summary>
        /// Zusammenstellung von Klassenbezeichnung, Eigenschaften und deren Werten eines Objektes
        /// </summary>
        /// <param name="obj">darzustellendes Objekt</param>
        /// <returns></returns>
        public static string NameAndProperties(this object obj)
        {
            Type type = obj.GetType();
            return type.GetProperties().Select(pi => $"   {pi.Name} {pi.GetValue(obj)}").Aggregate(new StringBuilder($"{type.Name}:"), (a, s) => a.Append(s)).ToString();
        }

        /// <summary>
        /// Wertvergleich zwischen zwei Objekten desselben Typs
        /// </summary>
        /// <typeparam name="T">Typ beider Vergleichsobjekte</typeparam>
        /// <param name="obj1">erstes Vergleichsobjekt</param>
        /// <param name="obj2">zweites Vergleichsobjekt</param>
        /// <remarks>gleiche Aufgabe wie Equals, aber
        /// <list type="bullet">
        /// <item>genauer aufgeschlüsselt</item>
        /// <item>mit Toleranz für double und decimal</item>
        /// <item>mit Fehlerzähler</item>
        /// </list>
        /// </remarks>
        public static void TestEqualProperties<T>(T obj1, T obj2)
        {
            Type type = typeof(T);
            foreach (PropertyInfo pi in type.GetProperties())
            {
                object val1 = pi.GetValue(obj1);
                object val2 = pi.GetValue(obj2);
                Console.Write($"{pi.Name}: {val1} vs. {val2} --> ");
                if (val1 is double val1Double)
                    CompareAndPrintDouble(val1Double, (double)val2);
                else if (val1 is decimal val1Decimal)
                    CompareAndPrintDecimal(val1Decimal, (decimal)val2);
                else
                    CompareAndPrint(val1, val2);
            }
        }

        /// <summary>
        /// Prüfung auf Nicht-Änderbarkeit der Eigenschaften einer Klasse (fehlender set-Accessor)
        /// </summary>
        /// <typeparam name="T">zu prüfende Klasse</typeparam>
        public static void TestForNonwriteableProperties<T>() => TestForNonwriteableProperties(typeof(T));

        /// <summary>
        /// Prüfung auf Nicht-Änderbarkeit der Eigenschaften einer Klasse (fehlender set-Accessor)
        /// </summary>
        /// <param name="type">zu prüfende Klasse</param>
        public static void TestForNonwriteableProperties(Type type)
        {
            foreach (PropertyInfo pi in type.GetProperties())
            {
                Console.Write(pi.Name + " änderbar: ");
                //  CompareAndPrint(pi.CanWrite, false);
                IsNull(pi.GetSetMethod());
            }
        }

        /// <summary>
        /// Prüfung auf Nicht-Änderbarkeit der Eigenschaften einer Klasse / eines Datensatzes (fehlender set-Accessor oder Init-only-Accessor)
        /// </summary>
        /// <typeparam name="T">zu prüfender Typ</typeparam>
        public static void TestForNonWriteableOrInitOnlyProperties<T>() => TestForNonWriteableOrInitOnlyProperties(typeof(T));

        /// <summary>
        /// Prüfung auf Nicht-Änderbarkeit der Eigenschaften einer Klasse / eines Datensatzes (fehlender set-Accessor oder Init-only-Accessor)
        /// </summary>
        /// <param name="T">zu prüfender Typ</param>
        public static void TestForNonWriteableOrInitOnlyProperties(Type T)
        {
            foreach (PropertyInfo pi in T.GetProperties())
            {
                Console.Write(pi.Name + ": ");
                MethodInfo mi = pi.GetSetMethod();
                if (mi == null)
                    ColorPrint("nur Get-Accessor", GoodColor);
                else
                    if (mi.ReturnParameter.GetRequiredCustomModifiers().Contains(typeof(IsExternalInit)))
                        ColorPrint("Init-only-Accessor", GoodColor);
                    else
                        ColorPrint("Set-Accessor", BadColor);
            }
        }

        /// <summary>
        /// Ausgabe einer Fehlermeldung
        /// </summary>
        /// <param name="message">Text der Fehlermeldung</param>
        public static void ErrorMessage(string message = "fehlende Fehlermeldung ;)") => ColorPrint(message, BadColor);

        /// <summary>
        /// Ausgabe der Quellcode-Datei und Zeilennummer
        /// </summary>
        /// <param name="file">optionaler Parameter Dateiname (wird automatisch bestimmt)</param>
        /// <param name="lineNumber">optionaler Parameter Zeilennummer (wird automatisch bestimmt)</param>
        public static void PL([CallerFilePath] string file = null, [CallerLineNumber] int lineNumber = 0)
            => Console.Write("[{0}:{1:000}] ", Regex.Replace(file, @"^.*\\", ""), lineNumber);

        /// <summary>
        /// Ausgabe Überschrift Stufe 1
        /// </summary>
        /// <param name="text">Text der Überschrift</param>
        public static void PrintHeading1(string text) => Console.WriteLine($"\n\n*** {text} ***\n\n");

        /// <summary>
        /// Ausgabe Überschrift Stufe 2
        /// </summary>
        /// <param name="text">Text der Überscrift</param>
        public static void PrintHeading2(string text) => Console.WriteLine($"\n-- {text}\n");

        /// <summary>
        /// Ausgabe des Fehlerzählers
        /// </summary>
        public static void PrintResult()
        {
            if (Errors > 0)
                ColorPrint("Fehler: " + Errors.ToString(), BadColor);
            else
                ColorPrint("keine Fehler", GoodColor);

        }
    }
}
