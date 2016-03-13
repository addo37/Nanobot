using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Nanobot
{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean ca = false, key = false, crt = false, ovpn = false;
            String name = "";
            String[] CurFiles = Directory.GetFiles(Directory.GetCurrentDirectory());
            foreach (String s in CurFiles)
            {
                if (s.Length > 4)
                    if (s.Substring(s.Length - 4, 4).Equals(".key"))
                    {
                        int i = s.Length - 5;
                        while (s.ElementAt(i) != '\\')
                        {
                            name = s.ElementAt(i) + name;
                            i--;
                        }
                        key = true;
                        break;
                    }
            }

            if (File.Exists("ca.crt"))
                ca = true;

            if (File.Exists(name + ".crt"))
                crt = true;

            if (Directory.Exists("C:\\Program Files\\OpenVPN"))
                ovpn = true;

            Console.WriteLine("Welcome to Nanobot, we'll now try to automate your OpenVPN Nano installation.\n");

            Console.WriteLine("CA certificate: " + isPresent(ca));

            Console.WriteLine("Private certificate: " + isPresent(crt));

            Console.WriteLine("Private key: " + isPresent(key));

            Console.WriteLine("OpenVPN: " + isPresent(ovpn)+ "\n");

            if (!ca || !key || !crt || !ovpn)
            {
                Console.WriteLine("We have identified that the requirements are not met, make sure to have all files in the same directory of Nanobot.\n");
                Console.WriteLine("Press enter to exit...");
                Console.Read();
                return;
            }

            Directory.CreateDirectory("C:\\Program Files\\OpenVPN\\cert");

            File.Copy(Directory.GetCurrentDirectory() + "\\ca.crt", "C:\\Program Files\\OpenVPN\\cert\\ca.crt", true);
            File.Copy(Directory.GetCurrentDirectory() + "\\" + name + ".key", "C:\\Program Files\\OpenVPN\\cert\\" + name + ".key", true);
            File.Copy(Directory.GetCurrentDirectory() + "\\" + name + ".crt", "C:\\Program Files\\OpenVPN\\cert\\" + name + ".crt", true);

            if (File.Exists("C:\\Program Files\\OpenVPN\\config\\nano.ovpn"))
            {
                File.Delete("C:\\Program Files\\OpenVPN\\config\\nano.ovpn");
            }

            if (!Directory.Exists("C:\\Program Files\\OpenVPN\\config\\"))
                Directory.CreateDirectory("C:\\Program Files\\OpenVPN\\config\\");

            System.IO.FileStream f = System.IO.File.Create("C:\\Program Files\\OpenVPN\\config\\nano.ovpn");
            f.Close();
            using (var file = File.AppendText("C:\\Program Files\\OpenVPN\\config\\nano.ovpn"))
            {
                file.Write("dev tap\nproto udp\nport 1194\nlog openvpn.log\nverb 3\n\nca \"C:\\\\Program Files\\\\OpenVPN\\\\cert\\\\ca.crt\"\ncert \"C:\\\\Program Files\\\\OpenVPN\\\\cert\\\\" + name + ".crt\"\nkey \"C:\\\\Program Files\\\\OpenVPN\\\\cert\\\\" + name + ".key\"\n\nclient\nremote-cert-tls server\nremote 149.3.152.245");
            }
            Console.WriteLine("Installation successful, Press enter to exit...");
            Console.Read();
        }

        static String isPresent(Boolean x)
        {
            if (x)
                return "Present";
            return "Not present";

        }
    }
}
